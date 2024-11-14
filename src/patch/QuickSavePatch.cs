using HarmonyLib;
using UnityEngine;

namespace D2E.src.patch
{
    public class QuickSavePatch
    {

        [HarmonyPatch(typeof(MainWindow), "CheckBtns")]
        [HarmonyPostfix]
        static void OnCheckBtns()
        {

            HandleSaveGame();
            HandleLoadGame();
        }

        [HarmonyPatch(typeof(PanelStart), "Update")]
        [HarmonyPostfix]
        static void OnStart()
        {
            HandleLoadGame();

        }

        private static void HandleSaveGame()
        {

            if (Input.GetKeyDown(KeyCode.F5))
            {
                Plugin.Logger.LogDebug($"MainWindow: GetKeyDown F5");
                UnitySingleton<GameDataManager>.Instance.Save(Plugin.QuickSaveSlot.Value);
            }
        }

        private static void HandleLoadGame()
        {

            if (Input.GetKeyDown(KeyCode.F9))
            {
                Plugin.Logger.LogDebug($"MainWindow: GetKeyDown F9");

                if (UnitySingleton<GameDataManager>.Instance.GetBinarySaveFileDetail(Plugin.QuickSaveSlot.Value) != null)
                {

                    UnitySingleton<GlobalManager>.Instance.ShowLoadingPage(bShowText: true);
                    UnitySingleton<GameDataManager>.Instance.Load(Plugin.QuickSaveSlot.Value);
                }
            }
        }
    }
}