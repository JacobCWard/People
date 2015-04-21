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

        public QueryResultRows<OrganizationPerson> SelectOrganizationPersons(Organization Organization) {
            return Db.SQL<OrganizationPerson>("SELECT op FROM Simplified.Ring2.OrganizationPerson op WHERE op.Organization = ?", Organization);
        }

        public Address GetDefaultAddress(Organization Organization) {
            return Db.SQL<Address>("SELECT r.Address FROM Simplified.Ring3.AddressRelation r WHERE r.Somebody = ?", Organization).First;
        }

        public EmailAddress GetDefaultEmailAddress(Organization Organization) {
            return Db.SQL<EmailAddress>("SELECT r.EmailAddress FROM Simplified.Ring3.EmailAddressRelation r WHERE r.Somebody = ?", Organization).First;
        }

        public PhoneNumber GetDefaultPhoneNumber(Organization Organization) {
            return Db.SQL<PhoneNumber>("SELECT r.PhoneNumber FROM Simplified.Ring3.PhoneNumberRelation r WHERE r.Somebody = ?", Organization).First;
        }

        public Person GetDefaultPerson(Organization Organization) {
            OrganizationPerson op = this.SelectOrganizationPersons(Organization).First;

            if (op != null) {
                return op.Person;
            }

            return null;
        }
    }
}
