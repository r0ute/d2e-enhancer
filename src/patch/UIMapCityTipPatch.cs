using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.UI;

namespace D2E.src.patch
{
    public class UIMapCityTipPatch
    {

        [HarmonyPatch(typeof(UIMapCityTip), nameof(UIMapCityTip.SetInfo))]
        [HarmonyPostfix]
        static void OnSetInfo(string cityid, ref UIMapCityTip __instance)
        {

            if (Town.m_Cities.TryGetValue(cityid, out TiggerStop value))
            {
                __instance.m_CityName.text = value.TownName + AddBuildingPositions(value.Config);
                Plugin.Logger.LogDebug($"SetInfo for City: {cityid}");

                LayoutRebuilder.ForceRebuildLayoutImmediate(__instance.m_SelfRect);
                LayoutRebuilder.ForceRebuildLayoutImmediate(__instance.m_GoodsRect);
                __instance.SetPosition(__instance.m_SelfRect);
                __instance.SetPosition(__instance.m_GoodsRect);
            }
        }

        private static string AddBuildingPositions(CityConfig config)
        {
            string result = "";

            if (config.BuildingPos.Length <= 1)
            {
                return result;
            }

            foreach (int position in config.Building)
            {
                if (position == 1) continue; // ignore Shop that every city has

                result += "\n - " + LanguageManager.Instance.GetKey("BuildingDesc_" + position);
            }

            return result;
        }


    }
}