using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssettoCorsaEventCreator.AssettoCorsa
{
    public class ACVehicle
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string skin;
        public string Skin
        {
            get { return skin; }
            set { skin = value; }
        }

        public ACVehicle()
        {
            name = string.Empty;
            skin = string.Empty;
        }

        public ACVehicle(string name, string skin)
        {
            this.name = name;
            this.skin = skin;
        }
    }
}
