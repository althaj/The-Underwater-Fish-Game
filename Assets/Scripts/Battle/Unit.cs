using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Battle.Abilities;
using TUFG.Battle.AI;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using TUFG.Inventory;

namespace TUFG.Battle
{
    /// <summary>
    /// MonoBehavior representing a unit.
    /// </summary>
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitData unitData;
        [SerializeField] private int health;

        /// <summary>
        /// Is the unit ally to the player or enemy?
        /// </summary>
        public bool IsAlly { get; set; }

        /// <summary>
        /// Name of the unit.
        /// </summary>
        public string Name { get => UnitData.name; private set => UnitData.name = value; }

        /// <summary>
        /// Is the unit player?
        /// </summary>
        public bool IsPlayer { get; set; } = false;

        /// <summary>
        /// Abilities of the unit.
        /// </summary>
        public Ability[] Abilities { get => UnitData.abilities; set => UnitData.abilities = value; }

        /// <summary>
        /// Data representing the unit.
        /// </summary>
        public UnitData UnitData { get => unitData; set => unitData = value; }

        /// <summary>
        /// Speed of the unit. Gets bonuses and penalties from player items.
        /// </summary>
        public int Speed { get => IsPlayer ? UnitData.speed + InventoryManager.Instance.GetStatBonuses(ItemStatType.Speed) : UnitData.speed; }

        /// <summary>
        /// Power of the unit. Power affects magical abilities. Gets bonuses and penalties from player items.
        /// </summary>
        public int Power { get => IsPlayer ? unitData.power + InventoryManager.Instance.GetStatBonuses(ItemStatType.Power) : unitData.power; }

        /// <summary>
        /// Strength of the unit. Strength affects physical abilities. Gets bonuses and penalties from player items.
        /// </summary>
        public int Strength { get =>IsPlayer ? unitData.strength + InventoryManager.Instance.GetStatBonuses(ItemStatType.Strenght) : unitData.strength; }

        /// <summary>
        /// Current health of the unit. Gets bonuses and penalties from player items.
        /// </summary>
        public int Health { get => health; set => health = value; }

        /// <summary>
        /// Armor of the unit. Armor lowers incoming damage. Gets bonuses and penalties from player items.
        /// </summary>
        public int Armor { get => IsPlayer ? unitData.armor + InventoryManager.Instance.GetStatBonuses(ItemStatType.Armor) : unitData.armor; }

        /// <summary>
        /// Maximum health of the unit. Gets bonuses and penalties from player items.
        /// </summary>
        public int MaxHealth { get => IsPlayer ? unitData.maxHealth + InventoryManager.Instance.GetStatBonuses(ItemStatType.Health) : unitData.maxHealth; }

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
            heal = Mathf.Min(heal, MaxHealth - Health);
            health += heal;

            UpdateHealthUI();
            return heal;
        }

        /// <summary>
        /// Updates health UI for the unit.
        /// </summary>
        /// <remarks>This sets unit name text, as well as max health and current health on the health bar.</remarks>
        public void UpdateHealthUI()
        {
            Slider healthBar = GetComponentInChildren<Slider>();
            TextMeshProUGUI[] textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI healthText = textMeshes.Where(x => x.gameObject.name == "HealthNumber").First();
            textMeshes.Where(x => x.gameObject.name == "Name").First().text = Name;

            healthBar.maxValue = MaxHealth;
            healthBar.minValue = 0;
            healthBar.value = Health;

            healthText.text = health.ToString();
        }
    }
}
