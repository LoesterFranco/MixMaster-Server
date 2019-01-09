using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ZoneServer.Network
{
    public class Server
    {
        private Socket s;
        private const int buffer_size = 2048;
        private byte[] buffer;
        private IPEndPoint ip;
        public static List<Client> clients;
        public Receive receiveManager;
        public Sender sendManager;
        private int port = 0;

        public Server(int port)
        {
            this.port = port;
            Start();
        }

        public void Start()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = new byte[buffer_size];
            // ip local
            ip = new IPEndPoint(IPAddress.Any, this.port);
            clients = new List<Client>();
            receiveManager = new Receive();
            sendManager = new Sender();
            try
            {
                s.Bind(ip);
                s.Listen(5);
                s.BeginAccept(new AsyncCallback(AcceptCallback), s);
                Init.logger.ConsoleLog("[SERVER] Socket criado com sucesso!", ConsoleColor.Green);
                Init.logger.WriteLog($"Socket inicializado com sucesso; servidor ouvindo na porta: {port}!", LogStatus.NetworkInfo);
            }
            catch(Exception e)
            {
                Init.logger.WriteLog("Falha ao inicializar servidor: " + e.Message, LogStatus.NetworkError);
                Environment.Exit(0);
                return;
            }
          
        }

        private int GenerateID(Socket s)
        {
            return s.GetHashCode();
        }

        private void AcceptCallback(IAsyncResult e)
        {
            Socket s = (Socket)e.AsyncState;
            try
            {
                Socket s_Client = s.EndAccept(e);
                Console.WriteLine("[+] Cliente Conectado!");
                int id = GenerateID(s_Client);

                Client client = new Client(id, s_Client);
                client.OnDisconnect += OnClientDisconnect;
                clients.Add(client);
                client.s.BeginReceive(client.buffer, 0, Client.buffer_size, SocketFlags.None, new AsyncCallback(ReceiveCallback), client);
                Init.logger.WriteLog("Client conectado com sucesso!", LogStatus.NetworkInfo);
                sendManager.SendHello(client);
            }
            catch(Exception er)
            {
                Init.logger.WriteLog("Client não conseguiu se conectar ao servidor: " + er.Message, LogStatus.NetworkError);
                return;
            }
            finally
            {
                s.BeginAccept(new AsyncCallback(AcceptCallback), s);
            }
        }

        private void OnClientDisconnect(object s, EventArgs e)
        {
            Client client = (Client)s;
            if (client == null) return;
            Console.WriteLine($"[{client.id}] Foi desconectado!");
            Init.logger.WriteLog($"[{ client.id}] Foi desconectado!", LogStatus.NetworkInfo);
            clients.Remove(client);
        }

        private void ReceiveCallback(IAsyncResult e)
        {
            Client client = (Client)e.AsyncState;
            if (client == null) return;
            if (!client.s.Connected) return;

            try
            {
                int bytes_received = client.s.EndReceive(e);
                if(bytes_received > 0)
                {
                    byte[] data = new byte[bytes_received];
                    Array.Copy(client.buffer, data, bytes_received);

                    receiveManager.AddPacket(new XPacket((short)bytes_received, data, client));
                    client.receivedBytes += bytes_received;

                    client.buffer = new byte[Client.buffer_size];
                    client.s.BeginReceive(client.buffer, 0, Client.buffer_size, SocketFlags.None, new AsyncCallback(ReceiveCallback), client);
                }
                else
                {
                    client.Disconnect();
                }

            }
            catch
            {
                client.Disconnect();
            }
        }


        public static void SendPacket(GamePacket packet)
        {
            try
            {
                packet.s.BeginSend(packet.data, 0, packet.data.Length, 0, new AsyncCallback(Sendcallback), packet.s);
            }
            catch
            {

            }
        }

        private static void Sendcallback(IAsyncResult e)
        {
            try
            {
                Socket sock = (Socket)e.AsyncState;
                sock.EndSend(e);
            }
            catch
            {

            }
        }

    }
}
