using Starcounter;

[CompaniesPage_json]
partial class CompaniesPage : Page
{
}

[CompaniesPage_json.Companies]
partial class CompaniesPageCompanies : Page, IBound<SuperCRM.Company_v2>
{
    protected override string UriFragment
    {
        get
        {
            return "/supercrm/companies/" + Data.GetObjectID();
        }
    }
}
