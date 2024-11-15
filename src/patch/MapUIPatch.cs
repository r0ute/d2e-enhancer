using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.UI;

namespace D2E.src.patch
{
    public class MapUIPatch
    {

        [HarmonyPatch(typeof(MapUI), nameof(MapUI.HideCity))]
        [HarmonyPrefix]
        static void OnHideCity(TiggerStop city, ref MapUI __instance, ref bool __runOriginal)
        {
            if (!city.m_bDiscovered)
            {
                __runOriginal = false;
                __instance.AddCity(city);
                Plugin.Logger.LogDebug($"MapUI: AddCity {city.TownName}");
            }
        }

        [HarmonyPatch(typeof(MiniMapUI), nameof(MiniMapUI.HideCity))]
        [HarmonyPrefix]
        static void OnHideCity(TiggerStop city, ref MiniMapUI __instance, ref bool __runOriginal)
        {
            if (!city.m_bDiscovered)
            {
                __runOriginal = false;
                __instance.AddCity(city);
                Plugin.Logger.LogDebug($"MiniMapUI: AddCity {city.TownName}");
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
                Plugin.Logger.LogDebug($"MapUI: AddMapDialogueStarter {starter.m_ShowName}");
            }
        }

        [HarmonyPatch(typeof(MiniMapUI), nameof(MiniMapUI.HideMapDialogueStarter))]
        [HarmonyPrefix]
        static void OnHideMapDialogueStarter(MapDialogueStarter starter, ref MiniMapUI __instance, ref bool __runOriginal)
        {
            if (starter.gameObject.activeSelf && !starter.ShowInMap)
            {
                __runOriginal = false;
                __instance.AddMapDialogueStarter(starter);
                Plugin.Logger.LogDebug($"MiniMapUI: AddMapDialogueStarter {starter.m_ShowName}");
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
                Plugin.Logger.LogDebug($"MapUI: AddRadiationArea {area.m_AreaID}");
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
                Plugin.Logger.LogDebug($"MapUI: AddRelic {relic.m_RelicName}");
            }
        }

        [HarmonyPatch(typeof(MiniMapUI), nameof(MiniMapUI.HideRelic))]
        [HarmonyPrefix]
        static void OnHideRelic(SingleRelic relic, ref MiniMapUI __instance, ref bool __runOriginal)
        {
            if (!relic.Discovered)
            {
                __runOriginal = false;
                __instance.AddRelic(relic);
                Plugin.Logger.LogDebug($"MiniMapUI: AddRelic {relic.m_RelicName}");
            }
        }

        [HarmonyPatch(typeof(UIMapCityTip), nameof(UIMapCityTip.SetInfo))]
        [HarmonyPostfix]
        static void OnSetInfo(string cityid, ref UIMapCityTip __instance)
        {

            if (Town.m_Cities.TryGetValue(cityid, out TiggerStop value))
            {
                __instance.m_CityName.text = string.Format("{0}<size=15><color=#c8beae>{1}</color></size>", value.TownName, GetBuildingPositions(value.Config));
                Plugin.Logger.LogDebug($"UIMapCityTip: SetInfo for City {cityid}");

                LayoutRebuilder.ForceRebuildLayoutImmediate(__instance.m_SelfRect);
                LayoutRebuilder.ForceRebuildLayoutImmediate(__instance.m_GoodsRect);
                __instance.SetPosition(__instance.m_SelfRect);
                __instance.SetPosition(__instance.m_GoodsRect);
            }
        }

        private static string GetBuildingPositions(CityConfig config)
        {

            if (config.BuildingPos.Length <= 1)
            {
                return "";
            }

            List<string> buildings = [];
            const int shopPosition = 1; // every city has a shop which should be ignored

            foreach (int position in config.Building)
            {
                if (position == shopPosition) continue;

                buildings.Add(LanguageManager.Instance.GetKey("BuildingDesc_" + position));
            }

            if (buildings.Count < 1)
            {
                return "";
            }

            const string delimiter = "\n -";

            return delimiter + string.Join(delimiter, buildings);
        }


    }
}