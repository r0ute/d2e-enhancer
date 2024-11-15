using HarmonyLib;

namespace D2E.src.patch
{
    public class EventNotificationPatch
    {

        [HarmonyPatch(typeof(BattleManager), nameof(BattleManager.CheckLevelUp))]
        [HarmonyPrefix]
        static void OnCheckLevelUp(ref BattleManager __instance, ref bool __runOriginal)
        {

            const int doctorMechanicTraderProfession = 3;

            foreach (int member in CharacterManager.Instance.m_MyTeam)
            {
                if (CharacterManager.Instance.m_AllCharacters[member].m_Attr.m_nProfessionId < doctorMechanicTraderProfession)
                {
                    __runOriginal = true;
                    return;
                }

            }

            __runOriginal = false;
        }
    }
}