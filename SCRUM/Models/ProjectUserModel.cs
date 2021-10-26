using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class ProjectUserModel
    {
        public int Oid { get; set; }
        public string Name { get; set; }

        public static ProjectUserModel From(ProjectUser user, IMapper mapper)
        {
            return mapper.Map<ProjectUserModel>(user);
        }

        public static IEnumerable From(List<ProjectUser> userList, IMapper mapper)
        {
            var list = new List<ProjectUserModel>();
            foreach (ProjectUser item in userList)
            {
                var obj = From(item, mapper);
                list.Add(obj);
            };
            return list;
        }
    }
}
