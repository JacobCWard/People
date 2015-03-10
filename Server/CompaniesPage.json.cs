using System;
using System.Linq;
using Starcounter;

namespace People {
    [CompaniesPage_json]
    partial class CompaniesPage : Page {
        public Action ConfirmAction;

        public void RefreshCompanies() {
            this.Companies = SQL<Company>("SELECT c FROM People.Company c");
        }
    }

    [CompaniesPage_json.Companies]
    partial class CompaniesPageCompanies : Page, IBound<Company> {
        protected override string UriFragment {
            get {
                return "/launcher/workspace/people/companies/" + Data.GetObjectID();
            }
        }

        void Handle(Input.Delete Action) {
            this.ParentView.Confirm.Message = "Are you sure want to delete [" + this.Data.Organisation.Name + "]?";
            this.ParentView.ConfirmAction = () => {
                Db.Transact(() => {
                    this.Data.Delete();
                });

                this.ParentView.RefreshCompanies();
            };
        }

        CompaniesPage ParentView {
            get {
                return this.Parent.Parent as CompaniesPage;
            }
        }
    }

    [CompaniesPage_json.Confirm]
    partial class CompaniesPageConfirm : Page {
        void Cancel() {
            this.ParentView.Confirm.Message = string.Empty;
            this.ParentView.ConfirmAction = null;
        }

        void Handle(Input.Ok Action) {
            if (this.ParentView.ConfirmAction != null) {
                this.ParentView.ConfirmAction();
            }

            this.Cancel();
        }

        void Handle(Input.Reject Action) {
            this.Cancel();
        }

        CompaniesPage ParentView {
            get {
                return this.Parent as CompaniesPage;
            }
        }
    }
}