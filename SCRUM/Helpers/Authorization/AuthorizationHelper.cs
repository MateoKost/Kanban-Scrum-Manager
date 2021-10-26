using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using SCRUM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using DevExpress.Xpo;
using AutoMapper;
using System.Collections;
using SCRUM.Helpers.Authorization.Enums;

namespace SCRUM.Helpers
{
    public class AuthorizationHelper        
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthorizationHelper(UnitOfWork uow, IMapper map)
        {
            unitOfWork = uow;
            mapper = map;
        }

        public async Task<ProjectUserModel> GetUser(string name)
        {
            var user = await unitOfWork.Query<ProjectUser>().Where(c => c.Name == name).FirstAsync();
           
            return ProjectUserModel.From(user, mapper);
        }
        public async Task<IEnumerable> GetRoles(int projOid, int userOid)
        {
            var roleList = await unitOfWork.Query<Role>().Where(c => c.ProjectId == projOid && c.UserId == userOid).ToListAsync();
            if(roleList == null)
            {
                return null;
            }
            return RoleModel.From(roleList);
        }
        public async Task<IEnumerable> GetAccTypes(RoleHeader roleHeader)
        {
            var roleList = await unitOfWork.Query<RoleAccess>().Where(c => c.Name == roleHeader).ToListAsync();
            if(roleList == null)
            {
                return null;
            }
            return RoleAccessModel.From(roleList);
        }
    }
}
