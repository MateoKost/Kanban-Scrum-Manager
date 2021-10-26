using SCRUM.Helpers.Authorization.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class RoleAccessModel
    {
        public int Oid { get; set; }
        public Permission Permission { get; set; }
        public RoleHeader Name { get; set; }

        public static RoleAccessModel From(RoleAccess role) => new RoleAccessModel()
        {
            Oid = role.Oid,
            Name = role.Name,
            Permission = role.Permission
        };

        public static IEnumerable From(List<RoleAccess> rolesList)
        {
            var list = new List<RoleAccessModel>();
            foreach (RoleAccess item in rolesList)
            {
                var obj = From(item);
                list.Add(obj);
            };
            return list;
        }
    }
}
