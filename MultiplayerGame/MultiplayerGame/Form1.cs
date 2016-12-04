using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;



namespace MultiplayerGame
{
    public partial class Form1 : Form
    {
        GameEngine gameEngine;
        MyTcpListener tcpListener;
        bool isServer;
        bool isClient;
        string message;
        Thread thread;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Form1()
        {
            tcpListener = new MyTcpListener();
            thread = new Thread(new ThreadStart(tcpListener.StartListener));
            AllocConsole();
            InitializeComponent();
            tcpListener.MessageReceived += TcpListener_MessageReceived;

        }

        private void TcpListener_MessageReceived(object sender, EventArgs e)
        {
            string message = tcpListener.Message;
            string[] values = message.Split(';');
            gameEngine.Click(new Point(int.Parse(values[0]), int.Parse(values[1])));
            DrawTheGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Your IP Address is: " + Helper.GetLocalIPAddress();
            gameEngine = GameEngine.GetGameEngineInstance(pictureBox1.Size);
            DrawTheGame();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            Point cellPosition = new Point(coordinates.X / ApplicationSettings.CellSize.Width, coordinates.Y / ApplicationSettings.CellSize.Height);
            gameEngine.Click(cellPosition);
            message = cellPosition.X + ";" + cellPosition.Y;
            DrawTheGame();
            MyTcpClient.Connect(textBox1.Text, message);
        }

        private void DrawTheGame()
        {
            pictureBox1.Image = gameEngine.Draw();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thread.Abort();
        }

        private void buttonCreateServer_Click(object sender, EventArgs e)
        {
            thread.Start();
            //MyTcpListener.StartListener();
        }

        private void buttonJoinServer_Click(object sender, EventArgs e)
        {
            MyTcpClient.Connect(textBox1.Text, message);
        }
    }
}
