using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Managers.Enums;

namespace _1_Scripts.CombatSystem.Events
{
    public static class CombatEvents
    {
        public static EventHandler<StartCombatEventArgs> OnCombatStart;
        public static void RaiseCombatStart(object sender, StartCombatEventArgs args) => OnCombatStart?.Invoke(sender, args);
        
        public static event Action<CombatState> OnCombatStateChanged;
        public static void RaiseCombatStateChanged(CombatState state) => OnCombatStateChanged?.Invoke(state);

        public static event Action<int, int> OnCombatEnd;
        public static void RaiseCombatEnd(int playersAlive, int enemiesAlive) => OnCombatEnd?.Invoke(playersAlive, enemiesAlive);

        public static event Action<CombatEntity, int, int> OnEntityDied;
        public static void RaiseCombatEntityDied(CombatEntity entity, int playersAlive, int enemiesAlive) => OnEntityDied?.Invoke(entity, playersAlive, enemiesAlive);
    }
}