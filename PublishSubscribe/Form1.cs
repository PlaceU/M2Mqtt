using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace PublishSubscribe
{
    public partial class Form1 : Form
    {
        MqttClient cliente;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cliente.Publish(tbTopic.Text, Encoding.Default.GetBytes(tbMessage.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Conecte-se ao host primeiro");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                cliente.Subscribe(new String[] { tbTopic.Text }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
            catch(Exception ex)
            {
                MessageBox.Show("Conecte-se ao host primeiro");
            }
        }

        private void onMessage(object sender, MqttMsgPublishEventArgs e)
        {
            this.richTextBox1.BeginInvoke((MethodInvoker)delegate () { richTextBox1.AppendText(Encoding.Default.GetString(e.Message) + "\n"); });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cliente = new MqttClient(tbServer.Text);

            cliente.MqttMsgPublishReceived += onMessage;

            string clientId = Guid.NewGuid().ToString();
            cliente.Connect(clientId);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cliente.Disconnect();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
