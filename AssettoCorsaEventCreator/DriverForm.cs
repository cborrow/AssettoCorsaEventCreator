using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AssettoCorsaEventCreator.AssettoCorsa;

namespace AssettoCorsaEventCreator
{
    public partial class DriverForm : Form
    {
        public string Name
        {
            get { return comboBox4.Text; }
            set { comboBox4.Text = value; }
        }

        public string Vehicle
        {
            get { return comboBox1.Text; }
            set { comboBox1.SelectedIndex = comboBox1.Items.IndexOf(value); }
        }

        public string VehicleSkin
        {
            get { return comboBox2.Text; }
            set { comboBox2.Text = value; }
        }

        public int AILevel
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        ACDriverType driverType;
        public ACDriverType DriverType
        {
            get { return driverType; }
            set { driverType = value; }
        }

        public string Setup
        {
            get { return comboBox3.Text; }
            set { comboBox3.Text = value; }
        }

        string selectedTrack;
        public string SelectedTrack
        {
            get { return selectedTrack; }
            set { selectedTrack = value; }
        }

        public DriverForm()
        {
            InitializeComponent();

            string[] cars = ACVehicleLoader.GetVehicleList().ToArray<string>();
            comboBox1.Items.AddRange(cars);

            ACDriver.LoadDriverNames();
            string[] drivers = ACDriver.GetDriverNames().ToArray<string>();

            comboBox4.Items.AddRange(drivers);

            driverType = ACDriverType.Computer;
        }

        public ACDriver GetDriver()
        {
            ACDriver driver = new ACDriver();
            driver.Name = comboBox4.Text;
            driver.Vehicle.Name = comboBox1.Text;
            driver.Vehicle.Skin = comboBox2.Text;
            driver.DriverType = driverType;
            driver.AILevel = (int)numericUpDown1.Value;

            if(!string.IsNullOrEmpty(comboBox3.Text) && comboBox3.Text != "Default")
                driver.Setup = comboBox3.Text;

            return driver;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox4.Text))
                MessageBox.Show("You must enter a driver's name.", "Add Driver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (string.IsNullOrEmpty(comboBox1.Text))
                MessageBox.Show("You must select a vehicle for the driver.", "Add Driver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (string.IsNullOrEmpty(comboBox2.Text))
                MessageBox.Show("You must select a skin for the vehicle.", "Add Driver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] skins = ACVehicleLoader.GetVehicleSkins(comboBox1.Text).ToArray<string>();
            string[] setups = ACVehicleLoader.GetVehicleSetups(comboBox1.Text, this.SelectedTrack).ToArray<string>();

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(skins);

            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(setups);
            comboBox3.Text = "Default";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string name = ACDriver.GetRandomName();
            comboBox4.Text = name;
        }
    }
}
