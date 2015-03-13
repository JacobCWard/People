using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    public class PersonsProvider {
        public QueryResultRows<Person> SelectPersons() {
            return Db.SQL<Person>("SELECT p FROM Simplified.Ring2.Person p ORDER By p.Name");
        }

        public void Delete(Person Person) {
            ContactInfoProvider provider = new ContactInfoProvider();

            foreach (AddressRelation item in provider.SelectAddressRelations(Person)) {
                item.Address.Delete();
                item.Delete();
            }

            foreach (EmailAddressRelation item in provider.SelectEmailAddressRelations(Person)) {
                item.EmailAddress.Delete();
                item.Delete();
            }

            foreach (PhoneNumberRelation item in provider.SelectPhoneNumberRelations(Person)) {
                item.PhoneNumber.Delete();
                item.Delete();
            }

            Person.Delete();
        }
    }
}
