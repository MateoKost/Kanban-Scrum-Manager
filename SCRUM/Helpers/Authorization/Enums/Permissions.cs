using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Helpers.Authorization
{
    public class Permissions : IAuthorizationRequirement
    {
        public static OperationAuthorizationRequirement ReadProposals 
            = new OperationAuthorizationRequirement(){ Name = nameof(ReadProposals) };
        public static OperationAuthorizationRequirement CreateProposal
            = new OperationAuthorizationRequirement() { Name = nameof(CreateProposal) };
        public static OperationAuthorizationRequirement UpdateProposal
            = new OperationAuthorizationRequirement() { Name = nameof(UpdateProposal) };        
        public static OperationAuthorizationRequirement UpdateMyProposal
            = new OperationAuthorizationRequirement() { Name = nameof(UpdateMyProposal) };
        public static OperationAuthorizationRequirement DeclineProposal
            = new OperationAuthorizationRequirement() { Name = nameof(DeclineProposal) };
        public static OperationAuthorizationRequirement AcceptProposal
            = new OperationAuthorizationRequirement() { Name = nameof(AcceptProposal) };
        public static OperationAuthorizationRequirement ReadRequirements
            = new OperationAuthorizationRequirement() { Name = nameof(ReadRequirements) };
        public static OperationAuthorizationRequirement UpdateRequirement
            = new OperationAuthorizationRequirement() { Name = nameof(UpdateRequirement) };        
        public static OperationAuthorizationRequirement UpdateNotStartedRequirement
            = new OperationAuthorizationRequirement() { Name = nameof(UpdateNotStartedRequirement) };        
        public static OperationAuthorizationRequirement ReorderRequirements
            = new OperationAuthorizationRequirement() { Name = nameof(ReorderRequirements) };
        public static OperationAuthorizationRequirement ChangeRequirementStatusAndTime
            = new OperationAuthorizationRequirement() { Name = nameof(ChangeRequirementStatusAndTime) };
        public static OperationAuthorizationRequirement UpdateProject
            = new OperationAuthorizationRequirement() { Name = nameof(UpdateProject) };
        public static OperationAuthorizationRequirement DeleteProject
            = new OperationAuthorizationRequirement() { Name = nameof(DeleteProject) };        
        public static OperationAuthorizationRequirement AddRole
            = new OperationAuthorizationRequirement() { Name = nameof(AddRole) };

        //ReadProposals,
        //CreateProposal,
        //UpdateProposal,
        //UpdateMyProposal,
        //DeclineProposal,
        //AcceptProposal,
        //ReadRequirements,
        //UpdateRequirement,
        //UpdateNotStartedRequirement,
        //ReorderRequirements,
        //ChangeRequirementStatusAndTime,
        //UpdateProject,
        //DeleteProject,
        //AddRole
    }
}
