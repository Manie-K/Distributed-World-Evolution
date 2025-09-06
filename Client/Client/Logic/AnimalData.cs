using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AnimalData : ICreatureData
    {
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int AttackRange { get; set; }
        public int MoveSpeed { get; set; }
        public int Armor { get; set; }
        public bool IsAggressive { get; set; }
        public bool IsFertile { get; set; }
        public bool IsTamable { get; set; }

        public AnimalData(string name, int health, int attack, int speed, int range, int armor, int breedingCooldown, bool aggressive, bool canReproduce, bool isTamable)
        {
            Type = CreatureType.Animal;
            Name = name;
            Health = health;
            AttackPower = attack;
            MoveSpeed = speed;
            AttackRange = range;
            Armor = armor;
            BreedingCooldown = breedingCooldown;
            IsAggressive = aggressive;
            IsFertile = canReproduce;
            IsTamable = isTamable;
        }
    }
}
