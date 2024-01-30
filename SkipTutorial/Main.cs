using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace SkipTutorial
{
    [BepInPlugin("Aidanamite.SkipTutorial", "Skip Tutorial", "1.0.1")]
    public class Main : BaseUnityPlugin
    {
        internal static Assembly modAssembly = Assembly.GetExecutingAssembly();
        internal static string modName = $"{modAssembly.GetName().Name}";
        internal static string modDir = $"{Environment.CurrentDirectory}\\BepInEx\\{modName}";

        void Awake()
        {
            new Harmony($"com.Aidanamite.{modName}").PatchAll(modAssembly);
            Logger.LogInfo($"{modName} has loaded");
        }
    }

    [HarmonyPatch(typeof(GameSlot), MethodType.Constructor)]
    class Patch_NewGameSlot
    {
        static void Postfix(GameSlot __instance)
        {
            __instance.playedTutorial = 4;
            __instance.willInventory.Add(new ItemSaveInfo(ItemDatabase.GetItemByName("Training Sword", 0), 1));
        }
    }

    [HarmonyPatch(typeof(StartGamePanel), "UpdateSlotToNewGamePlus")]
    class Patch_NewGamePlusSlot
    {
        static void Postfix(GameSlot slotToUpdate) => slotToUpdate.playedTutorial = 4;
    }

    [HarmonyPatch(typeof(Constants), "GetInt")]
    class Patch_GetInitGold
    {
        static bool Prefix(string key, ref int __result)
        {
            if (key == "kInitGold")
            {
                __result = 300;
                return false;
            }
            return true;
        }
    }
}