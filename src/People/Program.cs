using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Starcounter;
using PolyjuiceNamespace;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    partial class Program : Page {
        static void Main() {
            IHandlers[] handlers = new IHandlers[] { 
                new InitialData(), 
                new PolyjuiceHandlers(), 
                new PartialHandlers(), 
                new MainHandlers(),
                new OntologyMap()
            };

            foreach (IHandlers handler in handlers) {
                handler.Register();
            }
        }
    }
}
