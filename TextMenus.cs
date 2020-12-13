using System;
using System.Collections.Generic;

namespace LWTech.CSD228.TextMenus
{
    // Two classes that can be used to create a simple text menuing system
    //     <T> is the parameter that is passed to any menuitem's Action

    class TextMenuItem<T>
    {
        public string Description { get; private set; }
        public Action<T> Action { get; private set; }

        public TextMenuItem(string description, Action<T> action)
        {
            if (description == null)
                throw new ArgumentNullException("A Menuitem's description cannot be null");
            if (description.Length == 0)
                throw new ArgumentException("A Menuitem's description cannot be empty");
            if (action == null)
                throw new ArgumentNullException("A Menuitem's action cannot be null");

            this.Description = description;
            this.Action = action;
        }

        public void Run(T param)
        {
            Action(param);
        }

        public override string ToString()
        {
            return Description;
        }
    }

    class TextMenu<T>
    {
        private List<TextMenuItem<T>> menuItems;

        public TextMenu()
        {
            menuItems = new List<TextMenuItem<T>>();
        }

        public TextMenuItem<T> GetItem(int i)
        {
            if (i < 0 || i > menuItems.Count)
                throw new ArgumentOutOfRangeException();

            return menuItems[i];
        }

        public void AddItem(TextMenuItem<T> item)
        {
            if (item == null)
                throw new ArgumentNullException("Cannot add null menuitem to menu");

            menuItems.Add(item);
        }

        public void Run(int i, T param)
        {
            if (i < 0 || i >= menuItems.Count)
                throw new ArgumentOutOfRangeException("No menuitem found at that index");

            menuItems[i].Run(param);
        }

        public int Size()
        {
            return menuItems.Count;
        }

        public int GetMenuChoiceFromUser()
        {
            bool done = false;
            int choice = 0;
            while (!done)
            {
                Console.WriteLine();
                Console.WriteLine(this);
                Console.Write($"Enter a number from 1 to {Size()}: ");
                string s = Console.ReadLine();
                try
                {
                    choice = int.Parse(s);
                    if (choice < 1 || choice > Size())
                        Console.WriteLine("Invalid selection.  Please try again.");
                    else
                        done = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Invalid entry.  Please enter a number between 1 and {Size()}.");
                }
            }
            return choice;
        }

        public override string ToString()
        {
            int i = 1;                          // The menu that a user sees begins with "1"
            string s = "";
            foreach (TextMenuItem<T> item in menuItems)
            {
                s += $"{i}) {item}\n";
                i++;
            }
            return s;
        }
    }

}