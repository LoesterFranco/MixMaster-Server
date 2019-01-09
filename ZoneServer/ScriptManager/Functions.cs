using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace ZoneServer.LuaScript
{
    public class Functions
    {
        public bool CreateTimer(Closure s, long miliseconds, int times)
        {
            Init.scriptManager.timers.Add(new Timer(s, miliseconds, times));
            return true;
        }

        public bool CreateEventHandler(Closure s, string Event)
        {
            // register event
            Init.scriptManager.eventsHandler.Add(new EventsHandler(s, Event));
            return true;
        }

        public int GetRandom(int min, int max)
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            return rd.Next(min, max);
        }
    }
}
