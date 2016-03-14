using System;
using UnityEngine;

namespace UnityCommonLibrary.Attributes {
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class RelativesOnlyAttribute : PropertyAttribute {
		public ValidRelatives validRelatives { get; private set; }

		public RelativesOnlyAttribute() {
			validRelatives = ValidRelatives.SameHierarchyPath;
		}

		public RelativesOnlyAttribute(ValidRelatives validRelatives) {
			this.validRelatives = validRelatives;
		}

		public bool IsRuleSet(ValidRelatives rule) {
			return (validRelatives & rule) != 0;
		}

		public bool IsOnlyRuleSet(ValidRelatives rule) {
			return validRelatives == rule;
		}

	}

	[Flags]
	public enum ValidRelatives {
		SameGameObject = 1 << 0,
		Children = 1 << 1,
		Parents = 1 << 2,
		SameHierarchyPath = SameGameObject | Children | Parents
	}
}
