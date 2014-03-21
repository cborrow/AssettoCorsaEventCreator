using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using AssettoCorsaEventCreator.Content;
using AssettoCorsaEventCreator.AssettoCorsa;

namespace AssettoCorsaEventCreator
{
    public partial class MainForm : Form
    {
        CreateConditionForm createConditionForm;
        DriverForm driverForm;
        ACEvent acEvent;

        public MainForm()
        {
            SetupForm setupForm = new SetupForm();

            AssettoCorsaEventCreator.Properties.Settings.Default.Reload();

            string path = AssettoCorsaEventCreator.Properties.Settings.Default.InstallationPath;
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                if (setupForm.ShowDialog() == DialogResult.OK)
                {
                    string newPath = setupForm.InstallPath;
                    AssettoCorsaEventCreator.Properties.Settings.Default.InstallationPath = newPath;
                    AssettoCorsaEventCreator.Properties.Settings.Default.Save();
                }
            }

            AssettoCorsaEventCreator.Properties.Settings.Default.Reload();
            ACGame.InstallPath = AssettoCorsaEventCreator.Properties.Settings.Default.InstallationPath;

            createConditionForm = new CreateConditionForm();
            driverForm = new DriverForm();
            acEvent = new ACEvent();

            InitializeComponent();

            LoadTracks();
            //LoadCars();
        }

