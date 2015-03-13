using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    public class ContactInfoProvider {
        public QueryResultRows<AddressRelationType> SelectAddressRelationTypes() {
            return Db.SQL<AddressRelationType>("SELECT t FROM Simplified.Ring3.AddressRelationType t");
        }

        public QueryResultRows<EmailAddressRelationType> SelectEmailAddressRelationTypes() {
            return Db.SQL<EmailAddressRelationType>("SELECT t FROM Simplified.Ring3.EmailAddressRelationType t");
        }

        public QueryResultRows<PhoneNumberRelationType> SelectPhoneNumberRelationTypes() {
            return Db.SQL<PhoneNumberRelationType>("SELECT t FROM Simplified.Ring3.PhoneNumberRelationType t");
        }

        public QueryResultRows<AddressRelation> SelectAddressRelations(Somebody Somebody) {
            return Db.SQL<AddressRelation>("SELECT r FROM Simplified.Ring3.AddressRelation r WHERE r.Somebody = ?", Somebody);
        }

        public QueryResultRows<EmailAddressRelation> SelectEmailAddressRelations(Somebody Somebody) {
            return Db.SQL<EmailAddressRelation>("SELECT r FROM Simplified.Ring3.EmailAddressRelation r WHERE r.Somebody = ?", Somebody);
        }

        public QueryResultRows<PhoneNumberRelation> SelectPhoneNumberRelations(Somebody Somebody) {
            return Db.SQL<PhoneNumberRelation>("SELECT r FROM Simplified.Ring3.PhoneNumberRelation r WHERE r.Somebody = ?", Somebody);
        }
    }
}
