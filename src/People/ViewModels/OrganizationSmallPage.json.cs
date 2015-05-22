using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class OrganizationSmallPage : Page, IBound<Organization> {
        public void RefreshOrganization(string ID = null) {
            Organization org = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as Organization;
            OrganizationsProvider provider = new OrganizationsProvider();

            this.Data = org;
            this.DefaultPerson.Data = provider.GetDefaultPerson(org);
            this.DefaultAddress.Data = provider.GetDefaultAddress(org);
            this.DefaultEmailAddress.Data = provider.GetDefaultEmailAddress(org);
            this.DefaultPhoneNumber.Data = provider.GetDefaultPhoneNumber(org);

            if (this.DefaultPerson.Data == null) {
                OrganizationPerson op = new OrganizationPerson() {
                    Organization = this.Data,
                    Person = new Person()
                };

                this.DefaultPerson.Data = op.Person;
            }

            if (this.DefaultAddress.Data == null) {
                AddressRelation ar = new AddressRelation() {
                    Somebody = this.Data,
                    Address = new Address()
                };

                this.DefaultAddress.Data = ar.Address;
            }

            if (this.DefaultPhoneNumber.Data == null) {
                PhoneNumberRelation pmr = new PhoneNumberRelation() {
                    Somebody = this.Data,
                    PhoneNumber = new PhoneNumber()
                };

                this.DefaultPhoneNumber.Data = pmr.PhoneNumber;
            }

            if (this.DefaultEmailAddress.Data == null) {
                EmailAddressRelation ear = new EmailAddressRelation() {
                    Somebody = this.Data,
                    EmailAddress = new EmailAddress()
                };

                this.DefaultEmailAddress.Data = ear.EmailAddress;
            }
        }

        [OrganizationSmallPage_json.DefaultAddress]
        partial class OrganizationSmallPageDefaultAddress : Page, IBound<Address> { 
        }

        [OrganizationSmallPage_json.DefaultPhoneNumber]
        partial class OrganizationSmallPageDefaultPhoneNumber : Page, IBound<PhoneNumber> { 
        }

        [OrganizationSmallPage_json.DefaultEmailAddress]
        partial class OrganizationSmallPageDefaultEmailAddress : Page, IBound<EmailAddress> { 
        }

        [OrganizationSmallPage_json.DefaultPerson]
        partial class OrganizationSmallPageDefaultPerson : Page, IBound<Person> { 
        }
    }
}
