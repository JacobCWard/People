using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring3;

namespace People {
    [AddressRelationPage_json]
    partial class AddressRelationPage : Page, IBound<AddressRelation> {
        protected ContactInfoProvider contactInfoProvider = new ContactInfoProvider();
        public event EventHandler Deleted;

        public void RefreshAddressRelation(string addrelId) {
            var addrel = Db.SQL<AddressRelation>("SELECT a FROM Simplified.Ring3.AddressRelation a WHERE ObjectId = ?", addrelId).First;
            this.Data = addrel;

            var types = contactInfoProvider.SelectAddressRelationTypes();
            this.AddressRelationTypes.Data = types;
            var i = 0;
            foreach (var type in this.AddressRelationTypes) {
                if (type.Data.Equals(addrel.AddressRelationType)) {
                    this.TypeIndex = i;
                    break;
                }
                i++;
            }

            this.Address = X.GET("/people/partials/addresses/" + addrel.Address.Key);
        }

        void Handle(Input.TypeIndex Action) {
            int index = (int)Action.Value;
            this.Data.AddressRelationType = (AddressRelationType)AddressRelationTypes[index].Data;
        }

        void Handle(Input.Delete Action) {
            if (this.Parent.Parent is PersonPage) {
                PersonPage MyParent = (PersonPage)this.Parent.Parent;
                MyParent.Confirm.Message = "Are you sure want to delete address [" + this.Data.Address.Name + "]?";
                MyParent.ConfirmAction = () => {
                    this.Data.Delete();
                    this.OnDelete();
                };
            }

            if (this.Parent.Parent is OrganizationPage) {
                OrganizationPage MyParent = (OrganizationPage)this.Parent.Parent;
                MyParent.Confirm.Message = "Are you sure want to delete address [" + this.Data.Address.Name + "]?";
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
