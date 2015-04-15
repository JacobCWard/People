using System;
using System.IO;
using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    public class InitialData : IHandlers {
        public void Register() {
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

        public void Unload() {
            Db.Unload(@"F:\people.sql", 0, false);
        }

        public void ClearLayout() {
            Db.Transact(() => {
                Db.SlowSQL("DELETE FROM JuicyTiles.JuicyTilesSetup WHERE Key LIKE '/People/%'");
                //Db.SlowSQL("DELETE FROM JuicyTiles.JuicyTilesSetup");
            });
        }

        public void ApplyDefaultLayout() {
            TextReader treader = new StreamReader(typeof(InitialData).Assembly.GetManifestResourceStream("People.Content.default-layout.sql"));
            string sql = treader.ReadToEnd();

            treader.Dispose();
            this.ClearLayout();

            Db.Transact(() => {
                Db.SlowSQL(sql);
            });
        }
    }
}
