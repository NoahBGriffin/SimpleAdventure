using System;
using static System.Console;
using System.Collections.Generic;

namespace SimpleAdventure
{

    static class GameActions
    {
        public static void DrinkRiverWater(Player player)
        {
            player.Heal(4);
            WriteLine("You drink some water from the river. You feel refreshed.");
        }

        public static void TalkToGuard(Player player)
        {
            string dialogue = "";
            if (player.Has(GameItems.SilverRing))
            {
                if (player.Has(GameItems.Witchbone))
                    dialogue = "The guard gives you a strange look as you open your bag.\n";
                dialogue += "You pull out the silver ring, ready to explain how you defeated the goblin when the guard holds up a hand to stop you.\n";
                dialogue += $"Guard: I see you have defeated the goblin that has plagued our town for years. You may enter our town, {player.Name}!";
                player.MoveTo(Locale.Town);
            }
            else
            {
                dialogue = "Guard: Hello there stranger. Sorry, but I cannot let just anyone enter the town without proof that you are trustworthy.";
            }
            ShowMessage(dialogue);
        }

        public static void FindForrestItems(Player player)
        {
            player.Equip(GameItems.ChainMailArmor);
            player.Equip(GameItems.LongSword);
            ShowMessage("You found some Chain Mail and a Long Sword behind a tree!");
            Program.theWorld[player.Locale].RemoveMenuItem("Search the woods nearby");

        }

        public static void IntoTheWoods(Player player)
        {
            Random rng = new Random();
            int location = rng.Next(10);

            switch (location)
            {
                case 1:
                    if (player.Locale == Locale.HagHut)
                        ShowMessage("You wander for half an hour before finding yourself back at the old hut...how odd, you swear you were walking straight.");
                    else
                        player.MoveTo(Locale.HagHut);
                    break;
                case 2:
                case 3:
                    if (player.Locale == Locale.Clearing)
                        ShowMessage("After a few minutes walking in the woods you get spooked by a strange rustling and run back to the clearing.");
                    else
                        FindClearing(player);
                    break;
                default:
                    if (player.Locale != Locale.DeepWoods)
                        player.MoveTo(Locale.DeepWoods);
                    ShowMessage("You wander deep into the woods for a long while, all the trees begin to look the same....");
                    break;
            }
        }

        private static void FindClearing(Player player)
        {
            player.MoveTo(Locale.Clearing);
            ShowMessage("After wandering about for what feels like eternity you stumble upon an open clearing.");
        }

        public static void FindAxe(Player player)
        {
            ShowMessage("You find an old axe near one of the stumps and add it to your bag.");
            player.AddItem(GameItems.OldAxe);
            Program.theWorld[player.Locale].RemoveMenuItem("Examine the firepit");
        }

        public static void EnterHut(Player player)
        {
            ShowMessage("An old woman with a hooked nose and a large brimmed hat opens the door and greets you like an old friend." +
                        "\nShe ushers you inside before you can say a word.");
            player.MoveTo(Locale.PocketDimension);
        }

        public static void TalkToWitch(Player player)
        {
            if (player.Has(GameItems.RabbitsFoot))
            {
                WriteLine("Old Woman: You have something interesting with you I'd love to trade for. Dont worry, I have something very lucky to offer in return.");
                WriteLine($"Old Woman: Would you trade your {GameItems.RabbitsFoot} for this..?");
                WriteLine("She pulls out an elegantly carved wishbone that appears to be made of ivory.");
                
                bool validInput = false;
                string doTrade = "";
                while (!validInput)
                {
                    Write("Do you trade with her? (y/n) ");
                    doTrade = ReadLine();
                    if (doTrade == "")
                        continue;
                    else if (Char.ToLower(doTrade[0]) == 'n')
                    {
                        ShowMessage("The old woman looks disappointed but accepts your answer.");
                        validInput = true;
                    }
                    else if (Char.ToLower(doTrade[0]) == 'y')
                    {
                        ShowMessage($"The old woman cackles gleefully as she takes your {GameItems.RabbitsFoot}"
                                    + $" and hands you the {GameItems.Witchbone}." +
                                    "\nOld Woman: Thank you very much, dearie!");
                        player.RemoveItemFromBag(GameItems.RabbitsFoot);
                        player.AddItem(GameItems.Witchbone);
                        validInput = true;
                    }
                }
            }
            else
            {
                ShowMessage("The old woman places some tea before you." +
                            "\nOld Woman: So lovely to have a visitor, you know. Almost no one finds this place.");
            }
        }

        public static void LeaveWitchHut(Player player)
        {
            WriteLine("You kindly excuse yourself and leave Morgantha's hut");
            player.MoveTo(Locale.HagHut);
            if (player.Has(GameItems.Witchbone))
            {
                ShowMessage("You turn around to find the hut has vanished from where it stood, leaving behind a circle of mushrooms in it's place.");
                Program.theWorld[player.Locale].SetDescription("This is where that strange hut was, now there is only a circle of bright red mushrooms");
                Program.theWorld[player.Locale].RemoveMenuItem("Knock on the door");
            }
        }

        public static void ReturnFromTheWoods(Player player)
        {
            WriteLine("You retrace your steps and make it back to the forest's entrace.");
            player.MoveTo(Locale.Woods);
        }

