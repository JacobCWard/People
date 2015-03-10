using Starcounter;

namespace People {
    [CompanyPage_json]
    partial class CompanyPage : Page {
        void Handle(Input.Save Action) {
            Transaction.Commit();
            RedirectUrl = UrlHelper.GetUrl("/companies");
        }

        void Handle(Input.AddContact Action) {
            Transaction.Commit();
            RedirectUrl = "/launcher/workspace/people/contacts/add";
        }

        void Handle(Input.Cancel Action) {
            Transaction.Rollback();
            RedirectUrl = UrlHelper.GetUrl("/companies");
        }

        protected override string UriFragment {
            get {
                return "/launcher/workspace/people/companies/" + Data.GetObjectID();
            }
        }
    }
}