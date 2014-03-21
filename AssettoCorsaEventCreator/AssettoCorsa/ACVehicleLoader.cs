using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AssettoCorsaEventCreator.AssettoCorsa
{
    public class ACVehicleLoader
    {
        public static IEnumerable<string> GetVehicleList()
        {
            List<string> cars = new List<string>();

            foreach (string car in Directory.GetDirectories(ACGame.CarsPath))
            {
                cars.Add(Path.GetFileName(car));
            }

            return cars;
        }

        public static IEnumerable<string> GetVehicleSkins(string vehicleName)
        {
            List<string> skins = new List<string>();

            string path = Path.Combine(ACGame.CarsPath, vehicleName, "skins");

            foreach (string skin in Directory.GetDirectories(path))
            {
                skins.Add(Path.GetFileName(skin));
            }

            return skins;
        }

        public static IEnumerable<string> GetVehicleSetups(string vehicleName, string trackName)
        {
            List<string> setups = new List<string>();

            if (string.IsNullOrEmpty(vehicleName) || string.IsNullOrEmpty(trackName))
            {
                System.Windows.Forms.MessageBox.Show("Vehicle name or Track name is null or empty.");
                return setups;
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "setups", vehicleName, trackName);

            foreach (string file in Directory.GetFiles(path))
            {
                setups.Add(Path.GetFileNameWithoutExtension(file));
            }

            return setups;
        }
    }
}
