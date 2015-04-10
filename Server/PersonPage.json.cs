using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class PersonPage : Page, IBound<Person>, IConfirmPage {
        protected ContactInfoProvider contactInfoProvider = new ContactInfoProvider();
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
            this.RedirectUrl = UrlHelper.GetUrl("/persons");
        }

        public void RefreshPerson(string ID = null) {
            if (string.IsNullOrEmpty(ID)) {
                this.Data = new Person();
            } else {
                this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as Person;
                this.RefreshAddresses();
                this.RefreshEmailAddresses();
                this.RefreshPhoneNumbers();
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

        public void SetConfirmMessage(string Message) {
            this.Confirm.Message = Message;
        }

        public void SetConfirmAction(Action Action) {
            this.ConfirmAction = Action;
        }

        [PersonPage_json.Confirm]
        partial class PersonConfirmPage : Page {
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

            public PersonPage ParentPage {
                get {
                    return this.Parent as PersonPage;
                }
            }
        }
    }
}
