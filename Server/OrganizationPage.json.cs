using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class OrganizationPage : Page, IBound<Organization> {
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
            ear.EmailAddressRelationType = this.EmailAddressRelationTypes.First().Data as EmailAddressRelationType;

            this.RefreshEmailAddresses();
        }

        void Handle(Input.AddPhoneNumber Action) {
            PhoneNumber pn = new PhoneNumber();
            PhoneNumberRelation pnr = new PhoneNumberRelation();

            pnr.Somebody = this.Data;
            pnr.PhoneNumber = pn;
            pnr.PhoneNumberRelationType = this.PhoneNumberRelationTypes.First().Data as PhoneNumberRelationType;

            this.RefreshPhoneNumbers();
        }

        public void GoBack() {
            this.RedirectUrl = "/launcher/workspace/people/organizations";
        }

        public void RefreshOrganization(string ID = null) {
            this.EmailAddressRelationTypes = contactInfoProvider.SelectEmailAddressRelationTypes();
            this.PhoneNumberRelationTypes = contactInfoProvider.SelectPhoneNumberRelationTypes();

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

        public List<EmailAddressRelationType> GetEmailAddressRelationTypes() {
            return this.EmailAddressRelationTypes.Select(val => val.Data).OfType<EmailAddressRelationType>().ToList();
        }

        public List<PhoneNumberRelationType> GetPhoneNumberRelationTypes() {
            return this.PhoneNumberRelationTypes.Select(val => val.Data).OfType<PhoneNumberRelationType>().ToList();
        }

        public void RefreshAddresses() {
            this.Addresses.Clear();

            foreach (AddressRelation row in contactInfoProvider.SelectAddressRelations(this.Data)) {
                var page = X.GET<AddressRelationPage>("/people/partials/address-relations/" + row.Key);
                page.OnDelete = () => {
                    this.RefreshAddresses();
                };
                this.Addresses.Add(page);
            }
        }

        public void RefreshEmailAddresses() {
            List<EmailAddressRelationType> types = this.GetEmailAddressRelationTypes();

            this.EmailAddresses.Clear();
            this.EmailAddresses.Data = contactInfoProvider.SelectEmailAddressRelations(this.Data);

            foreach (var row in this.EmailAddresses) {
                EmailAddressRelation item = row.Data as EmailAddressRelation;

                row.TypeIndex = types.IndexOf(item.EmailAddressRelationType);
            }
        }

        public void RefreshPhoneNumbers() {
            List<PhoneNumberRelationType> types = this.GetPhoneNumberRelationTypes();

            this.PhoneNumbers.Clear();
            this.PhoneNumbers.Data = contactInfoProvider.SelectPhoneNumberRelations(this.Data);

            foreach (var row in this.PhoneNumbers) {
                PhoneNumberRelation item = row.Data as PhoneNumberRelation;

                row.TypeIndex = types.IndexOf(item.PhoneNumberRelationType);
            }
        }

        public void RefreshPersons() {
            this.Persons.Clear();
            this.Persons.Data = organizationsProvider.SelectOrganizationPersons(this.Data);
        }

        [OrganizationPage_json.EmailAddresses]
        partial class OrganizatioEmailAddressPage : Page, IBound<EmailAddressRelation> {
            void Handle(Input.TypeIndex Action) {
                List<EmailAddressRelationType> types = this.ParentPage.GetEmailAddressRelationTypes();
                int index = (int)Action.Value;

                this.Data.EmailAddressRelationType = types[index];
            }

            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = "Are you sure want to delete email address [" + this.Data.EmailAddress.Name + "]?";
                this.ParentPage.ConfirmAction = () => {
                    this.Data.Delete();
                    this.ParentPage.RefreshEmailAddresses();
                };
            }

            OrganizationPage ParentPage {
                get {
                    return this.Parent.Parent as OrganizationPage;
                }
            }
        }

        [OrganizationPage_json.PhoneNumbers]
        partial class OrganizatioPhoneNumberPage : Page, IBound<PhoneNumberRelation> {
            void Handle(Input.TypeIndex Action) {
                List<PhoneNumberRelationType> types = this.ParentPage.GetPhoneNumberRelationTypes();
                int index = (int)Action.Value;

                this.Data.PhoneNumberRelationType = types[index];
            }

            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = "Are you sure want to delete phone number [" + this.Data.PhoneNumber.Name + "]?";
                this.ParentPage.ConfirmAction = () => {
                    this.Data.Delete();
                    this.ParentPage.RefreshPhoneNumbers();
                };
            }

            OrganizationPage ParentPage {
                get {
                    return this.Parent.Parent as OrganizationPage;
                }
            }
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

        [OrganizationPage_json.Persons]
        partial class OrganizationPersonPage : Page, IBound<OrganizationPerson> {
            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = string.Format("Are you sure want to remove person [{0}] from this organization?", this.Data.Person.Name);
                this.ParentPage.ConfirmAction = () => {
                    this.Data.Delete();
                    this.ParentPage.Persons.Remove(this);
                };
            }

            public OrganizationPage ParentPage {
                get {
                    return this.Parent.Parent as OrganizationPage;
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

                if (this.ParentPage.Persons.Any(x => x.Data.Person.Equals(this.Data))) {
                    return;
                }

                OrganizationPerson op = new OrganizationPerson();
                op.Person = this.Data;
                op.Organization = this.ParentPage.Data;
                this.ParentPage.Persons.Add(new OrganizationPersonPage() { Data = op });
            }

            public OrganizationPage ParentPage {
                get {
                    return this.Parent.Parent.Parent as OrganizationPage;
                }
            }
        }
    }
}
