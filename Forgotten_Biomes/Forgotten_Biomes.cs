using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Forgotten_Biomes
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class Forgotten_Biomes : BaseUnityPlugin
    {
        const string pluginGUID = "alree.forgotten_biomes";
        const string pluginName = "Forgotten Biomes";
        public const string pluginVersion = "0.0.18";
        public static ManualLogSource log;

        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> debugEnabled;
        public static ConfigEntry<bool> LoadConfigFromFiles;
        public static ConfigEntry<int> NexusID;

        public static ConfigEntry<bool> ashlands_gen;
        public static ConfigEntry<bool> ashlands_genloc;
        
        public static ConfigEntry<bool> deepnorth_gen;
        public static ConfigEntry<bool> deepnorth_genloc;
        
        public static ConfigEntry<bool> meadows_gen;
        public static ConfigEntry<bool> meadows_genloc;
        
        public static ConfigEntry<bool> blackforest_gen;
        public static ConfigEntry<bool> blackforest_genloc;
        
        public static ConfigEntry<bool> mountain_gen;
        public static ConfigEntry<bool> mountain_genloc;
        
        public static ConfigEntry<bool> swamp_gen;
        public static ConfigEntry<bool> swamp_genloc;
        
        public static ConfigEntry<bool> plains_gen;
        public static ConfigEntry<bool> plains_genloc;
        
        public static ConfigEntry<bool> mistlands_gen;
        public static ConfigEntry<bool> mistlands_genloc;

        public static ConfigEntry<bool> ocean_gen;
        public static ConfigEntry<bool> ocean_genloc;

        public static ConfigEntry<bool> ashDamage;
        public static ConfigEntry<float> ashDamageValue;
        public static ConfigEntry<bool> ashDamageResist;

        public static ConfigEntry<bool> mistDamage;
        public static ConfigEntry<float> mistDamageValue;
        public static ConfigEntry<bool> mistDamageResist;

        public static ConfigEntry<bool> deepnorthDamage;
        public static ConfigEntry<float> deepnorthDamageValue;
        public static ConfigEntry<bool> deepnorthDamageResist;

        public static ConfigEntry<bool> HiddenLocations;
        public static ConfigEntry<bool> DisabledLocations;
        public static ConfigEntry<bool> RemoveLocations;

        public static ConfigEntry<bool> DisabledClutter;
        public static ConfigEntry<bool> ShadersForce;
        public static ConfigEntry<bool> SpawnLocation;
        
        public static ConfigEntry<float> locationMinDistance;
        
        public static string[] AshLands_Veg;
        public static string[] DeepNorth_Veg;
        public static string[] Meadows_Veg;
        public static string[] BlackForest_Veg;
        public static string[] Mountain_Veg;
        public static string[] Swamp_Veg;
        public static string[] Plains_Veg;
        public static string[] Mistlands_Veg;
        public static string[] Ocean_Veg;

        public static string[] Hidden_Loc;
        public static string[] Disabled_Loc;
        public static string[] Remove_Loc;
        
        public static bool InjectScript = false;

        public static string FB_Dir = Path.Combine(BepInEx.Paths.ConfigPath, "Forgotten_Biomes") + "/";

        public static GameObject objToSpawn;

        Harmony harmony = new Harmony(pluginGUID);

        // OBJECTS
        public static GameObject AshLands_Veg_gameObject;

        void Awake()
        {
           
            log = BepInEx.Logging.Logger.CreateLogSource("Forgotten_Biomes");

            Forgotten_Biomes.modEnabled = base.Config.Bind<bool>("_General", "Enabled", true, "Enable this mod");
            Forgotten_Biomes.debugEnabled = base.Config.Bind<bool>("_General", "Debug Mode", false, "Enable more log");
            Forgotten_Biomes.LoadConfigFromFiles = base.Config.Bind<bool>("_General", "Load Config from files", true, "Force use files instead of internal configuration (set to TRUE if you play on linux based server)");
            Forgotten_Biomes.NexusID = base.Config.Bind<int>("_General", "NexusID", 1128, "Nexus ID");

            Forgotten_Biomes.ashlands_gen = base.Config.Bind<bool>("Biomes", "AshLands", true, "Add vegetation/rocks in AshLands");
            Forgotten_Biomes.ashlands_genloc = base.Config.Bind<bool>("Biomes", "AshLands Locations", true, "Add locations in AshLands");

            Forgotten_Biomes.deepnorth_gen = base.Config.Bind<bool>("Biomes", "DeepNorth", true, "Add vegetation/rocks in DeepNorth");
            Forgotten_Biomes.deepnorth_genloc = base.Config.Bind<bool>("Biomes", "DeepNorth Locations", true, "Add locations in DeepNorth");

            Forgotten_Biomes.meadows_gen = base.Config.Bind<bool>("Biomes", "Meadows", false, "Add vegetation/rocks in Meadows");
            Forgotten_Biomes.meadows_genloc = base.Config.Bind<bool>("Biomes", "Meadows Locations", true, "Add locations in Meadows");

            Forgotten_Biomes.blackforest_gen = base.Config.Bind<bool>("Biomes", "BlackForest", false, "Add vegetation/rocks in BlackForest");
            Forgotten_Biomes.blackforest_genloc = base.Config.Bind<bool>("Biomes", "BlackForest Locations", true, "Add locations in BlackForest");

            Forgotten_Biomes.mountain_gen = base.Config.Bind<bool>("Biomes", "Mountain", false, "Add vegetation/rocks in Mountains");
            Forgotten_Biomes.mountain_genloc = base.Config.Bind<bool>("Biomes", "Mountain Locations", true, "Add locations/rocks in Mountains");

            Forgotten_Biomes.swamp_gen = base.Config.Bind<bool>("Biomes", "Swamp", false, "Add vegetation/rocks in Swamp");
            Forgotten_Biomes.swamp_genloc = base.Config.Bind<bool>("Biomes", "Swamp Locations", true, "Add locations in Swamp");

            Forgotten_Biomes.plains_gen = base.Config.Bind<bool>("Biomes", "Plains", false, "Add vegetation/rocks in Plains");
            Forgotten_Biomes.plains_genloc = base.Config.Bind<bool>("Biomes", "Plains Locations", true, "Add locations in Plains");

            Forgotten_Biomes.mistlands_gen = base.Config.Bind<bool>("Biomes", "Mistlands", true, "Add vegetation/rocks in Mistlands");
            Forgotten_Biomes.mistlands_genloc = base.Config.Bind<bool>("Biomes", "Mistlands Locations", true, "Add locations in Mistlands");

            Forgotten_Biomes.ocean_gen = base.Config.Bind<bool>("Biomes", "Ocean", false, "Add vegetation/rocks in Ocean");

            Forgotten_Biomes.ashDamage = base.Config.Bind<bool>("Config Ashlands", "Ashrain and AshLands damage", true, "Enable burn damage on Ashrain and AshLands");
            Forgotten_Biomes.ashDamageValue = base.Config.Bind<float>("Config Ashlands", "Damage value", 10f, "Damage done to your life");
            Forgotten_Biomes.ashDamageResist = base.Config.Bind<bool>("Config Ashlands", "Decrease Fire Resist", true, "Reduces the resistance needed to travel in AshLands without burning (You should be Resistant, and not Very Resistant)");

            Forgotten_Biomes.mistDamage = base.Config.Bind<bool>("Config Mistlands", "Mistlands damage", true, "Enable poison damage on Mistlands");
            Forgotten_Biomes.mistDamageValue = base.Config.Bind<float>("Config Mistlands", "Damage value", 10f, "Damage done to your life");
            Forgotten_Biomes.mistDamageResist = base.Config.Bind<bool>("Config Mistlands", "Decrease Poison Resist", true, "Reduces the resistance needed to travel in Mistland without be poisoned (You should be Resistant, and not Very Resistant)");

            Forgotten_Biomes.deepnorthDamage = base.Config.Bind<bool>("Config DeepNorth", "DeepNorth damage", true, "Enable thouger frost damage in DeepNorth");
            Forgotten_Biomes.deepnorthDamageValue = base.Config.Bind<float>("Config DeepNorth", "Damage value", 10f, "Damage done to your life");
            Forgotten_Biomes.deepnorthDamageResist = base.Config.Bind<bool>("Config DeepNorth", "Decrease Frost Resist", true, "Reduces the resistance needed to travel in DeepNorth (You should be Resistant, and not Very Resistant)");

            Forgotten_Biomes.HiddenLocations = base.Config.Bind<bool>("Locations", "Hidden Location", false, "Add 'Hidden locations' to the World Generation");
            Forgotten_Biomes.DisabledLocations = base.Config.Bind<bool>("Locations", "Disabled Location", false, "Add 'Disabled locations' to the World Generation");
            Forgotten_Biomes.RemoveLocations = base.Config.Bind<bool>("Locations", "Remove Location", false, "Remove locations from World Generation");
            Forgotten_Biomes.locationMinDistance = base.Config.Bind<float>("Locations", "Distance from center", 200f, "Minimal distance from map center to spawn Locations.");
            
            Forgotten_Biomes.DisabledClutter = base.Config.Bind<bool>("Clutter", "Disabled Clutter", true, "Add 'Disabled Clutter' to Grass Generation.");
            Forgotten_Biomes.SpawnLocation = base.Config.Bind<bool>("Commands", "Spawn Location", true, "commands 'location' prevent world save, if 'true' the world will save even with a use of 'location' command.");
            





            if (Forgotten_Biomes.modEnabled.Value)
            {

                if (!Directory.Exists(Path.Combine(BepInEx.Paths.ConfigPath, "Forgotten_Biomes") + "/"))
                {
                    ZLog.Log("Forgotten_Biomes: moving config files (big thanks to thedefside)"); 
                    Directory.CreateDirectory(Path.Combine(BepInEx.Paths.ConfigPath, "Forgotten_Biomes") + "/");
                    string[] loc_files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/", "*.loc");
                    foreach (string file in loc_files)
                    {
                        File.Copy(file, Path.Combine(BepInEx.Paths.ConfigPath, "Forgotten_Biomes") + "/" + Path.GetFileName(file));
                    }
                    string[] veg_files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/", "*.veg");
                    foreach (string file in veg_files)
                    {
                        File.Copy(file, Path.Combine(BepInEx.Paths.ConfigPath, "Forgotten_Biomes") + "/" + Path.GetFileName(file));
                    }
                }


                int p = (int)Environment.OSVersion.Platform;
                if (!LoadConfigFromFiles.Value)
                {
                    AshLands_Veg = AshLands_Vegetation;
                    DeepNorth_Veg = DeepNorth_Vegetation;
                    Mistlands_Veg = MistLand_Vegetation;
                    Hidden_Loc = Hidden_Location;
                    DisabledLocations.Value = false;
                    RemoveLocations.Value = false;

                }
                else
                {
                    LoadVegAndLocs();
                }
                harmony.PatchAll();

            }
        }


        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        public void LoadVegAndLocs()
        {
            if (debugEnabled.Value)
            {
                ZLog.Log("Forgotten_Biomes: Loading config files to memory...");
            }
            try
            {
                if (File.Exists(FB_Dir + "ashlands.veg") && Forgotten_Biomes.ashlands_gen.Value)
                {
                    AshLands_Veg = File.ReadLines(FB_Dir + "ashlands.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "ashlands.veg");
                }

                if (File.Exists(FB_Dir + "deepnorth.veg") && Forgotten_Biomes.deepnorth_gen.Value)
                {
                    DeepNorth_Veg = File.ReadLines(FB_Dir + "deepnorth.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "deepnorth.veg");
                }

                if (File.Exists(FB_Dir + "meadows.veg") && Forgotten_Biomes.meadows_gen.Value)
                {
                    Meadows_Veg = File.ReadLines(FB_Dir + "meadows.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "meadows.veg");
                }

                if (File.Exists(FB_Dir + "blackforest.veg") && Forgotten_Biomes.blackforest_gen.Value)
                {
                    BlackForest_Veg = File.ReadLines(FB_Dir + "blackforest.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "blackforest.veg");
                }

                if (File.Exists(FB_Dir + "mountain.veg") && Forgotten_Biomes.mountain_gen.Value)
                {
                    Mountain_Veg = File.ReadLines(FB_Dir + "mountain.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "mountain.veg");
                }

                if (File.Exists(FB_Dir + "swamp.veg") && Forgotten_Biomes.swamp_gen.Value)
                {
                    Swamp_Veg = File.ReadLines(FB_Dir + "swamp.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "swamp.veg");
                }

                if (File.Exists(FB_Dir + "plains.veg") && Forgotten_Biomes.plains_gen.Value)
                {
                    Plains_Veg = File.ReadLines(FB_Dir + "plains.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "plains.veg");
                }

                if (File.Exists(FB_Dir + "mistlands.veg") && Forgotten_Biomes.mistlands_gen.Value)
                {
                    Mistlands_Veg = File.ReadLines(FB_Dir + "mistlands.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "mistlands.veg");
                }

                if (File.Exists(FB_Dir + "ocean.veg") && Forgotten_Biomes.ocean_gen.Value)
                {
                    Ocean_Veg = File.ReadLines(FB_Dir + "ocean.veg").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "ocean.veg");
                }

                if (File.Exists(FB_Dir + "hiddenlocations.loc") && Forgotten_Biomes.HiddenLocations.Value)
                {
                    Hidden_Loc = File.ReadLines(FB_Dir + "hiddenlocations.loc").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "hiddenlocations.loc");
                }

                if (File.Exists(FB_Dir + "disabledlocation.loc") && Forgotten_Biomes.DisabledLocations.Value)
                {
                    Disabled_Loc = File.ReadLines(FB_Dir + "disabledlocation.loc").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "disabledlocation.loc");
                }

                if (File.Exists(FB_Dir + "removelocation.loc") && Forgotten_Biomes.RemoveLocations.Value)
                {
                    Remove_Loc = File.ReadLines(FB_Dir + "removelocation.loc").ToArray();
                    ZLog.Log("Forgotten_Biomes: loaded config from : " + FB_Dir + "removelocation.loc");
                }

                if (debugEnabled.Value)
                {
                    ZLog.Log("Forgotten_Biomes: config files loaded");
                }
            } catch(Exception ex)
            {
                ZLog.LogError("Forgotten_Biomes ERROR: " + ex.Message);
            }

        }

        public string[] AshLands_Vegetation = new string[]
        {
            "Waystone",
            "Skull1",
            "Skull2",
            "silvervein",
            "Rock_3",
            "Rock_4",
            "Rock_4_plains",
            "rock1_mountain",
            "rock2_heath",
            "rock2_mountain",
            "rock3_mountain",
            "rock3_silver",
            "rock4_coast",
            "rock4_copper",
            "rock4_forest",
            "rock4_heath",
            "rock_a",
            "rockformation1",
            "MineRock_Obsidian",
            "Pickable_Stone",
            "Pickable_Flint",
            "HeathRockPillar",
            "HugeStone1",
            "vfx_edge_clouds",
            "vfx_ocean_clouds",
            "vfx_swamp_mist"

        };
        public string[] DeepNorth_Vegetation = new string[] { 
            "Waystone",
            "silvervein",
            "Rock_3",
            "Rock_4",
            "Rock_4_plains",
            "rock1_mountain",
            "rock2_heath",
            "rock2_mountain",
            "rock3_mountain",
            "rock3_silver",
            "rock4_coast",
            "rock4_copper",
            "rock4_forest",
            "rock4_heath",
            "rock_a",
            "rockformation1",
            "Pickable_Stone",
            "Pickable_Flint",
            "HeathRockPillar",
            "HugeStone1",
            "ice1"
        };

        public string[] MistLand_Vegetation = new string[] {
        "tunnel_web",
        "vertical_web",
        "horizontal_web",
        "Skull1",
        "Skull2",
        "SwampTree2_darkland",
        "HugeRoot1",
        "FirTree_small_dead",
        "stubbe",
        "Pinetree_01",
        "GlowingMushroom",
        "Rock_3",
        "Rock_4",
        };

        public string[] Hidden_Location = new string[]
        {
            "TrollCamp",
            "CopperMine1",
            "CopperMine2",
            "CopperMine3",
            //"StoneTower",
            "StoneCircle_old",
            "XMasTree",
            "IronMine1",
            "IronMine2",
            "Runestone_Surtlings",
            "Runestone_Wraith",
            "StaminaTroll",
            "StaminaWraith",
            "StaminaGreydwarf",
            "StoneHouse6",
            "StoneTowerU",
            "StoneTowerV",
            "StoneTowerW",
            "StoneTowerX",
            "StoneTowerY",
            "StoneTowerZ",
            "StoneKeepX",
            "StoneKeepY",
            "StoneKeepZ",
            "ShipWreckOcean01",
            "ShipWreckOcean02",
            "ShipWreckOcean03",
            "ShipWreckOcean04",
            "WoodHouse14",
            "WoodHouse15",
            "WoodFarm1_Old",
            "StoneVillage1",
            "StoneVillage2",
            "StoneWall1",
            "StoneWall2",
            "SunkenCrypt4old",
            "SwampRuinX",
            "SwampRuinY",
            "Oktanc1",
            "Oktanc2",
            "AbandonedLogCabin01",
            "StoneTowerRuins06"
        };


    }

 

}
