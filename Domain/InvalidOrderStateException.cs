using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class InvalidOrderStateException:Exception
    {
        string msg;
        public InvalidOrderStateException(string message):base(message)
        {
            msg = message;
        }
    }
}