        public void LoadTracks()
        {
            string installPath = AssettoCorsaEventCreator.Properties.Settings.Default.InstallationPath;

            if (!Directory.Exists(installPath))
            {
                MessageBox.Show("Invalid installation path set.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                installPath = Path.Combine(installPath, "content\\tracks");

                foreach (string track in Directory.GetDirectories(installPath))
                {
                    comboBox1.Items.Add(Path.GetFileName(track));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying to load list of tracks. " + ex.Message);
            }
        }

        public void LoadCars()
        {
            string installPath = AssettoCorsaEventCreator.Properties.Settings.Default.InstallationPath;

            if (!Directory.Exists(installPath))
            {
                MessageBox.Show("Invalid installation path set.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                installPath = Path.Combine(installPath, "content\\cars");

                foreach (string car in Directory.GetDirectories(installPath))
                {
                    comboBox2.Items.Add(Path.GetFileName(car));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying to load list of tracks. " + ex.Message);
            }
        }

        public int RaceTypeToId(string name)
        {
            string ln = name.ToLower();
            int id = 0;

            switch (ln)
            {
                case "race":
                    id = 3;
                    break;
                case "hotlap":
                    id = 4;
                    break;
                case "drift":
                    id = 6;
                    break;
                case "checkpoint":
                    id = 5;
                    break;
                case "drag":
                    id = 7;
                    break;
                default:
                    id = 4;
                    break;
            }

            return id;
        }

        public string RaceTypeStart(string name)
        {
            string ln = name.ToLower();
            string start = "";

            switch (ln)
            {
                case "race":
                case "checkpoint":
                    start = "START";
                    break;
                case "drift":
                case "drag":
                    start = "PIT";
                    break;
                case "hotlap":
                    start = "HOTLAP_START";
                    break;
                default:
                    start = "PIT";
                    break;
            }

            return start;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown2.Value == (numericUpDown1.Value + 3) || numericUpDown2.Value == (numericUpDown1.Value + 5))
                numericUpDown2.Value = (numericUpDown1.Value + 4);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (createConditionForm.ShowDialog() == DialogResult.OK)
            {
                ACCondition acc = createConditionForm.GenerateCondition();
                listBox1.Items.Add(acc);
                acEvent.Conditions.Add(acc);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int index = listBox1.SelectedIndex;
            ACCondition acc = (ACCondition)listBox1.SelectedItem;

            if (acc != null)
            {
                createConditionForm.EditCondition(acc);

                if (createConditionForm.ShowDialog() == DialogResult.OK)
                {
                    ACCondition updatedAcc = createConditionForm.GenerateCondition();
                    acEvent.Conditions[index] = updatedAcc;
                    listBox1.Items[index] = updatedAcc;
                }
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                ACCondition acc = (ACCondition)listBox1.SelectedItem;
                acEvent.Conditions.Remove(acc);
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            acEvent.Name = textBox1.Text;
            acEvent.Description = textBox1.Text;
            //acEvent.Drivers.AddRange((IEnumerable<ACDriver>)listBox2.Items);
            //acEvent.Conditions.AddRange((IEnumerable<ACCondition>)listBox1.Items);

            int guid = (ACGame.EventCount + 1);

            IniWriter iniWriter = new IniWriter();

            IniSection s1 = new IniSection("EVENT");
            s1.AddEntry("NAME", textBox1.Text);
            s1.AddEntry("DESCRIPTION", "Race Session");
            iniWriter.Sections.Add(s1);

            IniSection s2 = new IniSection("SPECIAL_EVENT");
            s2.AddEntry("GUID", (guid + 100).ToString());
            iniWriter.Sections.Add(s2);

            IniSection s11 = new IniSection("LAP_INVALIDATOR");
            s11.AddEntry("ALLOWED_TYRES_OUT", "4");
            iniWriter.Sections.Add(s11);

            int car = 0;
            foreach (ACDriver driver in acEvent.Drivers)
            {
                IniSection s3 = new IniSection("CAR_" + car.ToString());
                if (car == 0)
                    s3.AddEntry("MODEL", "-");
                else
                    s3.AddEntry("MODEL", driver.Vehicle.Name);
                s3.AddEntry("SKIN", driver.Vehicle.Skin);
                s3.AddEntry("DRIVER_NAME", driver.Name);
                s3.AddEntry("NATIONALITY", driver.Nationality);
                s3.AddEntry("SETUP", driver.Setup);

                if (driver.DriverType == ACDriverType.Computer)
                    s3.AddEntry("AI_LEVEL", driver.AILevel.ToString());
                else
                    s3.AddEntry("AI_LEVEL", "");

                iniWriter.Sections.Add(s3);
                car++;
            }

            IniSection s4 = new IniSection("RACE");
            s4.AddEntry("TRACK", comboBox1.Text);
            s4.AddEntry("MODEL", acEvent.Drivers[0].Vehicle.Name);
            s4.AddEntry("CARS", acEvent.Drivers.Count.ToString());
            //s4.AddEntry("AI_LEVEL", "100");
            s4.AddEntry("FIXED_SETUP", "0");
            iniWriter.Sections.Add(s4);

            IniSection s5 = new IniSection("SESSION_0");
            //s5.AddEntry("NAME", " Session");
            s5.AddEntry("TYPE", RaceTypeToId(comboBox2.Text).ToString());
            s5.AddEntry("SPAWN_SET", RaceTypeStart(comboBox2.Text));
            s5.AddEntry("DURATION_MINUTES", "0");
            s5.AddEntry("LAPS", numericUpDown3.Value.ToString());
            iniWriter.Sections.Add(s5);

            int cond = 0;
            foreach (ACCondition condition in acEvent.Conditions)
            {
                IniSection s6 = new IniSection("CONDITION_" + cond.ToString());
                s6.AddEntry("TYPE", condition.Type.ToString().ToUpper());
                s6.AddEntry("OBJECTIVE", condition.Value.ToString());
                iniWriter.Sections.Add(s6);
                cond++;
            }

            IniSection s7 = new IniSection("GHOST_CAR");

            if (checkBox1.Checked)
            {
                s7.AddEntry("RECORDER", "1");
                s7.AddEntry("PLAYING", "1");
                s7.AddEntry("ENABLED", "1");
            }
            else
            {
                s7.AddEntry("RECORDING", "0");
                s7.AddEntry("PLAYING", "0");
                s7.AddEntry("ENABLED", "0");
            }

            s7.AddEntry("SECONDS_ADVANTAGE", "0");
            s7.AddEntry("LOAD", "1");
            s7.AddEntry("FILE", "");
            iniWriter.Sections.Add(s7);

            IniSection s8 = new IniSection("LIGHTING");
            s8.AddEntry("SUN_ANGLE", "20");
            s8.AddEntry("TIME_MULT", "1");
            s8.AddEntry("CLOUD_SPEED", "0.2");
            iniWriter.Sections.Add(s8);

            IniSection s9 = new IniSection("GROOVE");
            s9.AddEntry("VIRTUAL_LAPS", "10");
            s9.AddEntry("MAX_LAPS", "1");
            s9.AddEntry("STARTING_LAPS", "1");
            iniWriter.Sections.Add(s9);

            IniSection s10 = new IniSection("TEMPERATURE");
            s10.AddEntry("AMBIENT", numericUpDown1.Value.ToString());
            s10.AddEntry("ROAD", numericUpDown2.Value.ToString());
            iniWriter.Sections.Add(s10);

            string path = Path.Combine(ACGame.EventsPath, "CUSTOM_SPECIAL_EVENT_" + guid.ToString());
            string iniPath = Path.Combine(path, "event.ini");

            MessageBox.Show("Your event will be saved at " + path, "AssettoCorsa Event Creator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Directory.CreateDirectory(path);
            iniWriter.Save(iniPath);

            string previewImage = label9.Text;

            if (File.Exists(previewImage))
            {
                try
                {
                    string previewImageNew = Path.Combine(path, "preview.png");
                    Image image = Image.FromFile(previewImage);

                    if (image.Width > 928 || image.Height > 522)
                    {
                        Bitmap bmp = new Bitmap(image, new Size(928, 522));
                        bmp.Save(previewImageNew, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else
                    {
                        File.Copy(previewImage, previewImageNew, true);
                    }
                }
                catch (UnauthorizedAccessException uaex)
                {
                    string r = uaex.Message;
                    MessageBox.Show("Failed to copy the preview image to the destition due to improper rights.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not copy the preview image to the desition : " + ex.Message);
                }
            }
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (acEvent.Drivers.Count == 24)
            {
                MessageBox.Show("Only 24 vehicles / drivers can be added to a race.");
                return;
            }

            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("You will not be able to select a vehicle setup if no track is selected.");
            }

            driverForm.SelectedTrack = comboBox1.Text;
            if (driverForm.ShowDialog() == DialogResult.OK)
            {
                ACDriver driver = driverForm.GetDriver();

                if (acEvent.Drivers.Count == 0)
                    driver.DriverType = ACDriverType.Human;

                acEvent.Drivers.Add(driver);
                listBox2.Items.Add(driver);
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int index = listBox2.SelectedIndex;
            ACDriver driver = (ACDriver)listBox2.SelectedItem;

            if (driver != null && index > 0)
            {
                driverForm.Name = driver.Name;
                driverForm.Vehicle = driver.Vehicle.Name;
                driverForm.VehicleSkin = driver.Vehicle.Skin;
                driverForm.AILevel = driver.AILevel;
                driverForm.DriverType = driver.DriverType;
                driverForm.Setup = driver.Setup;
                driverForm.SelectedTrack = comboBox1.Text;

                if (driverForm.ShowDialog() == DialogResult.OK)
                {
                    ACDriver updatedDriver = driverForm.GetDriver();
                    acEvent.Drivers[index] = updatedDriver;
                    listBox2.Items[index] = updatedDriver;
                }
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listBox2.Items.Count > 0)
            {
                ACDriver item = (ACDriver)listBox2.SelectedItem;
                acEvent.Drivers.Remove(item);
                listBox2.Items.Remove(item);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = comboBox2.Text;

            switch (type)
            {
                case "Race":
                    acEvent.EventType = ACEventType.Race;
                    break;
                case "Hotlap":
                    acEvent.EventType = ACEventType.Hotlap;
                    break;
                case "Drift":
                    acEvent.EventType = ACEventType.Drift;
                    break;
                case "Drag":
                    acEvent.EventType = ACEventType.Drag;
                    break;
                case "Timeattack":
                    acEvent.EventType = ACEventType.TimeAttack;
                    break;
            }

            if (type == "Race" || type == "AI")
            {
                label8.Visible = true;
                numericUpDown3.Visible = true;
            }
            else
            {
                label8.Visible = false;
                numericUpDown3.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                string ext = Path.GetExtension(file).ToLower();

                if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".bmp")
                    label9.Text = file;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int caret = textBox1.SelectionStart;
            int length = textBox1.SelectionLength;

            char lastChar = textBox1.Text[textBox1.Text.Length - 1];

            if (!char.IsLetterOrDigit(lastChar))
            {
                textBox1.Text = textBox1.Text.Remove(caret - 1, 1);
                caret -= 1;
            }

            textBox1.SelectionStart = caret;
            textBox1.SelectionLength = length;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            driverForm.SelectedTrack = comboBox1.Text;
        }
    }
}
