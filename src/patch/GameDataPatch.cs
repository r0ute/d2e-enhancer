using HarmonyLib;
using UnityEngine;

namespace D2E.src.patch
{
    public class GameDataPatch
    {


        [HarmonyPatch(typeof(MainWindow), "CheckBtns")]
        [HarmonyPostfix]
        static void OnCheckBtns(ref MainWindow __instance, ref bool __runOriginal)
        {

            HandleSaveGame();
            HandleLoadGame();
        }

        [HarmonyPatch(typeof(PanelStart), "Update")]
        [HarmonyPostfix]
        static void OnStart(ref MainWindow __instance, ref bool __runOriginal)
        {
            HandleLoadGame();

        }

        private static void HandleSaveGame()
        {

            if (Input.GetKeyDown(KeyCode.F5))
            {
                Plugin.Logger.LogDebug($"MainWindow: GetKeyDown F5");
                UnitySingleton<GameDataManager>.Instance.Save(3);
            }
        }

        private static void HandleLoadGame()
        {

            if (Input.GetKeyDown(KeyCode.F9))
            {
                Plugin.Logger.LogDebug($"MainWindow: GetKeyDown F9");
                UnitySingleton<GlobalManager>.Instance.ShowLoadingPage(bShowText: true);
                UnitySingleton<GameDataManager>.Instance.Load(3);
            }
        }
    }
}