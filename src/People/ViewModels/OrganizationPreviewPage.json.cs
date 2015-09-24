using Starcounter;
using Simplified.Ring2;

namespace People {
    partial class OrganizationPreviewPage : Page, IBound<Organization> {
        protected override void OnData() {
            base.OnData();
            this.Url = string.Format("/people/organizations/{0}", this.Key);
        }
    }
}
