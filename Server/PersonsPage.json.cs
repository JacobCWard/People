using System;
using System.Linq;
using Starcounter;
using Simplified.Ring2;

namespace People {
    partial class PersonsPage : Page {
        public Action ConfirmAction = null;

        public void RefreshPersons() {
            this.Persons = Db.SQL("SELECT p FROM Simplified.Ring2.Person p ORDER By p.Name");
        }

        [PersonsPage_json.Persons]
        partial class PersonsPersonPage : Page, IBound<Person> {
            void Handle(Input.Delete Action) {
                this.ParentPage.Confirm.Message = "Are you sure want to delete [" + this.Data.Name + "]?";
                this.ParentPage.ConfirmAction = () => {
                    Db.Transact(() => {
                        this.Data.Delete();
                        this.ParentPage.RefreshPersons();
                    });
                };
            }

            void Handle(Input.Edit Action) {
                this.ParentPage.RedirectUrl = "/launcher/workspace/people/persons/" + this.Data.GetObjectID();
            }

            public PersonsPage ParentPage {
                get {
                    return this.Parent.Parent as PersonsPage;
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
