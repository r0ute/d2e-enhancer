using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.UI;

public class UIMapCityTipPatch
{

    internal static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("UIMapCityTipPatch");

    [HarmonyPatch(typeof(UIMapCityTip), nameof(UIMapCityTip.SetInfo))]
    [HarmonyPostfix]
    static void OnSetInfo(string cityid, ref UIMapCityTip __instance)
    {

        TiggerStop value = null;
        if (Town.m_Cities.TryGetValue(cityid, out value))
        {
            __instance.m_CityName.text = value.TownName + AddBuildingPositions(value.Config);
            Logger.LogDebug($"SetInfo for City: {cityid}");

            LayoutRebuilder.ForceRebuildLayoutImmediate(__instance.m_SelfRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(__instance.m_GoodsRect);
            __instance.SetPosition(__instance.m_SelfRect);
            __instance.SetPosition(__instance.m_GoodsRect);
        }
    }

    static string AddBuildingPositions(CityConfig config)
    {
        string result = "";

        if (config.BuildingPos.Length < 2)
        {
            return result;
        }

        foreach (int position in config.BuildingPos)
        {
            if (position == 1) continue; // Shop

            result += "\n - " + LanguageManager.Instance.GetKey("BuildingDesc_" + position);
        }

        return result;
    }


}