using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring3;

namespace People {
    [EmailAddressRelationPage_json]
    partial class EmailAddressRelationPage : Page, IBound<EmailAddressRelation> {
        protected ContactInfoProvider contactInfoProvider = new ContactInfoProvider();
        public event EventHandler Deleted;

        public void RefreshEmailAddressRelation(string ID) {
            QueryResultRows<EmailAddressRelationType> types = contactInfoProvider.SelectEmailAddressRelationTypes();
            EmailAddressRelation relation = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as EmailAddressRelation;
            int i = 0;

            this.Data = relation;
            this.EmailAddressRelationTypes.Data = types;

            foreach (var type in this.EmailAddressRelationTypes) {
                if (type.Data.Equals(relation.EmailAddressRelationType)) {
                    this.TypeIndex = i;
                    break;
                }

                i++;
            }

            this.EmailAddress = X.GET("/people/partials/email-addresses/" + relation.EmailAddress.Key);
        }

        void Handle(Input.TypeIndex Action) {
            int index = (int)Action.Value;
            this.Data.EmailAddressRelationType = (EmailAddressRelationType)EmailAddressRelationTypes[index].Data;
        }

        void Handle(Input.Delete Action) {
            if (this.Parent.Parent is PersonPage) {
                PersonPage MyParent = (PersonPage)this.Parent.Parent;
                MyParent.Confirm.Message = "Are you sure want to delete email [" + this.Data.EmailAddress.Name + "]?";
                MyParent.ConfirmAction = () => {
                    this.Data.Delete();
                    this.OnDelete();
                };
            }

            if (this.Parent.Parent is OrganizationPage) {
                OrganizationPage MyParent = (OrganizationPage)this.Parent.Parent;
                MyParent.Confirm.Message = "Are you sure want to delete email [" + this.Data.EmailAddress.Name + "]?";
                MyParent.ConfirmAction = () => {
                    this.Data.Delete();
                    this.OnDelete();
                };
            }
        }

        void OnDelete() {
            if (this.Deleted != null) {
                this.Deleted(this, EventArgs.Empty);
            }
        }
    }
}
