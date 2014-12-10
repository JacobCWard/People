using Starcounter;

[ContactPage_json]
partial class ContactPage : Page
{
    void Handle(Input.Save input)
    {
        Transaction.Commit();
        RedirectUrl = Uri;
    }

    void Handle(Input.SelectedCompanyIndex input)
    {
        var index = (int)input.Value;
        var company = Companies[index];
        ((SuperCRM.Contact_v2)Data).Company = (SuperCRM.Company_v2)company.Data;
    }

    protected override string UriFragment
    {
        get
        {
            return "/supercrm/contacts/" + Data.GetObjectID();
        }
    }
}
