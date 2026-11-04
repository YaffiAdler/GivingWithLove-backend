using Bl.BLApi;
using Bl.BLModels;
using Bl.Exceptions;
using Dal.Api;
using Dal.Models;

namespace Bl.BLServices
{
    public class BlSubProjectService : BlSubProjectInterface
    {
        private const string EntityName = nameof(SubProject);

        private readonly DalSubProjectInterface dal;

        public BlSubProjectService(DalSubProjectInterface dal)
        {
            this.dal = dal;
        }

        public async Task<List<BlSubProjectModel>> GetAllAsync(CancellationToken ct)
        {
            List<SubProject> subProjects = await dal.GetAllAsync(ct);
            return subProjects.Select(Convert).ToList();
        }

        public async Task<BlSubProjectModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            SubProject? subProject = await dal.GetByIdAsync(id, ct);
            return subProject == null ? null : Convert(subProject);
        }

        public async Task<BlSubProjectModel> CreateAsync(BlSubProjectModel item, CancellationToken ct)
        {
            SubProject created = await dal.CreateAsync(Convert(item), ct);
            return Convert(created);
        }

        public async Task UpdateAsync(BlSubProjectModel item, CancellationToken ct)
        {
            if (!await dal.UpdateAsync(Convert(item), ct))
                throw new NotFoundException(EntityName, item.SubProjectCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await dal.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }

        private static SubProject Convert(BlSubProjectModel subProject)
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

        private static BlSubProjectModel Convert(SubProject subProject)
        {
            return new BlSubProjectModel()
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
