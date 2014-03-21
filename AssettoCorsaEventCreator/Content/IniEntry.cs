using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssettoCorsaEventCreator.Content
{
    public class IniEntry
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string value;
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public IniEntry()
        {
            name = string.Empty;
            value = string.Empty;
        }

        public IniEntry(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
