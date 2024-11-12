using System;
using System.ComponentModel;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using D2E.src.patch;
using HarmonyLib;

namespace D2E.src
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        internal static ConfigEntry<int> MaxTradeLogSize;

        private ConfigEntry<bool> EnableMapUIPatch;

        private ConfigEntry<bool> EnableUIMapCityTipPatch;

        private ConfigEntry<bool> EnableBigTablePatch;

        private void Awake()
        {
            Logger = base.Logger;
            Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

            TogglePatch(harmony, typeof(MapUIPatch), out EnableMapUIPatch,
            @"**Map UI Enhancements**:
                - Reveal cities, dialog starters, radiation zones, and relics on the map for easier navigation and discovery");
            TogglePatch(harmony, typeof(UIMapCityTipPatch), out EnableUIMapCityTipPatch,
            @"**City Tooltips**:
                - Display building names within cities to provide a clearer view of available facilities");
            TogglePatch(harmony, typeof(BigTablePatch), out EnableBigTablePatch,
            @"**Core Gameplay Improvements**:
                - Increase the trade log item limit for better tracking of transactions");
            MaxTradeLogSize = Config.Bind(typeof(BigTablePatch).Name, "MaxTradeLogSize", 5,
             new ConfigDescription("Maximum number of items the trade log can store", new AcceptableValueRange<int>(1, 10)));

            Logger.LogInfo($"Plugin loaded");
        }

        private void TogglePatch(Harmony harmony, Type type, out ConfigEntry<bool> configEntry, string description)
        {
            configEntry = Config.Bind("*Features* (restart required)", type.Name, true, description);

            if (configEntry.Value)
            {
                harmony.PatchAll(type);
                Logger.LogInfo($"{type} enabled");
            }
        }

    }
}
