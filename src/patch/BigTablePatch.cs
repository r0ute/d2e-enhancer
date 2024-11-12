using BepInEx.Logging;
using HarmonyLib;

namespace D2E.src.patch
{
    public class BigTablePatch
    {

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.AddItemBuyInfo))]
        [HarmonyPrefix]
        static void OnAddItemBuyInfo(string itemid, string cityid, float price, ref BigTable __instance, ref bool __runOriginal)
        {
            Plugin.Logger.LogInfo("OK");
            __runOriginal = false;
            if (!__instance.m_ItemBuyInfo.ContainsKey(itemid))
            {
                ItemTradeInfo itemTradeInfo = new()
                {
                    m_CityID = cityid,
                    m_Price = price
                };
                __instance.m_ItemBuyInfo[itemid] = [itemTradeInfo];
                return;
            }
            int num = 0;
            while (num < __instance.m_ItemBuyInfo[itemid].Count)
            {
                if (__instance.m_ItemBuyInfo[itemid][num] == null)
                {
                    __instance.m_ItemBuyInfo[itemid].RemoveAt(num);
                    continue;
                }
                if (__instance.m_ItemBuyInfo[itemid][num].m_CityID == cityid)
                {
                    __instance.m_ItemBuyInfo[itemid].RemoveAt(num);
                    break;
                }
                num++;
            }
            if (__instance.m_ItemBuyInfo[itemid].Count >= 3)
            {
                __instance.m_ItemBuyInfo[itemid].RemoveAt(0);
            }
            ItemTradeInfo itemTradeInfo2 = new()
            {
                m_CityID = cityid,
                m_Price = price
            };
            __instance.m_ItemBuyInfo[itemid].Add(itemTradeInfo2);

        }

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.AddItemSellInfo))]
        [HarmonyPrefix]
        static void OnAddItemSellInfo(string itemid, string cityid, float price, ref BigTable __instance, ref bool __runOriginal)
        {
            __runOriginal = false;

        }
    }
}