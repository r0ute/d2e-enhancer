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

            const int nonCombatPosX = 2;

            foreach (int memberId in CharacterManager.Instance.m_MyTeam)
            {
                var member = CharacterManager.Instance.m_AllCharacters[memberId];

                if (member.m_Pos.x != nonCombatPosX && member.m_Attr.CanUpLevel)
                {

                    Plugin.Logger.LogDebug($"ShowLevelUpNotification name={member.m_Attr.m_PlayerName}, position={member.m_Pos}");
                    return true;
                }

            }

            return false;

        }
    }


}