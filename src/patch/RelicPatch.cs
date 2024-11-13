using System.Collections.Generic;
using Devdog.InventoryPro;
using HarmonyLib;
using PixelCrushers.DialogueSystem;

namespace D2E.src.patch
{
    public class RelicPatch
    {

        [HarmonyPatch(typeof(SingleRelic), nameof(SingleRelic.EnterRelic))]
        [HarmonyPrefix]
        static void OnEnterRelic(ref SingleRelic __instance, ref bool __runOriginal)
        {

            __runOriginal = false;

            if (PlayerMove.m_bInStorm)
            {
                return;
            }

            if (__instance.m_bIsNormalRelic)
            {
                if (__instance.m_bFake)
                {
                    UnityEngine.Object.Destroy(__instance.gameObject);
                    UnitySingleton<PlayerMove>.Instance.Pause();
                    MessageBoxManager.ShowMessageBox(LanguageManager.Instance.GetKey("fakeRelicTitle"), [
                        LanguageManager.Instance.GetKey("fakeRelic"),
                        null
                    ], MessageBoxType.Type_NormalSingle);
                    return;
                }

                UnitySingleton<PlayerMove>.Instance.Pause();

                if (PlayerMove.m_bInStorm)
                {
                    DialogueManager.ShowAlert(LanguageManager.Instance.GetKey("StormCantEnter"), 2f);
                    return;
                }

                string relicEnterTip = string.Format(LanguageManager.Instance.GetKey("RelicEnterTip"), __instance.m_RelicName);
                MessageBoxManager.ShowMessageBox(LanguageManager.Instance.GetKey("RelicTitle"), [
                    string.Format("<size=21>{0}\n{1} {2}</size>", relicEnterTip,
                        LanguageManager.Instance.GetKey("Basetip_Product"), GetAllProducts(ref __instance)),
                    null
                ], MessageBoxType.Type_Normal, __instance.EnterRelicFunction);
            }
            else
            {
                UnitySingleton<PlayerMove>.Instance.Pause();
                if (PlayerMove.m_bInStorm)
                {
                    DialogueManager.ShowAlert(LanguageManager.Instance.GetKey("StormCantEnter"), 2f);
                    return;
                }

                MessageBoxManager.ShowMessageBox(LanguageManager.Instance.GetKey("MissionRelicTitle"), [
                    string.Format(LanguageManager.Instance.GetKey("MissionRelicEnterTip"), __instance.m_RelicName),
                    null
                ], MessageBoxType.Type_Normal, __instance.EnterMissionRelicFunction);
            }

        }

        private static string GetAllProducts(ref SingleRelic __instance)
        {
            SortedSet<string> products = [];

            foreach (int baseId in __instance.m_Config.BaseID)
            {
                if (BaseConfig.dic.TryGetValue(baseId, out BaseConfig config))
                {
                    InventoryItemBase item = ItemManager.database.FindItemByID(config.ProductID);

                    if (item != null)
                    {
                        products.Add(item.name);
                    }
                }
            }

            string productStr = string.Join(", ", products);
            Plugin.Logger.LogDebug($"SingleRelic: EnterRelic GetAllProducts {productStr}");

            return productStr;
        }
    }
}