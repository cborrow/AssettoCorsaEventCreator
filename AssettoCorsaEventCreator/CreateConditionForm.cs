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
    public partial class CreateConditionForm : Form
    {
        public int Wins
        {
            get { return (int)numericUpDown1.Value; }
            internal set { numericUpDown1.Value = value; }
        }

        public int Points
        {
            get { return (int)numericUpDown2.Value; }
            internal set { numericUpDown2.Value = value; }
        }

        public TimeSpan Time
        {
            get { return new TimeSpan(0, (int)numericUpDown3.Value, (int)numericUpDown4.Value); }
            internal set
            {
                TimeSpan ts = value;
                numericUpDown3.Value = ts.Minutes;
                numericUpDown4.Value = ts.Seconds;
            }
        }

        public int Position
        {
            get { return (int)numericUpDown5.Value; }
            internal set { numericUpDown5.Value = value; }
        }

        public int AILevel
        {
            get { return (int)numericUpDown6.Value; }
            set { numericUpDown6.Value = value; }
        }

        public CreateConditionForm()
        {
            InitializeComponent();
        }

        public ACCondition GenerateCondition()
        {
            ACCondition acc = new ACCondition();

            if (comboBox1.Text == "Hotlap")
            {
                acc.Type = ACConditionType.Time;
                acc.EventType = ACEventType.Hotlap;
                acc.Value = (int)this.Time.TotalMilliseconds;
            }
            else if (comboBox1.Text == "Drift")
            {
                acc.Type = ACConditionType.Points;
                acc.EventType = ACEventType.Drift;
                acc.Value = this.Points;
            }
            else if (comboBox1.Text == "Checkpoint")
            {
                acc.Type = ACConditionType.Points;
                acc.EventType = ACEventType.TimeAttack;
                acc.Value = this.Points;
            }
            else if (comboBox1.Text == "Drag")
            {
                acc.Type = ACConditionType.Wins;
                acc.EventType = ACEventType.Drag;
                acc.Value = this.Wins;
            }
            else if (comboBox1.Text == "Race")
            {
                acc.Type = ACConditionType.Position;
                acc.EventType = ACEventType.Race;
                acc.Value = this.Position;
            }
            else if (comboBox1.Text == "AI")
            {
                acc.Type = ACConditionType.AI;
                acc.EventType = ACEventType.Race;
                acc.Value = this.AILevel;
            }

            return acc;
        }

        public void EditCondition(ACCondition acc)
        {
            if (acc.EventType == ACEventType.Drag)
            {
                this.Wins = acc.Value;
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Drag");
            }
            else if (acc.EventType == ACEventType.Drift)
            {
                this.Points = acc.Value;
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Drift");
            }
            else if (acc.EventType == ACEventType.Hotlap)
            {
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, acc.Value);
                this.Time = ts;
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Hotlap");
            }
            else if (acc.EventType == ACEventType.Race)
            {
                if (acc.Type == ACConditionType.AI)
                    this.AILevel = acc.Value;
                else
                    this.Position = acc.Value;

                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Race");
            }
            else if (acc.EventType == ACEventType.TimeAttack)
            {
                this.Points = acc.Value;
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Checkpoint");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //First label position 12,57
            //First numeric box position 12, 77
            //Second label position 65, 77
            //Second numerix box position 83, 77

            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;

            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            numericUpDown3.Visible = false;
            numericUpDown4.Visible = false;
            numericUpDown5.Visible = false;
            numericUpDown6.Visible = false;

            label2.Location = new Point(12, 57);
            label3.Location = new Point(12, 57);
            label4.Location = new Point(12, 57);
            label5.Location = new Point(65, 77);
            label6.Location = new Point(12, 57);
            label7.Location = new Point(12, 57);

            numericUpDown1.Location = new Point(12, 77);
            numericUpDown2.Location = new Point(12, 77);
            numericUpDown3.Location = new Point(12, 77);
            numericUpDown4.Location = new Point(83, 77);
            numericUpDown5.Location = new Point(12, 77);
            numericUpDown6.Location = new Point(12, 77);

            if (comboBox1.Text == "Hotlap")
            {
                label4.Visible = true;
                label5.Visible = true;
                numericUpDown3.Visible = true;
                numericUpDown4.Visible = true;
            }
            else if (comboBox1.Text == "Drift" || comboBox1.Text == "Checkpoint")
            {
                label3.Visible = true;
                numericUpDown2.Visible = true;
            }
            else if (comboBox1.Text == "Drag")
            {
                label2.Visible = true;
                numericUpDown1.Visible = true;
            }
            else if (comboBox1.Text == "Race")
            {
                label6.Visible = true;
                numericUpDown5.Visible = true;
            }
            else if (comboBox1.Text == "AI")
            {
                label7.Visible = true;
                numericUpDown6.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
