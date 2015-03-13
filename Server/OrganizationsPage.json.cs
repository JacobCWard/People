using System;
using System.Linq;
using Starcounter;
using Simplified.Ring2;

namespace People {
    partial class OrganizationsPage : Page {
        public OrganizationsProvider OrganizationsProvider = new OrganizationsProvider();
        public Action ConfirmAction = null;

        public void RefreshOrganizations() {
            this.Organizations = OrganizationsProvider.SelectOrganizations();
        }

        [OrganizationsPage_json.Organizations]
        partial class OrganizationsOrganizationPage : Page, IBound<Organization> {
            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = "Are you sure want to delete [" + this.Data.Name + "]?";
                this.ParentPage.ConfirmAction = () => {
                    Db.Transact(() => {
                        this.ParentPage.OrganizationsProvider.Delete(Data);
                        this.ParentPage.RefreshOrganizations();
                    });
                };
            }

            void Handle(Input.Edit Action) {
                this.ParentPage.RedirectUrl = "/launcher/workspace/people/organizations/" + this.Data.GetObjectID();
            }

            public OrganizationsPage ParentPage {
                get {
                    return this.Parent.Parent as OrganizationsPage;
                }
            }
        }

        [OrganizationsPage_json.Confirm]
        partial class OrganizationsConfirmPage : Page {
            void Cancel() {
                this.ParentPage.Confirm.Message = null;
                this.ParentPage.ConfirmAction = null;
            }

            void Handle(Input.Reject Action) {
                Cancel();
            }

            void Handle(Input.Ok Action) {
                if (this.ParentPage.ConfirmAction != null) {
                    this.ParentPage.ConfirmAction();
                }

                Cancel();
            }

            public OrganizationsPage ParentPage {
                get {
                    return this.Parent as OrganizationsPage;
                }
            }
        }
    }
}
