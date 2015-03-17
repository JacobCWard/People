using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    public class InitialData {
        public void Insert() {
            string[] defaultAddressTypes = new string[] { "Home", "Work" };
            string[] defaultEmailTypes = new string[] { "Primary", "Secondary", "Work", "Spam" };
            string[] defaultPhoneTypes = new string[] { "Mobile", "Home", "Work" };

            foreach (string t in defaultAddressTypes) {
                AddressRelationType type = Db.SQL<AddressRelationType>("SELECT t FROM Simplified.Ring3.AddressRelationType t WHERE t.Name = ?", t).First;

                if (type != null) {
                    continue;
                }

                Db.Transact(() => {
                    type = new AddressRelationType();
                    type.Name = t;
                });
            }

            foreach (string t in defaultEmailTypes) {
                EmailAddressRelationType type = Db.SQL<EmailAddressRelationType>("SELECT t FROM Simplified.Ring3.EmailAddressRelationType t WHERE t.Name = ?", t).First;

                if (type != null) {
                    continue;
                }

                Db.Transact(() => {
                    type = new EmailAddressRelationType();
                    type.Name = t;
                });
            }

            foreach (string t in defaultPhoneTypes) {
                PhoneNumberRelationType type = Db.SQL<PhoneNumberRelationType>("SELECT t FROM Simplified.Ring3.PhoneNumberRelationType t WHERE t.Name = ?", t).First;

                if (type != null) {
                    continue;
                }

                Db.Transact(() => {
                    type = new PhoneNumberRelationType();
                    type.Name = t;
                });
            }
        }
    }
}
