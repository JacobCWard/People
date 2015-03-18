using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People {
    public interface IConfirmPage {
        void SetConfirmMessage(string Message);
        void SetConfirmAction(Action Action);
    }
}
