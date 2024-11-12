using System;
using System.Collections.Generic;
using Devdog.InventoryPro;
using HarmonyLib;
using UnityEngine;

namespace D2E.src.patch
{
    public class BigTablePatch
    {

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.AddItemBuyInfo))]
        [HarmonyPrefix]
        static void OnAddItemBuyInfo(string itemid, string cityid, float price, ref BigTable __instance, ref bool __runOriginal)
        {
            __runOriginal = false;
            AddItemTradeInfo(itemid, cityid, price, ref __instance.m_ItemBuyInfo);
            Plugin.Logger.LogDebug($"BigTable: AddItemBuyInfo item={itemid}, Count={__instance.m_ItemBuyInfo[itemid].Count}");

        }

        [HarmonyPatch(typeof(BigTable), nameof(BigTable.AddItemSellInfo))]
        [HarmonyPrefix]
        static void OnAddItemSellInfo(string itemid, string cityid, float price, ref BigTable __instance, ref bool __runOriginal)
        {
            __runOriginal = false;
            AddItemTradeInfo(itemid, cityid, price, ref __instance.m_ItemSellInfo);
            Plugin.Logger.LogDebug($"BigTable: AddItemSellInfo item={itemid}, Count={__instance.m_ItemSellInfo[itemid].Count}");
        }

        private static void AddItemTradeInfo(string itemid, string cityid, float price,
                ref Dictionary<string, List<ItemTradeInfo>> itemTradeInfos)
        {
            if (!itemTradeInfos.ContainsKey(itemid))
            {
                itemTradeInfos[itemid] = [new()
                {
                    m_CityID = cityid,
                    m_Price = price
                }];
            }

            int num = 0;

            while (num < itemTradeInfos[itemid].Count)
            {
                if (itemTradeInfos[itemid][num] == null)
                {
                    itemTradeInfos[itemid].RemoveAt(num);
                    continue;
                }

                if (itemTradeInfos[itemid][num].m_CityID == cityid)
                {
                    itemTradeInfos[itemid].RemoveAt(num);
                    break;
                }

                num++;
            }

            if (itemTradeInfos[itemid].Count >= Plugin.MaxTradeLogSize.Value)
            {
                itemTradeInfos[itemid].RemoveAt(0);
            }

            itemTradeInfos[itemid].Add(new()
            {
                m_CityID = cityid,
                m_Price = price
            });
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