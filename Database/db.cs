using Starcounter;

namespace SuperCRM
{
    [Database]
    public class Company_v2
    {
        public Concepts.Ring2.Organisation Organisation;
        public decimal Revenue;
        public string LogoUrl;
    }

    [Database]
    public class Contact_v2
    {
        public Company_v2 Company;
        public Concepts.Ring1.Person Person;
        public string Email;
        public string Title;
    }

}
