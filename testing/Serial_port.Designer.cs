namespace testing
{
    partial class Serial_port
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Name_PORT = new System.Windows.Forms.ComboBox();
            this.Baund_Rate = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.Connect = new System.Windows.Forms.Button();
            this.disconnect = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Send = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Name_PORT
            // 
            this.Name_PORT.Font = new System.Drawing.Font("Arial Narrow", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Name_PORT.FormattingEnabled = true;
            this.Name_PORT.Location = new System.Drawing.Point(83, 88);
            this.Name_PORT.Name = "Name_PORT";
            this.Name_PORT.Size = new System.Drawing.Size(176, 45);
            this.Name_PORT.TabIndex = 3;
            // 
            // Baund_Rate
            // 
            this.Baund_Rate.Font = new System.Drawing.Font("Arial Narrow", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Baund_Rate.FormattingEnabled = true;
            this.Baund_Rate.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.Baund_Rate.Location = new System.Drawing.Point(318, 88);
            this.Baund_Rate.Name = "Baund_Rate";
            this.Baund_Rate.Size = new System.Drawing.Size(176, 45);
            this.Baund_Rate.TabIndex = 3;
            // 
            // Connect
            // 
            this.Connect.Font = new System.Drawing.Font("Arial Narrow", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Connect.Location = new System.Drawing.Point(83, 170);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(205, 50);
            this.Connect.TabIndex = 4;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // disconnect
            // 
            this.disconnect.Font = new System.Drawing.Font("Arial Narrow", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.disconnect.Location = new System.Drawing.Point(298, 170);
            this.disconnect.Name = "disconnect";
            this.disconnect.Size = new System.Drawing.Size(196, 50);
            this.disconnect.TabIndex = 4;
            this.disconnect.Text = "Disconnect";
            this.disconnect.UseVisualStyleBackColor = true;
            this.disconnect.Click += new System.EventHandler(this.disconnect_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.textBox1.Location = new System.Drawing.Point(83, 256);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(411, 44);
            this.textBox1.TabIndex = 6;
            // 
            // Send
            // 
            this.Send.Font = new System.Drawing.Font("Arial Narrow", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Send.Location = new System.Drawing.Point(182, 316);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(205, 50);
            this.Send.TabIndex = 4;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(595, 88);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(503, 278);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            // 
            // Serial_port
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1361, 584);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.disconnect);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.Baund_Rate);
            this.Controls.Add(this.Name_PORT);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Serial_port";
            this.Text = "Serial_port";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox Name_PORT;
        private System.Windows.Forms.ComboBox Baund_Rate;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Button disconnect;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}