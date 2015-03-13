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
            InitializeData();
            RegisterPartials();

            // Workspace home page (landing page from launchpad) dashboard alias
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
                    Html = "/People/html/app-icon.html"
                };

                return iconpage;
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

            Handle.GET("/people/organizations", () => {
                OrganizationsPage p = new OrganizationsPage() {
                    Html = "/People/html/organizations.html"
                };

                p.RefreshOrganizations();

                return p;
            });

            Handle.GET("/people/organizations/add", () => {
                return Db.Scope(() => {
                    return X.GET<Json>("/people/partials/organizations-add");
                });
            });

            Handle.GET("/people/organizations/{?}", (string id) => {
                return Db.Scope(() => {
                    return X.GET<Json>("/people/partials/organizations/" + id);
                });
            });

            Handle.GET("/people/persons", () => {
                PersonsPage p = new PersonsPage() {
                    Html = "/People/html/persons.html"
                };

                p.RefreshPersons();

                return p;
            });

            Handle.GET("/people/persons/add", () => {
                return Db.Scope(() => {
                    return X.GET<Json>("/people/partials/persons-add");
                });
            });

            Handle.GET<string>("/people/persons/{?}", (string id) => {
                return Db.Scope(() => {
                    return X.GET<Json>("/people/partials/persons/" + id);
                });
            });

            Handle.GET("/people/search?query={?}", (String query) => {
                Response resp = X.GET("/People/partials/search/" + HttpUtility.UrlEncode(query));

                SearchPage page = (SearchPage)resp.Resource;

                if (page.Organizations.Count == 0 && page.Persons.Count == 0) {
                    return new Page();
                }

                return resp;
            });

            OntologyMap();
        }

        static void OntologyMap() {
            Polyjuice.Map("/people/menu", "/polyjuice/menu");
            Polyjuice.Map("/people/app-name", "/polyjuice/app-name");
            Polyjuice.Map("/people/app-icon", "/polyjuice/app-icon");
            Polyjuice.Map("/people/dashboard", "/polyjuice/dashboard");
            Polyjuice.Map("/people/search?query=@w", "/polyjuice/search?query=@w");
        }

        static void RegisterPartials() {
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

        static void InitializeData() {
            string[] defaultAddressTypes = new string[] { "Home", "Work" };
            string[] defaultEmailTypes = new string[] { "Primary", "Secondary", "Work", "Spam" };
            string[] defaultPhoneTypes = new string[] { "Mobile", "Home", "Work" };

            foreach (string t in defaultAddressTypes) {
                AddressRelationType type = Db.SQL<AddressRelationType>("SELECT t FROM Simplified.Ring3.AddressRelationType t WHERE t.Name = ?", t).First;

                if (type != null) {
                    continue;
                }

                Db.Transact(() => {
                    type = new AddressRelationType();
                    type.Name = t;
                });
            }

            foreach (string t in defaultEmailTypes) {
                EmailAddressRelationType type = Db.SQL<EmailAddressRelationType>("SELECT t FROM Simplified.Ring3.EmailAddressRelationType t WHERE t.Name = ?", t).First;

                if (type != null) {
                    continue;
                }

                Db.Transact(() => {
                    type = new EmailAddressRelationType();
                    type.Name = t;
                });
            }

            foreach (string t in defaultPhoneTypes) {
                PhoneNumberRelationType type = Db.SQL<PhoneNumberRelationType>("SELECT t FROM Simplified.Ring3.PhoneNumberRelationType t WHERE t.Name = ?", t).First;

                if (type != null) {
                    continue;
                }

                Db.Transact(() => {
                    type = new PhoneNumberRelationType();
                    type.Name = t;
                });
            }
        }
    }
}
