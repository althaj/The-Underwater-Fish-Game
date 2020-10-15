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
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitData unitData;
        [SerializeField] private int health;

        public bool IsAlly { get; set; }
        public string Name { get => UnitData.name; private set => UnitData.name = value; }
        public bool IsPlayer { get; set; } = false;
        public Ability[] Abilities { get => UnitData.abilities; set => UnitData.abilities = value; }
        public UnitData UnitData { get => unitData; set => unitData = value; }
        public int Speed { get => IsPlayer ? UnitData.speed + InventoryManager.Instance.GetStatBonuses(ItemStatType.Speed) : UnitData.speed; }
        public int Power { get => IsPlayer ? unitData.power + InventoryManager.Instance.GetStatBonuses(ItemStatType.Power) : unitData.power; }
        public int Strength { get =>IsPlayer ? unitData.strength + InventoryManager.Instance.GetStatBonuses(ItemStatType.Strenght) : unitData.strength; }
        public int Health { get => health; set => health = value; }
        public int Armor { get => IsPlayer ? unitData.armor + InventoryManager.Instance.GetStatBonuses(ItemStatType.Armor) : unitData.armor; }
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
