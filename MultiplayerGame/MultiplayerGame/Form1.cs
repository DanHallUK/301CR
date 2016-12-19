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
using System.Net.Sockets;



namespace MultiplayerGame
{
    public partial class Form1 : Form
    {
        GameEngine gameEngine;

        System.Net.Sockets.TcpClient tClientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream tNetworkStream = default(NetworkStream);
        string sReadMessage = null;

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
            Point cellPosition = new Point(coordinates.X / ApplicationSettings.CellSize.Width, coordinates.Y / ApplicationSettings.CellSize.Height);
            if (gameEngine.Click(cellPosition))
            {
                string message = cellPosition.X + ";" + cellPosition.Y;
                DrawTheGame();

                //Attempt to send move to server
                try
                {
                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message + "\n");
                    tNetworkStream.Write(outStream, 0, outStream.Length);
                    tNetworkStream.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void buttonConnectChatServer_Click(object sender, EventArgs e)
        {
            // Attempt to connect to server on specified ip (default is local)
            try
            {
                tClientSocket.Connect(serverAddress.Text, 8888);
                tNetworkStream = tClientSocket.GetStream();
                sReadMessage = "Conected to Chat Server ...";
                WriteMessage();

                //If connected send our username (default is user)
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(clientName.Text + "$");
                tNetworkStream.Write(outStream, 0, outStream.Length);
                tNetworkStream.Flush();

                //Start listening for server messages on new thread
                Thread ctThread = new Thread(GetMessage);
                ctThread.Start();
            }
            catch(Exception ex)
            {
                sReadMessage = "Unable to connect to server, please try again later...";
                WriteMessage();
                Console.WriteLine(ex.ToString());
            }
        }

        private void messageText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                sendMessage.PerformClick();
            }
        }

        private void sendMessage_Click(object sender, EventArgs e)
        {
            //Attempt to send message to server
            try
            {
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(messageText.Text + "$");
                messageText.Text = "";
                tNetworkStream.Write(outStream, 0, outStream.Length);
                tNetworkStream.Flush();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void DrawTheGame()
        {
            pictureBox1.Image = gameEngine.Draw();
        }

        private void SetClientName()
        {
            //Update the client name and set as read only
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(SetClientName));
            else
            {
                clientName.Text = sReadMessage;
                clientName.ReadOnly = true;
            }
        }

        private void Disconnect()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(Disconnect));
            else
            {
                //Reset the client connection
                tClientSocket.Close();
                tClientSocket = new System.Net.Sockets.TcpClient();
                // Reset the game
                gameEngine.ResetGame();
                DrawTheGame();
                //Allow the user to change their name
                clientName.ReadOnly = false;
            }
        }

        private void GetMessage()
        {
            //Whilst connected to server listen for any messages
            bool bConnected = true;
            while (bConnected)
            {
                try
                {
                    //Read any messages to the byte buffer
                    tNetworkStream = tClientSocket.GetStream();
                    int iBuffSize = 0;
                    byte[] tByteBuffer = new byte[tClientSocket.ReceiveBufferSize];
                    iBuffSize = tClientSocket.ReceiveBufferSize;
                    tNetworkStream.Read(tByteBuffer, 0, iBuffSize);
                    //Convert the byte buffer to a string
                    string returndata = System.Text.Encoding.ASCII.GetString(tByteBuffer);
                    //If the string is null, we are disconnected from the server
                    if(returndata[0] == '\0')
                    {
                        sReadMessage = "Disconnected from server, please try again later...";
                        WriteMessage();
                        bConnected = false;
                    }
                    // If the string contains a new line character this is a hidden message
                    if (returndata.Contains("\n"))
                    {
                        // Update the client name
                        if(returndata.Contains("Connected as "))
                        {
                            sReadMessage = returndata.Substring(returndata.IndexOf("Connected as ") + 13, returndata.IndexOf("\n") - (returndata.IndexOf("Connected as ") + 13));
                            SetClientName();
                        }
                        // Set the appropriate colour piece for the client
                        if (returndata.Contains(" is white"))
                        {
                            string message2 = returndata.Substring(0, returndata.IndexOf(" is white") + 9);
                            message2 = message2.Substring(message2.LastIndexOf("\n") + 1);
                            string message3 = returndata.Substring(0, returndata.IndexOf(" is black") + 9);
                            message3 = message3.Substring(message3.LastIndexOf("\n") + 1);


                            sReadMessage = message2;
                            WriteMessage();
                            sReadMessage = message3;
                            WriteMessage();

                            string whiteUser = message2.Substring(0, message2.IndexOf(" "));
                            string blackUser = message3.Substring(0, message3.IndexOf(" "));

                            if(whiteUser == clientName.Text)
                            {
                                gameEngine.SetColour(true);
                            }
                            if(blackUser == clientName.Text)
                            {
                                gameEngine.SetColour(false);
                            }
                        }
                        // Restart the game if a player disconnects
                        if(returndata.Contains(" Disconnected"))
                        {
                            gameEngine.ResetGame();
                            DrawTheGame();
                            sReadMessage = "" + returndata;
                            WriteMessage();
                        }
                        // Handle an opponents move
                        if(returndata.Contains(";"))
                        {
                            string message = returndata.Substring(0, returndata.IndexOf("\n"));
                            string[] values = message.Split(';');
                            gameEngine.Click(new Point(int.Parse(values[0]), int.Parse(values[1])), true);
                            DrawTheGame();
                        }
                    }
                    // Otherwise just broadcast the message to chat
                    else
                    {
                        sReadMessage = "" + returndata;
                        WriteMessage();
                    }
                }
                // If there is an error disconnect from the server
                catch (Exception ex)
                {
                    sReadMessage = "Disconnected from server, please try again later...";
                    WriteMessage();
                    Console.WriteLine(ex.ToString());
                    bConnected = false;
                }
            }
            //Function to handle disconnect
            Disconnect();        
        }

        private void WriteMessage()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(WriteMessage));
            else
                //Send a new message to chat
                chatText.Text = chatText.Text + Environment.NewLine + " >> " + sReadMessage;
        }

    }
}
