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
        bool isServer;
        bool isClient;
        Thread thread = new Thread(new ThreadStart(MyTcpListener.StartListener));

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Form1()
        {
            AllocConsole();
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameEngine = GameEngine.GetGameEngineInstance(pictureBox1.Size);
            DrawTheGame();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            gameEngine.Click(new Point(coordinates.X / ApplicationSettings.CellSize.Width, coordinates.Y / ApplicationSettings.CellSize.Height));
            DrawTheGame();
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
            MyTcpClient.Connect(textBox1.Text, "Blabla");
        }
    }
}
