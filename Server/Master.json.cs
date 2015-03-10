using System;
using System.Web;
using System.Diagnostics;
using System.Linq;
using Starcounter;                                  // Most stuff relating to the database, JSON and communication is in this namespace
using Starcounter.Internal;
using Starcounter.Templates;
using PolyjuiceNamespace;

namespace People {
    [Master_json]                                       // This attribute tells Starcounter that the class corresponds to an object in the JSON-by-example file.
    partial class Master : Page {
        /// <summary>
        /// Every application in Starcounter works like a console application. They have an .EXE ending. They have a Main() function and
        /// they can do console output. However, they are run inside the scope of a database rather than connecting to it.
        /// </summary>
        static void Main() {
            RegisterPartials();

            // Workspace home page (landing page from launchpad)
            // dashboard alias
            Handle.GET("/people", () => {
                Response resp = X.GET("/people/dashboard");
                return resp;
            });

            Handle.GET("/people/", () => {
                Response resp = X.GET("/people/dashboard");
                return resp;
            });

            // App name required for Launchpad
            Handle.GET("/people/app-name", () => {
                return new AppName();
            });

            Handle.GET("/people/app-icon", () => {
                Page iconpage = new Page() {
                    Html = "/People/app-icon.html"
                };

                return iconpage;
            });

            Handle.GET("/people/menu", () => {
                var p = new Page() {
                    Html = "/menu.html"
                };
                return p;
            });

            Handle.GET("/people/dashboard", () => {
                Response resp = X.GET("/People/partials/search/");

                return resp;
            });

            Handle.GET("/people/companies", () => {
                var page = X.GET<Json>("/people/partials/companies");
                return page;
            });

            Handle.GET("/people/companies/add", () => {
                var page = X.GET<Json>("/people/partials/companies-add");
                return page;
            });

            Handle.GET("/people/companies/{?}", (String companyId) => {
                var page = X.GET<Json>("/People/partials/companies/" + companyId);
                return page;
            });

            Handle.GET("/people/contacts", () => {
                var page = X.GET<Json>("/people/partials/contacts");
                return page;
            });

            Handle.GET("/people/contacts/add", () => {
                var page = X.GET<Json>("/people/partials/contacts-add");
                return page;
            });

            Handle.GET("/people/contacts/{?}", (String objectId) => {
                var page = X.GET<Json>("/People/partials/contacts/" + objectId);
                return page;
            });

            Handle.GET("/people/search?query={?}", (String query) => {
                Response resp = X.GET("/People/partials/search/" + HttpUtility.UrlEncode(query));

                var page = (SearchPage)resp.Resource;
                if (page.Companies.Count == 0 && page.Contacts.Count == 0) {
                    //no results
                    return new Page() { };
                }

                return resp;
            });

            Handle.GET("/people/delete-all-data", () => {
                Master m = new Master() {
                    Html = "/People/message.html"
                };
                Db.Transact(() => {
                    SlowSQL("DELETE FROM People.Company");
                    SlowSQL("DELETE FROM People.Contact");
                    SlowSQL("DELETE FROM Concepts.Ring1.Person");
                    SlowSQL("DELETE FROM Concepts.Ring2.Organisation");
                });
                m.Message = "SugarCRM's company and contact data was removed";
                return m;
            });

            OntologyMap();
            InitializeData();
        }

        static void OntologyMap() {
            Polyjuice.OntologyMap("/people/partials/companies/@w", "/so/organization/@w",

                (String appObjectId) => {
                    Company company = Db.SQL<Company>("SELECT c FROM People.Company c WHERE c.ObjectId = ?", appObjectId).First;
                    return company.Organisation.GetObjectID();
                },
                (String soObjectId) => {
                    Company company = Db.SQL<Company>("SELECT c FROM People.Company c WHERE c.Organisation.ObjectId = ?", soObjectId).First;
                    return company.GetObjectID();
                }
            );

            Polyjuice.OntologyMap("/people/partials/contacts/@w", "/so/person/@w",

                (String appObjectId) => {
                    Contact contact = Db.SQL<Contact>("SELECT c FROM People.Contact c WHERE c.ObjectId = ?", appObjectId).First;
                    return contact.Person.GetObjectID();
                },
                (String soObjectId) => {
                    Contact contact = Db.SQL<Contact>("SELECT c FROM People.Contact c WHERE c.Person.ObjectId = ?", soObjectId).First;
                    return contact.GetObjectID();
                }
            );

            Polyjuice.Map("/people/menu", "/polyjuice/menu");
            Polyjuice.Map("/people/app-name", "/polyjuice/app-name");
            Polyjuice.Map("/people/app-icon", "/polyjuice/app-icon");
            Polyjuice.Map("/people/dashboard", "/polyjuice/dashboard");
            Polyjuice.Map("/people/search?query=@w", "/polyjuice/search?query=@w");
        }

