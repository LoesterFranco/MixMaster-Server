using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using ZoneServer.Logic;
using ZoneServer.Network;
using ZoneServer.Network.GMS;
using ZoneServer.LuaScript;
using ZoneServer.Database;
using ZoneServer.GameServerManager;


namespace ZoneServer
{
    public class ConsoleFunctions
    {
        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_SIZE = 0xF000;
        const uint ENABLE_QUICK_EDIT = 0x0040;
        const int STD_INPUT_HANDLE = -10;

        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public static void DisableResize()
        {
            //DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
        }

        public static bool DisableMouse()
        {

            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);

            // get current console mode
            uint consoleMode;
            if (!GetConsoleMode(consoleHandle, out consoleMode))
            {
                // ERROR: Unable to get console mode.
                return false;
            }

            // Clear the quick edit bit in the mode flags
            consoleMode &= ~ENABLE_QUICK_EDIT;

            // set the new mode
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
                // ERROR: Unable to set console mode
                return false;
            }

            return true;
        }
    }



    public class Init
    {

        // 1º LogManager
        public static Logger logger;
        // 2º Data & Files config Manager
        // public static DataManager dataManager;

        // 3º DatabaseManager
        public static DatabaseManager dbManager;
        // 4º NetworkManager
        public static Server server;

        


        public static Manager GMS_Manager;

        // 5º ScriptManager
        public static ScriptManager scriptManager;

        // 6º LogicManager
        public static Proc proc;

        // 7º GameManager
        public static GameManager game;


        public static void Main(string[] args)
        {
            // set console options
            Console.Title = "MixMaster Server - v1.0 ";
            Console.CursorVisible = false;
            ConsoleFunctions.DisableMouse();
            
           

            // initialize LogManager
            logger = new Logger();
            logger.ConsoleLog("      ___   ___|_____ __   __ ___  _ __                      ", ConsoleColor.Cyan);
            logger.ConsoleLog(@"     / __| / _ \| '__|\ \ / // _ \| '__|                     ", ConsoleColor.Cyan);
            logger.ConsoleLog(@"     \__ \|  __/| |    \ V /|  __/| |                        ", ConsoleColor.Cyan);
            logger.ConsoleLog(@"     |___/ \___||_|     \_/  \___||_|                        ", ConsoleColor.Cyan);
            logger.ConsoleLog("                                                             ", ConsoleColor.Cyan);
            logger.ConsoleLog("      Copyright (C) 2019 - Andre Murilo ", ConsoleColor.DarkCyan);
            Console.WriteLine("\n");

            // Initialize GameManager
            game = new GameManager();

            // initialize ScriptManager
            scriptManager = new ScriptManager();

            // Initialize DataBase
            dbManager = new DatabaseManager();

            // initialize server & connection
            server = new Server(20165);

            // initialize GMS connection
            GMS_Manager = new Manager();

            // initialize proc & logic server
            proc = new Proc();

            // Load All Scripts     
            scriptManager.LoadAll();

            game.mapManager.LoadAllMaps();
            
            logger.WriteLog("Servidor iniciado com sucesso!", LogStatus.NormalInfo);
            // wait this process exit


            // functiona apenas em windows & mac
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
