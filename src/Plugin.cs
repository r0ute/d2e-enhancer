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

        private ConfigEntry<bool> EnableMapUIPatch;

        private ConfigEntry<bool> EnableRelicPatch;

        private ConfigEntry<bool> EnableTradeInfoPatch;

        private void Awake()
        {
            Logger = base.Logger;
            Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

            TogglePatch(harmony, typeof(MapUIPatch), out EnableMapUIPatch,
                @"**Map UI Enhancements**:
                    - Reveal cities, dialog starters, radiation zones, and relics on the map for easier navigation and discovery
                    - Display building names within cities to provide a clearer view of available facilities");

            TogglePatch(harmony, typeof(RelicPatch), out EnableRelicPatch,
                @"**Relic Insights**:
                    - View a complete list of all specific products that a relic can produce before exploring it");

            TogglePatch(harmony, typeof(TradeInfoPatch), out EnableTradeInfoPatch,
                @"**Trade Information Enhancements**:
                    - Increase the trade log size limit, allowing for better tracking of recent transactions");
            MaxTradeLogSize = Config.Bind(typeof(TradeInfoPatch).Name, "MaxTradeLogSize", 5,
             new ConfigDescription("Maximum number of items the trade log can store", new AcceptableValueRange<int>(1, 10)));

            Logger.LogInfo($"Plugin loaded");
        }

        private void TogglePatch(Harmony harmony, Type type, out ConfigEntry<bool> configEntry, string description)
        {
            configEntry = Config.Bind("*Features*", type.Name, true, description);

            if (configEntry.Value)
            {
                harmony.PatchAll(type);
                Logger.LogInfo($"{type} enabled");
            }
        }

    }
}
