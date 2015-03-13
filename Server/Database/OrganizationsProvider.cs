using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    public class OrganizationsProvider {
        public QueryResultRows<Organization> SelectOrganizations() {
            return Db.SQL<Organization>("SELECT o FROM Simplified.Ring2.Organization o ORDER BY o.Name");
        }

        public void Delete(Organization Organization) {
            ContactInfoProvider provider = new ContactInfoProvider();

            foreach (AddressRelation item in provider.SelectAddressRelations(Organization)) {
                item.Address.Delete();
                item.Delete();
            }

            foreach (EmailAddressRelation item in provider.SelectEmailAddressRelations(Organization)) {
                item.EmailAddress.Delete();
                item.Delete();
            }

            foreach (PhoneNumberRelation item in provider.SelectPhoneNumberRelations(Organization)) {
                item.PhoneNumber.Delete();
                item.Delete();
            }

            Organization.Delete();
        }
    }
}
