using Starcounter;

namespace People {
    [SearchPage_json]
    partial class SearchPage : Page {
    }

    [SearchPage_json.Contacts]
    partial class SearchPageContacts : Page, IBound<Contact> {
        protected override string UriFragment {
            get {
                return UrlHelper.GetUrl("/contacts/") + Data.GetObjectID();
            }
        }
    }

    [SearchPage_json.Companies]
    partial class SearchPageCompanies : Page, IBound<Company> {
        protected override string UriFragment {
            get {
                return UrlHelper.GetUrl("/companies/") + Data.GetObjectID();
            }
        }
    }
}