using Starcounter;
using Simplified.Ring3;

namespace People {
    [AddressPage_json]
    partial class AddressPage : Page, IBound<Address> {
        public void RefreshAddress(string ID) {
            this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as Address;
        }
    }
}
