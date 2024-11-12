using System;
using System.Collections.Generic;
using Devdog.InventoryPro;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

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
            Plugin.Logger.LogDebug($"BigTable: AddItemBuyInfo item={itemid}, Count={__instance.m_ItemBuyInfo[itemid].Count}");

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
            Plugin.Logger.LogDebug($"BigTable: AddItemSellInfo item={itemid}, Count={__instance.m_ItemSellInfo[itemid].Count}");
        }

        [HarmonyPatch(typeof(InfoBoxUI), nameof(InfoBoxUI.AwakeFunction))]
        [HarmonyPrefix]
        static void BeforeAwake(ref InfoBoxUI __instance)
        {
            ResizeTradeInfoObj(ref __instance.m_BuyInfoObj, __instance.m_BuyObj);
            Plugin.Logger.LogDebug($"InfoBoxUI: AwakeFunction m_BuyInfoObj {__instance.m_BuyInfoObj.Length}");
            ResizeTradeInfoObj(ref __instance.m_SellInfoObj, __instance.m_SellObj);
            Plugin.Logger.LogDebug($"InfoBoxUI: AwakeFunction m_SellInfoObj {__instance.m_SellInfoObj.Length}");
        }

        private static void ResizeTradeInfoObj(ref CommonObj[] tradeInfoObj, RectTransform parent)
        {
            if (tradeInfoObj.Length > 0)
            {
                Array.Resize(ref tradeInfoObj, Plugin.MaxTradeLogSize.Value);

                if (tradeInfoObj.Length > 3)
                {
                    for (int ind = 3; ind < tradeInfoObj.Length; ind++)
                    {
                        tradeInfoObj[ind] = UnityEngine.Object.Instantiate(tradeInfoObj[0], parent, false);
                    }
                }

            }
        }

    }
}