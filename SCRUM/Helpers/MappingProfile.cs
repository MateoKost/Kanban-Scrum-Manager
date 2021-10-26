using AutoMapper;
using SCRUM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PendingRequirement, PendingRequirementModel>();
            CreateMap<PendingRequirementModel, PendingRequirement>();

            CreateMap<ProjectUser, ProjectUserModel>();
            CreateMap<ProjectUserModel, ProjectUser>();

            CreateMap<Project, ProjectModel>();
            CreateMap<ProjectModel, Project>();

            CreateMap<Requirement, RequirementModel>();
            CreateMap<RequirementModel, Requirement>();
        }
    }
}
