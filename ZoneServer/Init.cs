using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer
{


    class Init
    {
        static void Main(string[] args)
        {
            if(!BaseFunctions.ConsoleFunctions.DisableQuickEdit())
            {
                LogManager.CLogManager.WriteConsoleLog("[ERROR] Quick Edit disable failed!", ConsoleColor.Red);
            }

            // -> Load MY Info
            if(!MyInfo.LoadMyInfo())
            {
                LogManager.CLogManager.WriteConsoleLog("[ERROR] MY INFO load failed", ConsoleColor.Red);
                return;
            }

            // Load CLogManager(); -- Database and files
            

            // load ZS asyncSocket
            if(!Network.ZS.AsyncSocket.Start())
            {
                LogManager.CLogManager.WriteConsoleLog("[ERROR] ZS_Socket start failed", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("[SZ_Socket] Socket Created successfully!", ConsoleColor.Green);

            // connect Gamedata
            if (!Database.Gamedata.ConnectGamedata())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD GAMEDATA DB", ConsoleColor.Red);
                return;
            }

            // connect Member
            if (!Database.Member.ConnectGamedata())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD Member DB", ConsoleColor.Red);
                return;
            }

            // connect S_Data
            if (!Database.SData.ConnectGamedata())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD S_Data DB", ConsoleColor.Red);
                return;
            }

            // Connect Web_Account
            // ...

            //  Load_LvUserInfo
            if (!Database.SData.Load_LvUserInfo())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD s_lvuserinfo", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load LvUserInfo DB Successfully...", ConsoleColor.Yellow);

            // Load LvMonInfo
            if (!Database.SData.Load_LvMonInfo())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD LvMonInfo", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load LvMonInfo DB Successfully...", ConsoleColor.Yellow);


            // Load_Hero
            if (!Database.SData.Load_Hero())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD Hero", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load Hero DB Successfully...", ConsoleColor.Yellow);


            // Load Npc
            if (!Database.SData.Load_NPC())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD NPC", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load NPC DB Successfully...", ConsoleColor.Yellow);

            // Load Monster
            if (!Database.SData.Load_Monster())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD Monster", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load Monster DB Successfully...", ConsoleColor.Yellow);

            // Load MobItem
            if (!Database.SData.Load_MobItem())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD MobItem", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load MobItem DB Successfully...", ConsoleColor.Yellow);

            // Load SkillProperty
            if (!Database.SData.Load_SkillProperty())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD SkillProperty", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load SkillProperty DB Successfully...", ConsoleColor.Yellow);

            // Load SkillData
            if (!Database.SData.Load_SkillData())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD SkillData", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load SkillData DB Successfully...", ConsoleColor.Yellow);

            // Load Item
            if (!Database.SData.Load_Item())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD Item", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load Item DB Successfully...", ConsoleColor.Yellow);

            // Load ItemEffectiveData
            if (!Database.SData.Load_ItemEffectiveData())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD ItemEffectiveData", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load ItemEffectiveData DB Successfully...", ConsoleColor.Yellow);

            // Load ItemBox
            if (!Database.SData.Load_ItemBox())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD ItemBox", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load ItemBox DB Successfully...", ConsoleColor.Yellow);

            // Load Production
            if (!Database.SData.Load_Production())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD Production", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load Production DB Successfully...", ConsoleColor.Yellow);


            // Load ItemPowerAdd
            if (!Database.SData.Load_ItemPowerAdd())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD ItemPowerAdd", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load ItemPowerAdd DB Successfully...", ConsoleColor.Yellow);

            // Load Npc_Sale
            if (!Database.SData.Load_NpcSale())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD NPC_Sale", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load NPC_Sale DB Successfully...", ConsoleColor.Yellow);

            // Load Zone
            if (!Database.SData.Load_Zone())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD Zone", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load Zone DB Successfully...", ConsoleColor.Yellow);


            // Load Warp
            if (!Database.SData.Load_Gate())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD Gate", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load Gate DB Successfully...", ConsoleColor.Yellow);

            // Load MixSkill
            if (!Database.SData.Load_MixSkill())
            {
                LogManager.CLogManager.WriteConsoleLog("[DATABASE] FAILED TO LOAD MixSkill", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("    [I] Load MixSkill DB Successfully...", ConsoleColor.Yellow);


            // Load Guilds
            if (!Database.Gamedata.LoadGuilds())
            {
                LogManager.CLogManager.WriteConsoleLog("FAILED TO LOAD Guilds", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("[I] Guilds Loaded Successfully...", ConsoleColor.Green);

            // Load another tables s_data
            // ....
            // ....
            // ....

            // Start GMS Connection
           
            
            if(!Network.GMS.Connect.Start())
            {
                LogManager.CLogManager.WriteConsoleLog("[GMS] Connection Faile!", ConsoleColor.Red);
                return;
            }
            LogManager.CLogManager.WriteConsoleLog("[GMS] Connection successfully!", ConsoleColor.Green);
            Console.WriteLine("\n");

            LogManager.CLogManager.WriteConsoleLog("-------------------START-------------------", ConsoleColor.Gray);




            //Network.ZS.XHERO Hero = Database.Gamedata.LoadHero(2004, 2);
            //LogManager.CLogManager.WriteConsoleLog("Hero: " + Hero.name, ConsoleColor.Gray);

            /*
            for(int i=0; i < Data.SData.MixSkill.Count; i++)
            {
                Console.WriteLine("Item ID: " + Data.SData.MixSkill[i].MixSkillLelvel);
            }   */









            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
