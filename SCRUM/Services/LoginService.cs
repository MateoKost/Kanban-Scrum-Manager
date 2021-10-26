using AutoMapper;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SCRUM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCRUM.Services
{
    public class LoginService : ILoginService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoginService(UnitOfWork uow, IMapper map)
        {
            unitOfWork = uow;
            mapper = map;
        }
        #region Testing only

        // For data initialization
        public async Task<IEnumerable> GetUsers()
        {
            var result = await unitOfWork.Query<ProjectUser>().ToListAsync();
            return ProjectUserModel.From(result, mapper);
        }


        #endregion

        public async Task<IEnumerable> SignIn(ProjectUserModel userModel)
        {
            var userFromData = await unitOfWork.Query<ProjectUser>().Where(c => c.Name == userModel.Name).AnyAsync();
            if (userFromData) 
            {
                var claims = GetClaims(userModel);
                return claims;
            }
            else
            {
                return null;
            }
        }
        public async Task<IEnumerable> Register(ProjectUserModel userModel)
        {
            var userFromData = await unitOfWork.Query<ProjectUser>().Where(c => c.Name == userModel.Name).AnyAsync();
            
            if(userFromData)
            {
                return null;
            }
            ProjectUser.From(unitOfWork, userModel);

            var claims = GetClaims(userModel);
            
            await unitOfWork.CommitChangesAsync();
            return claims;
        }
        public async Task<int> GetUserOid(string userName)
        {
            var user = await unitOfWork.Query<ProjectUser>().Where(c => c.Name == userName).FirstAsync();
            return user.Oid;
        }

        public async Task<IEnumerable> GetMyPermissions(string userName)
        {
            var user = await unitOfWork.Query<ProjectUser>().Where(c => c.Name == userName).FirstAsync();
            if (user == null)
            {
                return null;
            }
            var roles = await unitOfWork.Query<Role>().Where(c => c.UserId == user.Oid).ToListAsync();
            if (roles == null)
            {
                return null;
            }
            var result = new List<object>();
            foreach (Role item in roles)
            {
                var acctypeList = await unitOfWork.Query<RoleAccess>().Where(c => c.Name == item.Name).ToListAsync();
                foreach (RoleAccess access in acctypeList)
                {
                    var converted = RoleAccessModel.From(access);
                    if (!result.Contains(converted))
                    {
                        var obj = new { Permission = access.Permission.ToString(), ProjectId = item.ProjectId, Role = item.Name.ToString() };
                        
                        result.Add(obj);
                    }
                }
            }
            return result;
        }
        public async Task CreateRole(RoleModel role)
        {
            var userOid = await GetUserOid(role.UserName);
            var project = await unitOfWork.Query<Project>().Where(c => c.Title == role.ProjectName).FirstOrDefaultAsync();

            role.ProjectId = project.Oid;
            role.UserId = userOid;

            Role.From(unitOfWork, role);

            await unitOfWork.CommitChangesAsync();
        }
        private List<Claim> GetClaims(ProjectUserModel user)
        {
            var claims = new List<Claim>();

            //if (user.Name == "StakeHolder")
            //{
            //    claims.AddRange(new List<Claim>
            //    {
            //        new Claim("user", "StakeHolder"),
            //        new Claim("role", "stakeHolderRole"),
            //        new Claim("oid", user.Oid.ToString(), ClaimValueTypes.Integer)
            //    });
            //}
            //else if (user.Name == "ProductOwner")
            //{
            //    claims.AddRange(new List<Claim>
            //    {
            //        new Claim("user", "ProjectOwner"),
            //        new Claim("role", "projectOwnerRole"),
            //        new Claim("oid", user.Oid.ToString(), ClaimValueTypes.Integer)
            //    });
            //}
            //else if (user.Name == "DevTeam")
            //{
            //    claims.AddRange(new List<Claim>
            //    {
            //        new Claim("user", "DevTeam"),
            //        new Claim("role", "devTeamRole"),
            //        new Claim("oid", user.Oid.ToString(), ClaimValueTypes.Integer)
            //    });
            //}
            //else
            //{
            //    claims.AddRange(new List<Claim>
            //    {
            //        new Claim("user", "unknown"),
            //        new Claim("role", "unknownRole"),
            //        new Claim("oid", user.Oid.ToString(), ClaimValueTypes.Integer)
            //    });
            //}

            claims.AddRange(new List<Claim>
            {
                new Claim("user", user.Name)
            });

            return claims;
        }

    }
}
