using Dal.Models;
using System.Linq.Expressions;

namespace Dal.Queries
{
    // The single definition of the two-level project hierarchy: departments (אגפים) and the projects under them.
    internal static class ProjectHierarchy
    {
        // The intended marker of a department is DomainCode == null,
        // but some existing rows mark a department by pointing DomainCode to their own ProjectCode.
        public static readonly Expression<Func<Project, bool>> IsDepartment =
            project => project.DomainCode == null || project.DomainCode == project.ProjectCode;

        // Every project with the department it belongs to; a department belongs to itself.
        // A project whose parent is not a department has no link and is therefore not counted under any department.
        public static IQueryable<ProjectDepartmentLink> DepartmentLinks(IQueryable<Project> projects)
        {
            IQueryable<Project> departments = projects.Where(IsDepartment);

            return from project in projects
                   from department in departments
                   where department.ProjectCode == project.ProjectCode || department.ProjectCode == project.DomainCode
                   select new ProjectDepartmentLink
                   {
                       ProjectCode = project.ProjectCode,
                       DepartmentCode = department.ProjectCode,
                       DepartmentName = department.ProjectName
                   };
        }
    }
}
