using System;

namespace SimpleAdventure
{
    // Useful, pre-created Items, Armor, and Weapons that can be used anywhere in the game

    class GameItems
    {
        // Items
        public static readonly Item RabbitsFoot = new Item("Rabbit's Foot");
        public static readonly Item SilverRing = new Item("Silver Ring");
        public static readonly Item OldAxe = new Item("Old Axe");
        public static readonly Item Witchbone = new Item("Strange Wishbone");

        // Armor
        public static readonly Armor Skin = new Armor("Skin");
        public static readonly Armor ClothArmor = new Armor("Cloth Armor", 1);
        public static readonly Armor LeatherArmor = new Armor("Leather Armor", 4);
        public static readonly Armor ChainMailArmor = new Armor("ChainMail Armor", 8);
        public static readonly Armor GuardsArmor = new Armor("Guards Armor", 1000);
        public static readonly Armor SkinDress = new Armor("'Leather' Dress", 10);

        // Weapons
        public static readonly Weapon Hands = new Weapon("Hands", attackText: "punches");
        public static readonly Weapon RustyAxe = new Weapon("Rusty Axe", 5);
        public static readonly Weapon GuardsPike = new Weapon("Guards Pike", 10);
        public static readonly Weapon BoneStaff = new Weapon("Old Cane", 15, "casts a spell with");
        public static readonly Weapon LongSword = new Weapon("Long Sword", 8, "strikes");        
        public static readonly Weapon Dagger = new Weapon("Dagger", 3, "stabs");

    }
}