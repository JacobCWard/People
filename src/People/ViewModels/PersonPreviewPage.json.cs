using Starcounter;
using Simplified.Ring2;

namespace People {
    partial class PersonPreviewPage : Page, IBound<Person> {
        protected override void OnData() {
            base.OnData();
            this.Url = string.Format("/people/persons/{0}", this.Key);
        }
    }
}
