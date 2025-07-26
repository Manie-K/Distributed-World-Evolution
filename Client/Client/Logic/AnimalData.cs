using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AnimalData : ICreatureData
    {
        public int speed { set; get; }
        public int health { set; get; }
        public int damage { set; get; }
        public int attackRange { set; get; }
        public int Defense { get; set; }

        public AnimalData(string name, int health, int damage, int speed, int attackRange, int defense, int reproducecooldown)
        {
            this.type = Type.Animal;
            this.name = name;
            this.health = health;
            this.damage = damage;
            this.speed = speed;
            this.attackRange = attackRange;
            this.Defense = defense;
            this.ReproduceCooldown = reproducecooldown;
        }
    }
}
