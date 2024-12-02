using System.Collections.Generic;
using Devdog.General;
using Devdog.InventoryPro;
using HarmonyLib;

namespace D2E.src.patch
{
    public class EventNotificationPatch
    {

        [HarmonyPatch(typeof(BattleManager), nameof(BattleManager.CheckLevelUp))]
        [HarmonyPrefix]
        static void OnCheckLevelUp(ref BattleManager __instance, ref bool __runOriginal)
        {

            foreach (int memberId in CharacterManager.Instance.m_MyTeam)
            {
                SingleCharacter member = CharacterManager.Instance.m_AllCharacters[memberId]; ;
                List<InventoryItemBase> list = ManagerBase<InventoryManager>.instance.Inventory.FindItemByType(ItemType.Type_Equipments, 4);
                InventoryItemBase inventoryItemBase = member.m_Attr.Equipments[0];
                int num = 0;
                while (num < list.Count)
                {
                    if (inventoryItemBase.EquipConfig.BulletType != list[num].EquipConfig.BulletType)
                    {
                        list.RemoveAt(num);
                    }
                    else
                    {
                        num++;
                    }
                }

                //todo: implement it

            }
        }


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