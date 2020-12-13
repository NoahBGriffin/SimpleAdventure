using System;
using System.Collections.Generic;
using LWTech.CSD228.TextMenus;
using static System.Console;

namespace SimpleAdventure
{
    class Program
    {
    public static Dictionary<Locale, Location> theWorld = new Dictionary<Locale, Location>();

        static void Main(string[] args)
        {

            WriteLine("Simple Adventure - \n=======================================================\n");

            Location townGates = new Location(Locale.TownGates, "You are at the gates of a town.");
            townGates.AddPathway(Direction.North, Locale.Crossroads);
            Actor townGuard = new Actor("Town Guard", 1000, Locale.TownGates, false);
            townGuard.Equip(GameItems.GuardsArmor);
            townGuard.Equip(GameItems.GuardsPike);
            townGates.AddResident(townGuard);
            townGates.AddMenuItem(new TextMenuItem<Player>("Talk to the guard", GameActions.TalkToGuard));
            theWorld.Add(Locale.TownGates, townGates);

            Location crossroads = new Location(Locale.Crossroads, "You are at a lonely 4-way crossroads. You cannot see what lies in each direction.");
            crossroads.AddPathway(Direction.North, Locale.River);
            crossroads.AddPathway(Direction.South, Locale.TownGates);
            crossroads.AddPathway(Direction.West, Locale.Woods);
            crossroads.AddPathway(Direction.East, Locale.Bridge);
            theWorld.Add(Locale.Crossroads, crossroads);

            Location bridge = new Location(Locale.Bridge, "You come up to a bridge that appears to have been barricaded.");
            bridge.AddPathway(Direction.West, Locale.Crossroads);
            Monster goblin = new Monster("Goblin", 20, Locale.Bridge);
            goblin.Equip(GameItems.LeatherArmor);
            goblin.Equip(GameItems.RustyAxe);
            goblin.AddItem(GameItems.SilverRing);
            bridge.AddResident(goblin);
            bridge.AddPreAction(GameActions.MonsterAttacks);
            bridge.AddMenuItemPostBattle(new TextMenuItem<Player>($"Break down the barricade", GameActions.DestroyBarricade));
            theWorld.Add(Locale.Bridge, bridge);

            Location field = new Location(Locale.Field, "Once across the bridge you end up in fields that appear to be farmland.");
            field.AddPathway(Direction.West, Locale.Bridge);
            Actor crier = new Actor("Town Crier", location: Locale.Field);
            crier.Equip(GameItems.ClothArmor);
            field.AddResident(crier);
            field.AddMenuItem(new TextMenuItem<Player>("Talk to the town crier", GameActions.TalkToCrier));
            theWorld.Add(Locale.Field, field);

            Location river = new Location(Locale.River, "You are at a swift-flowing, broad river that cannot be crossed.");
            river.AddPathway(Direction.South, Locale.Crossroads);
            river.AddMenuItem(new TextMenuItem<Player>("Drink some water", GameActions.DrinkRiverWater));
            theWorld.Add(Locale.River, river);

            Location woods = new Location(Locale.Woods, "You are in a dark forboding forest. Fallen trees block your way.");
            woods.AddPathway(Direction.East, Locale.Crossroads);
            woods.AddMenuItem(new TextMenuItem<Player>("Search the woods nearby", GameActions.FindForrestItems));
            woods.AddMenuItem(new TextMenuItem<Player>("Climb over the fallen trees and head deeper into the woods", GameActions.IntoTheWoods));
            theWorld.Add(Locale.Woods, woods);

            Location deepwoods = new Location(Locale.DeepWoods, 
                        "You are deep inside the forest, surrounded by forboding trees that cast long shadows and tall ferns that making walking difficult.");
            deepwoods.AddMenuItem(new TextMenuItem<Player>("Go back the way you came", GameActions.ReturnFromTheWoods));
            deepwoods.AddMenuItem(new TextMenuItem<Player>("Continue into the woods...", GameActions.IntoTheWoods));
            theWorld.Add(Locale.DeepWoods, deepwoods);

            Location clearing = new Location(Locale.Clearing, "You find yourself in a bright, sunny clearing. There are a few stumps and an abandoned firepit near the center.");
            clearing.AddMenuItem(new TextMenuItem<Player>("Examine the firepit", GameActions.FindAxe));
            clearing.AddMenuItem(new TextMenuItem<Player>("Return to the forest entrance", GameActions.ReturnFromTheWoods));  
            clearing.AddMenuItem(new TextMenuItem<Player>("Keep searching the woods", GameActions.IntoTheWoods));          
            theWorld.Add(Locale.Clearing, clearing);

            Location outsidehaghhut = new Location(Locale.HagHut, "You come across a tiny hut covered in moss, it looks ancient and rotted");
            outsidehaghhut.AddMenuItem(new TextMenuItem<Player>("Knock on the door", GameActions.EnterHut));
            outsidehaghhut.AddMenuItem(new TextMenuItem<Player>("Keep searching the woods", GameActions.IntoTheWoods));
            theWorld.Add(Locale.HagHut, outsidehaghhut);

            Location haghut = new Location(Locale.PocketDimension, "The inside of the hut is larger than the outside and full of clutter and plants." +
                                "\nThere is a lit fireplace with a cauldron on it");
            Actor hag = new Actor("Old Woman", 1000, Locale.PocketDimension, false);
            hag.Equip(GameItems.SkinDress);
            hag.Equip(GameItems.BoneStaff);
            haghut.AddResident(hag);
            haghut.AddMenuItem(new TextMenuItem<Player>("Talk to the old woman", GameActions.TalkToWitch));
            haghut.AddMenuItem(new TextMenuItem<Player>("Exit the hut", GameActions.LeaveWitchHut));
            theWorld.Add(Locale.PocketDimension, haghut);

            string playerName = "";
            bool validResponse = false;
            while (!validResponse)
            {
                Write("Please enter a name for your hero: ");
                playerName = ReadLine();
                if (playerName != "")
                {
                    validResponse = true;
                }
                else
                {
                    WriteLine("Player name cannot be blank.");
                }
            }

            Player ourHero = new Player(playerName, 20);
            ourHero.Equip(GameItems.LeatherArmor);
            ourHero.Equip(GameItems.Dagger);
            ourHero.AddItem(GameItems.RabbitsFoot);
            ourHero.MoveTo(Locale.TownGates);

            WriteLine($"\nWelcome to Simple AdventureLand, {ourHero.Name}!\n");

            bool done = false;
            while (ourHero.Health > 0 && ourHero.Locale != Locale.Town && !done)
            {
                Location location = theWorld[ourHero.Locale];
                WriteLine(ourHero);

                location.RunPreAction(ourHero);
                location.AddPreAction(null);
                location.CreatePostBattleOption();

                if (ourHero.Health == 0)
                    break;

                WriteLine(location);
                WriteLine();
                TextMenu<Player> menu = location.GetMenu();
                //menu.AddItem(new TextMenuItem<Player>("Quit", (p)=>{ done = true; }));

                WriteLine("What would you like to do?");
                int i = menu.GetMenuChoiceFromUser() - 1;
                menu.Run(i, ourHero);
            }

            WriteLine("THE END.");
        }
    }
}
