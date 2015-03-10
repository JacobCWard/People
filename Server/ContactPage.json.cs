using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;

namespace People {
    [ContactPage_json]
    partial class ContactPage : Page {
        public Action ConfirmAction;

        public void RefreshContact(Contact Contact) {
            QueryResultRows<Company> companies = Db.SQL<Company>("SELECT c FROM People.Company c");
            List<ContactInfoType> types = Db.SQL<ContactInfoType>("SELECT cit FROM ContactInfoType cit").ToList();
            List<ContactInfoRole> roles = Db.SQL<ContactInfoRole>("SELECT cir FROM ContactInfoRole cir").ToList();

            this.Data = Contact;
            this.SelectedCompanyIndex = -1;
            this.Companies.Data = companies;
            this.ContactInfoTypes.Data = types;
            this.ContactInfoRoles.Data = roles;

            if (Contact.Company != null) {
                this.SelectedCompanyIndex = companies.ToList().IndexOf(Contact.Company);
            }

            int i = 0;

            foreach (ContactInfo info in Contact.Infos) {
                this.Infos[i].SelectedContactInfoTypeIndex = types.IndexOf(info.Type);
                this.Infos[i].SelectedContactInfoRoleIndex = roles.IndexOf(info.Role);
                i++;
            }
        }

        void Handle(Input.Save Action) {
            Transaction.Commit();
            RedirectUrl = UrlHelper.GetUrl("/contacts");
        }

        void Handle(Input.Cancel Action) {
            //Transaction.Rollback();
            RedirectUrl = UrlHelper.GetUrl("/contacts");
        }

        void Handle(Input.SelectedCompanyIndex Action) {
            int index = (int)Action.Value;
            Company company = Companies[index].Data as Company;

            this.Contact.Company = company;
        }

        void Handle(Input.AddInfo Action) {
            ContactInfo info = new ContactInfo();

            info.Contact = this.Contact;
            info.Type = this.ContactInfoTypes.First().Data as ContactInfoType;
            info.Role = this.ContactInfoRoles.First().Data as ContactInfoRole;
        }

        Contact Contact {
            get {
                return this.Data as Contact;
            }
        }

        protected override string UriFragment {
            get {
                return UrlHelper.GetUrl("/contacts/") + Data.GetObjectID();
            }
        }

        [ContactPage_json.Infos]
        partial class ContactPageInfo : Page, IBound<ContactInfo> {
            void Handle(Input.SelectedContactInfoTypeIndex Action) {
                int index = (int)Action.Value;
                ContactInfoType type = this.ParentView.ContactInfoTypes[index].Data as ContactInfoType;

                this.Info.Type = type;
                this.ParentView.Contact.SetDefaultContact(this.Info, this.Info.Default);
            }

            void Handle(Input.SelectedContactInfoRoleIndex Action) {
                int index = (int)Action.Value;
                ContactInfoRole role = this.ParentView.ContactInfoRoles[index].Data as ContactInfoRole;

                this.Info.Role = role;
            }

            void Handle(Input.Delete Action) {
                this.ParentView.Confirm.Message = "Are you sure want to delete [" + this.Data.Value + "]?";
                this.ParentView.ConfirmAction = () => {
                    this.Data.Delete();
                };
            }

            void Handle(Input.Default Action) {
                this.ParentView.Contact.SetDefaultContact(this.Info, (bool)Action.Value);
            }

            ContactInfo Info {
                get {
                    return this.Data as ContactInfo;
                }
            }

            ContactPage ParentView {
                get {
                    return this.Parent.Parent as ContactPage;
                }
            }
        }

        [ContactPage_json.Confirm]
        partial class ContactPageConfirm : Page {
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

            ContactPage ParentView {
                get {
                    return this.Parent as ContactPage;
                }
            }
        }
    }
}