using SCRUM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Services
{
    interface IProjectService
    {
        public Task<IEnumerable> GetProjects();
        public Task<ProjectModel> GetByTag(string tag);
        public Task CreateProject(ProjectModel projectModel, int userId);
        public Task EditProject(ProjectModel projectModel);
        public Task DeleteProject(int id);
    }
}
