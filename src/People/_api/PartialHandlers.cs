using Starcounter;
using PolyjuiceNamespace;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    internal class PartialHandlers : IHandlers {
        public void Register() {
            Handle.GET("/people/partials/organizations", () => {
                OrganizationsPage p = new OrganizationsPage() {
                    Html = "/People/viewmodels/OrganizationsPage.html"
                };

                p.RefreshOrganizations();

                return p;
            });

            Handle.GET("/people/partials/persons", () => {
                PersonsPage p = new PersonsPage() {
                    Html = "/People/viewmodels/PersonsPage.html"
                };

                p.RefreshPersons();

                return p;
            });

            Handle.GET("/people/partials/organizations-add", () => {
                return Db.Scope<OrganizationPage>(() => {
                    OrganizationPage page = new OrganizationPage() {
                        Html = "/People/viewmodels/OrganizationPage.html"
                    };

                    page.RefreshOrganization();

                    return page;
                });
            });

            Handle.GET("/people/partials/organizations/{?}", (string id) => {
                return Db.Scope<OrganizationPage>(() => {
                    OrganizationPage page = new OrganizationPage() {
                        Html = "/People/viewmodels/OrganizationPage.html"
                    };

                    page.RefreshOrganization(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/persons-add", () => {
                return Db.Scope<PersonPage>(() => {
                    PersonPage page = new PersonPage() {
                        Html = "/People/viewmodels/PersonPage.html"
                    };

                    page.RefreshPerson();

                    return page;
                });
            });

            Handle.GET<string>("/people/partials/persons/{?}", (string id) => {
                return Db.Scope<PersonPage>(() => {
                    PersonPage page = new PersonPage() {
                        Html = "/People/viewmodels/PersonPage.html"
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
                        Html = "/People/viewmodels/AddressRelationPage.html"
                    };

                    page.RefreshAddressRelation(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/addresses/{?}", (string id) => {
                return Db.Scope<AddressPage>(() => {
                    AddressPage page = new AddressPage() {
                        Html = "/People/viewmodels/AddressPage.html"
                    };

                    page.RefreshAddress(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/email-address-relations/{?}", (string id) => {
                return Db.Scope<EmailAddressRelationPage>(() => {
                    EmailAddressRelationPage page = new EmailAddressRelationPage() {
                        Html = "/People/viewmodels/EmailAddressRelationPage.html"
                    };

                    page.RefreshEmailAddressRelation(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/email-addresses/{?}", (string id) => {
                return Db.Scope<EmailAddressPage>(() => {
                    EmailAddressPage page = new EmailAddressPage() {
                        Html = "/People/viewmodels/EmailAddressPage.html"
                    };

                    page.RefreshAddress(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/phone-number-relations/{?}", (string id) => {
                return Db.Scope<PhoneNumberRelationPage>(() => {
                    PhoneNumberRelationPage page = new PhoneNumberRelationPage() {
                        Html = "/People/viewmodels/PhoneNumberRelationPage.html"
                    };

                    page.RefreshPhoneNumberRelation(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/phone-number/{?}", (string id) => {
                return Db.Scope<PhoneNumberPage>(() => {
                    PhoneNumberPage page = new PhoneNumberPage() {
                        Html = "/People/viewmodels/PhoneNumberPage.html"
                    };

                    page.RefreshPhoneNumber(id);

                    return page;
                });
            });

            Handle.GET("/people/partials/search/{?}", (string query) => {
                SearchPage page = new SearchPage() {
                    Html = "/People/viewmodels/SearchPage.html"
                };

                SearchProvider provider = new SearchProvider();
                int fetch = 5;

                foreach (Organization item in provider.SelectOrganizations(query, fetch)) {
                    page.Organizations.Add(Self.GET<Json>("/people/partials/search-organization/" + item.Key));
                }

                foreach (Person item in provider.SelectPersons(query, fetch)) {
                    page.Persons.Add(Self.GET<Json>("/people/partials/search-person/" + item.Key));
                }

                return page;
            });

            Handle.GET("/people/partials/search-organization/{?}", (string id) => {
                SearchOrganizationPage page = new SearchOrganizationPage() {
                    Html = "/People/viewmodels/SearchOrganizationPage.html"
                };

                page.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id)) as Organization;

                return page;
            });

            Handle.GET("/people/partials/search-person/{?}", (string id) => {
                SearchPersonPage page = new SearchPersonPage() {
                    Html = "/People/viewmodels/SearchPersonPage.html"
                };

                page.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id)) as Person;

                return page;
            });
        }
    }
}
