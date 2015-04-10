using Starcounter;
using Simplified.Ring3;

namespace People {
    [PhoneNumberPage_json]
    partial class PhoneNumberPage : Page, IBound<PhoneNumber> {
        public void RefreshPhoneNumber(string ID) {
            this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as PhoneNumber;
        }
    }
}
