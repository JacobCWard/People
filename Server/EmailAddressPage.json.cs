using Starcounter;
using Simplified.Ring3;

namespace People {
    [EmailAddressPage_json]
    partial class EmailAddressPage : Page, IBound<EmailAddress> {
        public void RefreshAddress(string ID) {
            this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as EmailAddress;
        }
    }
}
