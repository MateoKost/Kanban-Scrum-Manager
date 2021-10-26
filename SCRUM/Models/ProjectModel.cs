using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class ProjectModel
    {
        public int Oid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }

        public static ProjectModel From(Project project, IMapper mapper)
        {
            return mapper.Map<ProjectModel>(project);
        }

        public static IEnumerable From(List<Project> projectList, IMapper mapper)
        {
            var list = new List<ProjectModel>();
            foreach (Project item in projectList)
            {
                var obj = From(item, mapper);
                list.Add(obj);
            };
            return list;
        }
    }
}
