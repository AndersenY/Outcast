using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using System.Media;
using System.Runtime.Remoting;
using System.Diagnostics;
using System.Timers;
using System.Collections;

namespace ROGALIK
{
    internal class Inventory
    {
        private static int index_inventory = 0;
        private static int index_inventory_move = 0;
        private static int selectedY;
        private static int count_endl = 1;
        private static bool is_exist;

        public static void PickUpThing(int userX, int userY, ref int[,] enemyCoord)
        {
            
            string[] ItemsKind = { "armor", "weapon", "potion", "coin" };
            int[] ItemsCount = { 11, 9, 6, 3 };

            for (int i = 0; i < enemyCoord.GetLength(0); i++)
            {
                if (userX == enemyCoord[i,0] && userY == enemyCoord[i, 1])
                {
                    Random rand1 = new Random();
                    int NumberOfFile = rand1.Next(0, 4);
                    int Count = ItemsCount[NumberOfFile];

                    string[] file = File.ReadAllLines($"Items/{ItemsKind[NumberOfFile]}.txt");
                    int random1 = rand1.Next(0, Count);

                    int count1 = random1 * 6;
                    Item item = new Item(file[count1], file[count1 + 1], file[count1 + 2], file[count1 + 3], file[count1 + 4], file[count1 + 5]);
                    Item.PutToInventory(item);

                    Graphics.PrintIvents($"Вы подобрали {item.Name}", ConsoleColor.DarkCyan);

                    enemyCoord[i, 0] = 0;
                    enemyCoord[i, 1] = 0;

                }
            }

        }
        public static void ShowInventory(ref Player player)
        {

            List<string> inventory_move_1 = new List<string>()
            {
                "Использовать",
                " Выбросить",
                "Выбросить все"
            };
            List<string> inventory_move_2 = new List<string>()
            {
                "Экипировать",
                " Выбросить",
                "Выбросить все"
            };
            List<string> inventory_move_3 = new List<string>()
            {
                "Снять с себя",
                " Выбросить",
                "Выбросить все"
            };
            string SelectedMenuMove;
            Item SelectedMenuItem = new Item();

            while (true)
            {

                List<Item> inventory = new List<Item>();

                string[] file = File.ReadAllLines("inventory.txt");
                if (file.Length == 0)
                {
                    Console.Clear();
                    Graphics.PrintText("Инвентарь пуст!!!", 90, 20);
                    Console.ReadKey();
                    break;

                }
                else
                {

                    for (int i = 0; i < file.Length; i += 6)
                    {
                        if (Convert.ToInt32(file[i + 5]) == 1)
                            inventory.Add(new Item(file[i], file[i + 1], file[i + 2], file[i + 3], file[i + 4], file[i + 5]));
                        else
                            inventory.Add(new Item(file[i], file[i + 1], file[i + 2] + $"({file[i + 5]})", file[i + 3], file[i + 4], file[i + 5]));

                    }


                    Console.Clear();
                    Graphics.PrintSelectedText("\n\n\n\n              Инвентарь: ", 0, 0, ConsoleColor.DarkCyan);
                    Graphics.PrintSelectedText("Информация о персонаже: ", 158, 15, ConsoleColor.DarkCyan);
                    Graphics.PrintText("Здоровье: ", 150, 17);
                    Player.HealthBar(ConsoleColor.Green, player);
                    player = Player.PlayerInformation(player, 150, 19);


                    if (index_inventory == inventory.Count && !is_exist)
                        index_inventory--;
                    SelectedMenuItem = Graphics.DrawMenu(inventory, 5, 7, count_endl, ref index_inventory, ref selectedY);

                    if (SelectedMenuItem.Kind == "Escape")
                    {
                        index_inventory = 0;
                        break;
                    }

                    else if (SelectedMenuItem.Kind == "Bib" || SelectedMenuItem.Kind == "Helm" || SelectedMenuItem.Kind == "Weapon")
                    {
                        while (true)
                        {
                            string[] eqp = File.ReadAllLines($"Equipment/equipped_{SelectedMenuItem.Kind}.txt");
                            bool is_eqp_exist = false;

                            for (int i = 0; i < eqp.Length; i++)
                            {
                                if (SelectedMenuItem.Name == eqp[i] || SelectedMenuItem.Name == eqp[i] + $"({SelectedMenuItem.CountThings})")
                                    is_eqp_exist = true;
                            }

                            if (is_eqp_exist)
                                SelectedMenuMove = Graphics.DrawMenu(inventory_move_3, 40, selectedY - count_endl - 1, count_endl, ref index_inventory_move);
                            else
                                SelectedMenuMove = Graphics.DrawMenu(inventory_move_2, 40, selectedY - count_endl - 1, count_endl, ref index_inventory_move);

                            if (SelectedMenuMove == "Экипировать")
                            {
                                index_inventory_move = 0;
                                Item.Equip(SelectedMenuItem);
                                player = new Player(player.X, player.Y, player.Health, player.Money);
                                Graphics.PrintIvents($"Вы экипировали {inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                Thread.Sleep(500);

                                break;
                            }
                            else if (SelectedMenuMove == " Выбросить")
                            {
                                index_inventory_move = 0;
                                is_exist = Item.Drop(false, "inventory.txt", SelectedMenuItem);
                                Item.Drop(false, $"Equipment/equipped_{SelectedMenuItem.Kind}.txt", SelectedMenuItem);
                                player = new Player(player.X, player.Y, player.Health, player.Money);
                                Graphics.PrintIvents($"Вы выбросили {inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                Thread.Sleep(500);

                                break;
                            }
                            else if (SelectedMenuMove == "Выбросить все")
                            {
                                index_inventory_move = 0;
                                is_exist = Item.Drop(true, "inventory.txt", SelectedMenuItem);
                                Item.Drop(true, $"Equipment/equipped_{SelectedMenuItem.Kind}.txt", SelectedMenuItem);
                                Graphics.PrintIvents($"Вы выбросили все {inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                Thread.Sleep(500);


                                break;
                            }
                            else if (SelectedMenuMove == "Снять с себя")
                            {
                                index_inventory_move = 0;
                                Item.Drop(true, $"Equipment/equipped_{SelectedMenuItem.Kind}.txt", SelectedMenuItem);
                                player = new Player(player.X, player.Y, player.Health, player.Money);
                                Graphics.PrintIvents($"Вы сняли с себя {inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                Thread.Sleep(500);


                                break;
                            }
                            else if (SelectedMenuMove == "Escape")
                            {
                                index_inventory_move = 0;
                                SelectedMenuMove = "";
                                break;
                            }
                        }
                    }
                    else if (SelectedMenuItem.Kind != "")
                    {
                        while (true)
                        {
                            SelectedMenuMove = Graphics.DrawMenu(inventory_move_1, 40, selectedY - count_endl - 1, count_endl, ref index_inventory_move);

                            if (SelectedMenuMove == "Использовать")
                            {
                                index_inventory_move = 0;
                                Item.Use(ref player, SelectedMenuItem);
                                Item.Drop(false, "inventory.txt", SelectedMenuItem);
                                Graphics.PrintIvents($"Вы использовали {inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                Thread.Sleep(500);

                                break;
                            }
                            else if (SelectedMenuMove == " Выбросить")
                            {
                                index_inventory_move = 0;
                                is_exist = Item.Drop(false, "inventory.txt", SelectedMenuItem);
                                Graphics.PrintIvents($"Вы выбросили {inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                Thread.Sleep(500);

                                break;
                            }
                            else if (SelectedMenuMove == "Выбросить все")
                            {
                                index_inventory_move = 0;
                                is_exist = Item.Drop(true, "inventory.txt", SelectedMenuItem);
                                Graphics.PrintIvents($"Вы выбросили все {inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                Thread.Sleep(500);

                                break;
                            }
                            else if (SelectedMenuMove == "Escape")
                            {
                                index_inventory_move = 0;
                                SelectedMenuMove = "";
                                break;
                            }
                        }
                    }
                }
            }
            Console.Clear();
        }

        public static void ClearFile(string arr)
        {
            string str = null;
            StreamWriter sw1 = new StreamWriter($"{arr}.txt");
            sw1.Write(str);
            sw1.Close();
        }

    }
}
