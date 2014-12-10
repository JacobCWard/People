using Starcounter;

[SearchPage_json]
partial class SearchPage : Page
{
}

[SearchPage_json.Contacts]
partial class SearchPageContacts : Page, IBound<SuperCRM.Contact_v2>
{
    protected override string UriFragment
    {
        get
        {
            return "/supercrm/contacts/" + Data.GetObjectID();
        }
    }
}

[SearchPage_json.Companies]
partial class SearchPageCompanies : Page, IBound<SuperCRM.Company_v2>
{
    protected override string UriFragment
    {
        get
        {
            return "/supercrm/companies/" + Data.GetObjectID();
        }
    }
}
