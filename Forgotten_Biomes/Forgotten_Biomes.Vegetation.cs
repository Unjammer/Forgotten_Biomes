using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Forgotten_Biomes.Vegetation
{

    [HarmonyPatch(typeof(ZoneSystem))]
    [HarmonyPatch(nameof(ZoneSystem.ValidateVegetation))]
    public class ZoneSystem_ValidateVegetation_Patch
    {
        
        public static bool Prefix(ref ZoneSystem __instance)
        {

            if (__instance == null)
            {
                if (Forgotten_Biomes.debugEnabled.Value)
                {
                    ZLog.Log("Forgotten_Biomes: ZoneSystem not initialized... ");
                }
                return true;
            }
            try
            {
                if (__instance.m_vegetation == null)
                {
                    if (Forgotten_Biomes.debugEnabled.Value)
                    {
                        ZLog.Log("Forgotten_Biomes: Vegetation list is Empty... ");
                    }

                        return true;
                }
                foreach (ZoneSystem.ZoneVegetation item in __instance.m_vegetation.ToList())
                {
                    string ItemName = item.m_prefab.ToString().Substring(0, item.m_prefab.ToString().Length - 25);
                   
                    if (Forgotten_Biomes.debugEnabled.Value)
                    {
                        ZLog.Log("Forgotten_Biomes: Attempt to add " + ItemName + " to Vegetation");
                    }
                    
                    if (item == null)
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: Vegetation item is null...");
                        }
                        continue;
                    }

                    if (Forgotten_Biomes.ashlands_gen.Value && Forgotten_Biomes.AshLands_Veg.Any(ItemName.Equals) && Forgotten_Biomes.AshLands_Veg != null)
                    {

                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "AshLands_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.AshLands,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome AshLands");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: AshLands vegetation is Disabled or Empty");
                        }
                    }


                    if (Forgotten_Biomes.deepnorth_gen.Value && Forgotten_Biomes.DeepNorth_Veg.Any(ItemName.Equals) && Forgotten_Biomes.DeepNorth_Veg != null)
                    {
                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "DeepNorth_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.DeepNorth,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome DeepNorth");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: DeepNorth vegetation is Disabled or Empty");
                        }
                    }

                    if (Forgotten_Biomes.meadows_gen.Value && Forgotten_Biomes.Meadows_Veg.Any(ItemName.Equals) && Forgotten_Biomes.Meadows_Veg != null)
                    {
                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "Meadows_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.Meadows,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome Meadows");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: Meadows vegetation is Disabled or Empty");
                        }
                    }

                    if (Forgotten_Biomes.blackforest_gen.Value && Forgotten_Biomes.BlackForest_Veg.Any(ItemName.Equals) && Forgotten_Biomes.BlackForest_Veg != null)
                    {
                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "Blackforest_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.BlackForest,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome BlackForest");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: BlackForest vegetation is Disabled or Empty");
                        }
                    }

                    if (Forgotten_Biomes.mountain_gen.Value && Forgotten_Biomes.Mountain_Veg.Any(ItemName.Equals) && Forgotten_Biomes.Mountain_Veg != null)
                    {
                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "Moutain_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.Mountain,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome Mountain");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: Mountain vegetation is Disabled or Empty");
                        }
                    }

                    if (Forgotten_Biomes.swamp_gen.Value && Forgotten_Biomes.Swamp_Veg.Any(ItemName.Equals) && Forgotten_Biomes.Swamp_Veg != null)
                    {
                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "Swamp_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.Swamp,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome Swamp");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: Swamp vegetation is Disabled or Empty");
                        }
                    }

                    if (Forgotten_Biomes.plains_gen.Value && Forgotten_Biomes.Plains_Veg.Any(ItemName.Equals) && Forgotten_Biomes.Plains_Veg != null)
                    {
                       


                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "Plains_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.Plains,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome Plains");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: Plains vegetation is Disabled or Empty");
                        }
                    }

                    if (Forgotten_Biomes.mistlands_gen.Value && Forgotten_Biomes.Mistlands_Veg.Any(ItemName.Equals) && Forgotten_Biomes.Mistlands_Veg != null)
                    {
                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "Mistlands_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.Mistlands,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome Mistlands");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: Mistlands vegetation is Disabled or Empty");
                        }
                    }

                    /* OCEAN */

                    if (Forgotten_Biomes.ocean_gen.Value && Forgotten_Biomes.Ocean_Veg.Any(ItemName.Equals) && Forgotten_Biomes.Ocean_Veg != null)
                    {
                        ZoneSystem.ZoneVegetation NewVeg = new ZoneSystem.ZoneVegetation
                        {
                            m_name = "Ocean_" + ItemName,
                            m_prefab = item.m_prefab,
                            m_enable = true,
                            m_min = item.m_min,
                            m_max = item.m_max,
                            m_forcePlacement = item.m_forcePlacement,
                            m_scaleMin = item.m_scaleMin,
                            m_scaleMax = item.m_scaleMax,
                            m_randTilt = item.m_randTilt,
                            m_chanceToUseGroundTilt = item.m_chanceToUseGroundTilt,
                            m_biome = Heightmap.Biome.Ocean,
                            m_biomeArea = item.m_biomeArea,
                            m_blockCheck = item.m_blockCheck,
                            m_minAltitude = item.m_minAltitude,
                            m_maxAltitude = item.m_maxAltitude,
                            m_minOceanDepth = item.m_minOceanDepth,
                            m_maxOceanDepth = item.m_maxOceanDepth,
                            m_minTilt = item.m_minTilt,
                            m_maxTilt = item.m_maxTilt,
                            m_terrainDeltaRadius = item.m_terrainDeltaRadius,
                            m_maxTerrainDelta = item.m_maxTerrainDelta,
                            m_minTerrainDelta = item.m_minTerrainDelta,
                            m_snapToWater = item.m_snapToWater,
                            m_groundOffset = item.m_groundOffset,
                            m_groupSizeMin = item.m_groupSizeMin,
                            m_groupSizeMax = item.m_groupSizeMax,
                            m_groupRadius = item.m_groupRadius,
                            m_inForest = item.m_inForest,
                            m_forestTresholdMin = item.m_forestTresholdMin,
                            m_forestTresholdMax = item.m_forestTresholdMax,
                            m_foldout = item.m_foldout
                        };
                        if (NewVeg.m_prefab.GetComponent<ZNetView>() == null)
                        {
                            NewVeg.m_prefab.AddComponent<ZNetView>();
                        }
                        if (NewVeg != null)
                        {
                            ZoneSystem.instance.m_vegetation.Add(NewVeg);
                        }
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + NewVeg.m_name + " added to Vegetation in biome Ocean");
                        }
                    }
                    else
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: Ocean vegetation is Disabled or Empty");
                        }
                    }

                }

                foreach (ZoneSystem.ZoneVegetation itemVerif in __instance.m_vegetation.ToList())
                {
                    if (!itemVerif.m_enable || itemVerif.m_prefab.GetComponent<ZNetView>() == null)
                    {
                        __instance.m_vegetation.Remove(itemVerif);
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Forgotten_Biomes: " + itemVerif.m_name + " has been removed from World Generation (Invalid or Corrupted)");
                        }
                    }
                }
                

                if (Forgotten_Biomes.debugEnabled.Value)
                {
                    ZLog.Log("Forgotten_Biomes: End of Vegetation Generation Patch");
                }

            } catch (Exception ex)
            {
                ZLog.Log("Forgotten_Biomes: Error while generating Vegetation > " + ex.Message);
            }
            return true;
        }
    }

 


  
    }
 
