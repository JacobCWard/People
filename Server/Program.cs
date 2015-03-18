using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Starcounter;
using PolyjuiceNamespace;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class Program : Page {
        static void Main() {
            InitialData data = new InitialData();
            
            data.Insert();
            RegisterPolyjucie();
            RegisterPartials();

            Handle.GET("/people/standalone", () => {
                Session session = Session.Current;

                if (session != null && session.Data != null)
                    return session.Data;

                var standalone = new StandalonePage();

                if (session == null) {
                    session = new Session(SessionOptions.PatchVersioning);
                    UrlHelper.BaseUrl = "/people";
                    standalone.Html = "/People/html/standalone.html";
                }

                standalone.Session = session;
                return standalone;
            });

            // Workspace home page (landing page from launchpad) dashboard alias
            Handle.GET("/people", () => {
                return X.GET("/people/persons");
            });

            Handle.GET("/people/organizations", () => {
                var master = (StandalonePage)X.GET("/people/standalone");
                if (!(master.CurrentPage is OrganizationsPage)) {
                    master.CurrentPage = GetLauncherPage("/people/partials/organizations");
                }
                return master;
            });

            Handle.GET("/people/organizations/add", () => {
                var master = (StandalonePage)X.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/organizations-add", true);
                return master;
            });

            Handle.GET("/people/organizations/{?}", (string id) => {
                var master = (StandalonePage)X.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/organizations/" + id, true);
                return master;
            });

            Handle.GET("/people/persons", () => {
                var master = (StandalonePage)X.GET("/people/standalone");
                if (!(master.CurrentPage is PersonsPage)) {
                    master.CurrentPage = GetLauncherPage("/people/partials/persons");
                }
                return master;
            });

            Handle.GET("/people/persons/add", () => {
                var master = (StandalonePage)X.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/persons-add", true);
                return master;
            });

            Handle.GET<string>("/people/persons/{?}", (string id) => {
                var master = (StandalonePage)X.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/persons/" + id, true);
                return master;
            });

            Handle.GET("/people/search?query={?}", (String query) => {
                var master = (StandalonePage)X.GET("/people/standalone");
                
                Response resp = X.GET("/People/partials/search/" + HttpUtility.UrlEncode(query));

                SearchPage page = (SearchPage)resp.Resource;

                if (page.Organizations.Count == 0 && page.Persons.Count == 0) {
                    master.CurrentPage = new Page();
                }
                else {
                    master.CurrentPage = resp;
                }

                return master;
            });

            OntologyMap.Register();
        }

        static void RegisterPolyjucie() {
            // App name required for Launchpad
            Handle.GET("/people/app-name", () => {
                return new AppName();
            });

            Handle.GET("/people/app-icon", () => {
                Page p = new Page() {
                    Html = "/People/html/app-icon.html"
                };

                return p;
            });

            Handle.GET("/people/menu", () => {
                Page p = new Page() {
                    Html = "/People/html/menu.html"
                };

                return p;
            });

            Handle.GET("/people/dashboard", () => {
                Page p = new Page() {
                    Html = "/People/html/dashboard.html"
                };

                return p;
            });
        }

        static void RegisterPartials() {
            Handle.GET("/people/partials/organizations", () => {
                OrganizationsPage p = new OrganizationsPage() {
                    Html = "/People/html/organizations.html"
                };

                p.RefreshOrganizations();

                return p;
            });

            Handle.GET("/people/partials/persons", () => {
                PersonsPage p = new PersonsPage() {
                    Html = "/People/html/persons.html"
                };

                p.RefreshPersons();

                return p;
            });

            Handle.GET("/people/partials/organizations-add", () => {
                return Db.Scope<OrganizationPage>(() => {
                    OrganizationPage page = new OrganizationPage() {
                        Html = "/People/html/organization.html"
                    };

                    page.RefreshOrganization();

                    return page;
                });
            });

            Handle.GET("/people/partials/organizations/{?}", (string id) => {
                return Db.Scope<OrganizationPage>(() => {
                    OrganizationPage page = new OrganizationPage() {
                        Html = "/People/html/organization.html"
                    };

                    page.RefreshOrganization(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/persons-add", () => {
                return Db.Scope<PersonPage>(() => {
                    PersonPage page = new PersonPage() {
                        Html = "/People/html/person.html"
                    };

                    page.RefreshPerson();

                    return page;
                });
            });

            Handle.GET<string>("/people/partials/persons/{?}", (string id) => {
                return Db.Scope<PersonPage>(() => {
                    PersonPage page = new PersonPage() {
                        Html = "/People/html/person.html"
                    };

                    page.RefreshPerson(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/organization-persons/{?}", (string id) => {
                return Db.Scope<OrganizationPersonPage>(() => {
                    OrganizationPersonPage page = new OrganizationPersonPage();

                    page.RefreshOrganizationPerson(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/address-relations/{?}", (string id) => {
                return Db.Scope<AddressRelationPage>(() => {
                    AddressRelationPage page = new AddressRelationPage() {
                        Html = "/People/html/address-relation.html"
                    };

                    page.RefreshAddressRelation(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/addresses/{?}", (string id) => {
                return Db.Scope<AddressPage>(() => {
                    AddressPage page = new AddressPage() {
                        Html = "/People/html/address.html"
                    };

                    page.RefreshAddress(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/email-address-relations/{?}", (string id) => {
                return Db.Scope<EmailAddressRelationPage>(() => {
                    EmailAddressRelationPage page = new EmailAddressRelationPage() {
                        Html = "/People/html/email-address-relation.html"
                    };

                    page.RefreshEmailAddressRelation(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/email-addresses/{?}", (string id) => {
                return Db.Scope<EmailAddressPage>(() => {
                    EmailAddressPage page = new EmailAddressPage() {
                        Html = "/People/html/email-address.html"
                    };

                    page.RefreshAddress(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/phone-number-relations/{?}", (string id) => {
                return Db.Scope<PhoneNumberRelationPage>(() => {
                    PhoneNumberRelationPage page = new PhoneNumberRelationPage() {
                        Html = "/People/html/phone-number-relation.html"
                    };

                    page.RefreshPhoneNumberRelation(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/phone-number/{?}", (string id) => {
                return Db.Scope<PhoneNumberPage>(() => {
                    PhoneNumberPage page = new PhoneNumberPage() {
                        Html = "/People/html/phone-number.html"
                    };

                    page.RefreshPhoneNumber(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/search/{?}", (String query) => {
                SearchPage page = new SearchPage() {
                    Html = "/People/html/search.html"
                };

                SearchProvider provider = new SearchProvider();
                int fetch = 5;

                foreach (Organization item in provider.SelectOrganizations(query, fetch)) {
                    page.Organizations.Add(X.GET<Json>("/people/partials/search-organization/" + item.Key));
                }

                foreach (Person item in provider.SelectPersons(query, fetch)) {
                    page.Persons.Add(X.GET<Json>("/people/partials/search-person/" + item.Key));
                }

                return page;
            });

            Handle.GET("/people/partials/search-organization/{?}", (String id) => {
                SearchOrganizationPage page = new SearchOrganizationPage() {
                    Html = "/People/html/search-organization.html"
                };

                page.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id)) as Organization;

                return page;
            });

            Handle.GET("/people/partials/search-person/{?}", (String id) => {
                SearchPersonPage page = new SearchPersonPage() {
                    Html = "/People/html/search-person.html"
                };

                page.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id)) as Person;

                return page;
            });
        }

        static Json GetLauncherPage(string Url, bool DbScope = false) {
            if (DbScope) {
                return Db.Scope(() => {
                    return X.GET<Json>(Url);
                });
            } else {
                return X.GET<Json>(Url);
            }
        }
    }
}
