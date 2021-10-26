using AutoMapper;
using DevExpress.Xpo;
using System;

namespace SCRUM.Models
{
    public class PendingRequirement : XPObject
    {
        public PendingRequirement(Session session) : base(session) {}

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

        private int fCreatorId;

        public int CreatorId
        {
            get { return fCreatorId; }
            set { SetPropertyValue(nameof(CreatorId), ref fCreatorId, value); }
        }

        private string fStatus;

        public string Status
        {
            get { return fStatus; }
            set { SetPropertyValue(nameof(Status), ref fStatus, value); }
        }

        public static PendingRequirement From(Session session, PendingRequirementModel pendingRequirement) => new PendingRequirement(session)
        {
            Oid = pendingRequirement.Oid,
            ProjectId = pendingRequirement.ProjectId,
            Title = pendingRequirement.Title,
            Description = pendingRequirement.Description,
            CreatorId = pendingRequirement.CreatorId,
            Status = pendingRequirement.Status
        };
    }

}