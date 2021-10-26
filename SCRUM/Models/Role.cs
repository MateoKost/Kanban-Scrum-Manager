using DevExpress.Xpo;
using SCRUM.Helpers.Authorization.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class Role : XPObject
    {
        public Role(Session session) : base(session) { }

        private int fProjectId;
        public int ProjectId
        {
            get { return fProjectId; }
            set { SetPropertyValue(nameof(ProjectId), ref fProjectId, value); }
        }

        private int fUserId;
        public int UserId
        {
            get { return fUserId; }
            set { SetPropertyValue(nameof(UserId), ref fUserId, value); }
        }

        private RoleHeader fName;
        public RoleHeader Name
        {
            get { return fName; }
            set { SetPropertyValue(nameof(Type), ref fName, value); }
        }

        public static Role From(Session session, RoleModel role) => new Role(session)
        {
            ProjectId = role.ProjectId,
            UserId = role.UserId,
            Name = role.Name
        };
    }
}
