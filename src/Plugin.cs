using System;
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

        internal static ConfigEntry<int> QuickSaveSlot;

        private void Awake()
        {
            Logger = base.Logger;
            Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

            InitPatch(harmony, typeof(MapUIPatch),
                @"**Map UI Enhancements**:
                    - Reveal cities, dialog starters, radiation zones, and relics on the map for easier navigation and discovery
                    - Display building names within cities to provide a clearer view of available facilities");

            InitPatch(harmony, typeof(QuickSavePatch),
                @"**Game Data Patch**:
                    - Quick save and load with F5 and F9 keys, featuring a configurable quick save slot option");
            QuickSaveSlot = Config.Bind(typeof(QuickSavePatch).Name, "QuickSaveSlot", 1,
                new ConfigDescription("Quick save slot number accessed with the F5 and F9 keys", new AcceptableValueRange<int>(1, 10)));

            InitPatch(harmony, typeof(RelicPatch),
                @"**Relic Insights**:
                    - View a complete list of all specific products that a relic can produce before exploring it");

            InitPatch(harmony, typeof(TradeInfoPatch),
                @"**Trade Information Enhancements**:
                    - Increase the trade log size limit, allowing for better tracking of recent transactions");
            MaxTradeLogSize = Config.Bind(typeof(TradeInfoPatch).Name, "MaxTradeLogSize", 5,
                new ConfigDescription("Maximum number of items the trade log can store", new AcceptableValueRange<int>(1, 10)));

            Logger.LogInfo($"Plugin loaded");
        }

        private void InitPatch(Harmony harmony, Type type, string description)
        {
            ConfigEntry<bool> configEntry = Config.Bind("*Features*", type.Name, true, description);

            if (configEntry.Value)
            {
                harmony.PatchAll(type);
                Logger.LogInfo($"{type} enabled");
            }
        }

    }
}
