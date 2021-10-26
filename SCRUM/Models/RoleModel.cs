using AutoMapper;
using SCRUM.Helpers.Authorization.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class RoleModel
    {
        public int Oid { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public RoleHeader Name { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; } 

        public static RoleModel From(Role role) => new RoleModel()
        {
            Oid = role.Oid,
            ProjectId = role.ProjectId,
            UserId = role.UserId,
            Name = role.Name
        };

        public static IEnumerable From(List<Role> rolesList)
        {
            var list = new List<RoleModel>();
            foreach (Role item in rolesList)
            {
                var obj = From(item);
                list.Add(obj);
            };
            return list;
        }
    }
}
