using BepInEx.Logging;
using HarmonyLib;

public class MapUIPatch
{

    internal static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("MapUIPatch");

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideCity))]
    [HarmonyPrefix]
    static void OnHideCity(TiggerStop city, ref MapUI __instance, ref bool __runOriginal)
    {
        if (!city.m_bDiscovered)
        {
            __runOriginal = false;
            __instance.AddCity(city);
            Logger.LogDebug($"Show City: {city.TownName}");
        }
    }

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideMapDialogueStarter))]
    [HarmonyPrefix]
    static void OnHideMapDialogueStarter(MapDialogueStarter starter, ref MapUI __instance, ref bool __runOriginal)
    {
        if (starter.gameObject.activeSelf && !starter.ShowInMap)
        {
            __runOriginal = false;
            __instance.AddMapDialogueStarter(starter);
            Logger.LogDebug($"Show MapDialogueStarter: {starter.m_ShowName}");
        }
    }

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideRadiationArea))]
    [HarmonyPrefix]
    static void OnHideRadiationArea(RadiationArea area, ref MapUI __instance, ref bool __runOriginal)
    {
        if (!area.m_bDisCovered)
        {
            __runOriginal = false;
            __instance.AddRadiationArea(area);
            Logger.LogDebug($"Show RadiationArea: {area.m_AreaID}");
        }
    }

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideRelic))]
    [HarmonyPrefix]
    static void OnHideRelic(SingleRelic relic, ref MapUI __instance, ref bool __runOriginal)
    {
        if (!relic.Discovered)
        {
            __runOriginal = false;
            __instance.AddRelic(relic);
            Logger.LogDebug($"Show SingleRelic: {relic.m_RelicName}");
        }
    }

    [HarmonyPatch(typeof(MapUI), nameof(MapUI.UnLocked))]
    [HarmonyPrefix]
    static void OnUnLocked(UIMapAreaBlock block, ref bool __runOriginal, ref bool __result)
    {
        __runOriginal = false;
        __result = true;
        Logger.LogDebug($"Unlock UIMapAreaBlock: {block}");
    }


}