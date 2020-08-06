using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Battle.Abilities;
using UnityEditor;

namespace TUFG.Battle.AI
{
    public abstract class UnitAI : MonoBehaviour
    {
        public abstract void GetChosenAbility(Battle battle, Unit unit, out Ability ability, out Unit target);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ClassTooltip : PropertyAttribute
    {
        public readonly string description;

        public ClassTooltip(string description)
        {
            this.description = description;
        }
    }

    [CustomEditor(typeof(UnitAI), editorForChildClasses: true)]
    public class MyTooltipDrawer : Editor
    {
        string tooltip;

        private void OnEnable()
        {
            var attributes = target.GetType().GetCustomAttributes(inherit: false);
            foreach (var attr in attributes)
            {
                if (attr is ClassTooltip tooltip)
                {
                    this.tooltip = tooltip.description;
                }
            }
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(tooltip, MessageType.Info);
            base.OnInspectorGUI();
        }
    }
}
