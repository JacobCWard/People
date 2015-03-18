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
            RegisterStandalone();

            // Workspace home page (landing page from launchpad) dashboard alias
            Handle.GET("/people", () => {
                Response resp = X.GET("/people/persons");
                return resp;
            });

            Handle.GET("/people/", () => {
                return X.GET("/people/persons");
            });

            Handle.GET("/people/organizations", () => {
                return GetLauncherPage("/people/partials/organizations");
            });

            Handle.GET("/people/organizations/add", () => {
                return GetLauncherPage("/people/partials/organizations-add", true);
            });

            Handle.GET("/people/organizations/{?}", (string id) => {
                return GetLauncherPage("/people/partials/organizations/" + id, true);
            });

            Handle.GET("/people/persons", () => {
                return GetLauncherPage("/people/partials/persons");
            });

            Handle.GET("/people/persons/add", () => {
                return GetLauncherPage("/people/partials/persons-add", true);
            });

            Handle.GET<string>("/people/persons/{?}", (string id) => {
                return GetLauncherPage("/people/partials/persons/" + id, true);
            });

            Handle.GET("/people/search?query={?}", (String query) => {
                Response resp = X.GET("/People/partials/search/" + HttpUtility.UrlEncode(query));

                SearchPage page = (SearchPage)resp.Resource;

                if (page.Organizations.Count == 0 && page.Persons.Count == 0) {
                    return new Page();
                }

                return resp;
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

            Handle.GET("/people/partials/address-relations/{?}", (string addrelId) => {
                return Db.Scope<AddressRelationPage>(() => {
                    AddressRelationPage page = new AddressRelationPage() {
                        Html = "/People/html/address-relation.html"
                    };

                    page.RefreshAddressRelation(addrelId);

                    return page;
                });
            });

            Handle.GET("/people/partials/addresses/{?}", (string addressId) => {
                return Db.Scope<AddressPage>(() => {
                    AddressPage page = new AddressPage() {
                        Html = "/People/html/address.html"
                    };

                    page.RefreshAddress(addressId);

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

        static void RegisterStandalone() {
            Handle.GET("/people/standalone/master", (Request r) => {
                Session session = Session.Current;
                UrlHelper.BaseUrl = "/people/standalone";

                if (session == null) {
                    session = new Session(SessionOptions.PatchVersioning);
                }

                if (session.Data != null) {
                    return session.Data;
                }

                StandalonePage page = new StandalonePage() {
                    Html = "/People/html/standalone.html"
                };
                page.Session = session;

                return page;
            });

            Handle.GET("/people/standalone", (Request r) => {
                return GetStandalonePage("/people/partials/persons");
            });

            Handle.GET("/people/standalone/persons", (Request r) => {
                return GetStandalonePage("/people/partials/persons");
            });

            Handle.GET("/people/standalone/persons/add", () => {
                return GetStandalonePage("/people/partials/persons-add", true);
            });

            Handle.GET<string>("/people/standalone/persons/{?}", (string id) => {
                return GetStandalonePage("/people/partials/persons/" + id, true);
            });

            Handle.GET("/people/standalone/organizations", (Request r) => {
                return GetStandalonePage("/people/partials/organizations");
            });

            Handle.GET("/people/standalone/organizations/add", () => {
                return GetStandalonePage("/people/partials/organizations-add", true);
            });

            Handle.GET<string>("/people/standalone/organizations/{?}", (string id) => {
                return GetStandalonePage("/people/partials/organizations/" + id, true);
            });
        }

        static StandalonePage GetStandalonePage(string CurrentPageUrl, bool DbScope = false) {
            StandalonePage master = null;

            if (DbScope) {
                Db.Scope(() => {
                    master = X.GET<StandalonePage>("/people/standalone/master");
                    master.CurrentPage = X.GET<Json>(CurrentPageUrl);
                });
            } else {
                master = X.GET<StandalonePage>("/people/standalone/master");
                master.CurrentPage = X.GET<Json>(CurrentPageUrl);
            }

            return master;
        }

        static Json GetLauncherPage(string Url, bool DbScope = false) {
            UrlHelper.BaseUrl = "/launcher/workspace/people";

            if (DbScope) {
                return Db.Scope<Json>(() => {
                    return X.GET<Json>(Url);
                });
            } else {
                return X.GET<Json>(Url);
            }
        }
    }
}
