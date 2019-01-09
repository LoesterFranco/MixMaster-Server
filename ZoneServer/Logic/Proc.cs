using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ZoneServer.Logic
{
    public class Proc
    {

   
        private DateTime startTime;
        private Stopwatch SERVER_TIME;
        private Stopwatch lastTime;
        private Stopwatch sleep;
        Stopwatch teste;
        private int ticksPerSecond = 0;
        private int _CurrentTicks = 0;
        private int FREQUENCY_RATE = 0;


        public Proc()
        {
            SERVER_TIME = new Stopwatch();
            startTime = DateTime.Now;
            lastTime = new Stopwatch();
            teste = new Stopwatch();
            sleep = new Stopwatch();
            sleep.Start();
            teste.Start();
            lastTime.Start();
            FREQUENCY_RATE = (int)Math.Floor(1000.0f / 240.0f);
            Thread t_proc = new Thread(new ThreadStart(Logic));
            t_proc.Priority = ThreadPriority.AboveNormal;
            t_proc.Start();
            Thread.Sleep(1500);
            Init.logger.ConsoleLog("[Logic] Server Logic Started!", ConsoleColor.Green);
            Init.logger.WriteLog($"Server Logic foi iniciado com sucesso; Taxa da frequencia: {GetCurrentTickSpeed()} fps.", LogStatus.GameInfo);
           
        }


        public uint GetTickCount()
        {
            DateTime now = DateTime.Now;
            int passedTime = now.Millisecond - startTime.Millisecond;
            uint result = (uint)passedTime;
            return result;
        }

        public int GetTickCountInt()
        {
            DateTime now = DateTime.Now;
            int passedTime = now.Millisecond - (int)SERVER_TIME.ElapsedMilliseconds;
            int result = passedTime;
            return result;
        }

        public int GetCurrentTickSpeed()
        {
            return this.ticksPerSecond;
        }

 

        private void Logic()
        {
            while(true)
            {
                long miliseconds = lastTime.ElapsedMilliseconds;
                if (miliseconds >= FREQUENCY_RATE)
                {
                    Update(miliseconds);
                    lastTime.Restart();
                }
                Thread.Sleep(1);
            }
        }

        long currentGameTime = 0;
        
        private void Update(long gameTime)
        {
            currentGameTime += gameTime;
            _CurrentTicks++;
            if (currentGameTime >= 1000)
            {
                ticksPerSecond = _CurrentTicks;
                _CurrentTicks = 0;
                //Console.WriteLine("[+] Ticks por segundo: " + GetCurrentTickSpeed());
                //Console.WriteLine("Time: " + teste.ElapsedMilliseconds);
                teste.Restart();
                currentGameTime = 0;
            }

            // update scripts time
            if(Init.scriptManager.ready)
                Init.scriptManager.Update(gameTime);

            // update network receive & sender
            Init.server.receiveManager.Update();
            Init.server.sendManager.Update();

            for(int i = 0; i < Network.Server.clients.Count; i++)
            {
                if(Network.Server.clients[i].zs_data.LOGGED)
                {
                    Network.Server.clients[i].zs_data.Update(gameTime);
                }
            }


            
        }
        
    }
}
