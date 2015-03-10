using System;
using System.Linq;
using Starcounter;

namespace People {
    [Database]
    public class Company {
        public Concepts.Ring2.Organisation Organisation;
        public decimal Revenue;

        public QueryResultRows<Contact> Contacts {
            get {
                return Db.SQL<Contact>("SELECT c FROM Contact c WHERE c.Company = ?", this);
            }
        }
    }

    [Database]
    public class ContactInfo {
        public Contact Contact;
        public ContactInfoType Type;
        public ContactInfoRole Role;
        public string Value;
        public string Comment;
        public bool Default;
    }

    [Database]
    public class ContactInfoType {
        public string Name;
        public string SysName;
        public int SortNumber;
        public bool Deletable;
    }

    [Database]
    public class ContactInfoRole {
        public string Name;
        public string SysName;
        public int SortNumber;
        public bool Deletable;
    }
}
