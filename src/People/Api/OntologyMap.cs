using System;
using PolyjuiceNamespace;
using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;

namespace People {
    internal class OntologyMap : IHandlers {
        public void Register() {
            Polyjuice.Map("/people/menu", "/polyjuice/menu");
            Polyjuice.Map("/people/app-name", "/polyjuice/app-name");
            Polyjuice.Map("/people/app-icon", "/polyjuice/app-icon");
            Polyjuice.Map("/people/dashboard", "/polyjuice/dashboard");
            Polyjuice.Map("/people/search?query=@w", "/polyjuice/search?query=@w");

            Polyjuice.OntologyMap("/people/partials/persons/@w", "/so/person/@w", null, null);
            Polyjuice.OntologyMap("/people/partials/organizations/@w", "/so/organization/@w", null, null);
            Polyjuice.OntologyMap("/people/partials/addresses/@w", "/so/address/@w", null, null);

            Polyjuice.OntologyMap("/people/partials/person-preview/@w", "/so/abstractcrossreference/@w", (string objectId) => {
                return objectId;
            }, (string objectId) => {
                Relation rel = DbHelper.FromID(DbHelper.Base64DecodeObjectID(objectId)) as Relation;

                if (rel.WhatIs != null && rel.WhatIs.GetType() == typeof(Person)) {
                    return rel.WhatIs.Key;
                }

                return null;
            });

            Polyjuice.OntologyMap("/people/partials/organization-preview/@w", "/so/abstractcrossreference/@w", (string objectId) => {
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
