using Starcounter;
using Simplified.Ring2;

namespace People {
    partial class OrganizationPage : Page, IBound<Organization> {
        void Handle(Input.Save Action) {
            this.Transaction.Commit();
            this.GoBack();
        }

        void Handle(Input.Cancel Action) {
            //this.Transaction.Rollback();
            this.GoBack();
        }

        public void GoBack() {
            this.RedirectUrl = "/launcher/workspace/people/organizations";
        }

        public void RefreshOrganization(string ID) {
            this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as Organization;
        }
    }
}
