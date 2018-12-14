using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ZoneServer.BaseFunctions
{
    public class ConsoleFunctions
    {

        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT = 0x0040;
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        // disable mouse interaction
        public static bool DisableQuickEdit()
        {
            bool result = false;

            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode = 0;
            if(!GetConsoleMode(consoleHandle, out consoleMode))
            {
                result = false;
                return result;
            }
            consoleMode &= ~ENABLE_QUICK_EDIT;
            if(!SetConsoleMode(consoleHandle, consoleMode))
            {
                result = false;
                return result;
            }
            result = true;


            return result;

        }
    }
}
