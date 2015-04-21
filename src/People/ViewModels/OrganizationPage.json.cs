using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class OrganizationPage : Page, IBound<Organization>, IConfirmPage {
        protected ContactInfoProvider contactInfoProvider = new ContactInfoProvider();
        protected OrganizationsProvider organizationsProvider = new OrganizationsProvider();
        public Action ConfirmAction = null;

        void Handle(Input.Save Action) {
            this.Transaction.Commit();
            this.GoBack();
        }

        void Handle(Input.Cancel Action) {
            //this.Transaction.Rollback();
            this.GoBack();
        }

        void Handle(Input.AddAddress Action) {
            Address a = new Address();
            AddressRelation ar = new AddressRelation();

            ar.Somebody = this.Data;
            ar.Address = a;
            ar.AddressRelationType = contactInfoProvider.SelectAddressRelationTypes().First;

            this.RefreshAddresses();
        }

        void Handle(Input.AddEmailAddress Action) {
            EmailAddress ea = new EmailAddress();
            EmailAddressRelation ear = new EmailAddressRelation();

            ear.Somebody = this.Data;
            ear.EmailAddress = ea;
            ear.EmailAddressRelationType = contactInfoProvider.SelectEmailAddressRelationTypes().First;

            this.RefreshEmailAddresses();
        }

        void Handle(Input.AddPhoneNumber Action) {
            PhoneNumber pn = new PhoneNumber();
            PhoneNumberRelation pnr = new PhoneNumberRelation();

            pnr.Somebody = this.Data;
            pnr.PhoneNumber = pn;
            pnr.PhoneNumberRelationType = contactInfoProvider.SelectPhoneNumberRelationTypes().First;

            this.RefreshPhoneNumbers();
        }

        public void GoBack() {
            this.RedirectUrl = UrlHelper.GetUrl("/organizations");
        }

        public void RefreshOrganization(string ID = null) {
            this.AddPersonUrl = UrlHelper.GetUrl("/persons/add");

            if (string.IsNullOrEmpty(ID)) {
                this.Data = new Organization();
            } else {
                this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as Organization;
                this.RefreshAddresses();
                this.RefreshEmailAddresses();
                this.RefreshPhoneNumbers();
                this.RefreshPersons();
            }
        }

        public void RefreshAddresses() {
            this.Addresses.Clear();

            foreach (AddressRelation row in contactInfoProvider.SelectAddressRelations(this.Data)) {
                AddressRelationPage page = Self.GET<AddressRelationPage>("/people/partials/address-relations/" + row.Key);

                page.Deleted += (s, e) => {
                    this.RefreshAddresses();
                };

                this.Addresses.Add(page);
            }
        }

        public void RefreshEmailAddresses() {
            this.EmailAddresses.Clear();

            foreach (EmailAddressRelation row in contactInfoProvider.SelectEmailAddressRelations(this.Data)) {
                EmailAddressRelationPage page = Self.GET<EmailAddressRelationPage>("/people/partials/email-address-relations/" + row.Key);

                page.Deleted += (s, a) => {
                    this.RefreshEmailAddresses();
                };

                this.EmailAddresses.Add(page);
            }
        }

        public void RefreshPhoneNumbers() {
            this.PhoneNumbers.Clear();

            foreach (PhoneNumberRelation row in contactInfoProvider.SelectPhoneNumberRelations(this.Data)) {
                PhoneNumberRelationPage page = Self.GET<PhoneNumberRelationPage>("/people/partials/phone-number-relations/" + row.Key);

                page.Deleted += (s, a) => {
                    this.RefreshEmailAddresses();
                };

                this.PhoneNumbers.Add(page);
            }
        }

        public void RefreshPersons() {
            this.Persons.Clear();

            foreach (OrganizationPerson row in organizationsProvider.SelectOrganizationPersons(this.Data)) {
                OrganizationPersonPage page = Self.GET<OrganizationPersonPage>("/people/partials/organization-persons/" + row.Key);

                page.Deleted += (s, e) => {
                    this.RefreshPersons();
                };

                this.Persons.Add(page);
            }
        }

        public void SetConfirmMessage(string Message) {
            this.Confirm.Message = Message;
        }

        public void SetConfirmAction(Action Action) {
            this.ConfirmAction = Action;
        }

        [OrganizationPage_json.Confirm]
        partial class OrganizatioConfirmPage : Page {
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

            public OrganizationPage ParentPage {
                get {
                    return this.Parent as OrganizationPage;
                }
            }
        }

        [OrganizationPage_json.Find]
        partial class OrganizationFindPage : Page {
            void Handle(Input.Query Action) {
                string query = Action.Value as string;

                this.Persons.Clear();

                if (!string.IsNullOrEmpty(query)) {
                    SearchProvider provider = new SearchProvider();

                    this.Persons.Data = provider.SelectPersons(query, 5);
                }
            }

            public OrganizationPage ParentPage {
                get {
                    return this.Parent as OrganizationPage;
                }
            }
        }

        [OrganizationPage_json.Find.Persons]
        partial class OrganizationFindPersonPage : Page, IBound<Person> {
            void Handle(Input.Add Action) {
                this.ParentPage.Find.Visible = false;

                if (this.ParentPage.Persons.Any(x => (x as OrganizationPersonPage).Data.Person.Equals(this.Data))) {
                    return;
                }

                OrganizationPerson op = new OrganizationPerson();
                op.Person = this.Data;
                op.Organization = this.ParentPage.Data;
                this.ParentPage.RefreshPersons();
            }

            public OrganizationPage ParentPage {
                get {
                    return this.Parent.Parent.Parent as OrganizationPage;
                }
            }
        }
    }
}
