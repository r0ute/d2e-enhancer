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

        private ConfigEntry<bool> EnableMapUIPatch;

        private ConfigEntry<bool> EnableUIMapCityTipPatch;

        internal static ConfigEntry<bool> EnableBigTablePatch;

        private void Awake()
        {
            Logger = base.Logger;
            Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

            TogglePatch(harmony, typeof(MapUIPatch), out EnableMapUIPatch,
            @"**Map UI Patch**:
            - Reveals cities
            - Reveals dialog starters
            - Reveals radiation zones
            - Reveals relics");
            TogglePatch(harmony, typeof(UIMapCityTipPatch), out EnableUIMapCityTipPatch,
            @"**City Tooltips Patch**:
            - Displays city building names");
            TogglePatch(harmony, typeof(BigTablePatch), out EnableBigTablePatch,
            @"**Core Patch**:
            - Configures trade log limit");

            Logger.LogInfo($"Plugin loaded");
        }

        private void TogglePatch(Harmony harmony, Type type, out ConfigEntry<bool> configEntry, string description)
        {
            configEntry = Config.Bind("Features", type.Name, true, description);

            if (configEntry.Value)
            {
                harmony.PatchAll(type);
                Logger.LogInfo($"{type} enabled");
            }
        }

    }
}
