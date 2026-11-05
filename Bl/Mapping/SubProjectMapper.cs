using Bl.Dtos.SubProject;
using Dal.Models;

namespace Bl.Mapping
{
    internal static class SubProjectMapper
    {
        public static SubProjectDto ToDto(SubProject subProject)
        {
            return new SubProjectDto(
                subProject.SubProjectCode,
                subProject.ProjectCode,
                subProject.SubProjectName,
                subProject.EstimatedTime,
                subProject.EstimatedCost);
        }

        public static SubProject ToEntity(CreateSubProjectDto subProject)
        {
            return new SubProject()
            {
                ProjectCode = subProject.ProjectCode,
                SubProjectName = subProject.SubProjectName,
                EstimatedTime = subProject.EstimatedTime,
                EstimatedCost = subProject.EstimatedCost
            };
        }

        public static SubProject ToEntity(UpdateSubProjectDto subProject)
        {
            return new SubProject()
            {
                SubProjectCode = subProject.SubProjectCode,
                ProjectCode = subProject.ProjectCode,
                SubProjectName = subProject.SubProjectName,
                EstimatedTime = subProject.EstimatedTime,
                EstimatedCost = subProject.EstimatedCost
            };
        }
    }
}
