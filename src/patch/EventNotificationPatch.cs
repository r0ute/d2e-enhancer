using HarmonyLib;

namespace D2E.src.patch
{
    public class EventNotificationPatch
    {

        [HarmonyPatch(typeof(BattleManager), nameof(BattleManager.CheckLevelUp))]
        [HarmonyPrefix]
        static void OnCheckLevelUp(ref bool __runOriginal)
        {

            __runOriginal = ShowLevelUpNotification();

        }


        [HarmonyPatch(typeof(MainWindow), nameof(MainWindow.ActiveBtnLabel))]
        [HarmonyPrefix]
        static void OnActiveBtnLabel(int index, int type, ref MainWindow __instance, ref bool __runOriginal)
        {

            if (index == 1 && type == 1)
            {
                __runOriginal = ShowLevelUpNotification();
            }
        }

        private static bool ShowLevelUpNotification()
        {
            const int doctorMechanicTraderProfession = 3;

            foreach (int member in CharacterManager.Instance.m_MyTeam)
            {
                if (CharacterManager.Instance.m_AllCharacters[member].m_Attr.CanUpLevel
                && CharacterManager.Instance.m_AllCharacters[member].m_Attr.m_nProfessionId < doctorMechanicTraderProfession)
                {
                    Plugin.Logger.LogDebug($"ShowLevelUpNotification member={CharacterManager.Instance.m_AllCharacters[member].m_Attr.m_PlayerName}");
                    return true;
                }

            }

            return false;

        }
    }


}