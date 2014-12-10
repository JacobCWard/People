using System;
using Starcounter;                                  // Most stuff relating to the database, JSON and communication is in this namespace
using Starcounter.Internal;
using Starcounter.Templates;
using System.Web;
using PolyjuiceNamespace;

[Master_json]                                       // This attribute tells Starcounter that the class corresponds to an object in the JSON-by-example file.
partial class Master : Page {
    /// <summary>
    /// Every application in Starcounter works like a console application. They have an .EXE ending. They have a Main() function and
    /// they can do console output. However, they are run inside the scope of a database rather than connecting to it.
    /// </summary>
    static void Main()
    {
        // App name required for Launchpad

        Handle.GET("/launcher/app-name", () =>
        {
            //return "SuperCRM";
            var json = new AppName();
            //json
            return json;
        }, HandlerOptions.ApplicationLevel);

        // App name required for Launchpad
        Handle.GET("/launcher/app-icon", () =>
        {
            var iconpage = new Page() { Html = "/SuperCRM/app-icon.html" };
            //json
            return iconpage;
        }, HandlerOptions.ApplicationLevel);

        Handle.GET("/supercrm/companies", () =>
        {
            var page = X.GET<Json>("/supercrm/partials/companies");
            // Master m = (Master)X.GET("/supercrm");
            // m.FavoriteCustomer = page;
            // return m;
            return page;
        });

        Handle.GET("/supercrm/companies/add", () =>
        {
            var page = X.GET<Json>("/supercrm/partials/companies-add");
            // Master m = (Master)X.GET("/supercrm");
            // m.FavoriteCustomer = page;
            // return m;
            return page;
        });

        Handle.GET("/supercrm/partials/companies-add", () =>
        {
            return Db.Scope<CompanyPage>(() => {
                CompanyPage page = new CompanyPage() {
                    Uri = "/launcher/workspace/supercrm/companies-add",
                    Html = "/company.html"
                };
                SuperCRM.Company_v2 company = new SuperCRM.Company_v2() {
                    Organisation = new Concepts.Ring2.Organisation()
                };
                page.Data = company;
                return page;
            });
        });

        Handle.GET("/supercrm/companies/{?}", (String companyId) =>
        {
            //var page = CompanyPage.GET("/supercrm/partials/companies/" + companyId);
            var page = X.GET<Json>("/supercrm/partials/companies/" + companyId);
            // Master m = (Master)X.GET("/supercrm");
            // m.FavoriteCustomer = page;
            // return m;
            return page;
        });
        Handle.GET("/supercrm/partials/companies/{?}", (String objectId) =>
        {
            return Db.Scope<CompanyPage>(() => {
                CompanyPage c = new CompanyPage() {
                    Html = "/company.html"
                };
                var company = SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c WHERE ObjectId = ?", objectId).First;
                c.Data = company;
                
                var contacts = SQL<SuperCRM.Contact_v2>("SELECT c FROM SuperCRM.Contact_v2 c WHERE Company = ?", company);
                var enumerator = contacts.GetEnumerator();
                while (enumerator.MoveNext()) {
                    var p = X.GET<Page>("/supercrm/partials/contacts/" + enumerator.Current.GetObjectID());
                    c.Contacts.Add(p);
                }
                return c;
            });
        });

        Handle.GET("/supercrm/partials/companies", () =>
        {
            CompaniesPage c = new CompaniesPage()
            {
                Html = "/companies.html"
            };
            var companies = SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c");
            c.Companies.Data = companies;

            return c;
        });

        Handle.GET("/supercrm/contacts/add", () =>
        {
            var page = X.GET<Json>("/supercrm/partials/contacts-add");
            // Master m = (Master)X.GET("/supercrm");
            // m.FavoriteCustomer = page;
            // /*if (m.AddContactToCompany.Data != null)
            // {
            //     ((SuperCRM.Contact)page.Data).Company = (SuperCRM.Company)m.AddContactToCompany.Data;
            //     m.AddContactToCompany.Data = null;
            // }*/
            // return m;
            return page;
        });

        Handle.GET("/supercrm/partials/contacts-add", () =>
        {
            return Db.Scope<Page>(() => {
                ContactPage page = new ContactPage() {
                    Html = "/contact.html",
                    Uri = "/supercrm/partials/contacts-add"
                };
                var companies = SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c");
                page.SelectedCompanyIndex = -1;
                
                var contact = new SuperCRM.Contact_v2() {
                    Person = new Concepts.Ring1.Person()
                };
                if (companies.First != null) {
                    contact.Company = companies.First;
                    page.SelectedCompanyIndex = 0;
                }
                page.Data = contact;
                page.Companies.Data = companies;

                /*page.Person.Transaction = new Transaction();
                page.Person.Transaction.Add(() =>
                {
                    var person = new SuperCRM.Person();
                    page.Person.Data = person;
                });

                page.Transaction = new Transaction();
                page.Transaction.Add(() =>
                {
                    var contact = new SuperCRM.Contact();
                    contact.Title = "Specialist";
                    contact.Person = page.Person.Data;
                    page.Data = contact;
                });*/

                return page;
            });
        });

        Handle.GET("/supercrm/contacts/{?}", (String objectId) =>
        {
            var page = X.GET<Json>("/supercrm/partials/contacts/" + objectId);
            // Master m = (Master)X.GET("/supercrm");
            // m.FavoriteCustomer = page;
            // return m;
            return page;
        });
        Handle.GET("/supercrm/partials/contacts/{?}", (String objectId) =>
        {
            return Db.Scope<Json>(() => {
                ContactPage page = new ContactPage() {
                    Html = "/contact.html"
                };
                var contact = SQL<SuperCRM.Contact_v2>("SELECT c FROM SuperCRM.Contact_v2 c WHERE ObjectId = ?", objectId).First;
                if (contact == null) {
                    //return empty response
                    return new Page() {
                        Html = ""
                    };
                }
                page.Data = contact;
                page.SelectedCompanyIndex = -1;
                var companies = SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c");
                page.Companies.Data = companies;
                var enumertator = companies.GetEnumerator();
                var i = 0;
                while (enumertator.MoveNext()) {
                    if (enumertator.Current.Equals(contact.Company)) {
                        page.SelectedCompanyIndex = i;
                        break;
                    }
                    i++;
                }
                return page;
            });
        });

        // Workspace home page (landing page from launchpad)
        // dashboard alias
        Handle.GET("/supercrm", ()=>{
            Response resp;
            X.GET("/launcher/dashboard", out resp, null, 0, HandlerOptions.ApplicationLevel);
            return resp;
        });

        Handle.GET("/launcher/dashboard", () => {
            Response resp;
            X.GET("/supercrm/partials/search/", out resp);
            return resp;
        }, HandlerOptions.ApplicationLevel);

        Handle.GET("/launcher/menu", () =>
        {
            var p = new Page() {
                Html = "/menu.html"
            };
            return p;
        }, HandlerOptions.ApplicationLevel);

        Handle.GET("/launcher/search?query={?}", (String query) =>
        {
            Response resp;
            X.GET("/supercrm/partials/search/" + HttpUtility.UrlEncode(query), out resp);
            return resp;
        }, HandlerOptions.ApplicationLevel);

        Handle.GET("/supercrm/partials/search/{?}", (String query) =>
        {
            SearchPage page = new SearchPage()
            {
                Html = "/search.html"
            };
            Rows<SuperCRM.Company_v2> companies;
            Rows<SuperCRM.Contact_v2> contacts;
            int count = 5;
            if (query == "")
            {
                companies = SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c FETCH ?", count);
                contacts = SQL<SuperCRM.Contact_v2>("SELECT c FROM SuperCRM.Contact_v2 c FETCH ?", count);
            }
            else
            {
                var wildcardQuery = "%" + query + "%";
                companies = SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c WHERE Organisation.Name LIKE ? FETCH ?", wildcardQuery, count);
                contacts = SQL<SuperCRM.Contact_v2>("SELECT c FROM SuperCRM.Contact_v2 c WHERE Person.FirstName LIKE ? OR Person.Surname LIKE ? OR Title LIKE ? OR Company.Organisation.Name LIKE ? FETCH ?", wildcardQuery, wildcardQuery, wildcardQuery, wildcardQuery, 5);
            }
            page.Companies.Data = companies;
            page.Contacts.Data = contacts;
            return page;
        });

        Handle.GET("/supercrm/delete-all-data", () =>
        {
            var m = new Master() {
                Html = "/SuperCRM/message.html"
            };
            Db.Transaction(() =>
            {
                SlowSQL("DELETE FROM SuperCRM.Company_v2");
                SlowSQL("DELETE FROM SuperCRM.Contact_v2");
                SlowSQL("DELETE FROM Concepts.Ring1.Person");
                SlowSQL("DELETE FROM Concepts.Ring2.Organisation");
            });
            m.Message = "SugarCRM's company and contact data was removed";
            return m;
        });

        Polyjuice.Map("/supercrm/partials/companies/@w", "/so/organization/@w",

            (String appObjectId) => {
                SuperCRM.Company_v2 company = Db.SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c WHERE c.ObjectId = ?", appObjectId).First;
                return company.Organisation.GetObjectID();
            },
            (String soObjectId) => {
                SuperCRM.Company_v2 company = Db.SQL<SuperCRM.Company_v2>("SELECT c FROM SuperCRM.Company_v2 c WHERE c.Organisation.ObjectId = ?", soObjectId).First;
                return company.GetObjectID();
            }
        );

        Polyjuice.Map("/supercrm/partials/contacts/@w", "/so/person/@w",

            (String appObjectId) => {
                SuperCRM.Contact_v2 contact = Db.SQL<SuperCRM.Contact_v2>("SELECT c FROM SuperCRM.Contact_v2 c WHERE c.ObjectId = ?", appObjectId).First;
                return contact.Person.GetObjectID();
            },
            (String soObjectId) => {
                SuperCRM.Contact_v2 contact = Db.SQL<SuperCRM.Contact_v2>("SELECT c FROM SuperCRM.Contact_v2 c WHERE c.Person.ObjectId = ?", soObjectId).First;
                return contact.GetObjectID();
            }
        );

    }
}




