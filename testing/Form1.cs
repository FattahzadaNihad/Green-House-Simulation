using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static testing.Serial_port;
using System.Xml.Linq;

namespace testing
{
    public partial class Green_House_ : Form
    {
        private Dictionary<Control, (Rectangle originalBounds, Size originalParentSize)> controlData;
        private List<Control> originalPanelControls;
        private StringBuilder _serialBuffer = new StringBuilder();
        private Sensor sensorForm;
        private Controls controls_form;
        private Serial_port serial;
        private Green_House_ green_House_FORM;
        private String chechkSerialText;
        private String controls_data;
        private Timer timer;

        public Green_House_()
        {
            InitializeComponent();
            custumize_design();
            originalPanelControls = new List<Control>();
            controlData = new Dictionary<Control, (Rectangle, Size)>();
            this.Load += new EventHandler(MainForm_Load);
            this.Resize += new EventHandler(MainForm_Resize);
            motor_sitution();
            
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


        private string write_label_txt_(String contains)
        {
            string value = "";

            List<read_txt> reformdata = read_txt_file(@"C:\Users\Lenovo\source\repos\testing\TXT_files\Home.txt");

            foreach (var data in reformdata)
            {
                if (data.data_name.Contains(contains))
                {

                    value=data.data_value;

                }
            }
            return value;
        }
        private void write_label_txt(System.Windows.Forms.Label label, String contains, String extension)
        {

            List<read_txt> reformdata = read_txt_file(@"C:\Users\Lenovo\source\repos\testing\TXT_files\Home.txt");

            foreach (var data in reformdata)
            {
                if (data.data_name.Contains(contains))
                {
                    label.Text = data.data_value + extension;
                }
            }
        }

        private void set_label(string name,Label labelname)
        {

            if (int.Parse(write_label_txt_(name))==0)
            {
                labelname.Text = "Passive";
                labelname.BackColor = Color.DarkRed;
            }
            else
            {
                labelname.Text = "Active";
                labelname.BackColor = Color.DarkGreen;
            }
        }

        private void motor_sitution ()
        {
            set_label("Water_pump",label13);
            set_label("Water_tank", label12);
            set_label("Lamp", label11);
            set_label("Heater", label10);
            set_label("Ventilation", label9);
            set_label("Humiliation", label8);
            set_label("Valve", label7);


        }



        public void ReceiveStringFromControl(string receivedString)

        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(receivedString);
                Serial_data_situation = "OPEN";

            }

        }
        public void CheckSerialPortStatus()
        {
            if (serialPort1.IsOpen)
            {
                chechkSerialText = "good";
            }
            else
            {
                chechkSerialText = "bad";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SaveControlBounds(Workspace);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ResizeControls(Workspace);
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
        private void custumize_design()
        {
            DATAPANEL.Visible = false;
        }

        private void hideMenu()
        {
            if (CONTROLS.Visible == true)
                DATAPANEL.Visible = false;
        }




        public string extractedValueH;
        public string extractedValueT;

        public string Serial_data_situation;
        public string Serial_data;





        public void receive_data_form(string name, string baund, string data)
        {
            if (data == "OPEN")
            {
                serialPort1.PortName = name;
                serialPort1.BaudRate = Convert.ToInt32(baund);
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.DataBits = 8;
                try
                {
                    serialPort1.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Serial port connection failed\n ERROR : {ex.Message}", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (data == "CLOSE")
            {
                serialPort1.Close();
                Serial_data_situation = "CLOSE";
            }

        }

        public void send_data (string data)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(data);
            }

        }



        public delegate void show_data(string data);

        public void ExtractAndInvoke(string extractedValue)
        {
            if (!string.IsNullOrEmpty(extractedValue) && sensorForm != null && !sensorForm.IsDisposed)
            {
                sensorForm.Invoke(new Action(() => sensorForm.UpdateSensorData(extractedValue)));
            }

            if (!string.IsNullOrEmpty(extractedValue) && controls_form != null && !controls_form.IsDisposed)
            {
                controls_form.Invoke(new Action(() => controls_form.Update_new_data(extractedValue)));
            }
        }

        private void UpdateLabelTextSafely(Label label, string text, Color backColor)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => UpdateLabelTextSafely(label, text, backColor)));
            }
            else
            {
                label.Text = text;
                label.BackColor = backColor;
            }
        }

        private void write_text_st(Label label,int value,int true_situation)
        {
            if (Convert.ToBoolean(value & true_situation))
            {
                UpdateLabelTextSafely(label, "Active", Color.DarkGreen);
            }
            else
            {
                UpdateLabelTextSafely(label, "Passive", Color.DarkRed);
            }
        }

        private void write_txt(String value_nname, String value)
        {

            string path_file = @"C:\Users\Lenovo\source\repos\testing\TXT_files\Home.txt";

            List<string> lines = File.ReadAllLines(path_file).ToList();

            int a = lines.Count - 1;

            while (a >= 0)
            {
                if (lines[a].Contains(value_nname))
                {
                    lines[a] = $"{value_nname} : {value}";
                    break;
                }
                a--;
            }
            File.WriteAllLines(path_file, lines);
        }

        private string reverseboolstr(int name,int trusty)
        {
            string boolname="";
            if((Convert.ToBoolean(name & trusty)).ToString()=="True")
            {
                boolname = "1";
            }
            else 
            {
                boolname = "0";
            }
            return boolname;
        }
        private void Update_new_data_(string value)
        {
            string binary_value = ExtractValueBetweenCharacters(value, 'B', 'b');
            int newvalue = (int.Parse(binary_value));
            if (binary_value != null) 
            {
                write_txt("Water_pump", reverseboolstr(newvalue, 1));
                write_txt("Ventilation", reverseboolstr(newvalue, 2));
                write_txt("Lamp", reverseboolstr(newvalue, 4));
                write_txt("Heater", reverseboolstr(newvalue, 8));
                write_txt("Valve", reverseboolstr(newvalue, 16));
                write_txt("Water_tank", reverseboolstr(newvalue,32));
                write_txt("Humiliation", reverseboolstr(newvalue, 64));

            }
            
            write_text_st(label13, newvalue, 1);
            write_text_st(label12, newvalue, 32);
            write_text_st(label11, newvalue, 4);
            write_text_st(label10, newvalue, 8);
            write_text_st(label9, newvalue, 2);
            write_text_st(label8, newvalue, 64);
            write_text_st(label7, newvalue, 16);


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

        public void ExtractAndInvoke_(string extractedValue)
        {
            if (!string.IsNullOrEmpty(extractedValue) && serial != null && !serial.IsDisposed)
            {
                serial.Invoke(new Action(() => serial.Update_new_data(extractedValue)));
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string receivedData = serialPort1.ReadExisting();


                ExtractAndInvoke_(receivedData);

                _serialBuffer.Append(receivedData);

                if (_serialBuffer.ToString().Contains("S"))
                {
                    string[] parts = _serialBuffer.ToString().Split('S');
                    string completeMessage = parts[0];
                    _serialBuffer.Clear();
                    if (parts.Length > 1)
                    {
                        _serialBuffer.Append(parts[1]);
                    }

                    ExtractAndInvoke(completeMessage);
                    Update_new_data_(completeMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Data processing failed\n ERROR: {ex.Message}", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void showMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            showMenu(DATAPANEL);
        }

        private Form activeForm = null;
        private short serial_pages = 0;
        private short sensor_pages = 0;
        private short controls_pages = 0;
        
        

        private void SavePanelControls()
        {
            originalPanelControls.Clear();
            foreach (Control control in Workspace.Controls)
            {
                originalPanelControls.Add(control);
            }
        }

        private void RestorePanelControls()
        {
            Workspace.Controls.Clear();
            foreach (Control control in originalPanelControls)
            {
                Workspace.Controls.Add(control);
            }
        }

        private void Show_form(Form form, Panel workspace_)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            workspace_.Controls.Clear();
            workspace_.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        private int[] pages=new int [3];
        private int home_pages = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            home_pages = 2;
            if (pages[0] == 0&& pages[1] == 0 && pages[2] == 0)
            {
                SavePanelControls();
            }
            pages[0]++;
            serial = new Serial_port(Serial_data_situation, Serial_data);
            ((Serial_port)serial).SetMainForm(this);
            Show_form(serial, Workspace);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            home_pages = 2;
            if (pages[0] == 0 && pages[1] == 0 && pages[2] == 0)
            {
                SavePanelControls();
            }
            pages[1]++;
            sensorForm = new Sensor(extractedValueH);
            ((Sensor)sensorForm).MainForm(this);
            Show_form(sensorForm, Workspace);

        }

        private void CONTROLS_Click(object sender, EventArgs e)
        {
            home_pages = 2;
            if (pages[0] == 0 && pages[1] == 0 && pages[2] == 0)
            {
                SavePanelControls();
            }
            pages[2]++;
            CheckSerialPortStatus();

            controls_form = new Controls(chechkSerialText, extractedValueH);
            ((Controls)controls_form).SetMainForm(this);
            Show_form(controls_form, Workspace);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (home_pages == 1)
            {
                SavePanelControls();
            }
            home_pages = 2;
            pages[0]=0;
            pages[1]=0;
            pages[2]=0;
            RestorePanelControls();

        }


    }
}
