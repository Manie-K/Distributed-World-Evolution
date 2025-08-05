using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AnimalData : ICreatureData
    {
        public int Speed { set; get; }
        public int Health { set; get; }
        public int Damage { set; get; }
        public int AttackRange { set; get; }
        public int Defense { get; set; }

        public AnimalData(string name, int health, int damage, int speed, int attackRange, int defense, int reproduceCooldown)
        {
            Type = CreatureType.Animal;
            Name = name;
            Health = health;
            Damage = damage;
            Speed = speed;
            AttackRange = attackRange;
            Defense = defense;
            ReproduceCooldown = reproduceCooldown;
        }
    }
}
