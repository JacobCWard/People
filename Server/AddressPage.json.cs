using Starcounter;

namespace People {
    [AddressPage_json]
    partial class AddressPage : Page, IBound<Simplified.Ring3.Address> {
        public void RefreshAddress(string addressId) {
            var address = Db.SQL<Simplified.Ring3.Address>("SELECT a FROM Simplified.Ring3.Address a WHERE ObjectId = ?", addressId).First;
            this.Data = address;
        }
    }
}
