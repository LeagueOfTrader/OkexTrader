using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Strategy
{
    abstract class OkexStrategy
    {
        public abstract void update();

        protected void trade()
        {
            //
        }
    }
}
