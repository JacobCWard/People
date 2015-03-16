using System;
using System.Linq;
using System.Collections.Generic;
using Starcounter;

namespace People {
    [AddressRelationPage_json]
    partial class AddressRelationPage : Page, IBound<Simplified.Ring3.AddressRelation> {
        protected ContactInfoProvider contactInfoProvider = new ContactInfoProvider();
        
        public Action OnDelete = () => { };

        public void RefreshAddressRelation(string addrelId) {
            var addrel = Db.SQL<Simplified.Ring3.AddressRelation>("SELECT a FROM Simplified.Ring3.AddressRelation a WHERE ObjectId = ?", addrelId).First;
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
            this.Data.AddressRelationType = (Simplified.Ring3.AddressRelationType)AddressRelationTypes[index].Data;
        }

        void Handle(Input.Delete Action) {
            this.Confirm.Message = "Are you sure want to delete address [" + this.Data.Address.Name + "]?";
            this.Confirm.ConfirmAction = () => {
                this.Data.Delete();
                this.OnDelete();
            };
        }
    }

    [AddressRelationPage_json.Confirm]
    partial class AddressRelationConfirmPage : Json {
        public Action ConfirmAction = null;

        void Cancel() {
            this.Message = null;
            this.ConfirmAction = null;
        }

        void Handle(Input.Reject Action) {
            Cancel();
        }

        void Handle(Input.Ok Action) {
            if (this.ConfirmAction != null) {
                this.ConfirmAction();
            }

            Cancel();
        }
    }
}
