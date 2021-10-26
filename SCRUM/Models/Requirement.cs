using DevExpress.Xpo;
using System;

namespace SCRUM.Models
{
    public class Requirement : XPObject
    {
        public Requirement(Session session) : base(session) {}

        private int fProjectId;

        public int ProjectId
        {
            get { return fProjectId; }
            set { SetPropertyValue(nameof(ProjectId), ref fProjectId, value); }
        }

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

        private string fTouchstone;

        public string Touchstone
        {
            get { return fTouchstone; }
            set { SetPropertyValue(nameof(Touchstone), ref fTouchstone, value); }
        }

        private int fPriority;

        public int Priority
        {
            get { return fPriority; }
            set { SetPropertyValue(nameof(Priority), ref fPriority, value); }
        }

        private int fEffortfulness;

        public int Effortfulness
        {
            get { return fEffortfulness; }
            set { SetPropertyValue(nameof(Effortfulness), ref fEffortfulness, value); }
        }

        private string fStatus;

        public string Status
        {
            get { return fStatus; }
            set { SetPropertyValue(nameof(Status), ref fStatus, value); }
        }

        private int fIndex;

        public int Index
        {
            get { return fIndex; }
            set { SetPropertyValue(nameof(Index), ref fIndex, value); }
        }

        public static Requirement From(Session session, RequirementModel requirement) => new Requirement(session)
        {
            Oid = requirement.Oid,
            ProjectId = requirement.ProjectId,
            Title = requirement.Title,
            Description = requirement.Description,
            Touchstone = requirement.Touchstone,
            Priority = requirement.Priority,
            Effortfulness = requirement.Effortfulness,
            Status = requirement.Status,
            Index = requirement.Index
        };
    }

}