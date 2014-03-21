using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AssettoCorsaEventCreator.AssettoCorsa
{
    public class ACGame
    {
        static string installPath;
        public static string InstallPath
        {
            get { return installPath; }
            set
            {
                if (Directory.Exists(value))
                {
                    installPath = value;
                    carsPath = Path.Combine(installPath, "content", "cars");
                    eventsPath = Path.Combine(installPath, "content", "specialevents");
                    eventCount = 0;

                    foreach (string dir in Directory.GetDirectories(eventsPath))
                    {
                        if (Path.GetFileName(dir).StartsWith("CUSTOM"))
                            eventCount++;
                    }
                }
            }
        }

        static string carsPath;
        public static string CarsPath
        {
            get { return carsPath; }
        }

        static string eventsPath;
        public static string EventsPath
        {
            get { return eventsPath; }
        }

        static int eventCount;
        public static int EventCount 
        {
            get { return eventCount; }
        }

        static string selectedTrack;
        public static string SelectedTrack
        {
            get { return selectedTrack; }
            set { selectedTrack = value; }
        }
    }
}
