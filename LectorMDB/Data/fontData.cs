using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class fontData
    {
        public int plus { get; private set; }
        public int minus { get; private set; }
        public List<string> combo { get; private set; }

        public void setPlus(int value)
        {
            plus = value;
        }
        public void setMinus(int value)
        {
            minus = value;
        }
        public void setCombo(List<string> value)
        {
            combo = value;
        }

    }
}
