using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace D2E;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(MapUIPatch));
        Logger.LogInfo($"Plugin loaded");
    }

}
