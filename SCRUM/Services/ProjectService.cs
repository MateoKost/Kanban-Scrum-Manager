using AutoMapper;
using DevExpress.Xpo;
using SCRUM.Helpers.Authorization.Enums;
using SCRUM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Services
{
    public class ProjectService : IProjectService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProjectService(UnitOfWork uow, IMapper map)
        {
            unitOfWork = uow;
            mapper = map;
        }

        public async Task<IEnumerable> GetProjects()
        {
            var result = await unitOfWork.Query<Project>().ToListAsync();
            if (result.Count == 0)
            {
                return null;
            }

            return ProjectModel.From(result, mapper);
        }

        public async Task<ProjectModel> GetByTag(string tag)
        {
            var result = await unitOfWork.Query<Project>().Where(c => c.Tag == tag).FirstAsync();

            return ProjectModel.From(result, mapper);
        }

        public async Task CreateProject(ProjectModel projectModel, int userId)
        {
            Project.From(unitOfWork, projectModel);
            await unitOfWork.CommitChangesAsync();

            var result = await unitOfWork.Query<Project>()
                .Where(c => c.Title == projectModel.Title && c.Description == projectModel.Description).FirstAsync();

            Role.From(unitOfWork, new RoleModel()
            {
                UserId = userId,
                ProjectId = result.Oid,
                Name = RoleHeader.ProductOwner
            });

            await unitOfWork.CommitChangesAsync();
        }

        public async Task EditProject(ProjectModel projectModel)
        {
            var toEdit = await unitOfWork.GetObjectByKeyAsync<Project>(projectModel.Oid);

            if (toEdit != null)
            {
                mapper.Map(projectModel, toEdit);

                await unitOfWork.CommitChangesAsync();
            }
        }

        public async Task DeleteProject(int id)
        {
            Project project = await unitOfWork.GetObjectByKeyAsync<Project>(id);
            if (project != null)
            {
                await unitOfWork.DeleteAsync(project);
                await unitOfWork.CommitChangesAsync();
            }
        }
    }
}
