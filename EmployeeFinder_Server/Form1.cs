using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoListServer
{
    public partial class Form1 : Form
    {
        //Тест
        private IPAddress IpAddress;
        private int Port;
        private DataBaseContext dbContext;
        private TcpListener tcpListener;

        private int DataGridCounter = 0;

        public Form1()
        {
            InitializeComponent();
            dbContext = new DataBaseContext();
            DrawDataGrid();
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
                UpdateDataGrid();
            }
        }
        private void rightBut_Click(object sender, EventArgs e)
        {
            if (DataGridCounter < 2)
            {
                DataGridCounter++;
                UpdateDataGrid();
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
                                string[] info = data.Substring(5).Split(' ');
                                string login = info[0];
                                string pass = info[1];

                                Classes.User user = null;
                                foreach (Classes.User item in dbContext.Users)
                                {
                                    if (item.Login == login)
                                    {
                                        user = item;
                                        break;
                                    }
                                }
                                if (user != null) //Если логин найден
                                {
                                    if (user.Password == pass) //Если пароль совпадает
                                    {
                                        WriteToStream(tcpClient.GetStream(), "OK");
                                        WriteToConsole($"{login}: Loged in.");
                                    }
                                    else //Если пароль не совпадает
                                    {
                                        WriteToStream(tcpClient.GetStream(), "PASS");
                                        WriteToConsole($"{login}: Wrong password.");
                                    }
                                }
                                else //Если логин не найден
                                {
                                    WriteToStream(tcpClient.GetStream(), "LOGIN");
                                    WriteToConsole($"{login}: Not found.");
                                }
                                break;
                            }
                        case "REGU": //Register user
                            {
                                string[] info = data.Substring(5).Split(' ');
                                string login = info[0];
                                string pass = info[1];

                                if (dbContext.Users.Where(u => u.Login == login).Count() > 0)
                                {
                                    WriteToStream(tcpClient.GetStream(), "USER");
                                    WriteToConsole($"{login}: Already added.");
                                }
                                else
                                {
                                    Classes.User newUser = new Classes.User() { Login = login, Password = pass };
                                    dbContext.Users.Add(newUser);
                                    dbContext.SaveChanges();
                                    UpdateDataGrid();

                                    WriteToStream(tcpClient.GetStream(), "OK");
                                    WriteToConsole("User added successfully.");
                                }
                                break;
                            }
                        case "RECG": //Receive groups
                            {
                                //RECG login
                                string login = data.Substring(5);
                                int userId = dbContext.Users.Where(u => u.Login == login).Select(u => u.Id).FirstOrDefault();

                                List<Classes.Group> groups = dbContext.Groups.Where(g => g.UserId == userId).ToList();
                                string json = JsonConvert.SerializeObject(groups, Formatting.Indented);
                                WriteToStream(tcpClient.GetStream(), json);
                                break;
                            }
                        case "RECN": //Receive notes
                            {
                                //RECN 3
                                int groupId = Int32.Parse(data.Substring(5));

                                List<Classes.Note> notes = dbContext.Notes.Where(n => n.GroupId == groupId).ToList();
                                string json = JsonConvert.SerializeObject(notes, Formatting.Indented);
                                WriteToStream(tcpClient.GetStream(), json);
                                break;
                            }
                        case "CRTG": //Create group
                            {
                                //CRTG login
                                string login = data.Substring(5);
                                int userId = dbContext.Users.Where(u => u.Login == login).Select(u => u.Id).FirstOrDefault();

                                Classes.Group newGroup = new Classes.Group() { Title = "New group", UserId = userId };
                                dbContext.Groups.Add(newGroup);
                                dbContext.SaveChanges();
                                WriteToConsole($"{login}: Added group");
                                UpdateDataGrid();
                                string json = JsonConvert.SerializeObject(newGroup, Formatting.Indented);
                                WriteToStream(tcpClient.GetStream(), json);
                                break;
                            }
                        case "SAVG": //Save group
                            {
                                //SAVG login
                                string json = data.Substring(5);

                                try
                                {
                                    Classes.Group editedGroup = JsonConvert.DeserializeObject<Classes.Group>(json);
                                    if (editedGroup != null)
                                    {
                                        Classes.Group groupOnServer = dbContext.Groups.Where(g => g.Id == editedGroup.Id).FirstOrDefault();

                                        if (editedGroup.Title != groupOnServer.Title)
                                        {
                                            groupOnServer.Title = editedGroup.Title;
                                        }

                                        if (editedGroup.Notes != null && editedGroup.Notes.Count != 0)
                                        {
                                            foreach (Classes.Note note in editedGroup.Notes)
                                            {
                                                Classes.Note noteOnServer = dbContext.Notes.Where(n => n.Id == note.Id).FirstOrDefault();

                                                if (note.Title != noteOnServer.Title)
                                                {
                                                    noteOnServer.Title = note.Title;
                                                }
                                                if (note.Description != noteOnServer.Description)
                                                {
                                                    noteOnServer.Description = note.Description;
                                                }
                                                if (note.IsChecked != noteOnServer.IsChecked)
                                                {
                                                    noteOnServer.IsChecked = note.IsChecked;
                                                }
                                            }
                                        }
                                    }
                                    dbContext.SaveChanges();

                                    WriteToStream(tcpClient.GetStream(), "OK");
                                    UpdateDataGrid();
                                }
                                catch (Exception ex)
                                {
                                    WriteToStream(tcpClient.GetStream(), "ERROR");
                                    MessageBox.Show(ex.Message);
                                    WriteToConsole(ex.Message);
                                }
                                break;
                            }
                        case "CRTN": //Create note
                            {
                                //CRTN groupId
                                int groupId = Int32.Parse(data.Substring(5));

                                Classes.Note newNote = new Classes.Note() { Title = "New note", GroupId = groupId, IsChecked = false };
                                dbContext.Notes.Add(newNote);
                                dbContext.SaveChanges();
                                WriteToConsole($"GroupId_{groupId}: Added note");
                                UpdateDataGrid();
                                string json = JsonConvert.SerializeObject(newNote, Formatting.Indented);
                                WriteToStream(tcpClient.GetStream(), json);
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


        //DataGrid
        private void UpdateDataGrid()
        {
            Action action = () => DrawDataGrid();
            this.Invoke(action);
        }
        private void DrawDataGrid()
        {
            switch (DataGridCounter)
            {
                case 0: { DataBox.DataSource = dbContext.Users.ToArray(); break; }
                case 1: { DataBox.DataSource = dbContext.Groups.ToArray(); break; }
                case 2: { DataBox.DataSource = dbContext.Notes.ToArray(); break; }
            }
        }


        //Console
        private void WriteToConsole(string message)
        {
            Action action = () => ConsoleBox.Items.Add(DateTime.Now.ToLongTimeString() + "   " + message);
            this.Invoke(action);
        }
    }
}
