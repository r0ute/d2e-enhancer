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

    private void Awake()
    {
        Logger = base.Logger;
        Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

        EnableMapUIPatch = Config.Bind("Features (game restart is required)", "MapUI", true, "Whether or not to enable Map UI Patch");
        TogglePatch(harmony, typeof(MapUIPatch), EnableMapUIPatch);

        Logger.LogInfo($"Plugin loaded");
    }

    private void TogglePatch(Harmony harmony, Type type, ConfigEntry<bool> configEntry)
    {
        if (configEntry.Value)
        {
            harmony.PatchAll(type);
            Logger.LogInfo($"{type} enabled.");
        }
    }

}
