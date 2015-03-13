using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class SearchPersonPage : Page, IBound<Person> {
        protected override void OnData() {
            base.OnData();

            AddressRelation ar = Db.SQL<AddressRelation>("SELECT ar FROM Simplified.Ring3.AddressRelation ar WHERE ar.Somebody = ?", this.Data).First;
            EmailAddressRelation ear = Db.SQL<EmailAddressRelation>("SELECT ar FROM Simplified.Ring3.EmailAddressRelation ar WHERE ar.Somebody = ?", this.Data).First;
            PhoneNumberRelation pnr = Db.SQL<PhoneNumberRelation>("SELECT ar FROM Simplified.Ring3.PhoneNumberRelation ar WHERE ar.Somebody = ?", this.Data).First;

            this.Address = ar != null ? ar.Address.Name : null;
            this.EmailAddress = ear != null ? ear.EmailAddress.Name : null;
            this.PhoneNumber = pnr != null ? pnr.PhoneNumber.Name : null;
        }
    }
}
