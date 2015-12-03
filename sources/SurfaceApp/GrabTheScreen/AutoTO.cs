using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrabTheScreen
{
    class AutoTO : Car
    {
        String baseString64;

        public void setBaseString64(String baseString)
        {
            this.baseString64 = baseString;
        }

        public String getBaseString64()
        {
            return this.baseString64;
        }
    }
}
