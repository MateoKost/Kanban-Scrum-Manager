using DevExpress.Xpo;

namespace SCRUM.Models
{
    public class ProjectUser : XPObject
    {
        public ProjectUser(Session session) : base(session) { }
        
        private string fName;

        public string Name
        {
            get { return fName; }
            set { SetPropertyValue(nameof(Name), ref fName, value); }
        }

        public static ProjectUser From(Session session, ProjectUserModel user) => new ProjectUser(session)
        {
            Oid = user.Oid,
            Name = user.Name
        };
    }
}
