using Dal.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Bl.Tests.Stats
{
    // A fresh SQLite in-memory database with a small fixed dataset; it lives as long as the open connection.
    //
    // Departments: 10 "רווחה" (DomainCode NULL), 20 "חסד" (DomainCode = own code).
    // Projects: 11 "הסעות לבתי חולים" -> 10, 21 "ארוחות לשבת" -> 20.
    // Sub-projects (minutes, NIS): 100 "שערי צדק" -> 11 (90, 50), 101 "הדסה" -> 11 (30, 20), 200 "חלוקת סלים" -> 21 (60, 100).
    // Volunteers: 1 אברהם כהן, 2 יצחק לוי, 3 יעקב מזרחי. Eichud 4 משה פרץ is a recipient only.
    public sealed class StatsTestDatabase : IDisposable
    {
        public static readonly DateOnly January4 = new DateOnly(2026, 1, 4);   // Sunday
        public static readonly DateOnly January5 = new DateOnly(2026, 1, 5);   // Monday
        public static readonly DateOnly March6 = new DateOnly(2026, 3, 6);     // Friday
        public static readonly DateOnly March8 = new DateOnly(2026, 3, 8);     // Sunday
        public static readonly DateOnly April1 = new DateOnly(2026, 4, 1);     // Wednesday
        public static readonly DateOnly April2 = new DateOnly(2026, 4, 2);     // Thursday

        private readonly SqliteConnection connection;

        public StatsTestDatabase()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using DbManager db = CreateContext();
            db.Database.EnsureCreated();
            Seed(db);
        }

        public CommandCounter Commands { get; } = new CommandCounter();

        public DbManager CreateContext()
        {
            DbContextOptions<DbManager> options = new DbContextOptionsBuilder<DbManager>()
                .UseSqlite(connection)
                .AddInterceptors(Commands)
                .Options;
            return new DbManager(options);
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        private static void Seed(DbManager db)
        {
            db.Eichuds.AddRange(
                // Padded like the fixed-length char columns of SQL Server, to check that names are trimmed.
                new Eichud { EichudCode = 1, FirstName = "אברהם   ", FamilyName = "כהן    " },
                new Eichud { EichudCode = 2, FirstName = "יצחק", FamilyName = "לוי" },
                new Eichud { EichudCode = 3, FirstName = "יעקב", FamilyName = "מזרחי" },
                new Eichud { EichudCode = 4, FirstName = "משה", FamilyName = "פרץ" });
            db.Volunteers.AddRange(
                new Volunteer { VolunteerCode = 1 },
                new Volunteer { VolunteerCode = 2 },
                new Volunteer { VolunteerCode = 3 });
            db.Projects.AddRange(
                new Project { ProjectCode = 10, ProjectName = "רווחה" },
                new Project { ProjectCode = 20, ProjectName = "חסד" });
            db.SaveChanges();

            // Set after the insert: EF cannot order the insert of a row whose foreign key points to itself.
            db.Database.ExecuteSqlRaw("UPDATE Projects SET DomainCode = ProjectCode WHERE ProjectCode = 20");

            db.Projects.AddRange(
                new Project { ProjectCode = 11, ProjectName = "הסעות לבתי חולים", DomainCode = 10 },
                new Project { ProjectCode = 21, ProjectName = "ארוחות לשבת", DomainCode = 20 });
            db.SubProjects.AddRange(
                new SubProject { SubProjectCode = 100, ProjectCode = 11, SubProjectName = "שערי צדק", EstimatedTime = 90, EstimatedCost = 50 },
                new SubProject { SubProjectCode = 101, ProjectCode = 11, SubProjectName = "הדסה", EstimatedTime = 30, EstimatedCost = 20 },
                new SubProject { SubProjectCode = 200, ProjectCode = 21, SubProjectName = "חלוקת סלים", EstimatedTime = 60, EstimatedCost = 100 });
            db.SaveChanges();

            InsertVolunteering(db, 1, January4, volunteerCode: 1, matcherCode: 2, poorManCode: 4, projectCode: 11, subProjectCode: 100);
            InsertVolunteering(db, 2, January5, volunteerCode: 1, matcherCode: null, poorManCode: 4, projectCode: 11, subProjectCode: 101);
            InsertVolunteering(db, 3, March6, volunteerCode: 2, matcherCode: 1, poorManCode: 4, projectCode: 21, subProjectCode: 200);
            // Points to the department itself and has no sub-project: counts, with 0 hours and 0 value.
            InsertVolunteering(db, 4, March8, volunteerCode: 2, matcherCode: null, poorManCode: null, projectCode: 20, subProjectCode: null);
            // No volunteer: counts in totals but is not ranked.
            InsertVolunteering(db, 5, April1, volunteerCode: null, matcherCode: null, poorManCode: null, projectCode: 21, subProjectCode: 200);
            // No project: counts in totals but in no department.
            InsertVolunteering(db, 6, April2, volunteerCode: 3, matcherCode: null, poorManCode: null, projectCode: null, subProjectCode: null);
        }

        // Plain SQL instead of SaveChanges: the model gives VolunteerCode, matcherCode and PoorManCode a database default of 0,
        // so EF leaves a null value out of the INSERT and the row would get 0 instead of NULL.
        private static void InsertVolunteering(
            DbManager db, int volunteeringCode, DateOnly date,
            int? volunteerCode, int? matcherCode, int? poorManCode, int? projectCode, int? subProjectCode)
        {
            db.Database.ExecuteSql($"""
                INSERT INTO Volunteerings (VolunteeringCode, DateOfVolunteering, VolunteerCode, matcherCode, PoorManCode, ProjectCode, SubProjectCode)
                VALUES ({volunteeringCode}, {date}, {volunteerCode}, {matcherCode}, {poorManCode}, {projectCode}, {subProjectCode})
                """);
        }
    }
}
