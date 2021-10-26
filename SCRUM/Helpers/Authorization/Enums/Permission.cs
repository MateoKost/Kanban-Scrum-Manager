using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Helpers.Authorization.Enums
{
    public enum Permission
    {
        ReadProposals,
        CreateProposal,
        UpdateProposal,
        UpdateMyProposal,
        DeclineProposal,
        AcceptProposal,
        ReadRequirements,
        UpdateRequirement,
        UpdateNotStartedRequirement,
        ReorderRequirements,
        ChangeRequirementStatusAndTime,
        UpdateProject,
        DeleteProject,
        AddRole
    }
}
