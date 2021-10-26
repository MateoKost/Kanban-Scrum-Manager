using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Models
{
    public class Project : XPObject
    {
        public Project(Session session) : base(session) { }

        private string fTitle;
        public string Title
        {
            get { return fTitle; }
            set { SetPropertyValue(nameof(Title), ref fTitle, value); }
        }

        private string fDescription;
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue(nameof(Description), ref fDescription, value); }
        }

        private string fTag;
        public string Tag
        {
            get { return fTag; }
            set { SetPropertyValue(nameof(Tag), ref fTag, value); }
        }

        public static Project From(Session session, ProjectModel project) => new Project(session)
        {
            Oid = project.Oid,
            Title = project.Title,
            Description = project.Description,
            Tag = project.Tag
        };
    }
}
