using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Battle.Abilities;
using TUFG.Battle.AI;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace TUFG.Battle
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitData unitData;

        public bool IsAlly { get; set; }
        public string Name { get => UnitData.name; private set => UnitData.name = value; }
        public bool IsPlayer { get; set; } = false;
        public int Speed { get => UnitData.speed; private set => UnitData.speed = value; }
        public Ability[] Abilities { get => UnitData.abilities; set => UnitData.abilities = value; }
        public UnitData UnitData { get => unitData; set => unitData = value; }

        public int health;

        /// <summary>
        /// Deal damage to a unit.
        /// </summary>
        /// <param name="damage">Damage to be dealt. Damage is clamped to 0.</param>
        /// <returns>Actual damage dealt.</returns>
        public int DealDamage(int damage)
        {
            damage = Mathf.Max(damage, 0);
            damage = Mathf.Min(damage, health);
            health -= damage;

            UpdateHealthUI();
            return damage;
        }

        /// <summary>
        /// Heal health to a unit.
        /// </summary>
        /// <param name="damage">Health to be healed. Heal is clamped to 0.</param>
        /// <returns>Actual health healed.</returns>
        public int Heal(int heal)
        {
            heal = Mathf.Max(heal, 0);
            heal = Mathf.Min(heal, UnitData.maxHealth - health);
            health += heal;

            UpdateHealthUI();
            return heal;
        }

        public void UpdateHealthUI()
        {
            Slider healthBar = GetComponentInChildren<Slider>();
            TextMeshProUGUI[] textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI healthText = textMeshes.Where(x => x.gameObject.name == "HealthNumber").First();
            textMeshes.Where(x => x.gameObject.name == "Name").First().text = Name;

            healthBar.maxValue = UnitData.maxHealth;
            healthBar.minValue = 0;
            healthBar.value = health;

            healthText.text = health.ToString();
        }
    }
}
