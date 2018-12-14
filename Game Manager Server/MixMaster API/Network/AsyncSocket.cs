using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Game_Manager_Server.MixMaster_API.Network
{
    // GameManagerServer socket


    public class AsyncSocket
    {
        public static Socket _listen;

        public static bool Start()
        {
            try
            {
                _listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _listen.Bind(new IPEndPoint(MyInfo.MY_IP, MyInfo.MY_PORT));
                Thread SocketThread = new Thread(new ThreadStart(InitializeSocket));
                SocketThread.Start();
                return true;
            } 
            catch
            {
                Console.WriteLine("[-] Fatal error: Socket can't initialize");
                Console.ReadLine();
                return false;
            }
           
        }

        public static void InitializeSocket()
        {
            try
            {
                _listen.Listen(MyInfo.MAX_CONNECTIONS);
                _listen.BeginAccept(new AsyncCallback(AcceptConnection), _listen);
            }
            catch
            {
                return;
            }
        }



        private static void AcceptConnection(IAsyncResult ar)
        {
            Socket _listen = (Socket)ar.AsyncState;
            try
            {
                Socket client = _listen.EndAccept(ar);
                if (client != null)
                {
                    int ClientID = client.GetHashCode();
                    if(!ClientFunctions.ClientIdExist(ClientID))
                    {
                        //Console.WriteLine("[+] Client is Connected: " + client.LocalEndPoint.ToString());
                        ClientManager MyClient = new ClientManager(ClientID, client);


                        init.Clients.Add(MyClient);
                        //Console.WriteLine("[+] Client added to list!");

                        if(MyClient != null)
                        {
                            int ClientIndex = ClientFunctions.GetClientIndex(ClientID);
                            if (ClientIndex != -1)
                            {
                                //Console.WriteLine(" [+] Client listening packets ...");
                                init.Clients[ClientIndex]._socket.BeginReceive(init.Clients[ClientIndex].buffer, 0, ClientManager.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), init.Clients[ClientIndex]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("[-] Client in list does not exist!");
                        }

                    }
                    else
                    {
                        // report error
                        // cloned client id hash code
                        Console.WriteLine("[-] Already client id exists: " + ClientID);
                    }
                }
            }
            catch
            {
                // add to log Accept Connection error
                return;
            }
            finally
            {
                _listen.BeginAccept(new AsyncCallback(AcceptConnection), _listen);
            }
        }
        public const int FIONREAD = 0x4004667F;
        public static int getPendingByteCount(Socket s)
        {
            //CÓDIGO
            try
            {
                byte[] outValue = BitConverter.GetBytes(0);

                // Checa quantos bytes foram recebidos
                s.IOControl(FIONREAD, null, outValue);

                int bytesAvailable = BitConverter.ToInt32(outValue, 0);
                //Console.WriteLine("server has {0} bytes pending. Available property says {1}.",
                //  bytesAvailable, s.Available);

                return bytesAvailable;
            }
            catch
            {
                return 0;
            }
        }


        private static void ReceiveCallback(IAsyncResult ar)
        {
            byte[] Data;
            ClientManager MyClient = (ClientManager)ar.AsyncState;
            Socket client = MyClient._socket;


            if(client == null) { return; }
            if(!client.Connected) { return; }
            int BufferSize = getPendingByteCount(client);
            if(BufferSize > 1000) { return; }

            try
            {
                int BytesReceive = client.EndReceive(ar);
                if(BytesReceive > 0)
                {
                    Data = new byte[BytesReceive];
                    Array.Copy(MyClient.buffer, Data, BytesReceive);
                    ReceiveData.ParsePacket(MyClient, Data);
                    MyClient.buffer = new byte[ClientManager.BufferSize];

                    // Process received data
                    
                }
                else
                {
                    ClientFunctions.DisconnectClientFromID(MyClient.id);
                    ClientFunctions.RemoveClientFromInstance(MyClient);
                }
                client.BeginReceive(MyClient.buffer, 0, ClientManager.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), MyClient);
            }
            catch(SocketException e)
            {
                if (e.ErrorCode == 10054)
                {
                    ClientFunctions.DisconnectClientFromID(MyClient.id);
                    ClientFunctions.RemoveClientFromInstance(MyClient);
                    //Console.WriteLine("Client disconnected!");
                    return;
                }
            }
            catch
            {
                ClientFunctions.DisconnectClientFromID(MyClient.id);
                ClientFunctions.RemoveClientFromInstance(MyClient);
                //Console.WriteLine("Client disconnected!");
                // add to log receive error
                // remove client connected if is a error: remove from clients list
                //
                return;
            }
        }
    }
}
