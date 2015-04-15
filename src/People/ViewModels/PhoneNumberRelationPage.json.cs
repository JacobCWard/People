using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring3;

namespace People {
    [PhoneNumberRelationPage_json]
    partial class PhoneNumberRelationPage : Page, IBound<PhoneNumberRelation> {
        protected ContactInfoProvider contactInfoProvider = new ContactInfoProvider();
        public event EventHandler Deleted;

        public void RefreshPhoneNumberRelation(string ID) {
            QueryResultRows<PhoneNumberRelationType> types = contactInfoProvider.SelectPhoneNumberRelationTypes();
            PhoneNumberRelation relation = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID)) as PhoneNumberRelation;
            int i = 0;

            this.Data = relation;
            this.PhoneNumberRelationTypes.Data = types;

            foreach (var type in this.PhoneNumberRelationTypes) {
                if (type.Data.Equals(relation.PhoneNumberRelationType)) {
                    this.TypeIndex = i;
                    break;
                }

                i++;
            }

            this.PhoneNumber = Self.GET("/people/partials/phone-number/" + relation.PhoneNumber.Key);
        }

        void Handle(Input.TypeIndex Action) {
            int index = (int)Action.Value;
            this.Data.PhoneNumberRelationType = (PhoneNumberRelationType)PhoneNumberRelationTypes[index].Data;
        }

        void Handle(Input.Delete Action) {
            IConfirmPage page = this.Parent.Parent as IConfirmPage;

            page.SetConfirmMessage("Are you sure want to delete phone [" + this.Data.PhoneNumber.Name + "]?");
            page.SetConfirmAction(() => {
                this.Data.Delete();
                this.OnDelete();
            });
        }

        void OnDelete() {
            if (this.Deleted != null) {
                this.Deleted(this, EventArgs.Empty);
            }
        }
    }
}
