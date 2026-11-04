using Bl.BLApi;
using Bl.BLModels;
using Bl.Exceptions;
using Dal.Api;
using Dal.Models;

namespace Bl.BLServices
{
    public class BlProjectService : BlProjectInterface
    {
        private const string EntityName = nameof(Project);

        private readonly DalProjectInterface dal;

        public BlProjectService(DalProjectInterface dal)
        {
            this.dal = dal;
        }

        public async Task<List<BlProjectModel>> GetAllAsync(CancellationToken ct)
        {
            List<Project> projects = await dal.GetAllAsync(ct);
            return projects.Select(Convert).ToList();
        }

        public async Task<BlProjectModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            Project? project = await dal.GetByIdAsync(id, ct);
            return project == null ? null : Convert(project);
        }

        public async Task<BlProjectModel> CreateAsync(BlProjectModel item, CancellationToken ct)
        {
            Project created = await dal.CreateAsync(Convert(item), ct);
            return Convert(created);
        }

        public async Task UpdateAsync(BlProjectModel item, CancellationToken ct)
        {
            if (!await dal.UpdateAsync(Convert(item), ct))
                throw new NotFoundException(EntityName, item.ProjectCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await dal.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }

        private static Project Convert(BlProjectModel project)
        {
            return new Project()
            {
                ProjectCode = project.ProjectCode,
                ProjectName = project.ProjectName,
                ProjectManagerCode = project.ProjectManagerCode,
                DomainCode = project.DomainCode
            };
        }

        private static BlProjectModel Convert(Project project)
        {
            return new BlProjectModel()
            {
                ProjectCode = project.ProjectCode,
                ProjectName = project.ProjectName,
                ProjectManagerCode = project.ProjectManagerCode,
                DomainCode = project.DomainCode
            };
        }
    }
}
