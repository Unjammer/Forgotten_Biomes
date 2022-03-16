using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Forgotten_Biomes.Locations
{
    [HarmonyPatch(typeof(ZoneSystem))]
    [HarmonyPatch(nameof(ZoneSystem.SpawnProxyLocation))]
    public class ZoneSystem_SpawnProxyLocation_Patch
    {
        public static bool Prefix(ref ZoneSystem __instance, int hash, int seed, Vector3 pos, Quaternion rot)
        {
            ZoneSystem.ZoneLocation location = __instance.GetLocation(hash);
            if (location != null && (location.m_prefabName.StartsWith("StoneKeep") || location.m_prefabName == "StoneTowerRuins06" || location.m_prefabName.StartsWith("StoneVillage")))
            {
                ZLog.Log(location.m_prefabName + " spawned [" + __instance.GetLocation(hash) + "] (" + location.m_prefab.transform.position.x +", " + location.m_prefab.transform.position.y +", " + location.m_prefab.transform.position.z +")");

                foreach (ZNetView netView in location.m_netViews)
                {
                        netView.m_persistent = false;
                }
            }
            return true;
        }
    }





    [HarmonyPatch(typeof(ZoneSystem))]
    [HarmonyPatch(nameof(ZoneSystem.CreateLocationProxy))]
    public class ZoneSystem_CreateLocationProxy_Patch
    {
        public static bool Prefix(ref ZoneSystem __instance, ZoneSystem.ZoneLocation location, int seed, Vector3 pos, Quaternion rotation, ZoneSystem.SpawnMode mode, List<GameObject> spawnedGhostObjects)
        {
            if (location != null && location.m_prefabName.StartsWith("StoneKeep") || location.m_prefabName.StartsWith("StoneTowerRuins06") || location.m_prefabName.StartsWith("StoneVillage"))
            {
                 return false;
            }
            return true;
        }
    }


    [HarmonyPatch(typeof(ZoneSystem))]
    [HarmonyPatch(nameof(ZoneSystem.SetupLocations))]
    public class ZoneSystem_SetupLocations_Patch
    {
        public static bool Prefix(ref ZoneSystem __instance)
        {

            if (!Forgotten_Biomes.RemoveLocations.Value && !Forgotten_Biomes.DisabledLocations.Value && !Forgotten_Biomes.HiddenLocations.Value)
            {
                if (Forgotten_Biomes.debugEnabled.Value)
                {
                    ZLog.Log("Forgotten_Biomes: Locations generation is Disabled");
                }
                return true;
            }

            if (Forgotten_Biomes.HiddenLocations.Value && Forgotten_Biomes.Hidden_Loc != null)
            {
                try
                {
                    if (Forgotten_Biomes.debugEnabled.Value)
                    {
                        ZLog.Log("Forgotten_Biomes: Hidden_Locations Enabled and not Empty...");
                    }

                    foreach (string LocNam in Forgotten_Biomes.Hidden_Loc)
                    {
                        if (LocNam != String.Empty && !LocNam.StartsWith("#") && LocNam != null)
                        {
                            if (Forgotten_Biomes.debugEnabled.Value)
                            {
                                ZLog.Log("Forgotten_Biomes: Attempt to add " + LocNam + " to Locations");
                            }
                            Heightmap.BiomeArea LocArea = Heightmap.BiomeArea.Everything;
                            Heightmap.Biome LocBiome = Heightmap.Biome.Meadows;
                            bool LocSnapWater = false;
                            int Loc_minAltitude = 1;
                            int Loc_maxAltitude = 1000;
                            int LocQty = 25;
                            int LocMaxTerrainDelta = 3;
                            int LocminDistanceFromSimilar = 100;
                            float LocmaxDistance = 5000f;
                            float LocExteriorRadius = 10;

                            switch (LocNam)
                            {
                                case "DevHouse1":
                                    LocBiome = Heightmap.Biome.Meadows;
                                    LocminDistanceFromSimilar = 0;
                                    LocmaxDistance = 8000;
                                    LocQty = 10;
                                    break;
                                case "Runestone_Surtlings":
                                case "CaveTest":
                                case "StoneWall1":
                                case "StoneWall2":
                                    LocBiome = Heightmap.Biome.AshLands;
                                    LocmaxDistance = 10500f;
                                    LocQty = 100;
                                    break;
                                case "ShipWreckOcean01":
                                case "ShipWreckOcean02":
                                case "ShipWreckOcean03":
                                case "ShipWreckOcean04":

                                    LocBiome = Heightmap.Biome.AshLands;
                                    LocmaxDistance = 10500f;
                                    Loc_minAltitude = -1;
                                    Loc_maxAltitude = 0;
                                    LocQty = 50;
                                    LocMaxTerrainDelta = 10;
                                    break;
                                case "StoneTowerRuins06":
                                case "StoneTowerU":
                                case "StoneTowerV":
                                case "StoneTowerW":
                                case "StoneTowerX":
                                case "StoneTowerY":
                                case "StoneTowerZ":
                                case "StoneTower":
                                    LocBiome = Heightmap.Biome.Mountain;
                                    LocmaxDistance = 10500f;
                                    LocMaxTerrainDelta = 40;
                                    break;
                                case "Runestone_Wraith":
                                case "StaminaWraith":
                                    LocBiome = Heightmap.Biome.Mistlands;
                                    LocminDistanceFromSimilar = 150;
                                    LocmaxDistance = 8500f;
                                    break;
                                case "XMasTree":
                                    LocBiome = Heightmap.Biome.DeepNorth;
                                    LocmaxDistance = 10500f;
                                    LocQty = 5;
                                    break;
                                case "AbandonedLogCabin01":

                                    LocBiome = Heightmap.Biome.DeepNorth;
                                    LocmaxDistance = 10500f;
                                    break;
                                case "WoodFarm1_Old":
                                    LocExteriorRadius = 30f;
                                    break;
                                case "StoneVillage1":
                                case "StoneVillage2":
                                    LocBiome = Heightmap.Biome.Plains;
                                    LocExteriorRadius = 20f;
                                    LocmaxDistance = 8500f;
                                    break;
                                case "SunkenCrypt4old":
                                case "SwampRuinX":
                                case "SwampRuinY":
                                case "IronMine1":
                                case "IronMine2":
                                    LocBiome = Heightmap.Biome.Swamp;
                                    LocQty = 50;
                                    LocmaxDistance = 8500f;
                                    break;
                                case "TrollCamp":
                                case "CopperMine1":
                                case "CopperMine2":
                                case "CopperMine3":
                                case "StoneCircle_old":
                                case "StoneKeepX":
                                case "StoneKeepY":
                                case "StoneKeepZ":
                                case "Oktanc1":
                                case "Oktanc2":
                                    LocBiome = Heightmap.Biome.BlackForest;
                                    LocmaxDistance = 8500f;
                                    break;
                                case "StaminaTroll":
                                    LocBiome = Heightmap.Biome.BlackForest;
                                    LocminDistanceFromSimilar = 150;
                                    LocQty = 10;
                                    LocmaxDistance = 8500f;
                                    break;
                                case "StaminaGreydwarf":
                                    LocBiome = Heightmap.Biome.Meadows;
                                    LocminDistanceFromSimilar = 150;
                                    LocQty = 10;
                                    break;
                                default:
                                    LocArea = Heightmap.BiomeArea.Everything;
                                    LocBiome = Heightmap.Biome.Meadows;
                                    Loc_minAltitude = 0;
                                    Loc_maxAltitude = 1000;
                                    LocSnapWater = false;
                                    LocQty = 25;
                                    LocMaxTerrainDelta = 3;
                                    LocminDistanceFromSimilar = 100;
                                    LocExteriorRadius = 10f;
                                    LocmaxDistance = 10000f;
                                    break;
                            }

                            ZoneSystem.ZoneLocation NewLoc = new ZoneSystem.ZoneLocation
                            {
                                m_biome = LocBiome,
                                m_biomeArea = LocArea,
                                m_centerFirst = false,
                                m_chanceToSpawn = 10,
                                m_enable = true,
                                m_interiorRadius = 10,
                                m_exteriorRadius = LocExteriorRadius,
                                m_foldout = false,
                                m_forestTresholdMax = 1,
                                m_forestTresholdMin = 0,
                                m_group = "Forgotten_Biomes",
                                m_hash = LocNam.GetStableHashCode(),
                                m_iconAlways = false,
                                m_iconPlaced = false,
                                m_inForest = false,
                                m_location = new Location(),
                                m_minDistance = Forgotten_Biomes.locationMinDistance.Value,
                                m_maxDistance = LocmaxDistance,
                                m_minAltitude = Loc_minAltitude,
                                m_maxAltitude = Loc_maxAltitude,
                                m_minDistanceFromSimilar = LocminDistanceFromSimilar,
                                m_minTerrainDelta = 0,
                                m_maxTerrainDelta = LocMaxTerrainDelta,
                                m_netViews = new List<ZNetView>(),
                                m_prefab = null,
                                m_prefabName = LocNam,
                                m_prioritized = false,
                                m_quantity = LocQty,
                                m_randomRotation = true,
                                m_randomSpawns = new List<RandomSpawn>(),
                                m_slopeRotation = false,
                                m_snapToWater = LocSnapWater,
                                m_unique = false
                            };
                            if (NewLoc != null)
                            {
                                
                                if (Forgotten_Biomes.ashlands_genloc.Value && NewLoc.m_biome == Heightmap.Biome.AshLands)
                                {
                                    
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to AshLands");
                                    }
                                }
                                if (Forgotten_Biomes.deepnorth_genloc.Value && NewLoc.m_biome == Heightmap.Biome.DeepNorth)
                                {
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to DeepNorth");
                                    }
                                }
                                if (Forgotten_Biomes.meadows_genloc.Value && NewLoc.m_biome == Heightmap.Biome.Meadows)
                                {
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to Meadows");
                                    }
                                }
                                if (Forgotten_Biomes.blackforest_genloc.Value && NewLoc.m_biome == Heightmap.Biome.BlackForest)
                                {
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to BlackForest");
                                    }
                                }
                                if (Forgotten_Biomes.mountain_genloc.Value && NewLoc.m_biome == Heightmap.Biome.Mountain)
                                {
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to Mountain");
                                    }
                                }
                                if (Forgotten_Biomes.swamp_genloc.Value && NewLoc.m_biome == Heightmap.Biome.Swamp)
                                {
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to Swamp");
                                    }
                                }
                                if (Forgotten_Biomes.plains_genloc.Value && NewLoc.m_biome == Heightmap.Biome.Plains)
                                {
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to Plains");
                                    }
                                }
                                if (Forgotten_Biomes.mistlands_genloc.Value && NewLoc.m_biome == Heightmap.Biome.Mistlands)
                                {
                                    ZoneSystem.instance.m_locations.Add(NewLoc);
                                    if (Forgotten_Biomes.debugEnabled.Value)
                                    {
                                        ZLog.Log("Forgotten_Biomes: " + NewLoc.m_prefabName + " added to Mistlands");
                                    }
                                }
                            }

                        }
                    }
                }catch (Exception ex)
                {
                        ZLog.Log("Forgotten_Biomes: Error while generating Hidden_Locations " + ex.Message);
                }
            }


            if (Forgotten_Biomes.DisabledLocations.Value && Forgotten_Biomes.Disabled_Loc != null)
            {
                try
                {
                    if (Forgotten_Biomes.debugEnabled.Value)
                    {
                        ZLog.Log("Forgotten_Biomes: Disabled_Locations Enabled and not Empty...");
                    }
                    //ZLog.Log("=========== LOCATIONS ==============");
                    //ZLog.Log("m_prefabName;m_biome;m_biomeArea;m_centerFirst;m_chanceToSpawn;m_enable;m_interiorRadius;m_exteriorRadius;m_foldout;m_forestTresholdMax;m_forestTresholdMin;m_group;m_hash;m_iconAlways;m_iconPlaced;m_inForest;m_location;m_minDistance;m_maxDistance;m_minAltitude;m_maxAltitude;m_minDistanceFromSimilar;m_minTerrainDelta;m_maxTerrainDelta;m_netViews;m_prefab;m_prioritized;m_quantity;m_randomRotation;m_randomSpawns;m_slopeRotation;m_snapToWater;m_unique");
                    foreach (ZoneSystem.ZoneLocation location in __instance.m_locations)
                    {
                        //ZLog.Log(location.m_prefabName + "; " + location.m_biome + "; " + location.m_biomeArea + "; " + location.m_centerFirst + "; " + location.m_chanceToSpawn + "; " + location.m_enable + "; " + location.m_interiorRadius + "; " + location.m_exteriorRadius + "; " + location.m_foldout + "; " + location.m_forestTresholdMax + "; " + location.m_forestTresholdMin + "; " + location.m_group + "; " + location.m_hash + "; " + location.m_iconAlways + "; " + location.m_iconPlaced + "; " + location.m_inForest + "; " + location.m_location + "; " + location.m_minDistance + "; " + location.m_maxDistance + "; " + location.m_minAltitude + "; " + location.m_maxAltitude + "; " + location.m_minDistanceFromSimilar + "; " + location.m_minTerrainDelta + "; " + location.m_maxTerrainDelta + "; " + location.m_netViews + "; " + location.m_prefab + "; " + location.m_prioritized + "; " + location.m_quantity + "; " + location.m_randomRotation + "; " + location.m_randomSpawns + "; " + location.m_slopeRotation + "; " + location.m_snapToWater + "; " + location.m_unique);
                        if (Forgotten_Biomes.Disabled_Loc.Any(location.m_prefabName.Equals))
                        {
                            if (location.m_prefabName != String.Empty && !location.m_prefabName.StartsWith("#") && location.m_prefabName != null)
                            {
                                location.m_enable = true;

                                if (location.m_quantity < 1)
                                {
                                    location.m_quantity = 25;
                                }
                                if (Forgotten_Biomes.debugEnabled.Value)
                                {
                                    ZLog.Log("Forgotten_Biomes: Disabled_Locations " + location.m_prefabName + " Enabled and not Empty...");
                                }
                            }
                        }
                    }
                }catch (Exception ex)
                {
                    ZLog.Log("Forgotten_Biomes: Error while generating Disabled_Locations " + ex.Message);
                }
            }

            if (Forgotten_Biomes.RemoveLocations.Value && Forgotten_Biomes.Remove_Loc != null)
            {
                try
                {
                    if (Forgotten_Biomes.debugEnabled.Value)
                    {
                        ZLog.Log("Forgotten_Biomes: Remove_Locations Enabled and not Empty...");
                    }
                    foreach (ZoneSystem.ZoneLocation location in __instance.m_locations.ToList())
                {
                    if (Forgotten_Biomes.Remove_Loc.Any(location.m_prefabName.Equals) && location.m_prefabName != "StartTemple")
                    {
                        if (location.m_prefabName != String.Empty && !location.m_prefabName.StartsWith("#") && location.m_prefabName != null)
                        {
                            __instance.m_locations.Remove(location);
                                if (Forgotten_Biomes.debugEnabled.Value)
                                {
                                    ZLog.Log("Forgotten_Biomes: Removed location " + location.m_prefabName + " from World Generation...");
                                }
                            }
                    }
                }
                }
                catch (Exception ex)
                {
                    ZLog.Log("Forgotten_Biomes: Error while Remove Locations " + ex.Message);
                }
            }

           

            if (Forgotten_Biomes.debugEnabled.Value)
            {
                ZLog.Log("Forgotten_Biomes: All Locations added to World Generation...");
            }

            GameObject[] array = Resources.FindObjectsOfTypeAll<GameObject>();
            List<Location> list = new List<Location>();
            List<LocationList> list2 = new List<LocationList>();
            GameObject[] array2 = array;
            foreach (GameObject gameObject in array2)
            {
                if (gameObject.name == "_Locations")
                {
                   
                    Location[] componentsInChildren = gameObject.GetComponentsInChildren<Location>(includeInactive: true);
                    list.AddRange(componentsInChildren);
                    LocationList component = gameObject.GetComponent<LocationList>();
                    if ((bool)component)
                    {
                        list2.Add(component);
                        
                    }
                }
            }
            list2.Sort((LocationList a, LocationList b) => a.m_sortOrder.CompareTo(b.m_sortOrder));
            foreach (LocationList item in list2)
            {
                __instance.m_locations.AddRange(item.m_locations);
                ZLog.Log("Added " + item.m_locations.Count + " locations from " + item.gameObject.scene.name);
            }
           foreach (Location item2 in list)
            {
                item2.m_applyRandomDamage = true;

                if (item2.transform.gameObject.activeInHierarchy)
                {
                    __instance.m_error = true;
                }
            }
            foreach (ZoneSystem.ZoneLocation location2 in __instance.m_locations.ToList())
            {
                Transform transform = null;
                foreach (Location item3 in list)
                {
                    if (item3.gameObject.name == location2.m_prefabName)
                    {
                        transform = item3.transform;
                        break;
                    }
                }
                if (transform == null && !location2.m_enable)
                {
                    continue;
                }
                if (transform == null)
                {
                    __instance.m_locations.Remove(location2);
                    if (Forgotten_Biomes.debugEnabled.Value)
                    {
                        ZLog.Log("Forgotten_Biomes: Removed " + location2.m_prefabName + " (invalid/corrupted location)");
                    }
                    continue;
                }
                else
                {
                    location2.m_prefab = transform.gameObject;
                    location2.m_hash = location2.m_prefab.name.GetStableHashCode();
                    Location location = (location2.m_location = location2.m_prefab.GetComponentInChildren<Location>());
                    location2.m_interiorRadius = (location.m_hasInterior ? location.m_interiorRadius : 0f);
                    location2.m_exteriorRadius = location.m_exteriorRadius;
                    location2.m_prefab.SetActive(true);

                    if (Application.isPlaying)
                    {
                        ZoneSystem.PrepareNetViews(location2.m_prefab, location2.m_netViews);
                        ZoneSystem.PrepareRandomSpawns(location2.m_prefab, location2.m_randomSpawns);
                        if (!__instance.m_locationsByHash.ContainsKey(location2.m_hash))
                        {
                            __instance.m_locationsByHash.Add(location2.m_hash, location2);
                        }
                    }
                }
            }
            if (Forgotten_Biomes.debugEnabled.Value)
            {
                ZLog.Log("Forgotten_Biomes: End Locations Generation...");
            }


            return false;
        }
        
    }
 }
