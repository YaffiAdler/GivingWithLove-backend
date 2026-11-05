using Bl.Dtos.Project;
using Dal.Models;
using Dal.Queries;

namespace Bl.Mapping
{
    internal static class ProjectMapper
    {
        public static ProjectDto ToDto(Project project)
        {
            return new ProjectDto(project.ProjectCode, project.ProjectName, project.ProjectManagerCode, project.DomainCode);
        }

        public static DepartmentNodeDto ToNode(DepartmentTreeRow department)
        {
            return new DepartmentNodeDto(
                department.ProjectCode,
                department.ProjectName,
                department.Projects.Select(ToNode).ToList());
        }

        public static ProjectNodeDto ToNode(ProjectTreeRow project)
        {
            return new ProjectNodeDto(
                project.ProjectCode,
                project.ProjectName,
                project.SubProjects.Select(SubProjectMapper.ToDto).ToList());
        }

        public static Project ToEntity(CreateProjectDto project)
        {
            return new Project()
            {
                ProjectName = project.ProjectName,
                ProjectManagerCode = project.ProjectManagerCode,
                DomainCode = project.DomainCode
            };
        }

        public static Project ToEntity(UpdateProjectDto project)
        {
            return new Project()
            {
                ProjectCode = project.ProjectCode,
                ProjectName = project.ProjectName,
                ProjectManagerCode = project.ProjectManagerCode,
                DomainCode = project.DomainCode
            };
        }
    }
}
