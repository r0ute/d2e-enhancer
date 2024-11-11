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
        // Plugin startup logic
        Logger = base.Logger;
        Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(MapUIPatch));
        Logger.LogInfo($"Plugin loaded");
    }

    class MapUIPatch
    {

        [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideCity))]
        [HarmonyPostfix]
        static void OnHideCity(TiggerStop city, ref MapUI __instance)
        {
            if (!city.m_bDiscovered)
            {
                __instance.AddCity(city);
            }
        }

        [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideMapDialogueStarter))]
        [HarmonyPostfix]
        static void OnHideMapDialogueStarter(MapDialogueStarter starter, ref MapUI __instance)
        {
            if (starter.gameObject.activeSelf)
            {
                __instance.AddMapDialogueStarter(starter);
            }
        }

    }
}
