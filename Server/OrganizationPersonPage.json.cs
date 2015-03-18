using System;
using Starcounter;
using Simplified.Ring2;

namespace People {
    [OrganizationPersonPage_json]
    partial class OrganizationPersonPage : Page, IBound<OrganizationPerson> {
        public event EventHandler Deleted;

        public void RefreshOrganizationPerson(string ID) {
            if (string.IsNullOrEmpty(ID)) {
                this.Data = new OrganizationPerson();
            } else {
                this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as OrganizationPerson;
            }

            this.EditUrl = UrlHelper.GetUrl("/persons/" + this.Data.Person.Key);
        }

        void Handle(Input.Delete Action) {
            IConfirmPage page = this.Parent.Parent as IConfirmPage;

            page.SetConfirmMessage("Are you sure want to delete person [" + this.Data.Person.Name + "]?");
            page.SetConfirmAction(() => {
                this.Data.Delete();
                this.OnDelete();
            });
        }

        void OnDelete() {
            if (this.Deleted != null) {
                this.Deleted(this, EventArgs.Empty);
            }
        }
    }
}
