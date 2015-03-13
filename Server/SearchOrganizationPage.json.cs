using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class SearchOrganizationPage : Page, IBound<Organization> {
        protected override void OnData() {
            base.OnData();

            ContactInfoProvider cip = new ContactInfoProvider();
            AddressRelation ar = cip.SelectAddressRelations(this.Data).First;
            EmailAddressRelation ear = cip.SelectEmailAddressRelations(this.Data).First;
            PhoneNumberRelation pnr = cip.SelectPhoneNumberRelations(this.Data).First;

            this.AddressName = ar != null ? ar.Address.Name : null;
            this.EmailAddressName = ear != null ? ear.EmailAddress.Name : null;
            this.PhoneNumberName = pnr != null ? pnr.PhoneNumber.Name : null;
        }
    }
}
