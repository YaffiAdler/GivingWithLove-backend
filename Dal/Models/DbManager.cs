using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dal.Models;

public partial class DbManager : DbContext
{
    public DbManager()
    {
    }

    public DbManager(DbContextOptions<DbManager> options)
        : base(options)
    {
    }

    public virtual DbSet<Eichud> Eichuds { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<SubProject> SubProjects { get; set; }

    public virtual DbSet<Volunteer> Volunteers { get; set; }

    public virtual DbSet<VolunteerDomain> VolunteerDomains { get; set; }

    public virtual DbSet<Volunteering> Volunteerings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
      
        => 
        //optionsBuilder.UseSqlServer(" Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\אדלר יפי\\C#\\GivingWithLove-c\\Dal\\GWLData.mdf\";Integrated Security = True;Connect Timeout=30\n");

    optionsBuilder.UseSqlServer(
    "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\אדלר יפי\\C#\\GivingWithLove-c\\Dal\\GWLData.mdf;Integrated Security=True;Connect Timeout=30"
);
    protected override void OnModelCreating(ModelBuilder modelBuilder)


    {
        modelBuilder.Entity<Eichud>(entity =>
        {
            entity.HasKey(e => e.EichudCode).HasName("PK__Eichud__22A0751692FF82C5");

            entity.ToTable("Eichud");

            entity.Property(e => e.EichudCode).ValueGeneratedNever();
            entity.Property(e => e.CellPhone)
                .HasMaxLength(15)
                .IsFixedLength();
            entity.Property(e => e.City)
                .HasMaxLength(10)
                .HasDefaultValue("???????")
                .IsFixedLength();
            entity.Property(e => e.Ending)
                .HasMaxLength(10)
                .HasDefaultValue("???")
                .IsFixedLength();
            entity.Property(e => e.FamilyName)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.FathersName)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.House)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.HousePhone)
                .HasMaxLength(12)
                .IsFixedLength();
            entity.Property(e => e.Shtibel)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Shver)
                .HasMaxLength(40)
                .IsFixedLength();
            entity.Property(e => e.Street)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Tohar)
                .HasMaxLength(10)
                .HasDefaultValue("???\"?")
                .IsFixedLength();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.positionCode).HasName("PK__Position__83745B0338CA6DCA");

            entity.Property(e => e.positionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectCode).HasName("PK__Projects__2F3A4949DE7E676F");

            entity.Property(e => e.ProjectName).HasMaxLength(35);

            entity.HasOne(d => d.DomainCodeNavigation).WithMany(p => p.InverseDomainCodeNavigation)
                .HasForeignKey(d => d.DomainCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Projects__Domain__37A5467C");

            entity.HasOne(d => d.ProjectManagerCodeNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ProjectManagerCode)
                .HasConstraintName("FK__Projects__Projec__6754599E");
        });

        modelBuilder.Entity<SubProject>(entity =>
        {
            entity.HasKey(e => e.SubProjectCode).HasName("PK__tmp_ms_x__49AE8E1C27D15DA7");

            entity.Property(e => e.SubProjectName).HasMaxLength(50);

            entity.HasOne(d => d.ProjectCodeNavigation).WithMany(p => p.SubProjects)
                .HasForeignKey(d => d.ProjectCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SubProjec__Proje__619B8048");
        });

        modelBuilder.Entity<Volunteer>(entity =>
        {
            entity.HasKey(e => e.VolunteerCode).HasName("PK__Voluntee__249FDF5695D33AC0");

            entity.Property(e => e.VolunteerCode).ValueGeneratedNever();

            entity.HasOne(d => d.PositionCodeNavigation).WithMany(p => p.Volunteers)
                .HasForeignKey(d => d.PositionCode)
                .HasConstraintName("FK__Volunteer__Posit__45F365D3");

            entity.HasOne(d => d.VolunteerCodeNavigation).WithOne(p => p.Volunteer)
                .HasForeignKey<Volunteer>(d => d.VolunteerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Volunteer__Volun__68487DD7");
        });

        modelBuilder.Entity<VolunteerDomain>(entity =>
        {
            entity.HasIndex(v => new { v.VolunteerCode, v.ProjectCode })
         .IsUnique();
            entity.HasKey(e => e.VolunteerDomainsCode).HasName("PK__Voluntee__580D6AD82A08ACE1");

            entity.Property(e => e.VolunteerDomainsCode).ValueGeneratedOnAdd();

            entity.HasOne(d => d.ProjectCodeNavigation).WithMany(p => p.VolunteerDomains)
                .HasForeignKey(d => d.ProjectCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Volunteer__Proje__47DBAE45");

            entity.HasOne(d => d.VolunteerCodeNavigation).WithMany(p => p.VolunteerDomains)
                .HasForeignKey(d => d.VolunteerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Volunteer__Volun__46E78A0C");
        });

        modelBuilder.Entity<Volunteering>(entity =>
        {
            entity.HasKey(e => e.VolunteeringCode).HasName("PK__tmp_ms_x__14E272E0721B3015");

            entity.Property(e => e.MatcherCode)
                .HasDefaultValue(0)
                .HasColumnName("matcherCode");
            entity.Property(e => e.PoorManCode).HasDefaultValue(0);
            entity.Property(e => e.VolunteerCode).HasDefaultValue(0);

            entity.HasOne(d => d.MatcherCodeNavigation).WithMany(p => p.VolunteeringMatcherCodeNavigations)
                .HasForeignKey(d => d.MatcherCode)
                .HasConstraintName("FK__Volunteer__match__6477ECF3");

            entity.HasOne(d => d.PoorManCodeNavigation).WithMany(p => p.Volunteerings)
                .HasForeignKey(d => d.PoorManCode)
                .HasConstraintName("FK__Volunteer__PoorM__6383C8BA");

            entity.HasOne(d => d.ProjectCodeNavigation).WithMany(p => p.Volunteerings)
                .HasForeignKey(d => d.ProjectCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Volunteer__Proje__656C112C");

            entity.HasOne(d => d.SubProjectCodeNavigation).WithMany(p => p.Volunteerings)
                .HasForeignKey(d => d.SubProjectCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Volunteer__SubPr__66603565");

            entity.HasOne(d => d.VolunteerCodeNavigation).WithMany(p => p.VolunteeringVolunteerCodeNavigations)
                .HasForeignKey(d => d.VolunteerCode)
                .HasConstraintName("FK__Volunteer__Volun__628FA481");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    //internal void saveChanges()
    //{
    //    throw new NotImplementedException();
    //}

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
