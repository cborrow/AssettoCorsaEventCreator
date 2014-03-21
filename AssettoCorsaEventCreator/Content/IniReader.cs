using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace AssettoCorsaEventCreator.Content
{
    public class IniReader
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

        public IniReader()
        {
            sections = new List<IniSection>();
            entries = new List<IniEntry>();
        }

        public IniReader(string file)
            : this()
        {

        }

        public void Load(string file)
        {
            if (File.Exists(file))
            {
                IniSection curSection = null;
                sections = new List<IniSection>();
                entries = new List<IniEntry>();

                using (StreamReader sr = new StreamReader(file))
                {
                    string text = string.Empty;

                    do
                    {
                        text = sr.ReadLine();

                        if (text.StartsWith("#"))
                            continue;
                        else if (text.StartsWith("["))
                        {
                            string sectionName = text.Substring(1, text.IndexOf("]"));
                            curSection = new IniSection(sectionName);
                        }
                        else if (string.IsNullOrWhiteSpace(text))
                        {
                            sections.Add(curSection);
                            curSection = null;
                        }
                        else
                        {
                            string key = text.Substring(0, text.IndexOf("="));
                            string val = text.Substring((text.IndexOf("=") + 1), text.Length - (key.Length + 1));

                            if (curSection != null)
                                curSection.AddEntry(key, val);
                            else
                                entries.Add(new IniEntry(key, val));
                        }
                    }
                    while (!string.IsNullOrEmpty(text));
                }
            }
        }
    }
}
