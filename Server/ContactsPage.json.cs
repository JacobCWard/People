using System;
using System.Linq;
using Starcounter;

namespace People {
    partial class ContactsPage : Page {
        public Action ConfirmAction;

        public void RefreshContacts() {
            this.Contacts = SQL<Contact>("SELECT c FROM People.Contact c");
        }
    }

    [ContactsPage_json.Contacts]
    partial class ContactsPageContacts : Page, IBound<Contact> {
        protected override string UriFragment {
            get {
                return "/launcher/workspace/people/contacts/" + Data.GetObjectID();
            }
        }

        void Handle(Input.Delete Action) {
            this.ParentView.Confirm.Message = "Are you sure want to delete [" + this.Data.Title + "]?";
            this.ParentView.ConfirmAction = () => {
                Db.Transact(() => {
                    this.Data.Delete();
                });

                this.ParentView.RefreshContacts();
            };
        }

        ContactsPage ParentView {
            get {
                return this.Parent.Parent as ContactsPage;
            }
        }
    }

    [ContactsPage_json.Confirm]
    partial class ContactsPageConfirm : Page {
        void Cancel() {
            this.ParentView.Confirm.Message = string.Empty;
            this.ParentView.ConfirmAction = null;
        }

        void Handle(Input.Ok Action) {
            if (this.ParentView.ConfirmAction != null) {
                this.ParentView.ConfirmAction();
            }

            this.Cancel();
        }

        void Handle(Input.Reject Action) {
            this.Cancel();
        }

        ContactsPage ParentView {
            get {
                return this.Parent as ContactsPage;
            }
        }
    }
}
