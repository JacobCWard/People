using Starcounter;

namespace People {
    [CompaniesPage_json]
    partial class CompaniesPage : Page {
    }

    [CompaniesPage_json.Companies]
    partial class CompaniesPageCompanies : Page, IBound<Company> {
        protected override string UriFragment {
            get {
                return "/launcher/workspace/people/companies/" + Data.GetObjectID();
            }
        }
    }
}