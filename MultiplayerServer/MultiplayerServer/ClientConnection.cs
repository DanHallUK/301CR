using System;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace MultiplayerServer
{
    public class ClientConnection
    {
        TcpClient tClientSocket;
        string sClientName;
        Hashtable tConnectedClients;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.tClientSocket = inClientSocket;
            this.sClientName = clineNo;
            this.tConnectedClients = cList;
            Thread tThread = new Thread(RevieveMessage);
            tThread.Start();
        }

        private void RevieveMessage()
        {
            byte[] tByteBuffer = new byte[tClientSocket.ReceiveBufferSize];
            string sMessageRecieved = null;

            bool bClientConnected = true;

            while (bClientConnected)
            {
                try
                {
                    //Read the message from the client
                    NetworkStream tClientStream = tClientSocket.GetStream();
                    tClientStream.Read(tByteBuffer, 0, (int)tClientSocket.ReceiveBufferSize);
                    sMessageRecieved = System.Text.Encoding.ASCII.GetString(tByteBuffer);
                    // If the message is a standard user message, broadcast it to other connected clients
                    if (sMessageRecieved.Contains("$"))
                    {
                        sMessageRecieved = sMessageRecieved.Substring(0, sMessageRecieved.IndexOf("$"));
                        Console.WriteLine("From client - " + sClientName + " : " + sMessageRecieved);
                        Program.BroadcastMessage(sMessageRecieved, sClientName, true);
                    }
                    // If the message is a gameplay message then broadcast it to other users as a hidden message
                    if (sMessageRecieved.Contains("\n"))
                    {
                        if (sMessageRecieved.Contains(";"))
                        {
                            string message = sMessageRecieved.Substring(0, sMessageRecieved.IndexOf("\n"));
                            string[] values = message.Split(';');
                            //Ensure move is valid before broadcasting it
                            if (Program.tGameLogic.ValidateMove(int.Parse(values[0]), int.Parse(values[1])))
                            {
                                sMessageRecieved = sMessageRecieved.Substring(0, sMessageRecieved.IndexOf("\n"));
                                Console.WriteLine("From client - " + sClientName + " : " + sMessageRecieved);
                                Program.BroadcastMessage(sMessageRecieved, sClientName, true, false);
                            }
                        }
                        else
                        {
                            sMessageRecieved = sMessageRecieved.Substring(0, sMessageRecieved.IndexOf("\n"));
                            Console.WriteLine("From client - " + sClientName + " : " + sMessageRecieved);
                            Program.BroadcastMessage(sMessageRecieved, sClientName, true, false);
                        }
                    }

                }
                // If there is a connection error, terminate the loop and remove the client from the connections list
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.ToString());
                    bClientConnected = false;
                    tConnectedClients.Remove(sClientName);
                    if (tConnectedClients.Count > 0)
                    {
                        Program.BroadcastMessage(sClientName + " Disconnected", sClientName, false, false);
                    }
                    Program.bColoursSent = false;
                    Program.tGameLogic.StartGame();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }

}
