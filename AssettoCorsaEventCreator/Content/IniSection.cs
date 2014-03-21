using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssettoCorsaEventCreator.Content
{
    public class IniSection
    {
        List<IniEntry> entries;
        public List<IniEntry> Entries
        {
            get { return entries; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public IniSection()
        {
            entries = new List<IniEntry>();
            name = string.Empty;
        }

        public IniSection(string name)
            : this()
        {
            this.name = name;
        }

        public void AddEntry(string name, string value)
        {
            entries.Add(new IniEntry(name, value));
        }
    }
}