        static void RegisterPartials() {
            Handle.GET("/people/partials/companies", () => {
                CompaniesPage c = new CompaniesPage() {
                    Html = "/companies.html"
                };

                c.RefreshCompanies();

                return c;
            });

            Handle.GET("/people/partials/companies-add", () => {
                return Db.Scope<CompanyPage>(() => {
                    CompanyPage page = new CompanyPage() {
                        Uri = "/launcher/workspace/people/companies-add",
                        Html = "/company.html"
                    };
                    Company company = new Company() {
                        Organisation = new Concepts.Ring2.Organisation()
                    };
                    page.Data = company;
                    return page;
                });
            });

            Handle.GET("/people/partials/companies/{?}", (String objectId) => {
                return Db.Scope<CompanyPage>(() => {
                    CompanyPage c = new CompanyPage() {
                        Html = "/company.html"
                    };

                    var company = Db.SQL<Company>("SELECT c FROM People.Company c WHERE ObjectId = ?", objectId).First;
                    c.Data = company;

                    return c;
                });
            });

            Handle.GET("/people/partials/contacts", () => {
                ContactsPage c = new ContactsPage() {
                    Html = "/contacts.html"
                };

                c.RefreshContacts();

                return c;
            });

            Handle.GET("/people/partials/contacts-add", () => {
                return Db.Scope<ContactPage>(() => {
                    ContactPage page = new ContactPage() {
                        Uri = "/launcher/workspace/people/contacts-add",
                        Html = "/contact.html"
                    };

                    Contact contact = new Contact() {
                        Person = new Concepts.Ring1.Person(),
                        Company = Db.SQL<Company>("SELECT c FROM People.Company c").First
                    };
                
                    page.RefreshContact(contact);

                    return page;
                });
            });

            Handle.GET("/people/partials/contacts/{?}", (String objectId) => {
                return Db.Scope<Page>(() => {
                    ContactPage page = new ContactPage() {
                        Html = "/contact.html"
                    };
                    
                    Contact contact = Db.SQL<Contact>("SELECT c FROM People.Contact c WHERE ObjectId = ?", objectId).First;
                    
                    if (contact == null) {
                        //return empty response
                        return new Page() {
                            Html = ""
                        };
                    }

                    page.RefreshContact(contact);

                    return page;
                });
            });

            Handle.GET("/people/partials/search/{?}", (String query) => {
                SearchPage page = new SearchPage() {
                    Html = "/search.html"
                };
                SearchProvider provider = new SearchProvider();
                int fetch = 5;

                page.Companies.Data = provider.SelectCompanies(query, fetch);
                page.Contacts.Data = provider.SelectContacts(query, fetch);

                return page;
            });
        }

        static void InitializeData() {
            string[] defaultContactInfoTypes = new string[] { "Email", "Phone", "Address" };
            string[] defaultContactInfoRoles = new string[] { "Personal", "Home", "Work" };

            for (int i = 0; i < defaultContactInfoTypes.Length; i++) {
                string t = defaultContactInfoTypes[i];
                ContactInfoType cit = Db.SQL<ContactInfoType>("SELECT cit FROM ContactInfoType cit WHERE cit.SysName = ?", t).First;

                if (cit == null) {
                    Db.Transact(() => {
                        cit = new ContactInfoType();
                        cit.Name = cit.SysName = t;
                        cit.Deletable = false;
                        cit.SortNumber = i;
                    });
                }
            }

            for (int i = 0; i < defaultContactInfoRoles.Length; i++) {
                string r = defaultContactInfoRoles[i];
                ContactInfoRole cir = Db.SQL<ContactInfoRole>("SELECT cir FROM ContactInfoRole cir WHERE cir.SysName = ?", r).First;

                if (cir == null) {
                    Db.Transact(() => {
                        cir = new ContactInfoRole();
                        cir.Name = cir.SysName = r;
                        cir.Deletable = false;
                        cir.SortNumber = i;
                    });
                }
            }
        }
    }
}