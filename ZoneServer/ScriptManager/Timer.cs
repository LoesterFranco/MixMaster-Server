using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace ZoneServer.LuaScript
{
    public class Timer
    {
        public EventHandler OnTerminate;
        public Closure closure;
        public long miliseconds;
        public int times;

        private long current_miliseconds;
        private int current_times;
        private bool infinite;

        public Timer(Closure _closure, long _ms, int _times)
        {
            closure = _closure;
            miliseconds = _ms;
            times = _times;
            if (times == 0)
                infinite = true;
            else
                infinite = false;
        }

        public void Update(long time)
        {
            current_miliseconds += time;
            if (current_miliseconds >= miliseconds)
            {
                if (infinite)
                {
                    closure.Call();
                }
                else
                {
                    if (current_times < times)
                    {
                        closure.Call();
                        times--;
                    }
                    else
                    {
                        // terminate
                        if (OnTerminate != null)
                            OnTerminate(this, EventArgs.Empty);
                    }
                }
                current_miliseconds = 0;
            }
        }
    }
}
