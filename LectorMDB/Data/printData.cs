using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class printData
    {
        public List<string> tiposHojas { get; private set; }
        public List<string> orientaciones { get; private set; }
        public List<string> fontSizes { get; private set; }

        public void setTiposHojas(List<string> value)
        {
            tiposHojas = value;
        }

        public void setOrientaciones(List<string> value)
        {
            orientaciones = value;
        }
        public void setFontSizes(List<string> value)
        {
            fontSizes = value;
        }
    }
}
