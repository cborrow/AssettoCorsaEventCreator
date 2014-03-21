using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AssettoCorsaEventCreator.Content
{
    public class IniWriter
    {
        List<IniSection> sections;
        public List<IniSection> Sections
        {
            get { return sections; }
        }

        List<IniEntry> entries;
        public List<IniEntry> Entries
        {
            get { return entries; }
        }

        public IniWriter()
        {
            sections = new List<IniSection>();
            entries = new List<IniEntry>();
        }

        public void Save(string file)
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (IniSection s in sections)
                {
                    sw.WriteLine(string.Format("[{0}]", s.Name));

                    foreach (IniEntry e in s.Entries)
                    {
                        sw.WriteLine(string.Format("{0}={1}", e.Name, e.Value));
                    }
                    sw.WriteLine();
                }

                sw.WriteLine();

                foreach (IniEntry e in entries)
                {
                    sw.WriteLine(string.Format("{0}={1}", e.Name, e.Value));
                }

                sw.Close();
            }
        }
    }
}
