using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using static testing.Green_House_;


namespace testing
{
    public partial class Serial_port : Form
    {
        private Green_House_ _House_FORM;
        private string _situation, _data;
        private int button1, button2;
        public Serial_port(string situation, string data)
        {
            InitializeComponent();
            foreach (var PORT_name in SerialPort.GetPortNames())
            {
                Name_PORT.Items.Add(PORT_name);
            }
            Name_PORT.SelectedIndex = 0;
            Baund_Rate.SelectedIndex = 0;

        }


        public void Update_new_data(string data)
        {
            richTextBox1.Invoke(new show_data(read_data), data);
        }
        public void read_data(string data)
        {
            richTextBox1.Text += data;
        }

        private void Connect_Click(object sender, EventArgs e)

        {
            button1++;
            button2 = 0;
            if (button1 == 1)
            {
                string port = Name_PORT.Text;
                string baund = Baund_Rate.Text;
                _House_FORM.receive_data_form(port, baund, "OPEN");
            }

        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            button2++;
            button1 = 0;
            if (button2 == 1)
            {
                _House_FORM.receive_data_form("", "", "CLOSE");
            }
            
        }

        private void Send_Click(object sender, EventArgs e)
        {
            _House_FORM.send_data(textBox1.Text + "\n");
        }

        public void SetMainForm(Green_House_ mainForm)
        {
            _House_FORM = mainForm;
        }
    }
}

