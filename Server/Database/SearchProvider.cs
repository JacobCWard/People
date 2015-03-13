using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starcounter;
using Simplified.Ring2;

namespace People {
    public class SearchProvider {
        public QueryResultRows<Organization> SelectOrganizations(string Key, int Fetch) {
            if (string.IsNullOrEmpty(Key)) {
                return Db.SQL<Organization>("SELECT o FROM Simplified.Ring2.Organization o FETCH ?", Fetch);
            }

            Key = this.PrepareLikeKey(Key);

            return Db.SQL<Organization>("SELECT o FROM Simplified.Ring2.Organization o WHERE o.Name LIKE ? FETCH ?", Key, Fetch);
        }

        public QueryResultRows<Person> SelectPersons(string Key, int Fetch) {
            if (string.IsNullOrEmpty(Key)) {
                return Db.SQL<Person>("SELECT p FROM Simplified.Ring2.Person p FETCH ?", Fetch);
            }

            Key = this.PrepareLikeKey(Key);

            return Db.SQL<Person>(@"SELECT p
                FROM Simplified.Ring2.Person p
                WHERE p.Name LIKE ?
                FETCH ?", Key, Fetch);
        }

        protected string PrepareLikeKey(string Key) {
            return "%" + Key.Trim('%') + "%";
        }
    }
}
