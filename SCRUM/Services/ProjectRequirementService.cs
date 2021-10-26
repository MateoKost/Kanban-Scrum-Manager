using AutoMapper;
using DevExpress.Xpo;
using SCRUM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Services
{
    public class ProjectRequirementService : IProjectRequirementService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProjectRequirementService(UnitOfWork uow, IMapper map)
        {
            unitOfWork = uow;
            mapper = map;
        }

        #region Pending Requirements

        public async Task CreatePendingRequirement(PendingRequirementModel pendingRequirement, int userOid)
        {
            pendingRequirement.CreatorId = userOid;
            pendingRequirement.Status = "Oczekuje";
            PendingRequirement.From(unitOfWork, pendingRequirement);

            await unitOfWork.CommitChangesAsync();
        }
        public async Task<IEnumerable> GetPendingAll(int projectId)
        {
            var result = await unitOfWork.Query<PendingRequirement>()
                .Where(c => c.ProjectId == projectId).ToListAsync();

            return PendingRequirementModel.From(result, mapper);
        }
        public async Task<IEnumerable> GetPendingBy(int projectId, int userId)
        {
            var result = await unitOfWork.Query<PendingRequirement>()
                .Where(c => c.CreatorId == userId && c.ProjectId == projectId).ToListAsync();

            return PendingRequirementModel.From(result, mapper);
        }
        public async Task<PendingRequirementModel> RejectPendingRequirement(int pendingId)
        {
            PendingRequirement pendingRequirement = await unitOfWork.GetObjectByKeyAsync<PendingRequirement>(pendingId);
            if (pendingRequirement is null)
            {
                return null;
            }
            pendingRequirement.Status = "Odrzucone";
            await unitOfWork.CommitChangesAsync();
            return PendingRequirementModel.From(pendingRequirement, mapper);
        }
        public async Task EditPendingRequrement(PendingRequirementModel pendingRequirement)
        {
            PendingRequirement toEdit = await unitOfWork.GetObjectByKeyAsync<PendingRequirement>(pendingRequirement.Oid);
            
            if (toEdit != null)
            {
                toEdit.Description = pendingRequirement.Description;
                toEdit.Title = pendingRequirement.Title;
                
                await unitOfWork.CommitChangesAsync();
            }
        }

        #endregion

        #region Requirements

        public async Task CreateRequirement(int pendingId, RequirementModel requirement)
        {
            requirement.Status = "Zaakceptowane";

            var listCount = unitOfWork.Query<Requirement>().Where(c=>c.Status == "Zaakceptowane").Count();
            requirement.Index = listCount + 1;

            Requirement.From(unitOfWork, requirement);
            PendingRequirement pendingRequirement = await unitOfWork.GetObjectByKeyAsync<PendingRequirement>(pendingId);


            await unitOfWork.DeleteAsync(pendingRequirement);
            await unitOfWork.CommitChangesAsync();
        }
        public async Task<IEnumerable> GetRequirements(int projectId)
        {
            var result = await unitOfWork.Query<Requirement>()
                .Where(c => c.ProjectId == projectId).ToListAsync();

            return RequirementModel.From(result, mapper);
        }
        public async Task ReorderRequirements(List<RequirementModel> requirementList)
        {
            var req = await unitOfWork.GetObjectByKeyAsync<Requirement>(requirementList.FirstOrDefault().Oid);
            var projId = req.ProjectId;

            foreach (RequirementModel item in requirementList)
            {
                var toEdit = await unitOfWork.GetObjectByKeyAsync<Requirement>(item.Oid);
                toEdit.Index = item.Index;
            }
            await unitOfWork.CommitChangesAsync();
        }
        public async Task EditRequirement(RequirementModel requirement)
        {
            Requirement toEdit = await unitOfWork.GetObjectByKeyAsync<Requirement>(requirement.Oid);
            
            if (toEdit != null)
            {
                if(toEdit.Status != "Zakończone" || toEdit.Status != "Anulowane")
                {
                    toEdit.Title = requirement.Title;
                    toEdit.Description = requirement.Description;
                    toEdit.Priority = requirement.Priority;
                    toEdit.Touchstone = requirement.Touchstone;
                    toEdit.Effortfulness = requirement.Effortfulness;

                    await unitOfWork.CommitChangesAsync();
                }
            }
        }

        public async Task EditRequirementStatus(RequirementModel requirement)
        {
            Requirement toEdit = await unitOfWork.GetObjectByKeyAsync<Requirement>(requirement.Oid);
            bool changed = false;
            if (toEdit != null)
            { 
                if(toEdit.Status == "Zaakceptowane")
                {
                    if(requirement.Status == "W trakcie")
                    {
                        toEdit.Status = "W trakcie";
                        changed = true;
                    }
                    else if(requirement.Status == "Anulowane")
                    {
                        toEdit.Status = "Anulowane";
                        changed = true;
                    }

                }
                else if(toEdit.Status == "W trakcie")
                {
                    if (requirement.Status == "Zaakceptowane")
                    {
                        toEdit.Status = "Zaakceptowane";
                        changed = true;
                    }
                    else if (requirement.Status == "Zakończone")
                    {
                        toEdit.Status = "Zakończone";
                        changed = true;
                    }
                    else if (requirement.Status == "Anulowane")
                    {
                        toEdit.Status = "Anulowane";
                        changed = true;
                    }
                }
                if (changed)
                {
                    var listCount = unitOfWork.Query<Requirement>().Where(c => c.Status == toEdit.Status);
                    int max = 0;
                    foreach(Requirement item in listCount)
                    {
                        if (item.Index > max) max = item.Index;
                    }

                    toEdit.Index = max + 1;
                    await unitOfWork.CommitChangesAsync();
                }
            }
        }

        #endregion
    }
}
