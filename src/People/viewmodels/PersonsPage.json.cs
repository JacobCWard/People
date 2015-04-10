using System;
using System.Linq;
using Starcounter;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class PersonsPage : Page {
        public PersonsProvider PersonsProvider = new PersonsProvider();
        public ContactInfoProvider ContactInfoProvider = new ContactInfoProvider();
        public Action ConfirmAction = null;

        public void RefreshPersons() {
            this.Persons = PersonsProvider.SelectPersons();
            this.AddUrl = UrlHelper.GetUrl("/persons/add");
        }

        [PersonsPage_json.Persons]
        partial class PersonsPersonPage : Page, IBound<Person> {
            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = "Are you sure want to delete [" + this.Data.Name + "]?";
                this.ParentPage.ConfirmAction = () => {
                    Db.Transact(() => {
                        this.ParentPage.PersonsProvider.Delete(this.Data);
                        this.ParentPage.RefreshPersons();
                    });
                };
            }

            void Handle(Input.Edit Action) {
                this.ParentPage.RedirectUrl = UrlHelper.GetUrl("/persons/" + this.Data.GetObjectID());
            }

            public PersonsPage ParentPage {
                get {
                    return this.Parent.Parent as PersonsPage;
                }
            }

            protected override void OnData() {
                base.OnData();

                ContactInfoProvider cip = this.ParentPage.ContactInfoProvider;
                Person p = this.Data;

                AddressRelation a = cip.SelectAddressRelations(p).First;
                EmailAddressRelation ea = cip.SelectEmailAddressRelations(p).First;
                PhoneNumberRelation pn = cip.SelectPhoneNumberRelations(p).First;

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

        [PersonsPage_json.Confirm]
        partial class PersonsConfirmPage : Page {
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

            public PersonsPage ParentPage {
                get {
                    return this.Parent as PersonsPage;
                }
            }
        }
    }
}
