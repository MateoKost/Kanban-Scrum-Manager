using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class RequirementModel
    {
        public int Oid { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Touchstone { get; set; }
        public int Priority { get; set; }
        public int Effortfulness { get; set; }
        public string Status { get; set; }
        public int Index { get; set; }

        public static RequirementModel From(Requirement requirement, IMapper mapper)
        {
            return mapper.Map<RequirementModel>(requirement);
        }
        public static IEnumerable From(List<Requirement> requirementList, IMapper mapper)
        {
            var list = new List<RequirementModel>();
            foreach (Requirement item in requirementList)
            {
                var obj = From(item, mapper);
                list.Add(obj);
            };
            return list;
        }
    }
}
