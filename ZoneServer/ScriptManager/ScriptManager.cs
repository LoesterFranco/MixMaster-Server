using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using System.IO;

namespace ZoneServer.LuaScript
{

    public class XSCript
    {
        public Script script;
        public DynValue res;
        public string scriptData;


        public XSCript(Script _script, DynValue _res, string data)
        {
            script = _script;
            res = _res;
            scriptData = data;
        }
    }

    


    public class ScriptManager
    {

        public bool ready;
        public List<XSCript> scripts;
        public List<Timer> timers;
        public List<EventsHandler> eventsHandler;
        public Functions functions;
        public Events events;
        

        public ScriptManager()
        {
            scripts = new List<XSCript>();
            timers = new List<Timer>();
            eventsHandler = new List<EventsHandler>();
            functions = new Functions();
            events = new Events();
            SetObjectsEvent();
            ready = true;
            Init.logger.WriteLog("Script Manager foi inicializado com sucesso!", LogStatus.ScriptManagerInfo);
        }

        private void SetObjectsEvent()
        {
            for(int i = 0; i < timers.Count; i++)
            {
                timers[i].OnTerminate += (s, e) =>
                {
                    timers.Remove(timers[i]);
                };
            }

            // OnScriptStart
            events.OnScriptStart += (s, e) =>
            {
                TriggerEvent("OnScriptStart");
            };

            // OnScriptStop
            events.OnScriptStop += (s, e) =>
            {
                TriggerEvent("OnScriptStop");
            };

            // OnScriptAbort
            events.OnScriptAbort += (s, e) =>
            {
                TriggerEvent("OnScriptAbort");
            };
        }

        public void TriggerEvent(string EventName)
        {
            for(int i = 0; i < eventsHandler.Count; i++)
            {
                EventsHandler handler = eventsHandler[i];
                if(handler.eventExecuted)
                {
                    eventsHandler.Remove(handler);
                    return;
                }

                if(handler.eventName == EventName)
                {
                    handler.closure.Call();
                }
            }
        }
     
        public void Update(long gameTime)
        {
            // update timers
            for(int i = 0; i < timers.Count; i++)
            {
                timers[i].Update(gameTime);
            }
        }

        public void LoadAll()
        {
            // if Scripts dir not exist, then create directory
            string dir = Directory.GetCurrentDirectory() + @"/Scripts/";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // get all files by extension .lua
            string[] files = Directory.GetFiles(dir, "*.lua");
            foreach(string file in files)
            {
                string data = File.ReadAllText(file);
                if(data.Length >= 0 && data != string.Empty)
                {
                    Script script = new Script();
                    ImportFunctions(script);
                    DynValue res;
                    try
                    {
                        res = script.DoString(data);
                    }
                    catch(Exception e)
                    {
                        Init.logger.WriteLog("Falha no carregamento do script: " + Path.GetFileName(file) + "; " + e.Message, LogStatus.ScriptManagerError);
                        continue;
                    }
                    scripts.Add(new XSCript(script, res, data));
                    Init.logger.WriteLog("Script: " + Path.GetFileName(file) + " foi inicializado com sucesso!", LogStatus.ScriptManagerInfo);
                }
            }

            if (scripts.Count > 0)
            {
                Init.logger.ConsoleLog($"[ScriptManager] {scripts.Count} script(s) foram carregados!", ConsoleColor.Green);
                Init.logger.WriteLog($"{scripts.Count} script(s) foram carregados!", LogStatus.ScriptManagerInfo);
            }

            events.OnScriptStart(this, EventArgs.Empty);
        }
        private void ImportFunctions(Script script)
        {
            // CreateTimer(function, miliseconds, times);
            script.Globals["CreateTimer"] = (Func<Closure, long, int, bool>)functions.CreateTimer;
            script.Globals["CreateEventHandler"] = (Func<Closure, string, bool>)functions.CreateEventHandler;
            script.Globals["CLog"] = (Func<string, int, bool>)Init.logger.ConsoleLog;
            script.Globals["GetRandom"] = (Func<int, int, int>)functions.GetRandom;
        }

    }
}
