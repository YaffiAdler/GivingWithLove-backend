using Dal.Models;

namespace Dal.Queries
{
    // A project under a department with its sub-projects (read without tracking).
    public sealed record ProjectTreeRow(
        int ProjectCode,
        string ProjectName,
        List<SubProject> SubProjects);
}
