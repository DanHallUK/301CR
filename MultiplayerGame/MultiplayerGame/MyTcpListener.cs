using System;
using System.Net;
using System.Net.Sockets;


namespace MultiplayerGame
{
    class MyTcpListener
    {
        public delegate void ChangedEventHandler(object sender, EventArgs e);

        public event ChangedEventHandler MessageReceived;

        public string Message;

        public void StartListener()
        {
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                //IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                //IPAddress localAddr = IPAddress.Parse("192.168.1.3");
                IPAddress localAddr = IPAddress.Parse(Helper.GetLocalIPAddress());


                // TcpListener server = new TcpListener(port);
                TcpListener server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine(String.Format("Received: {0}", data));

                        Message = data;
                        OnMessageReceived(EventArgs.Empty);


                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine(String.Format("Sent: {0}", data));
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        protected virtual void OnMessageReceived(EventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}
