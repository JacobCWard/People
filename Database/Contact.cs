using System;
using System.Linq;
using Starcounter;

namespace People {
    [Database]
    public class Contact {
        public Company Company;
        public Concepts.Ring1.Person Person;
        public string Title;

        public QueryResultRows<ContactInfo> Infos {
            get {
                return Db.SQL<ContactInfo>("SELECT ci FROM ContactInfo ci WHERE ci.Contact = ?", this);
            }
        }

        public string DefaultEmail {
            get {
                return this.GetDefaultContact("Email");
            }
        }

        public string DefaultPhone {
            get {
                return this.GetDefaultContact("Phone");
            }
        }

        public string DefaultAddress {
            get {
                return this.GetDefaultContact("Address");
            }
        }

        public string GetDefaultContact(string TypeName) {
            ContactInfo info = this.Infos.Where(x => x.Type != null && x.Type.SysName == TypeName).OrderByDescending(x => x.Default).FirstOrDefault();

            return info != null ? info.Value : string.Empty;
        }

        public void SetDefaultContact(ContactInfo Info, bool Default) {
            if (!Default) {
                return;
            }

            Info.Default = true;

            foreach (ContactInfo ci in this.Infos) {
                if (ci.Equals(Info)) {
                    continue;
                }

                if (ci.Default && ci.Type.Equals(Info.Type)) {
                    ci.Default = false;
                }
            }
        }

        public void DeleteRelations() {
            foreach (ContactInfo info in this.Infos) {
                info.Delete();
            }
        }
    }
}
