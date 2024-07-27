using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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

    public partial class Sensor : Form
    {
        private Dictionary<Control, (Rectangle originalBounds, Size originalParentSize)> controlData;

        private Queue<(DateTime, string)> HumSensorData = new Queue<(DateTime, string)>();
        private Queue<(DateTime, string)> TemSensorData = new Queue<(DateTime, string)>();
        private Queue<(DateTime, string)> LightSensorData = new Queue<(DateTime, string)>();
        private Queue<(DateTime, string)> PHSensorData = new Queue<(DateTime, string)>();
        private Queue<(DateTime, string)> ECSensorData = new Queue<(DateTime, string)>();
        private Queue<(DateTime, string)> ECTempSensorData = new Queue<(DateTime, string)>();
        private Queue<(DateTime, string)> WSpeedSensorData = new Queue<(DateTime, string)>();
        private Queue<(DateTime, string)> WDirectionSensorData = new Queue<(DateTime, string)>();

        private short[] toggle = new short[8];

        private string sensorData1;

        private AnimatedPanel panel_1;
        private AnimatedPanel panel_2;
        private AnimatedPanel panel_3;
        private AnimatedPanel panel_4;
        private AnimatedPanel panel_5;
        private AnimatedPanel panel_6;
        private int percentage, wt1, wt2;




        public Sensor(string initialData)
        {
            InitializeComponent();
            sensorData1 = initialData;
            UpdateSensorData(sensorData1);
            

            Timer timer1 = new Timer();
            timer1.Interval = 5000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();

            panel_1 = new AnimatedPanel(Temp_panel);
            panel_2 = new AnimatedPanel(Humidity_panel);
            panel_3 = new AnimatedPanel(Light_panel);
            panel_4 = new AnimatedPanel(PH_panel);
            panel_5 = new AnimatedPanel(EC_panel);
            panel_6 = new AnimatedPanel(Wind__panel);

            controlData = new Dictionary<Control, (Rectangle, Size)>();

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

        private void write_label_txt(System.Windows.Forms.Label label, String contains,String extension)
        {

            List<read_txt> reformdata = read_txt_file(@"C:\Users\Lenovo\source\repos\testing\TXT_files\Sensor.txt.txt");
            
            foreach (var data in reformdata)
            {
                if (data.data_name.Contains(contains))
                {
                    label.Text = data.data_value+ extension;
                }
            }
        }

        private void write_txt(String value_nname,String value)
        {

            string path_file = @"C:\Users\Lenovo\source\repos\testing\TXT_files\Sensor.txt.txt";

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

        private int write_label_txt_(String contains)
        {
            int value=0;

            List<read_txt> reformdata = read_txt_file(@"C:\Users\Lenovo\source\repos\testing\TXT_files\Sensor.txt.txt");

            foreach (var data in reformdata)
            {
                if (data.data_name.Contains(contains))
                {

                    int.TryParse(data.data_value, out value);

                }
            }
            return value;
        }

        private void write_all_label()
        {
            write_label_txt(TEMP_LABEL, "Temperature", " °C");
            write_label_txt(Hum_label, "Humidity", " %RH");
            write_label_txt(LUX_label, "Light", " LUX");
            write_label_txt(PH_label, "PH", " ph");
            write_label_txt(EC_label, "ECS", "");
            write_label_txt(EC_label_temp, "T_EC", " °C");
            write_label_txt(Wind_d, "Wind_d", "");
            write_label_txt(Wind_s, "Wind_s", " km/h");

        }


            


        private void ResizeControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (controlData.ContainsKey(control))
                {
                    var (originalBounds, originalParentSize) = controlData[control];
                    float xRatio = (float)parent.ClientSize.Width / originalParentSize.Width;
                    float yRatio = (float)parent.ClientSize.Height / originalParentSize.Height;

                    int newX = (int)(originalBounds.X * xRatio);
                    int newY = (int)(originalBounds.Y * yRatio);
                    int newWidth = (int)(originalBounds.Width * xRatio);
                    int newHeight = (int)(originalBounds.Height * yRatio);

                    control.Bounds = new Rectangle(newX, newY, newWidth, newHeight);

                    if (control.HasChildren)
                    {
                        ResizeControls(control);
                    }
                }
            }
        }

        private void SaveControlBounds(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                controlData[control] = (control.Bounds, parent.ClientSize);
                if (control.HasChildren)
                {
                    SaveControlBounds(control);
                }
            }
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
        

        private void Litr_Paint(object sender, PaintEventArgs e)
        {
            Font myfont = new Font("Arial", 30, FontStyle.Bold);
            Brush mybrush = new SolidBrush(Color.White);
            string text_litr = (wt1).ToString() + "Lt";

            e.Graphics.TranslateTransform(60, 40);
            e.Graphics.RotateTransform(90);
            e.Graphics.DrawString(text_litr, myfont, mybrush, 0, 0);
        }

        private void EnqueueAndTrimQueue(Queue<(DateTime, string)> sensorData, DateTime currentTime, string newData)
        {

            sensorData.Enqueue((currentTime, newData));


            while (sensorData.Count > 0 && (currentTime - sensorData.Peek().Item1).TotalSeconds > 5)
            {
                sensorData.Dequeue();
            }
        }
        public void UpdateSensorData(string value)
        {

            string temp = ExtractValueBetweenCharacters(value, 'T', 't');
            string humdt = ExtractValueBetweenCharacters(value, 'H', 'h');
            string EC = ExtractValueBetweenCharacters(value, 'E', 'e');
            string ECtemp = ExtractValueBetweenCharacters(value, 'K', 'k');
            string LDR = ExtractValueBetweenCharacters(value, 'L', 'l');
            string PH = ExtractValueBetweenCharacters(value, 'P', 'p');
            string Waterlevel = ExtractValueBetweenCharacters(value, 'W', 'w');
            string Waterlevel1 = ExtractValueBetweenCharacters(value, 'J', 'j');
            string Wind_direction = ExtractValueBetweenCharacters(value, 'D', 'd');
            string Wind = ExtractValueBetweenCharacters(value, 'O', 'o');

            switch (Wind_direction)
            {
                case "0":
                    Wind_direction = "NORTH";
                    break;
                case "1":
                    Wind_direction = "NORT WEST";
                    break;
                case "2":
                    Wind_direction = "WEST";
                    break;
                case "3":
                    Wind_direction = "SOUTH WEST";
                    break;
                case "4":
                    Wind_direction = "SOUTH";
                    break;
                case "5":
                    Wind_direction = "SOUTH EAST";
                    break;
                case "6":
                    Wind_d.Text = "EAST";
                    break;
                case "7":
                    Wind_direction = "NORTH EAST";
                    break;
                case "8":
                    Wind_direction = "NORTH";
                    break;
            }

            TEMP_LABEL.Text = temp + "°C";
            Hum_label.Text = humdt + "%RH";
            LUX_label.Text = LDR + "LUX";
            PH_label.Text = PH + "ph";
            EC_label.Text = EC;
            EC_label_temp.Text = ECtemp + "°C";
            Wind_d.Text = Wind_direction;
            Wind_s.Text = Wind + "km/h";

            if (temp != null&&humdt!=null&&LDR!=null&& PH!=null&& EC!=null&& ECtemp!=null&&Wind_direction!=null&& Wind!= null)
            {
                write_txt("Temperature", temp);
                write_txt("Humidity", humdt);
                write_txt("Light", LDR);
                write_txt("PH", PH);
                write_txt("ECS", EC);
                write_txt("T_EC", ECtemp);
                write_txt("Wind_d", Wind_direction);
                write_txt("Wind_s", Wind);
                write_txt("TankL", Waterlevel);
                write_txt("TankH", Waterlevel1);

                label1.Text = "CURRENT";
                label2.Text = "CURRENT";
                label3.Text = "CURRENT";
                label4.Text = "CURRENT";
                label6.Text = "CURRENT";
                label8.Text = "CURRENT";


            }
            else
            {
                write_all_label();
                label1.Text = "LAST";
                label2.Text = "LAST";
                label3.Text = "LAST";
                label4.Text = "LAST";
                label6.Text = "LAST";
                label8.Text = "LAST";
            }

            if( Waterlevel1 != null && Waterlevel != null)
            {
                int.TryParse(Waterlevel, out wt1);
                int.TryParse(Waterlevel1, out wt2);
            }
            else
            {
                wt1 = write_label_txt_("TankL");
                wt2 = write_label_txt_("TankH");
            }
            if(wt1!=0&& wt2 != 0)
            {
                percentage = wt1 * 100 / wt2;
                Water_percentage.Text = Convert.ToString(percentage) + "%";
                Water_tank.Value = percentage;
                Litr.Invalidate();
            }
           




            DateTime currentTime = DateTime.Now;

            EnqueueAndTrimQueue(HumSensorData, currentTime, humdt);
            EnqueueAndTrimQueue(ECSensorData, currentTime, EC);
            EnqueueAndTrimQueue(TemSensorData, currentTime, temp);
            EnqueueAndTrimQueue(PHSensorData, currentTime, PH);
            EnqueueAndTrimQueue(LightSensorData, currentTime, LDR);
            EnqueueAndTrimQueue(ECTempSensorData, currentTime, ECtemp);
            EnqueueAndTrimQueue(WSpeedSensorData, currentTime, Wind);
            EnqueueAndTrimQueue(WDirectionSensorData, currentTime, Wind_direction);


            float LUX_value;
            if (LDR != null)
            {
                LUX_value = float.Parse(LDR);
            }
            else
            {
                LUX_value = 0;
            }


            if (LUX_value > 1000)
            {
                SUN.Visible = true;
                CSUN.Visible = false;
                MOON.Visible = false;
                CMOON.Visible = false;
            }
            else if (LUX_value < 1000 && LUX_value > 100)
            {
                SUN.Visible = false;
                CSUN.Visible = true;
                MOON.Visible = false;
                CMOON.Visible = false;
            }
            else if (LUX_value < 100 && LUX_value > 10)
            {
                SUN.Visible = false;
                CSUN.Visible = false;
                MOON.Visible = false;
                CMOON.Visible = true;
            }
            else
            {
                SUN.Visible = false;
                CSUN.Visible = false;
                MOON.Visible = true;
                CMOON.Visible = false;
            }




            

            
        }


        private short[] pages = new short[6];
        private void TEMPERATURE_Click(object sender, EventArgs e)
        {

            panel_1.ToggleVisibility();
        }

        private void HUMIDTY_Click(object sender, EventArgs e)
        {
            panel_2.ToggleVisibility();
        }

        private void LIGHT__Click(object sender, EventArgs e)
        {
            panel_3.ToggleVisibility();
        }

        private void PH_Sensor_Click(object sender, EventArgs e)
        {
            panel_4.ToggleVisibility();
        }

        private void EC_sensor_Click(object sender, EventArgs e)
        {
            panel_5.ToggleVisibility();
        }

        private void Wind_Click(object sender, EventArgs e)
        {
            panel_6.ToggleVisibility();
        }



        private void UpdateSensorLabels(Queue<(DateTime, string)> sensorData, System.Windows.Forms.Label timeLabel1, System.Windows.Forms.Label timeLabel2, System.Windows.Forms.Label valueLabel1, System.Windows.Forms.Label valueLabel2, short[] toggle, int i)
        {
            DateTime currentTime = DateTime.Now;

            if (sensorData.Count > 0 && (currentTime - sensorData.Peek().Item1).TotalSeconds >= 5)
            {
                string dataFiveSecondsAgo = sensorData.Peek().Item2;
                string data = currentTime.AddSeconds(-5).ToString("HH:mm:ss");

                if (toggle[i] == 0)
                {
                    timeLabel1.Text = data;
                    valueLabel1.Text = dataFiveSecondsAgo;
                    toggle[i] = 1;
                }
                else if (toggle[i] == 1)
                {
                    timeLabel2.Text = data;
                    valueLabel2.Text = dataFiveSecondsAgo;
                    toggle[i] = 2;
                }
                else
                {
                    timeLabel1.Text = timeLabel2.Text;
                    valueLabel1.Text = valueLabel2.Text;
                    timeLabel2.Text = data;
                    valueLabel2.Text = dataFiveSecondsAgo;
                }
            }
        }

        private void UpdateSensorLabels_SP(Queue<(DateTime, string)> sensorData, System.Windows.Forms.Label valueLabel1, System.Windows.Forms.Label valueLabel2, short[] toggle, int i)
        {
            DateTime currentTime = DateTime.Now;

            if (sensorData.Count > 0 && (currentTime - sensorData.Peek().Item1).TotalSeconds >= 5)
            {
                string dataFiveSecondsAgo = sensorData.Peek().Item2;
                string data = currentTime.AddSeconds(-5).ToString("HH:mm:ss");

                if (toggle[i] == 0)
                {
                    valueLabel1.Text = dataFiveSecondsAgo;
                    toggle[i] = 1;
                }
                else if (toggle[i] == 1)
                {
                    valueLabel2.Text = dataFiveSecondsAgo;
                    toggle[i] = 2;
                }
                else
                {
                    valueLabel1.Text = valueLabel2.Text;
                    valueLabel2.Text = dataFiveSecondsAgo;
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateSensorLabels(HumSensorData, Htime1, Htime2, hum1, hum2, toggle, 0);
            UpdateSensorLabels(TemSensorData, Ttime1, Ttime2, Tem1, Tem2, toggle, 1);
            UpdateSensorLabels(LightSensorData, Ltime1, Ltime2, Light1, Light2, toggle, 2);
            UpdateSensorLabels(PHSensorData, Ptime1, Ptime2, PH1, PH2, toggle, 3);
            UpdateSensorLabels(ECSensorData, Etime1, Etime2, E1, E2, toggle, 4);
            UpdateSensorLabels(WDirectionSensorData, Wtime1, Wtime2, W1, W2, toggle, 5);
            UpdateSensorLabels_SP(ECTempSensorData, ET1, ET2, toggle, 6);
            UpdateSensorLabels_SP(WSpeedSensorData, S1, S2, toggle, 7);
        }



        public void MainForm(Green_House_ mainForm)
        {
        }
    }

}

