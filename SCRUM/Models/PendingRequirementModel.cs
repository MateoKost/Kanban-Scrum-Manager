using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class PendingRequirementModel
    {
        public int Oid { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public string Status { get; set; }
        
        public static PendingRequirementModel From(PendingRequirement pendingRequirement, IMapper mapper)
        {
            return mapper.Map<PendingRequirementModel>(pendingRequirement);
        }
        public static IList From(IList<PendingRequirement> pendingRequirementsList, IMapper mapper)
        {
            var list = new List<PendingRequirementModel>();
            foreach (PendingRequirement item in pendingRequirementsList)
            {
                var obj = From(item, mapper);
                list.Add(obj);
            };
            return list;
        }
    }
}
