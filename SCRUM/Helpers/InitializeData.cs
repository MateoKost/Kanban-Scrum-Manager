using AutoMapper;
using DevExpress.Xpo;
using SCRUM.Helpers.Authorization.Enums;
using SCRUM.Models;
using SCRUM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Helpers
{
    /// <summary>
    /// This class is made only for testing purposes. 
    /// It initializes starting data for make testing faster.
    /// 
    /// MUST BE DELETED BEFORE RELEASE
    /// </summary>
    public class InitializeData 
    {
        private readonly UnitOfWork unitOfWork;
        private readonly LoginService loginService;
        private readonly ProjectRequirementService projectRequirementService;
        private readonly ProjectService projectService;
        public InitializeData(UnitOfWork uow, IMapper mapper)
        {
            unitOfWork = uow;
            loginService = new LoginService(uow, mapper);
            projectRequirementService = new ProjectRequirementService(uow, mapper);
            projectService = new ProjectService(uow, mapper);
        }

        public async Task<bool> Initialize()
        {
            var userFromData = await unitOfWork.Query<ProjectUser>().Where(c => c.Name == "StakeHolder").AnyAsync();
            if (!userFromData)
            {
                #region StakeHolder permissions allocation
                {
                    new RoleAccess(unitOfWork) { Name = RoleHeader.StakeHolder, Permission = Permission.CreateProposal };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.StakeHolder, Permission = Permission.ReadProposals };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.StakeHolder, Permission = Permission.ReadRequirements };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.StakeHolder, Permission = Permission.UpdateMyProposal };
                }
                #endregion

                #region ProductOwner permissions allocation
                {
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.CreateProposal };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.ReadProposals };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.ReadRequirements };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.UpdateMyProposal };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.UpdateProposal };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.UpdateNotStartedRequirement };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.ReorderRequirements };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.UpdateProject };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.DeleteProject };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.AddRole };

                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.AcceptProposal };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.ProductOwner, Permission = Permission.DeclineProposal };
                }
                #endregion

                #region DevTeam permissions allocation
                {
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.CreateProposal };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.ReadProposals };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.ReadRequirements };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.UpdateMyProposal };

                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.UpdateProposal };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.UpdateNotStartedRequirement };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.ReorderRequirements };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.UpdateProject };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.DeleteProject };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.AddRole };

                    //new RoleAccess(unitOfWork) { name = RoleHeader.DevTeam, permission = Permission.AcceptProposal };
                    //new RoleAccess(unitOfWork) { name = RoleHeader.DevTeam, permission = Permission.DeclineProposal };

                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.UpdateRequirement };
                    new RoleAccess(unitOfWork) { Name = RoleHeader.DevTeam, Permission = Permission.ChangeRequirementStatusAndTime };
                }
                #endregion

                await unitOfWork.CommitChangesAsync();

                ProjectUser.From(unitOfWork, new ProjectUserModel() { Name = "StakeHolder" });
                ProjectUser.From(unitOfWork, new ProjectUserModel() { Name = "ProductOwner" });
                ProjectUser.From(unitOfWork, new ProjectUserModel() { Name = "DevTeam" });

                await unitOfWork.CommitChangesAsync();

                ProjectUserModel user1 = new ProjectUserModel();
                ProjectUserModel user2 = new ProjectUserModel();
                ProjectUserModel user3 = new ProjectUserModel();
                var userList = (List<ProjectUserModel>)await loginService.GetUsers();
                foreach(ProjectUserModel item in userList)
                {
                    if(item.Name == "StakeHolder")
                    {
                        user1 = item;
                    }
                    else if(item.Name == "ProductOwner")
                    {
                        user2 = item;
                    }
                    else if(item.Name == "DevTeam")
                    {
                        user3 = item;
                    }
                }

                // Adding project
                Project.From(unitOfWork, new ProjectModel()
                {
                    Title = "Tytul projektu 1",
                    Description = "Opis projektu 1",
                    Tag = "projectname"
                });
                await unitOfWork.CommitChangesAsync();

                // Adding roles
                var project = await unitOfWork.Query<Project>()
                    .Where(c => c.Title == "Tytul projektu 1" && c.Description == "Opis projektu 1").FirstAsync();

                var rol1 = new RoleModel()
                {
                    UserId = user1.Oid,
                    ProjectId = project.Oid,
                    Name = RoleHeader.StakeHolder
                };
                Role.From(unitOfWork, rol1);

                var rol2 = new RoleModel()
                {
                    UserId = user2.Oid,
                    ProjectId = project.Oid,
                    Name = RoleHeader.ProductOwner
                };
                Role.From(unitOfWork, rol2);

                var rol3 = new RoleModel()
                {
                    UserId = user3.Oid,
                    ProjectId = project.Oid,
                    Name = RoleHeader.DevTeam
                };
                Role.From(unitOfWork, rol3);

                // Adding Pending Requirements
                var pen1 = new PendingRequirementModel()
                {
                    Title = "Tytul propozycji 1 wymagania",
                    Description = "Opis propozycji 1 utworzony przez StakeHolder",
                    ProjectId = project.Oid,
                    CreatorId = user1.Oid,
                    Status = "Oczekuje"
                };

                PendingRequirement.From(unitOfWork, pen1);

                var pen2 = new PendingRequirementModel()
                {
                    Title = "Tytul propozycji 2 wymagania",
                    Description = "Opis propozycji 2 utworzony przez ProjectOwner",
                    ProjectId = project.Oid,
                    CreatorId = user2.Oid,
                    Status = "Oczekuje"
                };
                PendingRequirement.From(unitOfWork, pen2);

                var pen3 = new PendingRequirementModel()
                {
                    Title = "Tytul propozycji 3 wymagania",
                    Description = "Opis propozycji 3 utworzony przez StakeHolder",
                    ProjectId = project.Oid,
                    CreatorId = user2.Oid,
                    Status = "Oczekuje"
                };
                PendingRequirement.From(unitOfWork, pen3);

                var pen4 = new PendingRequirementModel()
                {
                    Title = "Tytul propozycji 4 wymagania",
                    Description = "Opis propozycji 4 utworzony przez StakeHolder",
                    ProjectId = project.Oid,
                    CreatorId = user2.Oid,
                    Status = "Oczekuje"
                };
                PendingRequirement.From(unitOfWork, pen4);

                var pen5 = new PendingRequirementModel()
                {
                    Title = "Tytul propozycji 5 wymagania",
                    Description = "Opis propozycji 5 utworzony przez StakeHolder",
                    ProjectId = project.Oid,
                    CreatorId = user2.Oid,
                    Status = "Oczekuje"
                };
                PendingRequirement.From(unitOfWork, pen5);

                await unitOfWork.CommitChangesAsync();

                // Adding Requirements
                var listCount = unitOfWork.Query<Requirement>().Where(c => c.Status == "To do").Count();
                var req1 = new RequirementModel()
                {
                    Title = "Tytul 4 wymagania",
                    Description = "Opis 4",
                    ProjectId = project.Oid,
                    Priority = 1,
                    Touchstone = "Kryt1",
                    Effortfulness = 5,
                    Status = "Zaakceptowane",
                    Index = 1
                };
                Requirement.From(unitOfWork, req1);
                var preq = await unitOfWork.Query<PendingRequirement>().Where(c => c.Title == "Tytul propozycji 4 wymagania").FirstAsync();

                await unitOfWork.DeleteAsync(preq);
                await unitOfWork.CommitChangesAsync();

                listCount = unitOfWork.Query<Requirement>().Where(c => c.Status == "To do").Count();
                var req2 = new RequirementModel()
                {
                    Title = "Tytul 5 wymagania",
                    Description = "Opis 5",
                    ProjectId = project.Oid,
                    Priority = 1,
                    Touchstone = "Kryt1",
                    Effortfulness = 5,
                    Status = "Zaakceptowane",
                    Index = 2
                };
                Requirement.From(unitOfWork, req2);
                var preq2 = await unitOfWork.Query<PendingRequirement>().Where(c => c.Title == "Tytul propozycji 5 wymagania").FirstAsync();

                await unitOfWork.DeleteAsync(preq2);
                await unitOfWork.CommitChangesAsync();

                
            }

            await unitOfWork.CommitChangesAsync();
            return true;
        }
    }
}
