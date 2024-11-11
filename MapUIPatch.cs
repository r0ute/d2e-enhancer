using BepInEx.Logging;
using HarmonyLib;

public class MapUIPatch
{

    internal static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("MapUIPatch");

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideCity))]
    [HarmonyPostfix]
    static void OnHideCity(TiggerStop city, ref MapUI __instance)
    {
        if (!city.m_bDiscovered)
        {
            __instance.AddCity(city);
            Logger.LogInfo($"Show City: {city.TownName}");
        }
    }

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideMapDialogueStarter))]
    [HarmonyPostfix]
    static void OnHideMapDialogueStarter(MapDialogueStarter starter, ref MapUI __instance)
    {
        if (starter.gameObject.activeSelf && !starter.ShowInMap)
        {
            __instance.AddMapDialogueStarter(starter);
            Logger.LogInfo($"Show MapDialogueStarter: {starter.m_ShowName}");
        }
    }

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideRadiationArea))]
    [HarmonyPostfix]
    static void OnHideRadiationArea(RadiationArea area, ref MapUI __instance)
    {
        if (!area.m_bDisCovered)
        {
            __instance.AddRadiationArea(area);
            Logger.LogInfo($"Show RadiationArea: {area.m_AreaID}");
        }
    }

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideRelic))]
    [HarmonyPostfix]
    static void OnHideRelic(SingleRelic relic, ref MapUI __instance)
    {
        if (!relic.Discovered)
        {
            __instance.AddRelic(relic);
            Logger.LogInfo($"Show SingleRelic: {relic.m_RelicName}");
        }
    }
}