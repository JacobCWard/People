/**
 * Ontology map lives in a separate file to emphasize that mapping  
 * should be done in an external PeopleMap.exe app or even on the fly
 */
using PolyjuiceNamespace;
using Starcounter;

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
        }
    }
}
