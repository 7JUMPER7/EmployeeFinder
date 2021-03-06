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
        private int DataGridCounter = 1;
        private ServerLogical server;

        public Form1()
        {
            InitializeComponent();
            server = new ServerLogical(this);
        }

        private void leftBut_Click(object sender, EventArgs e)
        {
            if (DataGridCounter > 0)
            {
                DataGridCounter--;
                TableDisplay();
            }
        }

        private void rightBut_Click(object sender, EventArgs e)
        {
            if (DataGridCounter <= 6)
            {
                DataGridCounter++;
                TableDisplay();
            }
        }

        public void TableDisplay()
        {
            switch (DataGridCounter)
            {
                case 1: { DataBox.DataSource = server.GetCandidates(); } break;
                case 2: { DataBox.DataSource = server.GetCities(); } break;
                case 3: { DataBox.DataSource = server.GetCompanies(); } break;
                case 4: { DataBox.DataSource = server.GetCompaniesWishLists(); } break;
                case 5: { DataBox.DataSource = server.GetSpecialisations(); } break;
                case 6: { DataBox.DataSource = server.GetMessages(); } break;
            }
            DataBox.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TableDisplay();
        }
    }
}