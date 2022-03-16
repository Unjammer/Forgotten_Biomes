using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using UnityEngine;


namespace Forgotten_Biomes.Patch

{
    [HarmonyPatch(typeof(Player), "UpdateEnvStatusEffects")]
    public class UpdateEnvStatusEffects_patch
    {
        public static bool Prefix(ref Player __instance)
        {
            if (!Forgotten_Biomes.ashDamage.Value && !Forgotten_Biomes.mistDamage.Value && !Forgotten_Biomes.deepnorthDamage.Value)
            {
                if (Forgotten_Biomes.debugEnabled.Value)
                {
                    ZLog.Log("Forgotten_Biomes: Environment Effects Disabled ! (UpdateEnvStatusEffects_patch)");
                }
                return true;
            }


            HitData hit = new HitData();
            float resistantDmg = 0f;
            float weakDmg = 0f;
            float immuneDmg = 0f;

            if (Player.m_localPlayer.IsDead())
            {
                __instance.m_seman.RemoveStatusEffect("Burning", false);
                __instance.m_seman.RemoveStatusEffect("Poison", false);
                __instance.m_seman.RemoveStatusEffect("Frost", false);
            }

            HitData.DamageModifiers damageModifiers = __instance.GetDamageModifiers();
            if (Forgotten_Biomes.ashDamage.Value)
            {
                float normalDmg = (__instance.GetMaxHealth() / Forgotten_Biomes.ashDamageValue.Value);
                HitData.DamageModifier modifier = damageModifiers.GetModifier(HitData.DamageType.Fire);
                if ((EnvMan.instance.IsEnvironment("Ashrain") || EnvMan.instance.GetBiome().ToString() == "AshLands"))
                {
                    __instance.m_seman.AddStatusEffect("Burning", true);
                    SE_Burning sE_Burn = __instance.m_seman.GetStatusEffect("Burning") as SE_Burning;
                    sE_Burn.m_name = "Burning ashes";

                    if (!Forgotten_Biomes.ashDamageResist.Value)
                    {
                        normalDmg *= 2;
                    }
                    sE_Burn.m_fireDamagePerHit = hit.ApplyModifier(normalDmg, modifier, ref normalDmg, ref resistantDmg, ref weakDmg, ref immuneDmg);
                    if (Forgotten_Biomes.ashDamageResist.Value && (modifier == HitData.DamageModifier.Resistant || modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.IsSwiming() || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Burning") && (__instance.GetSEMan().GetStatusEffect("Burning") as SE_Burning).m_name == "Burning ashes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Burning", false);
                        }
                    }
                    else if (!Forgotten_Biomes.ashDamageResist.Value && (modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.IsSwiming() || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Burning") && (__instance.GetSEMan().GetStatusEffect("Burning") as SE_Burning).m_name == "Burning ashes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Burning", false);
                        }
                    }
                } else
                {
                    if (Forgotten_Biomes.ashDamageResist.Value && (modifier == HitData.DamageModifier.Resistant || modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.IsSwiming() || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Burning") && (__instance.GetSEMan().GetStatusEffect("Burning") as SE_Burning).m_name == "Burning ashes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Burning", false);
                        }
                    }
                    else if (!Forgotten_Biomes.ashDamageResist.Value && (modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.IsSwiming() || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Burning") && (__instance.GetSEMan().GetStatusEffect("Burning") as SE_Burning).m_name == "Burning ashes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Burning", false);
                        }
                    }
                }
            }

            if (Forgotten_Biomes.mistDamage.Value)
            {
                
                float normalDmg = (__instance.GetMaxHealth() / Forgotten_Biomes.mistDamageValue.Value);
                HitData.DamageModifier modifier = damageModifiers.GetModifier(HitData.DamageType.Poison);
                if (EnvMan.instance.GetBiome().ToString() == "Mistlands" && (Heightmap.GetHeight(Player.m_localPlayer.transform.position, out var height) && height > 30f))
                {
                    __instance.m_seman.AddStatusEffect("Poison", false);
                    SE_Poison sE_Poison = __instance.m_seman.GetStatusEffect("Poison") as SE_Poison;
                    sE_Poison.m_name = "Poisoning fumes";

                    if (!Forgotten_Biomes.mistDamageResist.Value)
                    {
                        normalDmg *= 2;
                    }

                    sE_Poison.m_damagePerHit = hit.ApplyModifier(normalDmg, modifier, ref normalDmg, ref resistantDmg, ref weakDmg, ref immuneDmg);
                    if (Forgotten_Biomes.mistDamageResist.Value && (modifier == HitData.DamageModifier.Resistant || modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Poison") && (__instance.GetSEMan().GetStatusEffect("Poison") as SE_Poison).m_name == "Poisoning fumes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Poison", false);
                        }
                    }
                    else if (!Forgotten_Biomes.mistDamageResist.Value && (modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Poison") && (__instance.GetSEMan().GetStatusEffect("Poison") as SE_Poison).m_name == "Poisoning fumes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Poison", false);
                        }
                    }
                } else
                {
                    if (Forgotten_Biomes.mistDamageResist.Value && (modifier == HitData.DamageModifier.Resistant || modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Poison") && (__instance.GetSEMan().GetStatusEffect("Poison") as SE_Poison).m_name == "Poisoning fumes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Poison", false);
                        }
                    }
                    else if (!Forgotten_Biomes.mistDamageResist.Value && (modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Poison") && (__instance.GetSEMan().GetStatusEffect("Poison") as SE_Poison).m_name == "Poisoning fumes")
                        {
                            __instance.m_seman.RemoveStatusEffect("Poison", false);
                        }
                    }
                }
            }


            if (Forgotten_Biomes.deepnorthDamage.Value)
            {
                float normalDmg = (__instance.GetMaxHealth() / Forgotten_Biomes.deepnorthDamageValue.Value);
                HitData.DamageModifier modifier = damageModifiers.GetModifier(HitData.DamageType.Frost);
                if (((EnvMan.instance.GetBiome().ToString() == "DeepNorth") || (EnvMan.instance.GetBiome().ToString() == "Mountain")) && (Player.m_localPlayer.transform.position.magnitude > 8000f && Player.m_localPlayer.transform.position.z > 0))
                {
                    __instance.m_seman.AddStatusEffect("Frost", true);
                    SE_Frost sE_Frost = __instance.m_seman.GetStatusEffect("Frost") as SE_Frost;
                    sE_Frost.m_name = "Frostbite";

                    if (!Forgotten_Biomes.deepnorthDamageResist.Value)
                    {
                        normalDmg *= 2;
                    }
                    sE_Frost.AddDamage(hit.ApplyModifier(normalDmg, modifier, ref normalDmg, ref resistantDmg, ref weakDmg, ref immuneDmg));
                    if (Forgotten_Biomes.deepnorthDamageResist.Value && (modifier == HitData.DamageModifier.Resistant || modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter") || __instance.m_seman.HaveStatusEffect("CampFire")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Frost") && (__instance.GetSEMan().GetStatusEffect("Frost") as SE_Frost).m_name == "Frostbite")
                        {
                            __instance.m_seman.RemoveStatusEffect("Frost", false);
                        }
                    }
                    else if (!Forgotten_Biomes.deepnorthDamageResist.Value && (modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter") || __instance.m_seman.HaveStatusEffect("CampFire")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Frost") && (__instance.GetSEMan().GetStatusEffect("Frost") as SE_Frost).m_name == "Frostbite")
                        {
                            __instance.m_seman.RemoveStatusEffect("Frost", false);
                        }
                    }
                }
                else

                {
                    if (Forgotten_Biomes.deepnorthDamageResist.Value && (modifier == HitData.DamageModifier.Resistant || modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter") || __instance.m_seman.HaveStatusEffect("CampFire")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Frost") && (__instance.GetSEMan().GetStatusEffect("Frost") as SE_Frost).m_name == "Frostbite")
                        {
                            __instance.m_seman.RemoveStatusEffect("Frost", false);
                        }
                    }
                    else if (!Forgotten_Biomes.deepnorthDamageResist.Value && (modifier == HitData.DamageModifier.VeryResistant || modifier == HitData.DamageModifier.Ignore || modifier == HitData.DamageModifier.Immune || __instance.m_seman.HaveStatusEffect("Shelter") || __instance.m_seman.HaveStatusEffect("CampFire")))
                    {
                        if (__instance.GetSEMan().HaveStatusEffect("Frost") && (__instance.GetSEMan().GetStatusEffect("Frost") as SE_Frost).m_name == "Frostbite")
                        {
                            __instance.m_seman.RemoveStatusEffect("Frost", false);
                        }
                    }
                }
            } 

