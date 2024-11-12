using HarmonyLib;

namespace D2E.src.patch
{
    public class BigTablePatch
    {

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.AddItemBuyInfo))]
        [HarmonyPrefix]
        static void OnAddItemBuyInfo(string itemid, string cityid, float price, ref BigTable __instance, ref bool __runOriginal)
        {
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

            Plugin.Logger.LogDebug($"BigTable: AddItemBuyInfo MaxTradeLogSize {Plugin.MaxTradeLogSize.Value}");

            if (__instance.m_ItemBuyInfo[itemid].Count >= Plugin.MaxTradeLogSize.Value)
            {
                Plugin.Logger.LogDebug($"BigTable: AddItemBuyInfo RemoveAt(0) {itemid}");
                __instance.m_ItemBuyInfo[itemid].RemoveAt(0);
            }

            ItemTradeInfo itemTradeInfo2 = new()
            {
                m_CityID = cityid,
                m_Price = price
            };
            __instance.m_ItemBuyInfo[itemid].Add(itemTradeInfo2);
            Plugin.Logger.LogDebug($"BigTable: AddItemBuyInfo {itemid}");

        }

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.AddItemSellInfo))]
        [HarmonyPrefix]
        static void OnAddItemSellInfo(string itemid, string cityid, float price, ref BigTable __instance, ref bool __runOriginal)
        {
            __runOriginal = false;

            if (!__instance.m_ItemSellInfo.ContainsKey(itemid))
            {
                ItemTradeInfo itemTradeInfo = new()
                {
                    m_CityID = cityid,
                    m_Price = price
                };
                __instance.m_ItemSellInfo[itemid] = [itemTradeInfo];
                return;
            }

            int num = 0;

            while (num < __instance.m_ItemSellInfo[itemid].Count)
            {
                if (__instance.m_ItemSellInfo[itemid][num] == null)
                {
                    __instance.m_ItemSellInfo[itemid].RemoveAt(num);
                    continue;
                }

                if (__instance.m_ItemSellInfo[itemid][num].m_CityID == cityid)
                {
                    __instance.m_ItemSellInfo[itemid].RemoveAt(num);
                    break;
                }

                num++;
            }

            Plugin.Logger.LogDebug($"BigTable: AddItemSellInfo MaxTradeLogSize {Plugin.MaxTradeLogSize.Value}");

            if (__instance.m_ItemSellInfo[itemid].Count >= Plugin.MaxTradeLogSize.Value)
            {
                __instance.m_ItemSellInfo[itemid].RemoveAt(0);
                Plugin.Logger.LogDebug($"BigTable: AddItemSellInfo RemoveAt(0) {itemid}");
            }

            ItemTradeInfo itemTradeInfo2 = new()
            {
                m_CityID = cityid,
                m_Price = price
            };
            __instance.m_ItemSellInfo[itemid].Add(itemTradeInfo2);
            Plugin.Logger.LogDebug($"BigTable: AddItemSellInfo {itemid}");
        }
    }
}