using Devdog.General.UI;
using HarmonyLib;

namespace D2E.src.patch
{
    public class KeyboardConfigPatch
    {

        [HarmonyPatch(typeof(MainWindow), "Update")]
        [HarmonyPostfix]
        static void OnUpdate(ref MainWindow __instance)
        {

            UIWindow mapWindow = Traverse.Create(__instance).Field("MapWindow").GetValue() as UIWindow;

            if (mapWindow != null)
            {
                if (mapWindow.isVisible)
                {
                    HandleMapKey(ref __instance);

                    return;
                }
            }
            else
            {
                IFrame mapFrame = UnitySingleton<UIManager>.Instance.GetFrame(ID_FRAME.IFrame_MapUI);

                if (mapFrame != null && mapFrame.m_CurWindow != null)
                {
                    if (mapFrame.m_CurWindow.isVisible)
                    {
                        HandleMapKey(ref __instance);

                        return;
                    }
                }
            }

            if (!UnitySingleton<UIManager>.Instance.IsSettingUIShow() && !UnitySingleton<UIManager>.Instance.IsDialogueUIShow())
            {
                HandleInventoryKey(ref __instance);
                HandleMapKey(ref __instance);
            }
        }

        private static void HandleInventoryKey(ref MainWindow __instance)
        {
            if (Plugin.KeyInventory.Value.IsDown())
            {
                Plugin.Logger.LogDebug($"MainWindow: KeyDown KeyInventory");
                __instance.OnClickInventoryBag();
            }
        }

        private static void HandleMapKey(ref MainWindow __instance)
        {
            if (Plugin.KeyMap.Value.IsDown())
            {
                Plugin.Logger.LogDebug($"MainWindow: KeyDown KeyMap");
                __instance.OnClickMap();
            }
        }

    }
}