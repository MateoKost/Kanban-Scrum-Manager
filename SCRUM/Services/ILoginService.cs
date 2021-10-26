using SCRUM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Services
{
    interface ILoginService
    {
        #region Testing only
        // For data initialization
        public Task<IEnumerable> GetUsers();

        #endregion

        public Task<IEnumerable> SignIn(ProjectUserModel user);
        public Task<IEnumerable> Register(ProjectUserModel user);
        public Task<int> GetUserOid(string name);
        public Task<IEnumerable> GetMyPermissions(string name);
        public Task CreateRole(RoleModel role);

    }
}
