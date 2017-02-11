using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.Common
{
    public static class EntityAttributes
    {
        public static class SolutionEntityAttributes
        {
            public const string EntityName = "solution";
            public const string IdFieldName = "solutionid";
            public const string VersionFieldName = "version";
            public const string FriendlyNameFieldName = "friendlyname";
            public const string UniqueNameFieldName = "uniquename";
            public const string ParentSolutionIdFieldName = "parentsolutionid";
            public const string IsManagedFieldName = "ismanaged";
            public const string PublisherId = "publisherid";
            public const string Description = "description";
        }

        public static class SolutionComponentEntityAttributes
        {
            public const string EntityName = "solutioncomponent";
            public const string ComponentType = "componenttype";
            public const string ObjectId = "objectid";
            public const string SolutionComponentId = "solutioncomponentid";
            public const string SolutionId = "solutionid";
            public const string IsMetadata = "ismetadata";
            public const string RootComponentBehavior = "rootcomponentbehavior";
        }

        public static class PublisherEntityAttributes
        {
            public const string EntityName = "publisher";
            public const string FriendlyName = "friendlyname";
            public const string UniqueName = "uniquename";
        }
    }
}
