using AutoMapper;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using SCRUM.Helpers.Authorization.Enums;
using SCRUM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Helpers.Authorization
{
    public class DataAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, int>
    {
        private readonly AuthorizationHelper authHelper;
        private readonly UnitOfWork unitOfWork;

        public DataAuthorizationHandler(UnitOfWork uow, IMapper map)
        {
            authHelper = new AuthorizationHelper(uow, map);
            unitOfWork = uow;
        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, 
            int resourceId)
        {
            try
            {
                var user = authHelper.GetUser(context.User.Identity?.Name).Result;
                var roles = authHelper.GetRoles(resourceId, user.Oid).Result;
                if(roles == null)
                {
                    return Task.CompletedTask;
                }
                foreach (RoleModel item in roles)
                {
                    var acctypeList = authHelper.GetAccTypes(item.Name).Result;

                    foreach(RoleAccessModel acctype in acctypeList)
                    {
                        if (acctype.Permission.ToString().Contains(requirement.Name))
                        {
                            context.Succeed(requirement);
                            return Task.CompletedTask;
                        }
                        if(acctype.Permission.ToString() == requirement.Name)
                        {
                            context.Succeed(requirement);
                        }
                    }
                }

                return Task.CompletedTask;
            }
            catch (AggregateException)
            {
                return Task.CompletedTask;
            }
        }
    }
}
