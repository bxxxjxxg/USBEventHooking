using System;
using System.Management;
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

            // Method 1
            // UsbEventHooker.RegisterNotification(this.Handle, UsbEventHooker.RecipientType.WINDOW_PROGRAM);
            // outputTextBox.AppendText("UsbEventHooker.RegisterNotification() ... done! \r\n");

            // Method 2
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();
            outputTextBox.AppendText("WqlEventQuery for insertion events... done! \r\n");

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            removeWatcher.Start();
            outputTextBox.AppendText("WqlEventQuery for removal events ... done! \r\n");
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

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            foreach (var property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            foreach (var property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }
    }
}
