/**
 * Ontology map lives in a separate file to emphasize that mapping  
 * should be done in an external PeopleMap.exe app or even on the fly
 */
using PolyjuiceNamespace;

namespace People {
    class OntologyMap {
        static void Register() {
            Polyjuice.OntologyMap("/people/persons/@w", "/so/person/@w", null, null);
            Polyjuice.OntologyMap("/people/organizations/@w", "/so/organization/@w", null, null);
        }
    }
}
