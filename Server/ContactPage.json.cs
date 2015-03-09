using Starcounter;

namespace People {
    [ContactPage_json]
    partial class ContactPage : Page {
        void Handle(Input.Save input) {
            input.Value = false;
            Transaction.Commit();
            RedirectUrl = Uri;
        }

        void Handle(Input.SelectedCompanyIndex input) {
            var index = (int)input.Value;
            var company = Companies[index];
            ((Contact)Data).Company = (Company)company.Data;
        }

        protected override string UriFragment {
            get {
                return "/launcher/workspace/people/contacts/" + Data.GetObjectID();
            }
        }
    }
}