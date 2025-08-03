

using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using UnityEngine;

public class CombatEntitySpriteController : MonoBehaviour
{
    void OnPointerClick()
    {
        CombatEvents.RaiseOnTargetSelected(this.GetComponent<CombatEntity>());
    }
}