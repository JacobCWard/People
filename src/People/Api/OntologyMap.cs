using System;
using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;
using Simplified.Ring3;
using Simplified.Ring6;

namespace People {
    internal class OntologyMap : IHandlers {
        public void Register() {
            UriMapping.Map("/people/menu", UriMapping.MappingUriPrefix + "/menu");
            UriMapping.Map("/people/app-name", UriMapping.MappingUriPrefix + "/app-name");
            UriMapping.Map("/people/app-icon", UriMapping.MappingUriPrefix + "/app-icon");
            UriMapping.Map("/people/dashboard", UriMapping.MappingUriPrefix + "/dashboard");
            UriMapping.Map("/people/search?query=@w", UriMapping.MappingUriPrefix + "/search?query=@w");

            UriMapping.OntologyMap("/people/partials/persons/@w", typeof(Person).FullName, null, null);
            UriMapping.OntologyMap("/people/partials/organizations/@w", typeof(Organization).FullName, null, null);
            UriMapping.OntologyMap("/people/partials/addresses/@w", typeof(Address).FullName, null, null);

            UriMapping.OntologyMap("/people/partials/person-preview/@w", typeof(ChatAttachment).FullName, (string objectId) => {
                return objectId;
            }, (string objectId) => {
                Relation rel = DbHelper.FromID(DbHelper.Base64DecodeObjectID(objectId)) as Relation;

                if (rel.WhatIs != null && rel.WhatIs.GetType() == typeof(Person)) {
                    return rel.WhatIs.Key;
                }

                return null;
            });

            UriMapping.OntologyMap("/people/partials/organization-preview/@w", typeof(ChatAttachment).FullName, (string objectId) => {
                return objectId;
            }, (string objectId) => {
                Relation rel = DbHelper.FromID(DbHelper.Base64DecodeObjectID(objectId)) as Relation;

                if (rel.WhatIs != null && rel.WhatIs.GetType() == typeof(Organization)) {
                    return rel.WhatIs.Key;
                }

                return null;
            });
        }
    }
}
