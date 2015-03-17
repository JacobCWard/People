using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People {
    public class UrlHelper {
        public static string BaseUrl = "/launcher/workspace/people";

        public static string GetUrl(string Value) { 
            return BaseUrl + Value;
        }
    }
}
