using System;
using System.Web;
using Starcounter;

namespace People {
    internal class MainHandlers : IHandlers {
        public void Register() {
            Handle.GET("/people/standalone", () => {
                Session session = Session.Current;

                if (session != null && session.Data != null)
                    return session.Data;

                var standalone = new StandalonePage();

                if (session == null) {
                    session = new Session(SessionOptions.PatchVersioning);
                    standalone.Html = "/People/viewmodels/StandalonePage.html";
                }

                standalone.Session = session;
                return standalone;
            });

            // Workspace home page (landing page from launchpad) dashboard alias
            Handle.GET("/people", () => {
                return Self.GET("/people/organizations");
            });

            Handle.GET("/people/organizations", () => {
                var master = (StandalonePage)Self.GET("/people/standalone");
                if (!(master.CurrentPage is OrganizationsPage)) {
                    master.CurrentPage = GetLauncherPage("/people/partials/organizations");
                }
                return master;
            });

            Handle.GET("/people/organizations/add", () => {
                var master = (StandalonePage)Self.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/organizations-add", true);
                return master;
            });

            Handle.GET("/people/organizations/{?}", (string id) => {
                var master = (StandalonePage)Self.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/organizations/" + id, true);
                return master;
            });

            Handle.GET("/people/persons", () => {
                var master = (StandalonePage)Self.GET("/people/standalone");
                if (!(master.CurrentPage is PersonsPage)) {
                    master.CurrentPage = GetLauncherPage("/people/partials/persons");
                }
                return master;
            });

            Handle.GET("/people/persons/add", () => {
                var master = (StandalonePage)Self.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/persons-add", true);
                return master;
            });

            Handle.GET<string>("/people/persons/{?}", (string id) => {
                var master = (StandalonePage)Self.GET("/people/standalone");
                master.CurrentPage = GetLauncherPage("/people/partials/persons/" + id, true);
                return master;
            });

            Handle.GET("/people/search?query={?}", (string query) => {
                var master = (StandalonePage)Self.GET("/people/standalone");

                Response resp = Self.GET("/People/partials/search/" + HttpUtility.UrlEncode(query));

                SearchPage page = (SearchPage)resp.Resource;

                if (page.Organizations.Count == 0 && page.Persons.Count == 0) {
                    master.CurrentPage = new Page();
                } else {
                    master.CurrentPage = resp;
                }

                return master;
            });

            Handle.GET("/people/unload", () => {
                InitialData data = new InitialData();

                data.Unload();

                return 200;
            });

            Handle.GET("/people/apply-default-layout", () => {
                DefaultStyles styles = new DefaultStyles();
                Page p = new Page() {
                    Html = "/People/viewmodels/layout/ApplyDefaultLayoutPage.html"
                };

                styles.Apply();

                return p;
            });

            Handle.GET("/people/clear-layout", () => {
                DefaultStyles styles = new DefaultStyles();
                Page p = new Page() {
                    Html = "/People/viewmodels/layout/ClearLayoutPage.html"
                };

                styles.Clear();

                return p;
            });

            Handle.GET("/people/layout", () => {
                Page p = new Page() {
                    Html = "/People/viewmodels/layout/LayoutPage.html"
                };
                
                return p;
            });
        }

        static Json GetLauncherPage(string Url, bool DbScope = false) {
            if (DbScope) {
                return Db.Scope(() => {
                    return Self.GET<Json>(Url);
                });
            } else {
                return Self.GET<Json>(Url);
            }
        }
    }
}
