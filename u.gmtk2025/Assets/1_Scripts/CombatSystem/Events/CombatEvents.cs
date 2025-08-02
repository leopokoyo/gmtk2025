using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.Events
{
    public static class CombatEvents
    {
        public static EventHandler<StartCombatEventArgs> OnCombatStart;
        
        public static void RaiseCombatStart(object sender, StartCombatEventArgs args)
        {
            OnCombatStart?.Invoke(sender, args);
        }
        
    }
}