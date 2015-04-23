using System;
using System.Linq;
using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class OrganizationsPage : Page {
        public OrganizationsProvider OrganizationsProvider = new OrganizationsProvider();
        public ContactInfoProvider ContactInfoProvider = new ContactInfoProvider(); 
        public Action ConfirmAction = null;

        public void RefreshOrganizations() {
            this.Organizations = OrganizationsProvider.SelectOrganizations();
            this.AddUrl = "/people/organizations/add";
        }

        [OrganizationsPage_json.Organizations]
        partial class OrganizationsOrganizationPage : Page, IBound<Organization> {
            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = "Are you sure want to delete [" + this.Data.Name + "]?";
                this.ParentPage.ConfirmAction = () => {
                    Db.Transact(() => {
                        this.ParentPage.OrganizationsProvider.Delete(Data);
                        this.ParentPage.RefreshOrganizations();
                    });
                };
            }

            void Handle(Input.Edit Action) {
                this.ParentPage.RedirectUrl = "/people/organizations/" + this.Data.GetObjectID();
            }

            public OrganizationsPage ParentPage {
                get {
                    return this.Parent.Parent as OrganizationsPage;
                }
            }

            protected override void OnData() {
                base.OnData();

                ContactInfoProvider cip = this.ParentPage.ContactInfoProvider;
                Organization o = this.Data;

                OrganizationPerson op = this.ParentPage.OrganizationsProvider.SelectOrganizationPersons(o).First;
                AddressRelation a = cip.SelectAddressRelations(o).First;
                EmailAddressRelation ea = cip.SelectEmailAddressRelations(o).First;
                PhoneNumberRelation pn = cip.SelectPhoneNumberRelations(o).First;

                if (op != null) {
                    this.PersonName = op.Person.Name;
                }

                if (a != null) {
                    this.AddressName = a.Address.Name;
                }

                if (ea != null) {
                    this.EmailAddressName = ea.EmailAddress.Name;
                }

                if (pn != null) {
                    this.PhoneNumberName = pn.PhoneNumber.Name;
                }
            }
        }

        [OrganizationsPage_json.Confirm]
        partial class OrganizationsConfirmPage : Page {
            void Cancel() {
                this.ParentPage.Confirm.Message = null;
                this.ParentPage.ConfirmAction = null;
            }

            void Handle(Input.Reject Action) {
                Cancel();
            }

            void Handle(Input.Ok Action) {
                if (this.ParentPage.ConfirmAction != null) {
                    this.ParentPage.ConfirmAction();
                }

                Cancel();
            }

            public OrganizationsPage ParentPage {
                get {
                    return this.Parent as OrganizationsPage;
                }
            }
        }
    }
}
