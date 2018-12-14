using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Login_Server.MixMaster_API;

namespace Login_Server.MixMaster_API.Network
{
    public class ReceiveData
    {
        public static void ParsePacket(ClientManager MyClient, byte[] data)
        {
            if (MyClient == null) { return; }
            if (MyClient._socket == null) { return; }
            if (!MyClient._socket.Connected) { return; }

            ReadPacket Cpacket = new ReadPacket(data);
            if(Cpacket.IsInitialized())
            {
                ProcessPacket(MyClient, Cpacket);
            }
            else
            {
                // add to log, packet receive don't read
            }
        }

        private static void ProcessPacket(ClientManager MyClient, ReadPacket CPacket)
        {
            Header PacketHead = CPacket.GetHeader();
            byte[] PacketBody = CPacket.GetBody();
            byte PacketType = CPacket.GetPacketType();

            //Console.WriteLine("Receive Packet len: " + PacketHead.len);
            //Console.WriteLine("Packet Type: " + PacketType);
            switch(PacketType)
            {
                case 10: // version receive
                    ParseVersionReceived(MyClient, CPacket);
                    break;
                case 21: // login request
                    ParseLoginRequest(MyClient, CPacket);
                    break;
                case 27: // Enter server request
                    //Console.WriteLine("Enter server request!");
                    SendData.SendResponseEnterServer(MyClient);
                    // send packet type 28 user_id (4bytes) + GMS_token (4 bytes)
                    break;
                case 153: // login request 2
                    ParseLoginRequest2(MyClient, CPacket);
                    break;
                case 151: // login request 3
                    ParseLoginRequest3(MyClient, CPacket);
                    break;
                default:
                    Console.WriteLine("packet unknow: " + PacketType);
                    break;


            }
        }

        private static void ParseVersionReceived(ClientManager MyClient, ReadPacket CPacket)
        {

            using (MemoryStream stream = new MemoryStream(CPacket.GetBody()))
            {
                using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8))
                {
                    br.ReadByte();
                    float version = br.ReadSingle();
                    //Console.WriteLine("Version received: " + version);
                    if (MyInfo.MyVersion == version)
                    {
                        MyClient.data.Version = version;
                        MyClient.data.VersionOK = true;
                        // versão ok, pode passar
                        //Console.WriteLine("Version is OK");
                        SendData.SendVersionResponse(MyClient, 1);
                       
                        // enviar 0x0B
                    }
                    else if(version < MyInfo.MyVersionLowLimit)
                    {
                        //Console.WriteLine("Version is very low");
                        SendData.SendVersionResponse(MyClient, 2);
                        // versão muito baixa
                        MyClient.data.VersionOK = true;
                    }
                    else
                    {
                        //Console.WriteLine("Version to patch");
                        SendData.SendVersionResponse(MyClient, 3);
                        MyClient.data.VersionOK = false;
                    }
                    
                }
            }

            
        }
        private static void ParseLoginRequest(ClientManager MyClient, ReadPacket CPacket)
        {
            using (MemoryStream stream = new MemoryStream(CPacket.GetBody()))
            {
                using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8))
                {
                    br.ReadByte();
                    byte[] userbytes = br.ReadBytes(21);
                    byte[] passbytes = br.ReadBytes(21);

                    string Username = PacketFunctions.ExtractStringFromBytes(userbytes);
                    string Password = PacketFunctions.ExtractStringFromBytes(passbytes);



                    Credentials MyCredential = new Credentials();
                    MyCredential.Username = Username;
                    MyCredential.Password = Password;

                    //Console.WriteLine("login request: " + Username + " | " + Password);
                    SendData.SendLoginResponse(MyClient, MyCredential);
                    
                }
            }
        }

        private static void ParseLoginRequest2(ClientManager MyClient, ReadPacket CPacket)
        {
            using (MemoryStream stream = new MemoryStream(CPacket.GetBody()))
            {
                using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8))
                {
                    br.ReadByte();
                    byte[] userbytes = br.ReadBytes(21);
                    byte[] passbytes = br.ReadBytes(21);

                    string Username = PacketFunctions.ExtractStringFromBytes(userbytes);
                    string Password = PacketFunctions.ExtractStringFromBytes(passbytes);



                    Credentials MyCredential = new Credentials();
                    MyCredential.Username = Username;
                    MyCredential.Password = Password;

                    //Console.WriteLine("login request mode 2: " + Username + " | " + Password);
                    SendData.SendLoginResponse2(MyClient, MyCredential);

                }
            }
        }

        private static void ParseLoginRequest3(ClientManager MyClient, ReadPacket CPacket)
        {
            using (MemoryStream stream = new MemoryStream(CPacket.GetBody()))
            {
                using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8))
                {
                    br.ReadByte();
                    byte[] userbytes = br.ReadBytes(21);
                    byte[] passbytes = br.ReadBytes(21);

                    string Username = PacketFunctions.ExtractStringFromBytes(userbytes);
                    string Password = PacketFunctions.ExtractStringFromBytes(passbytes);



                    Credentials MyCredential = new Credentials();
                    MyCredential.Username = Username;
                    MyCredential.Password = Password;

                    //Console.WriteLine("login request mode 3: " + Username + " | " + Password);
                    SendData.SendLoginResponse3(MyClient, MyCredential);

                }
            }
        }



    }
}
