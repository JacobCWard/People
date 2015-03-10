using Starcounter;

namespace People {
    [ContactPage_json]
    partial class ContactPage : Page {
        void Handle(Input.Save Action) {
            Transaction.Commit();
            RedirectUrl = UrlHelper.GetUrl("/contacts");
        }

        void Handle(Input.Cancel Action) {
            //Transaction.Rollback();
            RedirectUrl = UrlHelper.GetUrl("/contacts");
        }

        void Handle(Input.SelectedCompanyIndex Action) {
            int index = (int)Action.Value;
            Company company = Companies[index].Data as Company;

            ((Contact)Data).Company = (Company)company;
        }

        protected override string UriFragment {
            get {
                return UrlHelper.GetUrl("/contacts/") + Data.GetObjectID();
            }
        }
    }
}