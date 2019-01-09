using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZoneServer
{
    public enum LogStatus
    {
        FatalError = 0,
        CriticalError = 1,
        SystemError = 2,
        GameError = 3,
        DataError = 4,
        DatabaseError = 12,
        NetworkError = 5,
        ScriptManagerError = 6,
        NormalInfo = 7,
        NetworkInfo = 8,
        GameInfo = 9,
        ScriptManagerInfo = 10,
        DatabaseInfo = 13,
        UnknowError = 11
    };

    public class Logger
    {
        private string logDIR;
        private string fileName;
        private string fileExt;

        public Logger()
        {
            logDIR = Directory.GetCurrentDirectory() + @"/log/";
            fileExt = ".log";
            UpdateFilename();
            if (!Directory.Exists(logDIR))
                Directory.CreateDirectory(logDIR);

            this.WriteLog("----------------------- STARTING SERVER -----------------------" + Environment.NewLine, LogStatus.NormalInfo);
        }

        private void UpdateFilename()
        {
            fileName = DateTime.Now.ToString("dd-MM-yyyy") + fileExt;
        }

        public void DeleteAllLogs()
        {
            foreach(string file in Directory.GetFiles(logDIR))
            {
                try
                {
                    File.Delete(file);
                }
                catch {}
            }
        }

        private void CreateLogFile()
        {
            if (!File.Exists(logDIR + fileName))
                File.Create(logDIR + fileName);
        }

        private string PrepareMessage(LogStatus status)
        {
            string messageType = "";
            switch(status)
            {
                case LogStatus.CriticalError:
                    messageType = "[CRITICAL ERROR]";
                    break;
                case LogStatus.DataError:
                    messageType = "[DATA ERROR]";
                    break;
                case LogStatus.DatabaseError:
                    messageType = "[DATABASE ERROR]";
                    break;
                case LogStatus.FatalError:
                    messageType = "[FATAL ERROR]";
                    break;
                case LogStatus.GameError:
                    messageType = "[GAME SERVER ERROR]";
                    break;
                case LogStatus.NetworkError:
                    messageType = "[NETWORK SERVER ERROR]";
                    break;
                case LogStatus.ScriptManagerError:
                    messageType = "[SCRIPT MANAGER ERROR]";
                    break;
                case LogStatus.SystemError:
                    messageType = "[SYSTEM ERROR]";
                    break;
                case LogStatus.NormalInfo:
                    messageType = "[INFORMATION INFO]";
                    break;
                case LogStatus.NetworkInfo:
                    messageType = "[NETWORK SERVER INFO]";
                    break;
                case LogStatus.GameInfo:
                    messageType = "[GAME SERVER INFO]";
                    break;
                case LogStatus.ScriptManagerInfo:
                    messageType = "[SCRIPT MANAGER INFO]";
                    break;
                case LogStatus.DatabaseInfo:
                    messageType = "[DATABASE INFO]";
                    break;
                case LogStatus.UnknowError:
                    messageType = "[UNKNOW ERROR]";
                    break;
                default:
                    messageType = "[UNKNOW ERROR]";
                    break;

            }
            string text = "[" + DateTime.Now.ToString() +"] " + messageType + ": ";
            return text;
        }

        public void WriteLog(string text, LogStatus status)
        {
            UpdateFilename();
            CreateLogFile();
            string path = logDIR + fileName;

            string message = PrepareMessage(status) + text;
            try
            {
                File.AppendAllText(path, message + Environment.NewLine);
            }
            catch
            {

            }
        }


   
        // console logger & colors
        public bool ConsoleLog(string text, int indexColor)
        {
            ConsoleColor lastColor = Console.ForegroundColor;
            Console.ForegroundColor = (ConsoleColor)indexColor;
            Console.WriteLine(text);


            Console.ForegroundColor = lastColor;
            return true;
        }
        public bool ConsoleLog(string text, ConsoleColor color)
        {
            ConsoleColor lastColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);


            Console.ForegroundColor = lastColor;
            return true;
        }

    }
}
