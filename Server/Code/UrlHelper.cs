using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People {
    public static class UrlHelper {
        public static string GetUrl(string LocalUrl) {
            return "/launcher/workspace/people" + LocalUrl;
        }

        public static string GetUrl() {
            return GetUrl(string.Empty);
        }
    }
}
