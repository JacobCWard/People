using Starcounter;

[CompanyPage_json]
partial class CompanyPage : Page
{
    void Handle(Input.Save input)
    {
        input.Value = false;
        Transaction.Commit();
        RedirectUrl = Uri;
    }

    void Handle(Input.AddContact input)
    {    
        input.Value = false;
        Transaction.Commit();
        RedirectUrl = "/launcher/workspace/supercrm/contacts/add";
    }

    protected override string UriFragment
    {
        get
        {
            return "/launcher/workspace/supercrm/companies/" + Data.GetObjectID();
        }
    }
}
