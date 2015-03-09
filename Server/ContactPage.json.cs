using Starcounter;

namespace People {
    [ContactPage_json]
    partial class ContactPage : Page {
        void Handle(Input.Save Action) {
            Transaction.Commit();
            RedirectUrl = UrlHelper.GetUrl();
        }

        void Handle(Input.Cancel Action) {
            Transaction.Rollback();
            RedirectUrl = UrlHelper.GetUrl();
        }

        void Handle(Input.SelectedCompanyIndex Action) {
            var index = (int)Action.Value;
            var company = Companies[index];
            ((Contact)Data).Company = (Company)company.Data;
        }

        protected override string UriFragment {
            get {
                return UrlHelper.GetUrl("/contacts/") + Data.GetObjectID();
            }
        }
    }
}