        public static void DestroyBarricade(Player player)
        {
            if (player.Has(GameItems.OldAxe))
            {
                WriteLine($"You use the {GameItems.OldAxe} to bust through the barricade.");
                Program.theWorld[player.Locale].SetDescription("You come up to a bridge that looks safe to cross.");
                Program.theWorld[player.Locale].AddPathway(Direction.East, Locale.Field);
                Program.theWorld[player.Locale].RemoveMenuItem("Break down the barricade");
            }
            else
            {
                ShowMessage("You think you need some sort of tool to break through this barricade...");
            }
        }

        public static void TalkToCrier(Player player)
        {
            Actor crier = Program.theWorld[player.Locale].Resident;
            if (crier.Health < 1)
            {
                ShowMessage("No point speaking to a corpse, is there?");
                Program.theWorld[player.Locale].RemoveMenuItem("Talk to the town crier");
            }
            else if (crier.Health == crier.MaxHealth)
            {
                WriteLine("The Town Crier is a jolly looking fellow dressed in patched-up clothes");
                WriteLine("You go and introduce yourself to him.");
                WriteLine($"Town Crier: Nice to meet you {player.Name}.");
                WriteLine("Town Crier: I'm on my way to town, I can give you a new title and let everyone there know if you like.");
                Write("Town Crier: What do ya say? (y/n) ");
                bool changeTitle = true;
                bool originalTitle = true;
                string wantAChange = "";
                while (changeTitle)
                {
                    wantAChange = ReadLine();
                    if (wantAChange != "" && Char.ToLower(wantAChange[0]) == 'n')
                    {
                        WriteLine($"Town Crier: {player.Name} is pretty good, isn't it?");
                        if (!originalTitle)
                            WriteLine("The Town Crier puffs out his chest proudly.");
                        changeTitle = false;
                    }
                    else if (wantAChange != "" && Char.ToLower(wantAChange[0]) == 'y')
                    {
                        player.GenerateTitle();
                        WriteLine($"Congratulations! Your new title is: {player.Name}");
                        originalTitle = false;
                        Write("Town Crier: How's that? Should I think of another one? (y/n) ");
                    }
                    else
                    {
                        Write("Town Crier: I'm sorry, not sure I understood you. Would you like me to change your title? (y/n)");
                    }

                }
                ShowMessage("Town Crier: Well now I best be on my way. It was nice meeting you, hero! Best of luck in your adventures.");
            }
            else
            {
                ShowMessage("As you approach the Town Crier he runs away from you in fear, not wanting to be attacked again");
                Program.theWorld[player.Locale].AddResident(null);
            }
        }

        public static void PlayerAttacks(Player player)
        {
            Fight(player, false);
        }

        public static void MonsterAttacks(Player player)
        {
            Fight(player, true);
        }

        private static void Fight(Player player, bool monsterStarts)
        {
            Actor opponent = Program.theWorld[player.Locale].Resident;
            bool stillFighting = true;

            if (monsterStarts)
            {
                opponent.Attack(player);
                WriteLine($"The {opponent.Name} {opponent.Weapon.AttackFlavor} with their {opponent.Weapon}! You now have {player.Health} health points.");
                if (player.Health <= 0)
                {
                    stillFighting = false;
                    WriteLine("\nOh no! You are mortally wounded!  You are dead...");
                }
            }

            while (stillFighting)
            {
                player.Attack(opponent);
                WriteLine($"{player.Name} {player.Weapon.AttackFlavor} the {opponent.Name} with {player.Weapon}! The {opponent.Name} now has {opponent.Health} health points.");
                if (opponent.Health == 0)
                {
                    WriteLine();
                    WriteLine("YOU ARE VICTORIOUS!!!");
                    if (opponent is Monster)
                    {
                        Monster monster = (Monster)opponent;
                        List<Item> myLoot = monster.DropLoot();
                        foreach (Item item in myLoot)
                        {
                            player.AddItem(item);
                            WriteLine($"You find a {item} on the monster's body and you add it to your bag.");
                        }
                    }
                    if (opponent.Weapon.Damage > player.Weapon.Damage)
                    {
                        WriteLine($"You take the {opponent.Name}'s {opponent.Weapon} and equip it");
                        player.Equip(opponent.Weapon);
                        opponent.Equip(GameItems.Hands);
                    }
                    stillFighting = false;
                }

                if (stillFighting)
                {

                    opponent.Attack(player);
                    WriteLine($"The {opponent.Name} attacks you with their {opponent.Weapon}! You now have {player.Health} health points.");
                    if (player.Health == 0)
                    {
                        WriteLine("\nOh no! You are mortally wounded! You are dead...");
                        stillFighting = false;
                    }
                }

                WriteLine();
                if (stillFighting)
                {
                    string continueFight = "";
                    bool validResponse = false;
                    while (!validResponse)
                    {
                        Write("Do you wish to continue fighting (y/n)? ");
                        continueFight = ReadLine();
                        if (continueFight == "")
                            continue;
                        if (Char.ToLower(continueFight[0]) == 'n')
                        {
                            stillFighting = false;
                            validResponse = true;
                            WriteLine("You beat a hasty retreat and live to fight another day.");
                        }
                        else if (Char.ToLower(continueFight[0]) == 'y')
                        {
                            validResponse = true;
                        }
                    }

                }
            }
        }

        public static void ShowMessage(string message)
        {
            WriteLine();
            WriteLine(message);
            WriteLine();
            Write("Press Enter to continue...");
            ReadLine();
        }

    }
}