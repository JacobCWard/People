using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People {
    public class UrlHelper {
        private static string baseUrl = "/launcher/workspace/people";

        public static string BaseUrl {
            get {
                return baseUrl;
            }
            set {
                baseUrl = value;
            }
        }

        public static string GetUrl(string Value) {
            return BaseUrl + Value;
        }
    }
}
