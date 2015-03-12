using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class PersonPage : Page, IBound<Person> {
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
            ar.AddressRelationType = this.AddressRelationTypes.First().Data as AddressRelationType;

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
            this.RedirectUrl = "/launcher/workspace/people/persons";
        }

        public void RefreshPerson(string ID = null) {
            QueryResultRows<EmailAddressRelationType> emailAddressRelationTypes = Db.SQL<EmailAddressRelationType>("SELECT t FROM Simplified.Ring3.EmailAddressRelationType t");
            QueryResultRows<PhoneNumberRelationType> phoneNumberRelationTypes = Db.SQL<PhoneNumberRelationType>("SELECT t FROM Simplified.Ring3.PhoneNumberRelationType t");

            this.AddressRelationTypes = Db.SQL<AddressRelationType>("SELECT t FROM Simplified.Ring3.AddressRelationType t");
            this.EmailAddressRelationTypes = emailAddressRelationTypes;
            this.PhoneNumberRelationTypes = phoneNumberRelationTypes;

            if (string.IsNullOrEmpty(ID)) {
                this.Data = new Person();
            } else {
                this.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as Person;
                this.RefreshAddresses();
                this.RefreshEmailAddresses();
                this.RefreshPhoneNumbers();
            }
        }

        public List<AddressRelationType> GetAddressRelationTypes() {
            return this.AddressRelationTypes.Select(val => val.Data).OfType<AddressRelationType>().ToList();
        }

        public List<EmailAddressRelationType> GetEmailAddressRelationTypes() {
            return this.EmailAddressRelationTypes.Select(val => val.Data).OfType<EmailAddressRelationType>().ToList();
        }

        public List<PhoneNumberRelationType> GetPhoneNumberRelationTypes() {
            return this.PhoneNumberRelationTypes.Select(val => val.Data).OfType<PhoneNumberRelationType>().ToList();
        }

        public void RefreshAddresses() {
            List<AddressRelationType> types = this.GetAddressRelationTypes();

            this.Addresses.Data = Db.SQL<AddressRelation>("SELECT r FROM Simplified.Ring3.AddressRelation r WHERE r.Somebody = ?", this.Data);

            foreach (var row in this.Addresses) {
                AddressRelation item = row.Data as AddressRelation;

                row.TypeIndex = types.IndexOf(item.AddressRelationType);
            }
        }

        public void RefreshEmailAddresses() {
            List<EmailAddressRelationType> types = this.GetEmailAddressRelationTypes();

            this.EmailAddresses.Data = Db.SQL<EmailAddressRelation>("SELECT r FROM Simplified.Ring3.EmailAddressRelation r WHERE r.Somebody = ?", this.Data);

            foreach (var row in this.EmailAddresses) {
                EmailAddressRelation item = row.Data as EmailAddressRelation;

                row.TypeIndex = types.IndexOf(item.EmailAddressRelationType);
            }
        }

        public void RefreshPhoneNumbers() {
            List<PhoneNumberRelationType> types = this.GetPhoneNumberRelationTypes();

            this.PhoneNumbers.Data = Db.SQL<PhoneNumberRelation>("SELECT r FROM Simplified.Ring3.PhoneNumberRelation r WHERE r.Somebody = ?", this.Data);

            foreach (var row in this.PhoneNumbers) {
                PhoneNumberRelation item = row.Data as PhoneNumberRelation;

                row.TypeIndex = types.IndexOf(item.PhoneNumberRelationType);
            }
        }

        [PersonPage_json.Addresses]
        partial class PersonAddressPage : Page, IBound<AddressRelation> {
            void Handle(Input.TypeIndex Action) {
                List<AddressRelationType> types = this.ParentPage.GetAddressRelationTypes();
                int index = (int)Action.Value;

                this.Data.AddressRelationType = types[index];
            }

            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = "Are you sure want to delete address [" + this.Data.Address.Name + "]?";
                this.ParentPage.ConfirmAction = () => {
                    this.Data.Delete();
                    this.ParentPage.RefreshAddresses();
                };
            }

            PersonPage ParentPage {
                get {
                    return this.Parent.Parent as PersonPage;
                }
            }
        }

        [PersonPage_json.EmailAddresses]
        partial class PersonEmailAddressPage : Page, IBound<EmailAddressRelation> {
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

            PersonPage ParentPage {
                get {
                    return this.Parent.Parent as PersonPage;
                }
            }
        }

        [PersonPage_json.PhoneNumbers]
        partial class PersonPhoneNumberPage : Page, IBound<PhoneNumberRelation> {
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

            PersonPage ParentPage {
                get {
                    return this.Parent.Parent as PersonPage;
                }
            }
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
