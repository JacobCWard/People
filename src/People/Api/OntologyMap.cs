using System;
using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;

namespace People {
    internal class OntologyMap : IHandlers {
        public void Register() {
            UriMapping.Map("/people/menu", UriMapping.MappingUriPrefix + "/menu");
            UriMapping.Map("/people/app-name", UriMapping.MappingUriPrefix + "/app-name");
            UriMapping.Map("/people/app-icon", UriMapping.MappingUriPrefix + "/app-icon");
            UriMapping.Map("/people/dashboard", UriMapping.MappingUriPrefix + "/dashboard");
            UriMapping.Map("/people/search?query=@w", UriMapping.MappingUriPrefix + "/search?query=@w");

            UriMapping.OntologyMap("/people/partials/persons/@w", "/so/person/@w", null, null);
            UriMapping.OntologyMap("/people/partials/organizations/@w", "/db/simplified.ring2.organization/@w", null, null);
            UriMapping.OntologyMap("/people/partials/addresses/@w", "/so/address/@w", null, null);

            UriMapping.OntologyMap("/people/partials/person-preview/@w", "/so/abstractcrossreference/@w", (string objectId) => {
                return objectId;
            }, (string objectId) => {
                Relation rel = DbHelper.FromID(DbHelper.Base64DecodeObjectID(objectId)) as Relation;

                if (rel.WhatIs != null && rel.WhatIs.GetType() == typeof(Person)) {
                    return rel.WhatIs.Key;
                }

                return null;
            });

            UriMapping.OntologyMap("/people/partials/organization-preview/@w", "/so/abstractcrossreference/@w", (string objectId) => {
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
