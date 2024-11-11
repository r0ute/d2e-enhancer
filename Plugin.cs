using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace D2E;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private ConfigEntry<bool> EnableMapUIPatch;

    private ConfigEntry<bool> EnableUIMapCityTipPatch;

    private void Awake()
    {
        Logger = base.Logger;
        Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

        TogglePatch(harmony, typeof(MapUIPatch), out EnableMapUIPatch);
        TogglePatch(harmony, typeof(UIMapCityTipPatch), out EnableUIMapCityTipPatch);

        Logger.LogInfo($"Plugin loaded");
    }

    private void TogglePatch(Harmony harmony, Type type, out ConfigEntry<bool> configEntry)
    {
        configEntry = Config.Bind("Features (game restart is required)", type.Name, true, $"Whether or not to enable {type} Patch");

        if (configEntry.Value)
        {
            harmony.PatchAll(type);
            Logger.LogInfo($"{type} enabled");
        }
    }

}
