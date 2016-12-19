using System;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace MultiplayerServer
{
    class Program
    {
        public static Hashtable tConnectedClients = new Hashtable();
        public static bool bColoursSent = false;
        public static GameLogic tGameLogic;

        static void Main(string[] args)
        {
            tGameLogic = new GameLogic();
            tGameLogic.StartGame();
            // Start listening on port 8888
            TcpListener tServerSocket = new TcpListener(8888);
            TcpClient tClientSocket = default(TcpClient);

            tServerSocket.Start();
            Console.WriteLine("Game Server Started");

            bool bRunServer = true;
            while (bRunServer)
            {
                // If there are 2 clients connected, assign them their piece colour so they can begin playing
                if (tConnectedClients.Count == 2 && !bColoursSent)
                {
                    string sMesage = "\n";
                    bColoursSent = true;
                    // As the usernames are unique, we can simply send a message to both clients identifying which user is which colour
                    int i = 0;
                    foreach (string s in tConnectedClients.Keys)
                    {
                        sMesage = sMesage + s + (i == 0 ? " is white\n" : " is black\n");
                        i++;
                        if (i == 2)
                        {
                            break;
                        }
                    }
                    BroadcastMessage(sMesage, sMesage, false);
                }

                tClientSocket = tServerSocket.AcceptTcpClient();

                byte[] tByteBuffer = new byte[tClientSocket.ReceiveBufferSize];
                string sMessageRecieved = null;

                //Read the username the client has attempted to connect as
                NetworkStream tClientStream = tClientSocket.GetStream();
                tClientStream.Read(tByteBuffer, 0, (int)tClientSocket.ReceiveBufferSize);
                sMessageRecieved = Encoding.ASCII.GetString(tByteBuffer);
                sMessageRecieved = sMessageRecieved.Substring(0, sMessageRecieved.IndexOf("$"));

                //If the username is not unique add a 1 to the end of the name
                while (tConnectedClients.ContainsKey(sMessageRecieved))
                {
                    sMessageRecieved = sMessageRecieved + "1";
                }

                //Add the client to our hashtable of clients
                tConnectedClients.Add(sMessageRecieved, tClientSocket);

                //Send a message to the chat server showing they have joined
                BroadcastMessage(sMessageRecieved + " Joined ", sMessageRecieved, false);

                Console.WriteLine(sMessageRecieved + " Joined chat room ");
                //Establish the new client connection to monitor for messages the client sends
                ClientConnection client = new ClientConnection();
                client.startClient(tClientSocket, sMessageRecieved, tConnectedClients);

                //Send a message just to the newly connected client showing what username they have connected as
                BroadcastMessage("Connected as " + sMessageRecieved, sMessageRecieved, false, false, sMessageRecieved);
            }
            //If we choose to stop the server, close the connections first
            tClientSocket.Close();
            tServerSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }

        public static void BroadcastMessage(string i_sMessage, string i_sClientName, bool i_bUserMessage, bool i_bMessage = true, string i_sClientKey = null)
        {
            //For each connected client, broadcast a message unless...
            foreach (DictionaryEntry Item in tConnectedClients)
            {
                // ...we have a specific client specified, at which point broadacast only to them
                if (i_sClientKey == null || Item.Key.ToString() == i_sClientKey)
                {
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();
                    Byte[] broadcastBytes = null;

                    // If this is a standard message send it normally
                    if (i_bMessage)
                    {
                        // If this is a message from another user, prefix with "Username says: "
                        if (i_bUserMessage == true)
                        {
                            broadcastBytes = Encoding.ASCII.GetBytes(i_sClientName + " says : " + i_sMessage); ;
                        }
                        else
                        {
                            broadcastBytes = Encoding.ASCII.GetBytes(i_sMessage);
                        }
                    }
                    // If this is a hidden message containing game instructions, append the new line character to flag it as such
                    else
                    {
                        broadcastBytes = Encoding.ASCII.GetBytes(i_sMessage + "\n"); ;
                    }

                    //Broadcast the message
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                    broadcastStream.Flush();
                }
            }
        }
    }
}