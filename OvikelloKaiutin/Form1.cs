using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace OvikelloKaiutin
{
    public partial class Form1 : Form
    {
        private Thread thread;
        public Form1()
        {
            InitializeComponent();
            thread = new Thread(MQTTConnect);
            thread.Start();
        }
        private void MQTTConnect()
        {
            MqttClient client = new MqttClient("127.0.0.1");
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { "/ovikello" }, new byte[] { 0 });
        }
        delegate void SetTextCallback(string t);
        private void SetText(string t)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { t });
            }
            else
            {
                this.textBox1.AppendText(t);
            }
        }
        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "C:/Ding-dong.wav";
            player.Load();
            player.Play();
            SetText(Encoding.UTF8.GetString(e.Message));
        }

    
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
