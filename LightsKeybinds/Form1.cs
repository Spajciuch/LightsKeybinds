using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace LightsKeybinds
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        const int MYACTION_HOTKEY_ID = 1;

        public Form1()
        {
            InitializeComponent();
            this.Text = "";

            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;

            int upKeyID = 1;
            int allKeyID = 2;
            int downKeyID = 3;

            int HotKeyUP = (int)Keys.F15;
            Boolean keyUpRegistered = RegisterHotKey(
                this.Handle, upKeyID, 0x0000, HotKeyUP
            );

            int HotKeyALL = (int)Keys.F16;
            Boolean keyAllRegistered = RegisterHotKey(
                this.Handle, allKeyID, 0x0000, HotKeyALL
            );

            int HotKeyDOWN = (int)Keys.F17;
            Boolean keyDownRegistered = RegisterHotKey(
                this.Handle, downKeyID, 0x0000, HotKeyDOWN
            );    

            if (keyUpRegistered)
            {
                Console.WriteLine("Global Hotkey F15 was succesfully registered");
            }
            else
            {
                Console.WriteLine("Global Hotkey F15 couldn't be registered !");
            }

            if (keyAllRegistered)
            {
                Console.WriteLine("Global Hotkey F16 was succesfully registered");
            }
            else
            {
                Console.WriteLine("Global Hotkey F16 couldn't be registered !");
            }

            if (keyDownRegistered)
            {
                Console.WriteLine("Global Hotkey F17 was succesfully registered");
            }
            else
            {
                Console.WriteLine("Global Hotkey F17 couldn't be registered !");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sendMessage("UP");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sendMessage("ALL");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendMessage("DOWN");
        }

        public void sendMessage(string message)
        {
            var client = new TcpClient();
            client.Connect("192.168.0.64", 3001);

            NetworkStream networkStream = client.GetStream();
            networkStream.ReadTimeout = 2000;

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            networkStream.Write(bytes, 0, bytes.Length);

            client.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }
        protected override void WndProc(ref Message m)
        { 
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();

                switch(id)
                {
                    case 1:
                        sendMessage("UP");
                        break;
                    case 2:
                        sendMessage("ALL");
                        break;
                    case 3:
                        sendMessage("DOWN");
                        break;
                }
            }

            base.WndProc(ref m);
        }
    }
}
