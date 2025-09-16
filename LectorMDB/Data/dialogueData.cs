using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class dialogueData
    {
        public string defaultExtension { get; private set; }
        public string filterExtension { get; private set; }
        public string fileOfHojas { get; private set; }
        public string errorWindowTitle { get; private set; }
        public string errorWindowWrongFile { get; private set; }
        public string errorWindowNotContentFile { get; private set; }

        public void setDefaultExt(string value)
        {
            defaultExtension = value;
        }
        public void setFilterExt(string value)
        {
            filterExtension = value;
        }
        public void setFileOfHojas(string value)
        {
            fileOfHojas = value;
        }
        public void setErrorWindowTitle(string value)
        {
            errorWindowTitle = value;
        }
        public void setErrorWindowWrongFile(string value)
        {
            errorWindowWrongFile = value;
        }
        public void setErrorWindowNotContentFile(string value)
        {
            errorWindowNotContentFile = value;
        }
    }
}
