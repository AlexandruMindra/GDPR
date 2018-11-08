using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;

namespace Client
{
    public partial class ClientForm : Form
    {
        public NetConnection client = new NetConnection();
        public ClientForm()
        {
            InitializeComponent();
            //client.OnConnect += client_onConnect;
            //client.OnDataReceived += Client_OnDataReceived;
            //client.OnDisconnect += client_onDisconnect;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Connect("localhost", 55555);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client.Disconnect();
        }
    }
}
