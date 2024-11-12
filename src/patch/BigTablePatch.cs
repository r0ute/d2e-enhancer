using System.Collections.Generic;
using Devdog.InventoryPro;
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
                ItemTradeInfo itemTradeInfo = new ItemTradeInfo();
                itemTradeInfo.m_CityID = cityid;
                itemTradeInfo.m_Price = price;
                __instance.m_ItemBuyInfo[itemid] = new List<ItemTradeInfo>();
                __instance.m_ItemBuyInfo[itemid].Add(itemTradeInfo);
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
            if (__instance.m_ItemBuyInfo[itemid].Count >= Plugin.MaxTradeLogSize.Value)
            {
                __instance.m_ItemBuyInfo[itemid].RemoveAt(0);
            }
            ItemTradeInfo itemTradeInfo2 = new ItemTradeInfo();
            itemTradeInfo2.m_CityID = cityid;
            itemTradeInfo2.m_Price = price;
            __instance.m_ItemBuyInfo[itemid].Add(itemTradeInfo2);
            Plugin.Logger.LogDebug($"BigTable: AddItemBuyInfo item={itemid}, Count={__instance.m_ItemBuyInfo[itemid].Count}, Capacity={__instance.m_ItemBuyInfo[itemid].Capacity}");

        }

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.AddItemSellInfo))]
        [HarmonyPrefix]
        static void OnAddItemSellInfo(string itemid, string cityid, float price, ref BigTable __instance, ref bool __runOriginal)
        {
            __runOriginal = false;
            if (!__instance.m_ItemSellInfo.ContainsKey(itemid))
            {
                ItemTradeInfo itemTradeInfo = new ItemTradeInfo();
                itemTradeInfo.m_CityID = cityid;
                itemTradeInfo.m_Price = price;
                __instance.m_ItemSellInfo[itemid] = new List<ItemTradeInfo>();
                __instance.m_ItemSellInfo[itemid].Add(itemTradeInfo);
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
            if (__instance.m_ItemSellInfo[itemid].Count >= Plugin.MaxTradeLogSize.Value)
            {
                __instance.m_ItemSellInfo[itemid].RemoveAt(0);
            }
            ItemTradeInfo itemTradeInfo2 = new ItemTradeInfo();
            itemTradeInfo2.m_CityID = cityid;
            itemTradeInfo2.m_Price = price;
            __instance.m_ItemSellInfo[itemid].Add(itemTradeInfo2);
            Plugin.Logger.LogDebug($"BigTable: AddItemSellInfo item={itemid}, Count={__instance.m_ItemSellInfo[itemid].Count}, Capacity={__instance.m_ItemSellInfo[itemid].Capacity}");
        }

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.Clear))]
        [HarmonyPrefix]
        static void OnClear(ref BigTable __instance)
        {
            Plugin.Logger.LogDebug($"BigTable: Clear");
        }

        [HarmonyPatch(typeof(InfoBoxUI), "BuyTip")]
        [HarmonyPrefix]
        static void OnBuyTip(ref InfoBoxUI __instance)
        {
            Plugin.Logger.LogDebug($"InfoBoxUI: BuyTip {__instance.m_BuyInfoObj.Length}");
        }

        [HarmonyPatch(typeof(InfoBoxUI), "SellTip")]
        [HarmonyPrefix]
        static void OnSellTip(ref InfoBoxUI __instance)
        {
            Plugin.Logger.LogDebug($"InfoBoxUI: SellTip {__instance.m_BuyInfoObj.Length}");
        }
    }
}