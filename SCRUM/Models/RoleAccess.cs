using DevExpress.Xpo;
using SCRUM.Helpers.Authorization.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class RoleAccess : XPObject
    {
        public RoleAccess(Session session) : base(session) { }

        private RoleHeader fName;
        public RoleHeader Name
        {
            get { return fName; }
            set { SetPropertyValue(nameof(Name), ref fName, value); }
        }

        private Permission fPermission;
        public Permission Permission
        {
            get { return fPermission; }
            set { SetPropertyValue(nameof(Permission), ref fPermission, value); }
        }
    }
}
