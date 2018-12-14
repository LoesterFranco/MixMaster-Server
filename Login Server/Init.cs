using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Login_Server.MixMaster_API.Crypto;
using Login_Server.MixMaster_API.Database;
using Login_Server.MixMaster_API.Network;
using Login_Server.MixMaster_API.LogManager;
using System.Threading;
using System.Windows.Forms;

namespace Login_Server
{
    public class Init
    {


        public static List<ClientManager> Clients = new List<ClientManager>();
        

        static void Main(string[] args)
        {
            Console.Title = "MixMaster Server Remake - LoginServer";

            // load information
            if (!MyInfo.LoadMyInfo())
            {
                Console.WriteLine("Load MyInfo Failed");
                return;
            } 

            // start bind LoginServer
            if(!AsyncSocket.Start())
            {
                Console.WriteLine("CLIENT SOCKET CREATE FAILED!");
                return;
            }

            // start bind GameManagerServer
            if(!GMS_bind.Start())
            {
                Console.WriteLine("CLIENT GMS SOCKET CREATE FAILED!");
                return;
            }


            // connect member database
            if(!Member.ConnectMember())
            {
                Console.WriteLine("CONNECT TO LOGIN_DATABASE FAILED");
                return;
            }

            Console.WriteLine("\n");
            Console.WriteLine("---------------LOGIN SERVER----------------\n");



            Console.CursorSize = 1;
           
            while(true)
            {
                Thread.Sleep(5);
                Console.ResetColor();
                Console.Write("");
                //Console.ForegroundColor = ConsoleColor.Green;
                string comando = Console.ReadLine();
                CommandHandler(comando);
                
                Console.ResetColor();
            }
        }

        private static void CommandHandler(string comando)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            switch(comando.ToLower())
            {
                case "help":
                    Console.WriteLine("restart   -> Reseta o LoginServer");
                    Console.WriteLine("kick all  -> Kicka todos os clients conectados");
                    Console.WriteLine("quit      -> Sair do Login Server");
                    Console.WriteLine("clear     -> Limpa a tela do console");
                    Console.WriteLine("\n");
                    break;
                case "quit":
                    for(int i=3; i > 0; i--)
                    {
                        Console.WriteLine("Fechando LoginServer em: " + i + " segundos ...");
                        Thread.Sleep(1000);
                    }
                    Environment.Exit(0);
                    break;
                case "restart":
                    for (int i = 3; i > 0; i--)
                    {
                        Console.WriteLine("Reiniciando LoginServer em: " + i + " segundos ...");
                        Thread.Sleep(1000);
                    }
                    Application.Restart();
                    Environment.Exit(0);
                    break;
                case "kick all":
                    Console.WriteLine("Desconectando: " + Clients.Count + " clients");
                    ClientFunctions.DisconnectAll();
                    break;
                case "clients":
                    Console.WriteLine("Total clients connected: " + Clients.Count);
                    for(int i=0; i<Clients.Count; i++)
                    {
                        Console.WriteLine("[*] Client: " + i);
                        Console.WriteLine("     ID: " + Clients[i].id);
                        Console.WriteLine("     Version: " + Clients[i].data.Version);
                        Console.WriteLine("     IP Address: " + Clients[i]._socket.RemoteEndPoint.ToString());
                    }
                    Console.WriteLine("\n");
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "cls":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Command invalid");
                    break;
            }
        }

    }
}
