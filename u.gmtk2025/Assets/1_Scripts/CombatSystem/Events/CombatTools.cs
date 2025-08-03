using UnityEngine;

namespace _1_Scripts.CombatSystem.Events
{
    /// <summary>
    /// Class for misc combat tools to use Project wide
    /// </summary>
    public static class CombatTools
    {
        /// <summary>
        /// Checks if a row index is within the legal combat row range.
        /// </summary>
        public static bool IsCombatRowIndexInRange(int rowIndex)
        {
            return rowIndex is >= CombatConstants.StartingRowIndex and <= CombatConstants.EndingRowIndex;
        }

        /// <summary>
        /// Calculates the row distance between two entities.
        /// </summary>
        public static int RowDistance(int rowA, int rowB)
        {
            return Mathf.Abs(rowA - rowB);
        }

        /// <summary>
        /// Determines if target is within allowed range from caster based on row index.
        /// </summary>
        public static bool IsTargetWithinRange(int casterRow, int targetRow, int allowedRange)
        {
            return RowDistance(casterRow, targetRow) <= allowedRange;
        }

        /// <summary>
        /// Clamps a row index to valid combat rows.
        /// </summary>
        public static int ClampToValidRow(int rowIndex)
        {
            return Mathf.Clamp(rowIndex, CombatConstants.StartingRowIndex, CombatConstants.EndingRowIndex);
        }
    }
}