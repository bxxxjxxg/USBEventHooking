using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USBEventHooking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            outputTextBox.AppendText("Form::Onload()\r\n");
            UsbEventHooker.RegisterNotification(this.Handle, UsbEventHooker.RecipientType.WINDOW_PROGRAM);
            outputTextBox.AppendText("UsbEventHooker.RegisterNotification() ... done! \r\n");
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == UsbEventHooker.WmDevicechange)
            {
                switch ((int)m.WParam)
                {
                    case UsbEventHooker.DbtDeviceremovecomplete:
                        outputTextBox.AppendText("UsbEventHooker.DbtDeviceremovecomplete\r\n");
                        break;
                    case UsbEventHooker.DbtDevicearrival:
                        outputTextBox.AppendText("UsbEventHooker.DbtDevicearrival\r\n");
                        break;
                }
            }
        }
    }
}
