using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using TheArtOfDev.HtmlRenderer.Adapters;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace testing
{
    public partial class Controls : Form
    {
        private Green_House_ _House_FORM;
        private int EC_int;
        private Green_House_ form1;
        private String text;
        private String text2;
        public Controls(String serial_test,String received)
        {

            InitializeComponent();
            set_time_hour_combobox();
            panel10.Visible = false;
            text = serial_test;
            text2 = received;
            Update_new_data(text2);
            all_labels();
            time_label();


        }
        private string ExtractValueBetweenCharacters(string message, char startChar, char endChar)
        {
            if (message != null)
            {
                int startIndex = message.IndexOf(startChar) + 1;
                int endIndex = message.IndexOf(endChar);
                if (startIndex >= 0 && endIndex > startIndex)
                {
                    return message.Substring(startIndex, endIndex - startIndex);
                }
            }
            return null;
        }

        private List<read_txt> read_txt_file(String path_file)
        {
            List<read_txt> all = new List<read_txt>();

            List<string> alldatalist = File.ReadAllLines(path_file).ToList();

            foreach (string line in alldatalist)
            {
                string[] split_ = line.Split(':');
                read_txt alldata = new read_txt();

                alldata.data_name = split_[0];
                alldata.data_value = split_[1];

                all.Add(alldata);
            }

            return all;
        }

        private void write_label_txt(System.Windows.Forms.Label label, String contains, String extension)
        {

            List<read_txt> reformdata = read_txt_file(@"C:\Users\Lenovo\source\repos\testing\TXT_files\Controls.txt");

            foreach (var data in reformdata)
            {
                if (data.data_name.Contains(contains))
                {
                    label.Text = data.data_value + extension;
                }
            }
        }

        private string write_label_txt_(String contains)
        {
            string new_data="";
            List<read_txt> reformdata = read_txt_file(@"C:\Users\Lenovo\source\repos\testing\TXT_files\Controls.txt");
            foreach (var data in reformdata)
            {
                if (data.data_name.Contains(contains))
                {
                    int value= int.Parse(data.data_value);
                    new_data = value.ToString("D2");
                }
            }
            return new_data;
        }

        private void all_labels()
        {

            write_label_txt(label36, "TemperatureMin", " °C");
            write_label_txt(label37, "TemperatureMax", " °C");

            write_label_txt(label39, "HumidityMin", " %RH");
            write_label_txt(label40, "HumidityMax", " %RH");

            write_label_txt(label45, "PHMin", " ");
            write_label_txt(label46, "PHMax", " ");

            write_label_txt(label42, "ECSMin", " ");
            write_label_txt(label43, "ECSMax", " ");

            write_label_txt(label52, "Lumen", " lm");

            write_label_txt(label59, "WLitr", " lt");
            write_label_txt(label61, "WHeight", " cm");

            label48.Text = $"{write_label_txt_("HourL")} : {write_label_txt_("MinuteL")} : {write_label_txt_("SecondL")}";
            label50.Text = $"{write_label_txt_("HourSL")} : {write_label_txt_("MinuteSL")} : {write_label_txt_("SecondSL")}";
            label56.Text = $"{write_label_txt_("HourWS")} : {write_label_txt_("MinuteWS")} : {write_label_txt_("SecondWS")}";
            label54.Text = $"{write_label_txt_("HourWP")} : {write_label_txt_("MinuteWP")} : {write_label_txt_("SecondWP")}";

            if (write_label_txt_("Valve")=="00")
            {
                label57.Text = "OFF";
                label57.ForeColor = Color.DarkRed;
            }
            else
            {
                label57.Text = "ON";
                label57.ForeColor = Color.DarkGreen;
            }
        }

        private void write_txt(String value_nname, String value)
        {

            string path_file = @"C:\Users\Lenovo\source\repos\testing\TXT_files\Controls.txt";

            List<string> lines = File.ReadAllLines(path_file).ToList();

            int a = lines.Count - 1;

            while (a >= 0)
            {
                if (lines[a].Contains(value_nname))
                {
                    lines[a] = $"{value_nname} : {value}";
                    break;
                }
                Console.WriteLine(a);
                a--;
            }
            File.WriteAllLines(path_file, lines);
        }

        private (string, string) SplitStringIntoTwo(string message, char separator)
        {
            if (message != null)
            {
                string[] parts = message.Split(separator);
                if (parts.Length == 2)
                {
                    int part1 = int.Parse(parts[0]);
                    int part2 = int.Parse(parts[1]);

                    return (part1.ToString("D2"), part2.ToString("D2"));
                }
            }
            return (null, null);
        }

        private (string, string,string) SplitStringIntoThree(string message, char separator)
        {
            if (message != null)
            {
                string[] parts = message.Split(separator);
                if (parts.Length == 3)
                {
                    int part1 = int.Parse(parts[0]);
                    int part2 = int.Parse(parts[1]);
                    int part3 = int.Parse(parts[2]);
                    return (part1.ToString("D2"), part2.ToString("D2"), part3.ToString("D2"));
                }
            }
            return (null, null,null);
        }



        public void Update_new_data(String data)
        {
            
            String temp=ExtractValueBetweenCharacters(data, 'Q', 'q');
            String humd = ExtractValueBetweenCharacters(data, 'R', 'r');
            String EC = ExtractValueBetweenCharacters(data, 'I', 'i');
            String PH = ExtractValueBetweenCharacters(data, 'F', 'f');
            String Water_tank= ExtractValueBetweenCharacters(data, 'U', 'u');
            String Lumenn = ExtractValueBetweenCharacters(data, 'Y', 'y');
            String valve = ExtractValueBetweenCharacters(data, 'V', 'v');
            String WTstring = ExtractValueBetweenCharacters(data, 'Z', 'z');
            String WPstring = ExtractValueBetweenCharacters(data, 'X', 'x');
            String Lstring = ExtractValueBetweenCharacters(data, 'G', 'g');
            String LSstring = ExtractValueBetweenCharacters(data, 'N', 'n');

            (string tpart1, string tpart2) = SplitStringIntoTwo(temp, '-');
            (string hpart1, string hpart2) = SplitStringIntoTwo(humd, '-');
            (string epart1, string epart2) = SplitStringIntoTwo(EC, '-');
            (string ppart1, string ppart2) = SplitStringIntoTwo(PH, '-');
            (string wpart1, string wpart2) = SplitStringIntoTwo(Water_tank, '-');
            (string lpart1, string lpart2) = SplitStringIntoTwo(Lumenn, '-');

            (string TSpart1, string TSpart2, string TSpart3) = SplitStringIntoThree(WTstring, ':');
            (string TPpart1, string TPpart2, string TPpart3) = SplitStringIntoThree(WPstring, ':');
            (string Lightpart1, string Lightpart2, string Lightpart3) = SplitStringIntoThree(Lstring, ':');
            (string SLightpart1, string SLightpart2, string SLightpart3) = SplitStringIntoThree(LSstring, ':');

            label36.Text = tpart1+ " °C";
            label37.Text = tpart2+ " °C";

            label39.Text = hpart1 + " %RH";
            label40.Text = hpart2 + " %RH";

            label42.Text = epart1;
            label43.Text = epart2;

            label45.Text = ppart1;
            label46.Text = ppart2;

            
            label52.Text = lpart1 + " lm";

            label61.Text = wpart2 + " cm";
            label59.Text = wpart1 + " lt";

            if (valve == "1")
            {
                label57.Text = "ON";
                label57.ForeColor = Color.DarkGreen;
            }
            else
            {
                label57.Text = "OFF";
                label57.ForeColor = Color.DarkRed;
            }

            label48.Text = $"{Lightpart1} : {Lightpart2} : {Lightpart3}";
            label50.Text = $"{SLightpart1} : {SLightpart2} : {SLightpart3}";
            label56.Text = $"{TSpart1} : {TSpart2} : {TSpart3}";
            label54.Text = $"{TPpart1} : {TPpart2} : {TPpart3}";;


            if(tpart1!=null&& tpart2 !=null&& TPpart3!=null)
            {
                write_txt("TemperatureMin", tpart1);
                write_txt("TemperatureMax", tpart2);

                write_txt("HumidityMin", hpart1);
                write_txt("HumidityMax", hpart2);

                write_txt("PHMin",ppart1);
                write_txt("PHmax", ppart2);

                write_txt("ECSMin", epart1);
                write_txt("ECSMax", epart2);

                write_txt("Lumen", lpart1);
                write_txt("HoursLight", lpart2);

                write_txt("WLitr", wpart1);
                write_txt("WHeight", wpart2);

                write_txt("HourL", Lightpart1);
                write_txt("MinuteL", Lightpart2);
                write_txt("SecondL", Lightpart3);

                write_txt("HourSL", SLightpart1);
                write_txt("MinuteSL", SLightpart2);
                write_txt("SecondSL", SLightpart3);

                write_txt("HourWS", TSpart1);
                write_txt("MinuteWS", TSpart2);
                write_txt("SecondWS", TSpart3);

                write_txt("HourWP", TPpart1);
                write_txt("MinuteWP", TPpart2);
                write_txt("SecondWP", TPpart3);

                write_txt("Valve", valve);
            }
        }

        public void SetMainForm(Green_House_ mainForm)
        {
            _House_FORM = mainForm;
        }


        private void set_time_hour_combobox()
        {
            for (int i = 40; i < 91; i++) 
            {
                Humdmin.Items.Add(i.ToString());
                Humdmax.Items.Add(i.ToString());
            }
            for (int i = 0; i < 60; i++)
            {
                if (i < 24)
                {
                    Hour_P_S.Items.Add(i.ToString("D2"));
                    Lhour.Items.Add(i.ToString("D2"));
                    LHour_h.Items.Add(i.ToString("D2"));
                }
                if (i < 16)
                {
                    PHmin.Items.Add(i.ToString());
                    PHmax.Items.Add(i.ToString());
                }
                if (i > 9 && i < 41)
                {
                    Tempmin.Items.Add(i.ToString());
                    Tempmax.Items.Add(i.ToString());    
                }
                if (i < 51)
                {
                    float x = Convert.ToSingle(i)/10;
                    ECmax.Items.Add(x.ToString());
                    ECmin.Items.Add(x.ToString());
                }
                if (i < 5)
                {
                    Hour_P_T.Items.Add(i.ToString("D2"));
                }
                Lminute.Items.Add(i.ToString("D2"));
                Lsecond.Items.Add(i.ToString("D2"));
                Minute_P_S.Items.Add(i.ToString("D2"));
                Second_P_S.Items.Add(i.ToString("D2"));
                Minute_P_T.Items.Add(i.ToString("D2"));
                Second_P_T.Items.Add(i.ToString("D2"));
                LHour_m.Items.Add(i.ToString("D2"));
                LHour_s.Items.Add(i.ToString("D2"));
            }
            Humdmax.SelectedIndex = 0;
            Humdmin.SelectedIndex = 0;

            Tempmax.SelectedIndex = 0;  
            Tempmin.SelectedIndex = 0;

            ECmax.SelectedIndex = 0;
            ECmin.SelectedIndex = 0;

            PHmax.SelectedIndex = 0;
            PHmin.SelectedIndex = 0;

            LHour_h.SelectedIndex = 0;
            LHour_m.SelectedIndex = 0;
            LHour_s.SelectedIndex = 0;

            Lminute.SelectedIndex = 0;
            Lsecond.SelectedIndex = 0;
            Lhour.SelectedIndex = 0;    

            Hour_P_S.SelectedIndex = 0;
            Minute_P_S.SelectedIndex = 0;
            Second_P_S.SelectedIndex = 0;

            Hour_P_T.SelectedIndex = 0;
            Minute_P_T.SelectedIndex = 0;
            Second_P_T.SelectedIndex = 0;



        }



        private void SendStringToMainForm(string message)
        {
            if (_House_FORM != null)
            {
                _House_FORM.ReceiveStringFromControl(message);
            }
        }



        private bool[] select = new bool[5];
        private bool select_new = true;
        private bool select_new1;

        private void rjButton1_Click(object sender, EventArgs e)

        {

            panel10.Visible = !panel10.Visible;
            panel12.Visible = false;
            panel3.Visible = false;
            panel5.Visible = false;
            new_panel.Visible = false;
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            panel12.Visible = !panel12.Visible;
            panel10.Visible = false;
            panel3.Visible = false;
            panel5.Visible = false;
            new_panel.Visible = false;
        }

        private void PH_EC_Click(object sender, EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
            panel12.Visible = false;
            panel10.Visible = false;
            panel5.Visible = false;
            new_panel.Visible = false;
        }

        private void Water_tank_Click(object sender, EventArgs e)
        {

            panel5.Visible = !panel5.Visible;
            panel12.Visible = false;
            panel10.Visible = false;
            panel3.Visible = false;
            new_panel.Visible = false;

        }

        private void Temp_Humdt_Click(object sender, EventArgs e)
        {

            new_panel.Visible = !new_panel.Visible;
            panel5.Visible = false;
            panel12.Visible = false;
            panel10.Visible = false;
            panel3.Visible = false;

        }


        private void Send_P_T_Click(object sender, EventArgs e)
        {
            try
            {
                int hour_ = Convert.ToInt16(Hour_P_S.Text);
                int second_ = Convert.ToInt16(Second_P_S.Text);
                int minute_ = Convert.ToInt16(Minute_P_S.Text);

                int hour = Convert.ToInt16(Hour_P_T.Text);
                int second = Convert.ToInt16(Second_P_T.Text);
                int minute = Convert.ToInt16(Minute_P_T.Text);

                if (hour < hour_)
                {
                    MessageBox.Show("Please select a valid time.\nStop time must be greater than start time minumum 1 hour ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                String time = "Motor:" + hour_ + "-" + minute_ + "_" + second_ +"+"+ hour + "-" + minute + "_" + second+";";

                SendStringToMainForm(time);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer.", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Lumen_h_send_Click(object sender, EventArgs e)
        {

            try
            {
                int hour_ = Convert.ToInt16(Lhour.Text);
                int minute_ = Convert.ToInt16(Lminute.Text);
                int second_ = Convert.ToInt16(Lsecond.Text);

                int hour = Convert.ToInt16(LHour_h.Text);
                int minute = Convert.ToInt16(LHour_m.Text);
                int second = Convert.ToInt16(LHour_s.Text);

                if (hour < hour_)
                {
                    MessageBox.Show("Please select a valid time.\nStop time must be greater than start time minumum 1 hour ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                String time = "Lumen:" + hour_ + "-" + minute_ + "_" + second_ +"+"+ hour + "-" + minute + "_" + second+";";

                SendStringToMainForm(time);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer.", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PHsend_Click(object sender, EventArgs e)
        {
            try
            {
                int min = Convert.ToInt16(PHmin.Text);
                int max = Convert.ToInt16(PHmax.Text);

                if (min < 0 || max < 0 || min > 15 || max > 15)
                {
                    MessageBox.Show("Please select a valid value.\nPH value must be between 0-15 ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (max < min)
                {
                    MessageBox.Show("Max value must be greater or equal than min value ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                String PHvalue_send = "PH:" + min + "-" + max + ";";

                SendStringToMainForm(PHvalue_send);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer.", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ECsend_Click(object sender, EventArgs e)
        {

            try
            {
                float min = Convert.ToSingle(ECmin.Text) * 10;
                float max = Convert.ToSingle(ECmax.Text) * 10;

                int min_num = Convert.ToInt16(min);
                int max_num = Convert.ToInt16(max);

                if (min_num < 0 || max_num < 0 || min_num > 50 || max_num > 50)
                {
                    MessageBox.Show("Please select a valid value.\nEC value must be between 0-5 ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (max_num < min_num)
                {
                    MessageBox.Show("Max value must be greater or equal than min value ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                String ECvalue_send = "EC:" + min_num + "-" + max_num + ";";

                SendStringToMainForm(ECvalue_send);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer.", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Temp_send_Click(object sender, EventArgs e)
        {
            try
            {
                int min = Convert.ToInt16(Tempmin.Text);
                int max = Convert.ToInt16(Tempmax.Text);

                if (min < 10 || max < 10 || min > 40 || max > 40)
                {
                    MessageBox.Show("Please select a valid value.\nTemperature value must be between 10-40 ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (max < min)
                {
                    MessageBox.Show("Max value must be greater or equal than min value ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                String ECvalue_send = "Temp:" + min + "-" + max + ";";

                SendStringToMainForm(ECvalue_send);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer.", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

 
        }

        private void Humd_send_Click(object sender, EventArgs e)
        {
            try
            {
                int min = Convert.ToInt16(Humdmin.Text);
                int max = Convert.ToInt16(Humdmax.Text);

                if (min < 40 || max < 40 || min > 90 || max > 90)
                {
                    MessageBox.Show("Please select a valid value.\nHumidty value must be between 40-90 ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (max < min)
                {
                    MessageBox.Show("Max value must be greater or equal than min value ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                String ECvalue_send = "Humd:" + min + "-" + max + ";";

                SendStringToMainForm(ECvalue_send);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer.", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

 
        private void Lumensend_Click(object sender, EventArgs e)
        {
            int lumen_value = Convert.ToInt16(lumentextbox.Text);
            if (lumen_value < 100 || lumen_value > 800)
            {
                MessageBox.Show("Please select a valid value.\nLumen value must be between 100-800 ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            String LM = "Light:" + lumen_value + "-" + "0" + ";";

            SendStringToMainForm(LM);
        }

        

        private void SizeSend_Click(object sender, EventArgs e)
        {
   
            try
            {
                int Liter = Convert.ToInt16(LTtextbox.Text);
                int Height = Convert.ToInt16(HGtextbox.Text);
                if (Liter < 5 || Liter > 2000)
                {
                    MessageBox.Show("Please select a valid value.\nWater tank`s liters value must be between 5-2000 ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Height < 25 || Height > 200)
                {
                    MessageBox.Show("Please select a valid value.\nWater tank`s height value hours must be between 25-200 ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                String WT = "WTank:" + Liter + "-" + Height + ";";

                SendStringToMainForm(WT);

            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer.", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Valve_send_Click(object sender, EventArgs e)
        {
            if (!ON.Checked&& !OFF.Checked)
            {
                MessageBox.Show("Please select a valid selection ", "ERROR !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ON.Checked) 
            {
                SendStringToMainForm("Valve:1;");
            }
            if (OFF.Checked) 
            {
                SendStringToMainForm("Valve:0;");
            }
            
        }
        Timer tmr;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToString("HH : mm : ss");
        }

        private void time_label()
        {
            tmr = new Timer();
            tmr.Interval = 1000;
            tmr.Tick += new EventHandler( timer1_Tick);
            tmr.Enabled = true;
        }


    }

}
