using Starcounter;

namespace People {
    [Database]
    public class Company {
        public Concepts.Ring2.Organisation Organisation;
        public decimal Revenue;
        public string LogoUrl;
    }

    [Database]
    public class Contact {
        public Company Company;
        public Concepts.Ring1.Person Person;
        public string Email;
        public string Title;
    }

}
