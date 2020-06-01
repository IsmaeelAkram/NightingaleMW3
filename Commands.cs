using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    class Command
    {
        private Func<int> function;

        public Command(string name, Func<int> function_)
        {
            function = function_;
        }
    }

    class Commands
    {

    }
}
