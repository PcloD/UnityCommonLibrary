using System;
using UnityEngine;

namespace UnityCommonLibrary.Attributes
{
    /// <summary>
    ///     Restricts what can be assigned to an Object field in Unity's inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class RelativesOnlyAttribute : PropertyAttribute
    {
        public ValidRelatives ValidRelatives { get; }

        /// <summary>
        ///     Restricts this field to only accept Objects that are in the same
        ///     path in the Hierarchy as GameObject that owns this field.
        /// </summary>
        public RelativesOnlyAttribute()
        {
            ValidRelatives = ValidRelatives.SameHierarchyPath;
        }

        /// <summary>
        ///     Restricts this field to only accept Objects that follow the rules
        ///     set by <see cref="ValidRelatives" />
        /// </summary>
        public RelativesOnlyAttribute(ValidRelatives validRelatives)
        {
            ValidRelatives = validRelatives;
        }

        /// <summary>
        ///     Check if a rule flag is the ONLY flag set.
        /// </summary>
        /// <param name="rule">The rule to check.</param>
        /// <returns>True if only flag set, false otherwise.</returns>
        public bool IsOnlyRuleSet(ValidRelatives rule)
        {
            return ValidRelatives == rule;
        }

        /// <summary>
        ///     Check if a rule flag is set.
        /// </summary>
        /// <param name="rule">The rule to check.</param>
        /// <returns>True if set, false otherwise.</returns>
        public bool IsRuleSet(ValidRelatives rule)
        {
            return (ValidRelatives & rule) != 0;
        }
    }

    [Flags]
    public enum ValidRelatives
    {
        /// <summary>
        ///     Only allow Objects on the same GameObject to be assigned.
        /// </summary>
        SameGameObject = 1 << 0,

        /// <summary>
        ///     Only allow Objects that are children to be assigned.
        /// </summary>
        Children = 1 << 1,

        /// <summary>
        ///     Only allow Objects that are parents to be assigned.
        /// </summary>
        Parents = 1 << 2,

        /// <summary>
        ///     Allow any Object that is on the same GameObject,
        ///     a child or a parent to be assigned.
        /// </summary>
        SameHierarchyPath = SameGameObject | Children | Parents
    }
}