using Starcounter;

namespace People {
    public class SearchProvider {
        public QueryResultRows<Contact> SelectContacts(string Key, int Fetch) {
            if (string.IsNullOrEmpty(Key)) {
                return Db.SQL<Contact>("SELECT c FROM People.Contact c FETCH ?", Fetch);
            } else {
                Key = this.GetKey(Key);

                return Db.SQL<Contact>(@"SELECT c 
                                    FROM People.Contact c 
                                    WHERE Person.FirstName LIKE ? 
                                        OR Person.Surname LIKE ? 
                                        OR Title LIKE ? 
                                        OR Company.Organisation.Name LIKE ? 
                                        OR DefaultEmail LIKE ?
                                        OR DefaultPhone LIKE ?
                                    FETCH ?", Key, Key, Key, Key, Key, Key, Fetch);
            }
        }

        public QueryResultRows<Company> SelectCompanies(string Key, int Fetch) {
            if (string.IsNullOrEmpty(Key)) {
                return Db.SQL<Company>("SELECT c FROM People.Company c FETCH ?", Fetch);
            } else {
                Key = this.GetKey(Key);

                return Db.SQL<Company>("SELECT c FROM People.Company c WHERE Organisation.Name LIKE ? FETCH ?", Key, Fetch);
            }
        }

        protected string GetKey(string Key) {
            return Key = "%" + Key.Trim('%') + "%";
        }
    }
}
