using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace ZoneServer.LuaScript
{
    public class EventsHandler
    {
        public Closure closure;
        public string eventName;
        public bool eventExecuted = false;

        public EventsHandler(Closure _closure, string _Event)
        {
            closure = _closure;
            eventName = _Event;
            eventExecuted = false;
        }
    }
}
