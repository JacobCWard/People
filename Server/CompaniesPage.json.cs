using Starcounter;

[CompaniesPage_json]
partial class CompaniesPage : Page
{
}

[CompaniesPage_json.Companies]
partial class CompaniesPageCompanies : Page, IBound<SuperCRM.Company>
{
    protected override string UriFragment
    {
        get
        {
            return "/launcher/workspace/supercrm/companies/" + Data.GetObjectID();
        }
    }
}
