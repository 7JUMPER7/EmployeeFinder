using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EmployeeFinder_Server
{
    public partial class Form1 : Form
    {
        private IPAddress IpAddress;
        private int Port;
        private TcpListener tcpListener;

        private int DataGridCounter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        //События
        private void StartBut_Click(object sender, EventArgs e)
        {
            IpAddress = IPAddress.Parse(IpBox.Text);
            Port = Int32.Parse(PortBox.Text);
            IpBox.ReadOnly = true;
            PortBox.ReadOnly = true;
            StartBut.Enabled = false;

            tcpListener = new TcpListener(IpAddress, Port);
            tcpListener.Start();
            Thread thread = new Thread(ListenerMethod);
            thread.IsBackground = true;
            thread.Start();
            WriteToConsole("Server started.");
        }

        private void leftBut_Click(object sender, EventArgs e)
        {
            if (DataGridCounter > 0)
            {
                DataGridCounter--;
            }
        }

        private void rightBut_Click(object sender, EventArgs e)
        {
            if (DataGridCounter < 2)
            {
                DataGridCounter++;
            }
        }

        //Ожидание запросов
        private void ListenerMethod()
        {
            while (true)
            {
                try
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    string data = ReadFromStream(tcpClient.GetStream());
                    tcpClient.GetStream().Flush();

                    WriteToConsole($"{(tcpClient.Client.RemoteEndPoint as IPEndPoint).Address}:" +
                        $"{(tcpClient.Client.RemoteEndPoint as IPEndPoint).Port} - {data}");
                    string command = data.Substring(0, 4);

                    switch (command)
                    {
                        case "LGIN": //LogIn
                            {
                                break;
                            }
                        case "REGU": //Register user
                            {
                                break;
                            }
                        case "RECG": //Receive groups
                            {
                                break;
                            }
                        case "RECN": //Receive notes
                            {
                                break;
                            }
                        case "CRTG": //Create group
                            {
                                break;
                            }
                        case "SAVG": //Save group
                            {
                                break;
                            }
                        case "CRTN": //Create note
                            {
                                break;
                            }
                        default:
                            MessageBox.Show($"'{command}' is worng command.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    WriteToConsole(ex.Message);
                }
            }
        }

        //Работа с потоком
        private string ReadFromStream(NetworkStream stream)
        {
            byte[] buf = new byte[64];
            int len = 0, sum = 0;
            List<byte> allBytes = new List<byte>();
            do
            {
                len = stream.Read(buf, 0, buf.Length);
                allBytes.AddRange(buf);
                sum += len;
            } while (len >= buf.Length);
            return Encoding.Unicode.GetString(allBytes.ToArray(), 0, sum);
        }

        private void WriteToStream(NetworkStream stream, string message)
        {
            byte[] buf = Encoding.Unicode.GetBytes(message);
            stream.Write(buf, 0, buf.Length);
        }

        //Console
        private void WriteToConsole(string message)
        {
            Action action = () => ConsoleBox.Items.Add(DateTime.Now.ToLongTimeString() + "   " + message);
            this.Invoke(action);
        }
    }
}