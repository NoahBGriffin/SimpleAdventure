using System;
using System.Collections.Generic;
using LWTech.CSD228.TextMenus;

namespace SimpleAdventure
{
    public enum Locale {River, Town, Woods, Bridge, TownGates, Crossroads, DeepWoods, HagHut, PocketDimension, Clearing, Field, Nowhere}
    public enum Direction {North, East, South, West}

    class Location
    {
        public Locale ID { get; private set; }
        public String Description { get; private set; }
        public Actor Resident { get; private set; }
        public List<TextMenuItem<Player>> MenuItems { get; private set; }
        public Dictionary<Direction, Locale> Pathways { get; private set; }
        public Action<Player> PreAction { get; private set; }
        public TextMenuItem<Player> PostBattleOption { get; private set; }

        public Location(Locale locale, String description)
        {
            if (description == null)
                throw new ArgumentNullException("Description cannot be null");
            if (description == "")
                throw new ArgumentException("Description cannot be blank");

            this.ID = locale;
            this.Description = description;
            this.Resident = null;
            this.Pathways = new Dictionary<Direction, Locale>();
            this.MenuItems = new List<TextMenuItem<Player>>();
            this.PreAction = null;
            this.PostBattleOption = null;
        }

        public void SetDescription(string description)
        {
            if (description != "" && description != null)
                this.Description = description;
        }

        public void AddPathway(Direction direction, Locale locale)
        {
            Pathways.Add(direction, locale);
        }

        public void AddResident(Actor resident)
        {
            Resident = resident;        //null is allowed
        }

        public void AddMenuItem(TextMenuItem<Player> menuItem)
        {
            if (menuItem == null)
                throw new ArgumentException("Menu Item cannot be null");
            MenuItems.Add(menuItem);
        }

        public void RemoveMenuItem(string description)
        {
            foreach (TextMenuItem<Player> menuItem in MenuItems)
            {
                if (menuItem.Description.Equals(description))
                {
                    MenuItems.Remove(menuItem);
                    return;
                }
            }
        }

        public void AddPreAction(Action<Player> preaction)
        {
            this.PreAction = preaction;
        }

        public void RunPreAction(Player player)
        {
            if (PreAction != null)
            {
                PreAction(player);
            }
        }

        public void AddMenuItemPostBattle(TextMenuItem<Player> postoption)
        {
            this.PostBattleOption = postoption;
        }

        public void CreatePostBattleOption()
        {
            if (PostBattleOption != null && Resident.Health < 1)
            {
                Program.theWorld[this.ID].AddMenuItem(PostBattleOption);
                this.PostBattleOption = null;
            }
        }

        public TextMenu<Player> GetMenu()
        {
            TextMenu<Player> menu = new TextMenu<Player>();
            foreach (Direction direction in Pathways.Keys)
            {
                menu.AddItem(new TextMenuItem<Player>($"Go {direction}", 
                                (p)=>{ p.MoveTo(Pathways[direction]); }));
            }
            foreach (TextMenuItem<Player> menuitem in MenuItems)
                menu.AddItem(menuitem);
            if (Resident != null && Resident.Health > 0 && Resident.CanFight)
                menu.AddItem(new TextMenuItem<Player>($"Fight {Resident.Name}", GameActions.PlayerAttacks));
            return menu;
        }

        public override String ToString()
        {
            string s = Description;
            if (Resident != null && Resident.Health > 0)
                s += $"\nA {Resident.Name} is standing nearby.";
            return s;   
        }
    }
}
