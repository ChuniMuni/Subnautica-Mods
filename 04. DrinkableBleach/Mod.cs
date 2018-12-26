﻿using Harmony;
using ModdingAdventCalendar.Utility;
using SMLHelper.V2.Handlers;
using System;
using System.Reflection;
using UnityEngine;
using Logger = ModdingAdventCalendar.Utility.Logger;

namespace ModdingAdventCalendar.DrinkableBleach
{
    public static class QMod
    {
        public static string assembly;

        public static void Patch()
        {
            try
            {
                assembly = Assembly.GetExecutingAssembly().GetName().FullName;

                HarmonyInstance.Create("moddingadventcalendar.drinkablebleach").PatchAll(Assembly.GetExecutingAssembly());

                Console.WriteLine($"[{assembly}] Patched successfully!");

                LanguageHandler.SetLanguageLine("Tooltip_Bleach", "NaClO. Sodium hypochlorite bleach. Sanitizing applications. (If you cannot drink it, you need to craft a new one)");

                Console.WriteLine($"[{assembly}] Updated Bleach tooltip");
            }
            catch (Exception e)
            {
                Logger.Exception(e, LoggedWhen.Patching);
            }
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(CraftData), "GetPrefabForTechType")]
        public static class CraftData_GetPrefabForTechType
        {
            [HarmonyPostfix]
            public static void Postfix(GameObject __result, TechType techType)
            {
                try
                {
                    if (techType == TechType.Bleach)
                    {
                        Eatable eatable = __result.AddComponent<Eatable>();
                        eatable.decomposes = false;
                        eatable.despawns = true;
                        eatable.foodValue = -1000;
                        eatable.waterValue = -1000;

                        Console.WriteLine($"[{QMod.assembly}] Added eatable component to bleach object!");
                    }
                }
                catch (Exception e)
                {
                    Logger.Exception(e, LoggedWhen.InPatch);
                }
            }
        }
    }
}