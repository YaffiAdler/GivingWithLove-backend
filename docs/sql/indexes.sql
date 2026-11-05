-- Missing indexes for the paged list endpoints (BE-04) and the coming statistics API (BE-05).
-- Run manually against the GWLData database AFTER a backup. Every statement is guarded, so the script is safe to re-run.
-- Existing before this script: only the clustered primary keys of the seven tables.
-- Creating an index does not change any row.

SET NOCOUNT ON;

-- GET /api/Volunteering default order (DateOfVolunteering DESC, VolunteeringCode DESC) and the from/to filter.
-- BE-05: grouping volunteerings by date (per day / month / year).
-- The clustered key VolunteeringCode is part of every nonclustered index, so it completes the sort for free.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Volunteerings_DateOfVolunteering' AND object_id = OBJECT_ID(N'dbo.Volunteerings'))
    CREATE INDEX IX_Volunteerings_DateOfVolunteering
        ON dbo.Volunteerings (DateOfVolunteering DESC);
GO

-- GET /api/Volunteering?volunteerCode=... ordered by date (equality on the first column, sort on the second).
-- BE-05: grouping volunteerings by volunteer; also the join from Volunteers.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Volunteerings_VolunteerCode_DateOfVolunteering' AND object_id = OBJECT_ID(N'dbo.Volunteerings'))
    CREATE INDEX IX_Volunteerings_VolunteerCode_DateOfVolunteering
        ON dbo.Volunteerings (VolunteerCode, DateOfVolunteering DESC);
GO

-- GET /api/Volunteering?projectCode=... ordered by date.
-- BE-05: grouping volunteerings by project.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Volunteerings_ProjectCode_DateOfVolunteering' AND object_id = OBJECT_ID(N'dbo.Volunteerings'))
    CREATE INDEX IX_Volunteerings_ProjectCode_DateOfVolunteering
        ON dbo.Volunteerings (ProjectCode, DateOfVolunteering DESC);
GO

-- BE-05: grouping volunteerings by matcher (the volunteer who arranged the match).
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Volunteerings_MatcherCode' AND object_id = OBJECT_ID(N'dbo.Volunteerings'))
    CREATE INDEX IX_Volunteerings_MatcherCode
        ON dbo.Volunteerings (matcherCode, DateOfVolunteering DESC);
GO

-- BE-05: grouping volunteerings by sub-project.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Volunteerings_SubProjectCode' AND object_id = OBJECT_ID(N'dbo.Volunteerings'))
    CREATE INDEX IX_Volunteerings_SubProjectCode
        ON dbo.Volunteerings (SubProjectCode, DateOfVolunteering DESC);
GO

-- GET /api/Eichud and GET /api/Volunteer with sort=name (default): FamilyName, FirstName, code.
-- Lets SQL Server read the first page in order instead of sorting the whole table.
-- Note: search uses LIKE '%text%', which cannot seek; at tens of thousands of rows a scan of this narrow index is acceptable.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Eichud_FamilyName_FirstName' AND object_id = OBJECT_ID(N'dbo.Eichud'))
    CREATE INDEX IX_Eichud_FamilyName_FirstName
        ON dbo.Eichud (FamilyName, FirstName);
GO

-- GET /api/Volunteer and GET /api/Volunteer/{id}: projectCodes of each volunteer on the page,
-- and PUT /api/Volunteer/domains, which reads the domains of one volunteer.
-- Non-unique on purpose: the EF model declares (VolunteerCode, ProjectCode) unique, but no such constraint exists
-- in the database; making it unique is a separate decision for the owner (existing duplicates would make it fail).
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_VolunteerDomains_VolunteerCode_ProjectCode' AND object_id = OBJECT_ID(N'dbo.VolunteerDomains'))
    CREATE INDEX IX_VolunteerDomains_VolunteerCode_ProjectCode
        ON dbo.VolunteerDomains (VolunteerCode, ProjectCode);
GO
