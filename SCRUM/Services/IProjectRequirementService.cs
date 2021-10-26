using SCRUM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Services
{
    interface IProjectRequirementService
    {
        #region Pending Requirements

        public Task CreatePendingRequirement(PendingRequirementModel pendingRequirement, int userOid);
        public Task<IEnumerable> GetPendingAll(int projectId);
        public Task<IEnumerable> GetPendingBy(int projectId, int userId);
        public Task<PendingRequirementModel> RejectPendingRequirement(int pendingId);
        public Task EditPendingRequrement(PendingRequirementModel pendingRequirement);
        
        #endregion

        #region Requirements

        public Task CreateRequirement(int pendingId, RequirementModel requirement);
        public Task<IEnumerable> GetRequirements(int projectId);
        public Task ReorderRequirements(List<RequirementModel> requirementList);
        public Task EditRequirement(RequirementModel requirement);
        public Task EditRequirementStatus(RequirementModel requirement);

        #endregion

        //public Task<int> GetUserOid(string name);
    }
}
