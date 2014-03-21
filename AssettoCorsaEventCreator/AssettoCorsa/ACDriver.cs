using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AssettoCorsaEventCreator.AssettoCorsa
{
    public enum ACDriverType
    {
        Human,
        Computer
    };

    public class ACDriver
    {
        static List<string> driverNames;

        ACDriverType type;
        public ACDriverType DriverType
        {
            get { return type; }
            set { type = value; }
        }

        ACVehicle vehicle;
        public ACVehicle Vehicle
        {
            get { return vehicle; }
            set { vehicle = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string nationality;
        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        int aiLevel;
        public int AILevel
        {
            get { return aiLevel; }
            set { aiLevel = value; }
        }

        string setup;
        public string Setup
        {
            get { return setup; }
            set { setup = value; }
        }

        public ACDriver()
        {
            type = ACDriverType.Human;
            vehicle = new ACVehicle();
            name = string.Empty;
            nationality = "Planet Earth";
            setup = "";

            ACDriver.LoadDriverNames();
        }

        public static string GetRandomName()
        {
            if (driverNames == null || driverNames.Count == 0)
                ACDriver.LoadDriverNames();

            string chars = "abcdefgihjklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string name = string.Empty;
            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 8; i++)
            {
                name += chars[r.Next(0, chars.Length)];
            }

            return name;
        }

        public static IEnumerable<string> GetDriverNames()
        {
            return driverNames;
        }

        public static void LoadDriverNames()
        {
            driverNames = new List<string>();
            string path = Path.Combine(ACGame.InstallPath, "system", "data", "drivers.txt");
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string d = sr.ReadLine();

                    while (!string.IsNullOrEmpty(d))
                    {
                        driverNames.Add(d);
                        d = sr.ReadLine();
                    }

                    sr.Close();
                }
            }
        }

        public void SetRandomName()
        {
            string name = ACDriver.GetRandomName();
            this.name = name;
        }

        public override string ToString()
        {
            string text = string.Format("{0} : {1} in {2}", type, name, vehicle.Name);
            return text;
        }
    }
}
