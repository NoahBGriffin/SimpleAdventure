using System;
using System.Collections.Generic;

namespace SimpleAdventure
{
    class Item
    {
        public String Name { get; private set; }

        public Item(String name)
        {
            if (name == null)
                throw new ArgumentNullException("Name cannot be null.");
            if (name == "")
                throw new ArgumentException("Name cannot be blank");
            this.Name = name;
        }

        public override String ToString()
        {
            return Name;
        }
    }

    class Weapon : Item
    {
        public int Damage { get; private set; }
        public String AttackFlavor { get; private set; }

        public Weapon(String name, int damage = 1, String attackText = "attacks") : base(name)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException("Damage must be a positive value");
            this.Damage = damage;
            this.AttackFlavor = attackText;
        }

        public override String ToString()
        {
            return $"{Name}({Damage})";
        }
    }

    class Armor : Item
    {
        public int Protection { get; private set; }

        public Armor(String name, int protection = 0) : base(name)
        {
            if (protection < 0)
                throw new ArgumentOutOfRangeException("Protection must be a positive value");
            this.Protection = protection;
        }

        public override string ToString()
        {
            return $"{Name}({Protection})";
        }
    }
}