            return true;
        }
    }






    [HarmonyPatch(typeof(ZNetView))]
    [HarmonyPatch(nameof(ZNetView.Awake))]
    public class ZNetView_Awake_Patch
    {
        public static bool Prefix(ref ZNetView __instance)
        {
            if (!Forgotten_Biomes.RemoveLocations.Value && !Forgotten_Biomes.DisabledLocations.Value && !Forgotten_Biomes.HiddenLocations.Value)
            {
                if (Forgotten_Biomes.debugEnabled.Value)
                {
                    ZLog.Log("Forgotten_Biomes: Locations Generations Disabled ! (ZNetView_Awake_Patch)");
                }
                return true;
            }
                if (ZNetView.m_useInitZDO && ZNetView.m_initZDO == null)
            {
                
                string prefabName = __instance.gameObject.name;
                __instance.m_zdo = ZDOMan.instance.CreateNewZDO(__instance.transform.position);
                __instance.m_zdo.m_persistent = false;
                __instance.m_zdo.m_type = __instance.m_type;
                __instance.m_zdo.m_distant = __instance.m_distant;
                __instance.m_zdo.SetPrefab(prefabName.GetStableHashCode());
                __instance.m_zdo.SetRotation(__instance.transform.rotation);
                
                if (__instance.m_syncInitialScale)
                {
                    __instance.m_zdo.Set("scale", __instance.transform.localScale);
                }
                ZNetView.m_useInitZDO = false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ZoneSystem))]
    [HarmonyPatch(nameof(ZoneSystem.SkipSaving))]
    public class ZoneSystem_SkipSaving_Patch
    {
        public static bool Prefix(ref ZoneSystem __instance, ref bool __result)
        {
            if (Forgotten_Biomes.SpawnLocation.Value)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(ZNetScene))]
    [HarmonyPatch(nameof(ZNetScene.CreateObjectsSorted))]
    public class CreateObjectsSorted_Patch
    {
        public static bool Prefix(ref ZNetScene __instance, List<ZDO> currentNearObjects, int maxCreatedPerFrame, ref int created)
        {
            if (!ZoneSystem.instance.IsActiveAreaLoaded())
            {
                return false;
            }
            if (!Forgotten_Biomes.RemoveLocations.Value && !Forgotten_Biomes.DisabledLocations.Value && !Forgotten_Biomes.HiddenLocations.Value)
            {
                if (Forgotten_Biomes.debugEnabled.Value)
                {
                    ZLog.Log("Forgotten_Biomes: Locations Generations Disabled ! (CreateObjectsSorted_Patch)");
                }
                return true;
            }
            __instance.m_tempCurrentObjects2.Clear();
            int frameCount = Time.frameCount;
            Vector3 referencePosition = ZNet.instance.GetReferencePosition();
            foreach (ZDO zdo in currentNearObjects)
            {
                if (zdo.m_tempCreateEarmark != frameCount)
                {
                    zdo.m_tempSortValue = Utils.DistanceSqr(referencePosition, zdo.GetPosition());
                    __instance.m_tempCurrentObjects2.Add(zdo);
                }
            }
            int num = Mathf.Max(__instance.m_tempCurrentObjects2.Count / 100, maxCreatedPerFrame);
            __instance.m_tempCurrentObjects2.Sort(new Comparison<ZDO>(ZNetScene.ZDOCompare));
            foreach (ZDO zdo2 in __instance.m_tempCurrentObjects2)
            {
  
                if (__instance.CreateObject(zdo2) != null)
                {
                    created++;
                    if (created > num)
                    {
                        break;
                    }
                }
                else if (ZNet.instance.IsServer())
                {
                    zdo2.SetOwner(ZDOMan.instance.GetMyID());
                    if (Forgotten_Biomes.debugEnabled.Value)
                    {
                        ZLog.Log("Destroyed invalid predab ZDO:" + zdo2.m_uid);
                    }
                    ZDOMan.instance.DestroyZDO(zdo2);
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.Awake))]
    public class Player_Awake_Patch
    {
        public static bool Prefix(ref Player __instance)
        {
            foreach (Recipe recipe in ObjectDB.instance.m_recipes)
            {
                if (recipe.m_item != null && !recipe.m_enabled)
                {
                    recipe.m_enabled = true;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ClutterSystem))]
    [HarmonyPatch(nameof(ClutterSystem.Awake))]
    public class ClutterSystem_Awake_Patch
    {
        public static bool Prefix(ref ClutterSystem __instance)
        {
            if (Forgotten_Biomes.DisabledClutter.Value)
            {
                foreach (ClutterSystem.Clutter clutter in __instance.m_clutter)
                {
                    
                    if (!clutter.m_enabled)
                    {
                        if (Forgotten_Biomes.debugEnabled.Value)
                        {
                            ZLog.Log("Clutter/Grass : [" + clutter.m_name + "] Enabled");
                        }
                        clutter.m_enabled = true;
                    }
                }
            }
            return true;
        }

    }

}
