using System;
using System.Collections.Generic;

namespace SimpleAdventure
{
    class Actor
    {
        public Locale Locale { get; private set; }
        public String Name { get; protected set; }
        public int Health { get; private set; }
        public Weapon Weapon { get; private set; }
        public Armor Armor { get; private set; }
        public int MaxHealth { get; private set; }
        public bool CanFight { get; private set; }
        public Actor (string name, int health = 8, Locale location = Locale.Crossroads, bool canFight = true)
        {
            if (name == null)
                throw new ArgumentNullException("Name cannot be null");
            if (name == "")
                throw new ArgumentException("Name cannot be blank");
            if (health < 0)
                throw new ArgumentOutOfRangeException("Health must be a positive value");

            this.Name = name;
            this.Locale = location;
            this.Health = health;
            this.Weapon = GameItems.Hands;
            this.Armor = GameItems.Skin;
            this.MaxHealth = health;
            this.CanFight = canFight;
        }

        public void Equip(Weapon weapon)
        {
            if (weapon == null)
                throw new ArgumentNullException("Weapon can not be null");
            Weapon = weapon;
        }

        public void Equip(Armor armor)
        {
            if (armor == null)
                throw new ArgumentNullException("Armor can not be null.");
            Armor = armor;
        }

        private void Defend(int initialDamage)
        {
            Random rng = new Random();
            int protection = rng.Next(Armor.Protection);
            int actualDamage = initialDamage - protection;
            if (actualDamage < 1)
                actualDamage = 1;
            Health -= actualDamage;
            if (Health < 0)
                Health = 0;
        }

        public void Attack(Actor target)
        {
            if (target == null)
                throw new ArgumentNullException("Target cannot be null");
            target.Defend(new Random().Next(Weapon.Damage));
        }

        public void Heal(int healing)
        {
            if (healing < 1)
                throw new ArgumentOutOfRangeException("Health must be increased by 1 or more");
            this.Health += healing;
            if (Health > MaxHealth)
                Health = MaxHealth;
        }

        public void MoveTo(Locale newLocation)
        {
            Locale = newLocation;
        }

        public override String ToString()
        {
            String s = $"{Name}       Health: {Health}\n";
            s += $"Weapon: {Weapon}     Armor: {Armor}\n";
            return s;
        }

    }

    class Monster : Actor
    {
        public List<Item> Loot { get; private set; }
        public Monster(String name, int health = 10, Locale location = Locale.Woods) 
                : base(name, health, location)
        {
            this.Loot = new List<Item>();            
        }

        public void AddItem(Item newItem)
        {
            if (newItem == null)
                throw new ArgumentNullException("Item cannot be null");
            Loot.Add(newItem);
        }

        public List<Item> DropLoot()
        {
            if (Health == 0)
                return Loot;
            return new List<Item>();
        }
    }

    class Player : Actor
    {
        private string basicName;
        public List<Item> Bag { get; private set; }

        public Player(String name, int health = 20)
                    : base(name, health)
        {
            Bag = new List<Item>();
            this.basicName = name;
            GenerateTitle();
        }

        public bool At(Locale isHere)
        {
            if (isHere == Locale)
                return true;
            else
                return false;
        }

        public void AddItem(Item newItem)
        {
            if (newItem == null)
                throw new ArgumentNullException("Item cannot be null");
            Bag.Add(newItem);
        }

        public Item RemoveItemFromBag(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item cannot be null");
            if (Bag.Remove(item))
                return item;
            return null;
        }

        public bool Has(Item item)
        {
            foreach (Item i in Bag)
            {
                if (i == item)
                    return true;
            }
            return false;
        }

        public void LootMonster(Monster deadMonster)
        {
            if (deadMonster == null)
                throw new ArgumentNullException("Monster cannot be null");
            if (deadMonster.Health > 0)
                throw new ArgumentException("Cannot loot a live monster!");

            Bag.AddRange(deadMonster.DropLoot());
        }

        public override String ToString()
        {
            String s = "-----------------------------------------------------\n";
            s += base.ToString();

            s += "Inventory: [";
            foreach (Item item in Bag)
                s += $"{item}, ";  
            s = s.TrimEnd(new char[] {',', ' '});
            s += "]\n";

            s += "-----------------------------------------------------\n";
            return s;
        }

        public void GenerateTitle()
        {
            Random rng = new Random();
            String heroTitle = "";
            //titles are from dieharddice.com's "Ye Old Champion Honorific Generator" with some adjustments
            var title1 = new List<String>() {"Honorable", "Beloved", "Hero's-Hero", "Knighted", "Divine", 
                                        "Double-Forged", "Twice-Blessed", "Unkempt", "Respectable", "Illustrious"};
            var title2 = new List<String>() {"The Nervous", "The Just", "The Glorious", "The Average", "The Mighty",
                                            "The Noble", "The Proud", "The Bold", "The Magnificent", "The Kinda Invincible"};
            heroTitle = $"{title1[rng.Next(10)]} {basicName} {title2[rng.Next(10)]}";
            this.Name = heroTitle;
        }

    }
}