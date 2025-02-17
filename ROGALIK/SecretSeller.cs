using System;
using ROGALIK;
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
using static System.Net.Mime.MediaTypeNames;

namespace ROGALIK
{
    class SecretSeller
    {

        public bool haveSeller = false;
        private int sellerCoordX;
        private int sellerCoordY;
        public void GenerateSeller(ref char[,] map1)
        {
            if (haveSeller == false)
            {
                Random rand = new Random();
                while (true)
                {
                    sellerCoordX = rand.Next(0, map1.GetLength(0));
                    sellerCoordY = rand.Next(0, map1.GetLength(1));
                    if (map1[sellerCoordX, sellerCoordY] == ' ')
                    {
                        map1[sellerCoordX, sellerCoordY] = '#';
                        haveSeller = true;
                        break;
                    }
                }
            }
        }

        public void OpenSeller(int userX, int userY, ref Player player)
        {
            for (int i = sellerCoordX - 1; i <= (sellerCoordX + 1); i++)
            {
                for (int j = sellerCoordY - 1; j <= (sellerCoordY + 1); j++)
                {
                    if (userX == i && userY == j)
                    {
                        SecretSeller.SecretShopWindow(ref player);
                        break;
                    }
                }
            }
        }

        public static int SecretShopWindow(ref Player player)
        {
            Console.Clear();
            List<string> ShopMove = new List<string>()
            {
                "Купить",
                "Продать"
            };


            string[] temp = File.ReadAllLines("SecretSeller/AllItems.txt");
            int count = 2;
            string[] arrayItemsNames = new string[temp.Length / 6];
            for (int i = 0; i < temp.Length / 6; i++)
            {
                arrayItemsNames[i] = temp[count];
                count += 6;
            }

            string[] ItemsNames = new string[15];

            Random rand = new Random();
            int num = 0;
            int[] arr = new int[15];
            bool is_here = false;
            int count1 = 0;

            while (count1 < arr.Length)
            {
                num = rand.Next(0, temp.Length / 6);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (num == arr[i])
                    {
                        is_here = true;
                        break;
                    }
                }
                if (is_here == false)
                {
                    arr[count1] = num;
                    count1++;
                }
                is_here = false;


            }

            for (int i = 0; i < arr.Length; i++)
            {
                ItemsNames[i] = arrayItemsNames[arr[i]];
            }

            string[] im = File.ReadAllLines("SecretSeller/SecretInventory.txt");

            if (im.Length != 0)
            {
                StreamWriter fs = new StreamWriter("SecretSeller/SecretInventory.txt", false);
                fs.Close();
            }

            for (int i = 0; i < ItemsNames.Length; i++)
            {
                for (int j = 0; j < temp.Length; j++)
                {
                    if (temp[j] == ItemsNames[i])
                    {
                        Item item = new Item(temp[j - 2], temp[j - 1], temp[j], temp[j + 1], temp[j + 2], Convert.ToString(rand.Next(1, 11)));
                        Item.PutToSecretInventory(item);
                        break;
                    }
                }
            }

            im = File.ReadAllLines("SecretSeller/left.txt");
            Graphics.DrawImage(im, true, 57, 13);

            im = File.ReadAllLines("SecretSeller/welcome.txt");
            Graphics.DrawImage(im, true, 35, 2);


            Console.ForegroundColor = ConsoleColor.DarkCyan;
            im = File.ReadAllLines("SecretSeller/buy.txt");
            Graphics.DrawImage(im, true, 15, 23);
            Console.ResetColor();

            im = File.ReadAllLines("SecretSeller/sell.txt");
            Graphics.DrawImage(im, true, 150, 23);

            Graphics.TextWriterForAnimation("Выйти", 190, 45, true, 40);
            int index = 0;


            while (true)
            {
                ConsoleKeyInfo ckey = Console.ReadKey();

                if (ckey.Key == ConsoleKey.LeftArrow)
                {
                    Console.Beep(500, 10);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    im = File.ReadAllLines("SecretSeller/buy.txt");
                    Graphics.DrawImage(im, true, 15, 23);
                    Console.ResetColor();

                    im = File.ReadAllLines("SecretSeller/sell.txt");
                    Graphics.DrawImage(im, true, 150, 23);

                    im = File.ReadAllLines("SecretSeller/left.txt");
                    Graphics.DrawImage(im, true, 57, 13);

                    Graphics.TextWriterForAnimation("Выйти", 190, 45, true, 40);

                    index = 0;

                }
                else if (ckey.Key == ConsoleKey.RightArrow || ckey.Key == ConsoleKey.UpArrow)
                {
                    Console.Beep(500, 10);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    im = File.ReadAllLines("SecretSeller/sell.txt");
                    Graphics.DrawImage(im, true, 150, 23);
                    Console.ResetColor();

                    im = File.ReadAllLines("SecretSeller/buy.txt");
                    Graphics.DrawImage(im, true, 15, 23);

                    im = File.ReadAllLines("SecretSeller/right.txt");
                    Graphics.DrawImage(im, true, 57, 13);

                    Graphics.TextWriterForAnimation("Выйти", 190, 45, true, 40);

                    index = 1;


                }
                else if (ckey.Key == ConsoleKey.DownArrow)
                {
                    Console.Beep(500, 10);
                    im = File.ReadAllLines("SecretSeller/sell.txt");
                    Graphics.DrawImage(im, true, 150, 23);

                    im = File.ReadAllLines("SecretSeller/buy.txt");
                    Graphics.DrawImage(im, true, 15, 23);

                    im = File.ReadAllLines("SecretSeller/exit.txt");
                    Graphics.DrawImage(im, true, 57, 13);

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Graphics.TextWriterForAnimation("Выйти", 190, 45, true, 40);
                    Console.ResetColor();

                    index = 2;


                }
                else if (ckey.Key == ConsoleKey.Enter)
                {
                    if (index == 0)
                    {
                        Item SelectedMenuItem = new Item();

                        int index_inventory = 0;
                        int selectedY = 0;
                        int count_endl = 1;
                        bool is_exist = true;

                        while (true)
                        {

                            int exit = 1;


                            List<Item> inventory = new List<Item>();
                            List<Item> secret_inventory = new List<Item>();

                            string[] file = File.ReadAllLines("SecretSeller/SecretInventory.txt");


                            for (int i = 0; i < file.Length; i += 6)
                            {
                                if (Convert.ToInt32(file[i + 5]) == 1)
                                {
                                    inventory.Add(new Item(file[i], file[i + 1], file[i + 2], file[i + 3], file[i + 4], file[i + 5]));
                                    secret_inventory.Add(new Item(file[i], file[i + 1], file[i + 2], file[i + 3], file[i + 4], "1"));

                                }
                                else
                                {
                                    inventory.Add(new Item(file[i], file[i + 1], file[i + 2] + $"({file[i + 5]})", file[i + 3], file[i + 4], file[i + 5]));
                                    secret_inventory.Add(new Item(file[i], file[i + 1], file[i + 2], file[i + 3], file[i + 4], "1"));

                                }

                            }


                            Console.Clear();
                            //Player.PlayerInformation(player, 150, 19);
                            Graphics.PrintSelectedText("\n\n\n\n              Ваши предметы: ", 0, 0, ConsoleColor.DarkCyan);
                            Graphics.PrintSelectedText("Характеристики: ", 162, 15, ConsoleColor.DarkCyan);

                            im = File.ReadAllLines("SecretSeller/price_buy.txt");
                            int price = 0;

                            for (int i = 0; i < im.Length; i++)
                            {
                                if (im[i] == inventory[index_inventory].Name || im[i] + $"({inventory[index_inventory].CountThings})" == inventory[index_inventory].Name)
                                {
                                    price = Convert.ToInt32(im[i + 1]);
                                    break;
                                }
                            }


                            if (inventory[index_inventory].Kind == "Bib")
                            {
                                Graphics.PrintText($"Защита: {inventory[index_inventory].Property1} Def", 150, 19);
                                Graphics.PrintText($"Цена: {price} Coins", 150, 21);
                            }
                            else if (inventory[index_inventory].Kind == "Helm")
                            {

                                Graphics.PrintText($"Защита: {inventory[index_inventory].Property1} Def", 150, 19);
                                Graphics.PrintText($"Радиус видимости: {inventory[index_inventory].Property2} RadD", 150, 21);
                                Graphics.PrintText($"Цена: {price} Coins", 150, 23);

                            }
                            else if (inventory[index_inventory].Kind == "Weapon")
                            {
                                Graphics.PrintText($"Урон: {inventory[index_inventory].Property1} Dmg", 150, 19);
                                Graphics.PrintText($"Радиус атаки: {inventory[index_inventory].Property2} RadD", 150, 21);
                                Graphics.PrintText($"Цена: {price} Coins", 150, 23);

                            }
                            else if (inventory[index_inventory].Kind == "PotionH")
                            {
                                Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Hp", 150, 19);
                                Graphics.PrintText($"Цена: {price} Coins", 150, 21);

                            }
                            else if ((inventory[index_inventory].Kind == "PotionD"))
                            {
                                Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Dmg", 150, 19);
                                Graphics.PrintText($"Количество ходов: +{inventory[index_inventory].Property2} Dmg", 150, 21);
                                Graphics.PrintText($"Цена: {price} Coins", 150, 23);


                            }
                            else if ((inventory[index_inventory].Kind == "PotionDef"))
                            {
                                Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Def", 150, 19);
                                Graphics.PrintText($"Количество ходов: +{inventory[index_inventory].Property2} Dmg", 150, 21);
                                Graphics.PrintText($"Цена: {price} Coins", 150, 23);



                            }
                            else if ((inventory[index_inventory].Kind == "PotionR"))
                            {
                                Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Rad", 150, 19);
                                Graphics.PrintText($"Количество ходов: +{inventory[index_inventory].Property2} Dmg", 150, 21);
                                Graphics.PrintText($"Цена: {price} Coins", 150, 23);

                            }

                            Graphics.PrintText($"Ваш баланс: {player.Money} Coins", 150, 43);


                            if (index_inventory == inventory.Count && is_exist == false)
                                index_inventory--;
                            SelectedMenuItem = Graphics.DrawMenu(inventory, 5, 7, count_endl, ref index_inventory, ref selectedY);

                            if (SelectedMenuItem.Kind == "Escape")
                            {
                                Console.Clear();
                                im = File.ReadAllLines("SecretSeller/left.txt");
                                Graphics.DrawImage(im, true, 57, 13);

                                im = File.ReadAllLines("SecretSeller/welcome.txt");
                                Graphics.DrawImage(im, true, 35, 2);


                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                im = File.ReadAllLines("SecretSeller/buy.txt");
                                Graphics.DrawImage(im, true, 15, 23);
                                Console.ResetColor();

                                im = File.ReadAllLines("SecretSeller/sell.txt");
                                Graphics.DrawImage(im, true, 150, 23);

                                Graphics.TextWriterForAnimation("Выйти", 190, 45, true, 40);
                                index = 0;
                                index_inventory = 0;
                                break;
                            }
                            else
                            {
                                while (exit != -1)
                                {
                                    if (SelectedMenuItem.Kind != "" && SelectedMenuItem.Kind != "Escape")
                                    {
                                        if (player.Money >= price)
                                        {
                                            im = File.ReadAllLines("SecretSeller/empty.txt");
                                            Graphics.DrawImage(im, true, 57, 4);
                                            Graphics.DrawImage(im, true, 120, 4);

                                            im = File.ReadAllLines("SecretSeller/sure.txt");
                                            Graphics.DrawImage(im, true, 135, 4);

                                            im = File.ReadAllLines("SecretSeller/4.txt");
                                            Graphics.DrawImage(im, true, 45, 8);

                                            im = File.ReadAllLines("SecretSeller/no.txt");
                                            Graphics.DrawImage(im, true, 168, 22);

                                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                                            im = File.ReadAllLines("SecretSeller/yes.txt");
                                            Graphics.DrawImage(im, true, 142, 22);
                                            Console.ResetColor();

                                            int index_exit = 0;

                                            while (index_exit != -1)
                                            {
                                                ckey = Console.ReadKey();
                                                if (ckey.Key == ConsoleKey.LeftArrow)
                                                {
                                                    Console.Beep(500, 10);

                                                    im = File.ReadAllLines("SecretSeller/empty.txt");
                                                    Graphics.DrawImage(im, true, 57, 4);
                                                    Graphics.DrawImage(im, true, 120, 4);

                                                    im = File.ReadAllLines("SecretSeller/sure.txt");
                                                    Graphics.DrawImage(im, true, 135, 4);

                                                    im = File.ReadAllLines("SecretSeller/4.txt");
                                                    Graphics.DrawImage(im, true, 45, 8);

                                                    im = File.ReadAllLines("SecretSeller/no.txt");
                                                    Graphics.DrawImage(im, true, 168, 22);

                                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                    im = File.ReadAllLines("SecretSeller/yes.txt");
                                                    Graphics.DrawImage(im, true, 142, 22);
                                                    Console.ResetColor();

                                                    index_exit = 0;

                                                }
                                                else if (ckey.Key == ConsoleKey.RightArrow)
                                                {
                                                    Console.Beep(500, 10);

                                                    im = File.ReadAllLines("SecretSeller/empty.txt");
                                                    Graphics.DrawImage(im, true, 57, 4);
                                                    Graphics.DrawImage(im, true, 120, 4);

                                                    im = File.ReadAllLines("SecretSeller/sure.txt");
                                                    Graphics.DrawImage(im, true, 135, 4);

                                                    im = File.ReadAllLines("SecretSeller/exit.txt");
                                                    Graphics.DrawImage(im, true, 45, 8);

                                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                    im = File.ReadAllLines("SecretSeller/no.txt");
                                                    Graphics.DrawImage(im, true, 168, 22);
                                                    Console.ResetColor();

                                                    im = File.ReadAllLines("SecretSeller/yes.txt");
                                                    Graphics.DrawImage(im, true, 142, 22);

                                                    index_exit = 1;

                                                }
                                                else if (ckey.Key == ConsoleKey.Enter)
                                                {
                                                    if (index_exit == 0)
                                                    {
                                                        is_exist = Item.Drop(false, "SecretSeller/SecretInventory.txt", SelectedMenuItem);
                                                        Item.PutToInventory(secret_inventory[index_inventory]);
                                                        Graphics.PrintIvents($"Вы приобрели {secret_inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                                        Thread.Sleep(500);

                                                        SoundPlayer soundPlayer = new SoundPlayer("SecretSeller/coins.wav");
                                                        soundPlayer.Play();

                                                        im = File.ReadAllLines("SecretSeller/empty.txt");
                                                        Graphics.DrawImage(im, true, 57, 4);
                                                        Graphics.DrawImage(im, true, 120, 4);

                                                        im = File.ReadAllLines("SecretSeller/smoke.txt");
                                                        Graphics.DrawImage(im, true, 45, 8);


                                                        im = File.ReadAllLines("SecretSeller/success.txt");
                                                        Graphics.DrawImage(im, true, 147, 4);

                                                        Console.ReadKey();

                                                        player.Money -= price;
                                                        index_exit = -1;
                                                        exit = -1;
                                                        break;


                                                    }
                                                    else if (index_exit == 1)
                                                    {
                                                        index_exit = -1;
                                                        exit = -1;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            im = File.ReadAllLines("secretseller/empty.txt");
                                            Graphics.DrawImage(im, true, 57, 4);
                                            Graphics.DrawImage(im, true, 120, 4);

                                            im = File.ReadAllLines("secretseller/no_money.txt");
                                            Graphics.DrawImage(im, true, 130, 36);

                                            im = File.ReadAllLines("secretseller/exit.txt");
                                            Graphics.DrawImage(im, true, 45, 8);

                                            Console.ReadKey();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            }

                        }
                    }
                    else if (index == 1)
                    {
                        Item SelectedMenuItem = new Item();

                        int index_inventory = 0;
                        int selectedY = 0;
                        int count_endl = 1;
                        bool is_exist = true;

                        while (true)
                        {

                            int exit = 1;




                            List<Item> inventory = new List<Item>();
                            List<Item> secret_inventory = new List<Item>();

                            string[] file = File.ReadAllLines("inventory.txt");
                            if (file.Length == 0)
                            {
                                Console.Clear();
                                Graphics.PrintText("У вас ничего нет!!!", 90, 20);
                                Console.ReadKey();
                                break;

                            }
                            else
                            {

                                for (int i = 0; i < file.Length; i += 6)
                                {
                                    if (Convert.ToInt32(file[i + 5]) == 1)
                                    {
                                        inventory.Add(new Item(file[i], file[i + 1], file[i + 2], file[i + 3], file[i + 4], file[i + 5]));
                                        secret_inventory.Add(new Item(file[i], file[i + 1], file[i + 2], file[i + 3], file[i + 4], "1"));

                                    }
                                    else
                                    {
                                        inventory.Add(new Item(file[i], file[i + 1], file[i + 2] + $"({file[i + 5]})", file[i + 3], file[i + 4], file[i + 5]));
                                        secret_inventory.Add(new Item(file[i], file[i + 1], file[i + 2], file[i + 3], file[i + 4], "1"));

                                    }

                                }


                                Console.Clear();

                                Console.SetCursorPosition(70, 47);
                                Console.WriteLine(index_inventory);

                                //Player.PlayerInformation(player, 150, 19);
                                Graphics.PrintSelectedText("\n\n\n\n              Ваши предметы: ", 0, 0, ConsoleColor.DarkCyan);
                                Graphics.PrintSelectedText("Характеристики: ", 162, 15, ConsoleColor.DarkCyan);

                                im = File.ReadAllLines("SecretSeller/price_sell.txt");
                                int price = 0;

                                if (index_inventory == inventory.Count && is_exist == false)
                                    index_inventory--;


                                for (int i = 0; i < im.Length; i++)
                                {
                                    if (im[i] == inventory[index_inventory].Name || im[i] + $"({inventory[index_inventory].CountThings})" == inventory[index_inventory].Name)
                                    {
                                        price = Convert.ToInt32(im[i + 1]);
                                        break;
                                    }
                                }





                                if (inventory[index_inventory].Kind == "Bib")
                                {
                                    Graphics.PrintText($"Защита: {inventory[index_inventory].Property1} Def", 150, 19);
                                    Graphics.PrintText($"Цена: {price} Coins", 150, 21);
                                }
                                else if (inventory[index_inventory].Kind == "Helm")
                                {

                                    Graphics.PrintText($"Защита: {inventory[index_inventory].Property1} Def", 150, 19);
                                    Graphics.PrintText($"Радиус видимости: {inventory[index_inventory].Property2} RadD", 150, 21);
                                    Graphics.PrintText($"Цена: {price} Coins", 150, 23);

                                }
                                else if (inventory[index_inventory].Kind == "Weapon")
                                {
                                    Graphics.PrintText($"Урон: {inventory[index_inventory].Property1} Dmg", 150, 19);
                                    Graphics.PrintText($"Радиус атаки: {inventory[index_inventory].Property2} RadD", 150, 21);
                                    Graphics.PrintText($"Цена: {price} Coins", 150, 23);

                                }
                                else if (inventory[index_inventory].Kind == "PotionH")
                                {
                                    Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Hp", 150, 19);
                                    Graphics.PrintText($"Цена: {price} Coins", 150, 21);

                                }
                                else if ((inventory[index_inventory].Kind == "PotionD"))
                                {
                                    Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Dmg", 150, 19);
                                    Graphics.PrintText($"Количество ходов: +{inventory[index_inventory].Property2} Dmg", 150, 21);
                                    Graphics.PrintText($"Цена: {price} Coins", 150, 23);


                                }
                                else if ((inventory[index_inventory].Kind == "PotionDef"))
                                {
                                    Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Def", 150, 19);
                                    Graphics.PrintText($"Количество ходов: +{inventory[index_inventory].Property2} Dmg", 150, 21);
                                    Graphics.PrintText($"Цена: {price} Coins", 150, 23);



                                }
                                else if ((inventory[index_inventory].Kind == "PotionR"))
                                {
                                    Graphics.PrintText($"Баф: +{inventory[index_inventory].Property1} Rad", 150, 19);
                                    Graphics.PrintText($"Количество ходов: +{inventory[index_inventory].Property2} Dmg", 150, 21);
                                    Graphics.PrintText($"Цена: {price} Coins", 150, 23);

                                }

                                Graphics.PrintText($"Ваш баланс: {player.Money} Coins", 150, 43);




                                //Graphics.PrintText($"Защита: {inventory[index_inventory].Property1} Def", 150, 19);
                                //Graphics.PrintText($"Урон: {player.OwnDamage} Dmg", x, y + 2);
                                //Graphics.PrintText($"Радиус атаки: {player.RadDamage} RadD", x, y + 4);
                                //Graphics.PrintText($"Радиус видимости: {player.RadVision} RadD", x, y + 6);



                                //if (index_inventory < 0)
                                //    index_inventory = 0;

                                SelectedMenuItem = Graphics.DrawMenu(inventory, 5, 7, count_endl, ref index_inventory, ref selectedY);

                                if (SelectedMenuItem.Kind == "Escape")
                                {
                                    Console.Clear();
                                    im = File.ReadAllLines("SecretSeller/left.txt");
                                    Graphics.DrawImage(im, true, 57, 13);

                                    im = File.ReadAllLines("SecretSeller/welcome.txt");
                                    Graphics.DrawImage(im, true, 35, 2);


                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    im = File.ReadAllLines("SecretSeller/buy.txt");
                                    Graphics.DrawImage(im, true, 15, 23);
                                    Console.ResetColor();

                                    im = File.ReadAllLines("SecretSeller/sell.txt");
                                    Graphics.DrawImage(im, true, 150, 23);

                                    Graphics.TextWriterForAnimation("Выйти", 190, 45, true, 40);
                                    index = 0;
                                    index_inventory = 0;
                                    break;
                                }
                                else
                                {
                                    while (exit != -1)
                                    {
                                        if (SelectedMenuItem.Kind != "" && SelectedMenuItem.Kind != "Escape")
                                        {

                                            string[] jm = File.ReadAllLines("SecretSeller/4.txt");

                                            if (secret_inventory[index_inventory].Name == "Камень" || secret_inventory[index_inventory].Name == "Палка" || secret_inventory[index_inventory].Name == "Венок из одуванчиков" || secret_inventory[index_inventory].Name == "Ведро")
                                            {
                                                Graphics.PrintText("ЧТО ЗА ПРЕЛЕСТЬ!!! ПРОДАЙ!!!", 100, 44);
                                                jm = File.ReadAllLines("SecretSeller/2.txt");
                                            }
                                            else
                                                jm = File.ReadAllLines("SecretSeller/4.txt");

                                            im = File.ReadAllLines("SecretSeller/empty.txt");
                                            Graphics.DrawImage(im, true, 57, 4);
                                            Graphics.DrawImage(im, true, 120, 4);

                                            im = File.ReadAllLines("SecretSeller/sure.txt");
                                            Graphics.DrawImage(im, true, 135, 4);

                                            Graphics.DrawImage(jm, true, 45, 8);

                                            im = File.ReadAllLines("SecretSeller/no.txt");
                                            Graphics.DrawImage(im, true, 168, 22);

                                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                                            im = File.ReadAllLines("SecretSeller/yes.txt");
                                            Graphics.DrawImage(im, true, 142, 22);
                                            Console.ResetColor();

                                            int index_exit = 0;

                                            while (index_exit != -1)
                                            {
                                                ckey = Console.ReadKey();
                                                if (ckey.Key == ConsoleKey.LeftArrow)
                                                {
                                                    Console.Beep(500, 10);


                                                    if (secret_inventory[index_inventory].Name == "Камень" || secret_inventory[index_inventory].Name == "Палка" || secret_inventory[index_inventory].Name == "Венок из одуванчиков" || secret_inventory[index_inventory].Name == "Ведро")
                                                    {
                                                        jm = File.ReadAllLines("SecretSeller/2.txt");
                                                        Graphics.PrintText("ЧТО ЗА ПРЕЛЕСТЬ!!! ПРОДАЙ!!!", 100, 44);
                                                    }
                                                    else
                                                        jm = File.ReadAllLines("SecretSeller/4.txt");

                                                    im = File.ReadAllLines("SecretSeller/empty.txt");
                                                    Graphics.DrawImage(im, true, 57, 4);
                                                    Graphics.DrawImage(im, true, 120, 4);

                                                    im = File.ReadAllLines("SecretSeller/sure.txt");
                                                    Graphics.DrawImage(im, true, 135, 4);

                                                    //im = File.ReadAllLines("SecretSeller/4.txt");
                                                    Graphics.DrawImage(jm, true, 45, 8);



                                                    im = File.ReadAllLines("SecretSeller/no.txt");
                                                    Graphics.DrawImage(im, true, 168, 22);

                                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                    im = File.ReadAllLines("SecretSeller/yes.txt");
                                                    Graphics.DrawImage(im, true, 142, 22);
                                                    Console.ResetColor();

                                                    index_exit = 0;

                                                }
                                                else if (ckey.Key == ConsoleKey.RightArrow)
                                                {
                                                    Console.Beep(500, 10);


                                                    im = File.ReadAllLines("SecretSeller/empty.txt");
                                                    Graphics.DrawImage(im, true, 57, 4);
                                                    Graphics.DrawImage(im, true, 120, 4);

                                                    im = File.ReadAllLines("SecretSeller/sure.txt");
                                                    Graphics.DrawImage(im, true, 135, 4);

                                                    im = File.ReadAllLines("SecretSeller/exit.txt");
                                                    Graphics.DrawImage(im, true, 45, 8);

                                                    if (secret_inventory[index_inventory].Name == "Камень" || secret_inventory[index_inventory].Name == "Палка" || secret_inventory[index_inventory].Name == "Венок из одуванчиков" || secret_inventory[index_inventory].Name == "Ведро")
                                                        Graphics.PrintText("ну пж... продай...          ", 100, 44);

                                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                    im = File.ReadAllLines("SecretSeller/no.txt");
                                                    Graphics.DrawImage(im, true, 168, 22);
                                                    Console.ResetColor();

                                                    im = File.ReadAllLines("SecretSeller/yes.txt");
                                                    Graphics.DrawImage(im, true, 142, 22);

                                                    index_exit = 1;

                                                }
                                                else if (ckey.Key == ConsoleKey.Enter)
                                                {
                                                    if (index_exit == 0)
                                                    {
                                                        is_exist = Item.Drop(false, "inventory.txt", SelectedMenuItem);
                                                        Graphics.PrintIvents($"Вы продали {secret_inventory[index_inventory].Name}", ConsoleColor.DarkCyan);
                                                        Thread.Sleep(500);

                                                        if (inventory[index_inventory].Kind == "Helm" || inventory[index_inventory].Kind == "Bib" || inventory[index_inventory].Kind == "Weapon")
                                                            Item.Drop(false, $"Equipment/equipped_{SelectedMenuItem.Kind}.txt", SelectedMenuItem);

                                                        SoundPlayer soundPlayer = new SoundPlayer("SecretSeller/coins.wav");
                                                        soundPlayer.Play();



                                                        im = File.ReadAllLines("SecretSeller/empty.txt");
                                                        Graphics.DrawImage(im, true, 57, 4);
                                                        Graphics.DrawImage(im, true, 120, 4);

                                                        if (secret_inventory[index_inventory].Name == "Камень" || secret_inventory[index_inventory].Name == "Палка" || secret_inventory[index_inventory].Name == "Венок из одуванчиков" || secret_inventory[index_inventory].Name == "Ведро")
                                                        {
                                                            jm = File.ReadAllLines("SecretSeller/3.txt");
                                                            im = File.ReadAllLines("SecretSeller/thank.txt");
                                                            Graphics.DrawImage(im, true, 140, 4);


                                                        }
                                                        else
                                                        {
                                                            jm = File.ReadAllLines("SecretSeller/smoke.txt");
                                                            im = File.ReadAllLines("SecretSeller/success.txt");
                                                            Graphics.DrawImage(im, true, 147, 4);


                                                        }
                                                        Graphics.DrawImage(jm, true, 45, 8);

                                                        Console.ReadKey();

                                                        player.Money += price;
                                                        index_exit = -1;
                                                        exit = -1;
                                                        break;

                                                    }
                                                    else if (index_exit == 1)
                                                    {
                                                        index_exit = -1;
                                                        exit = -1;
                                                    }
                                                }
                                            }

                                            //Console.ReadKey();
                                            //break;

                                        }
                                        else
                                        {
                                            break;
                                        }

                                        //if  (Se)
                                        //{
                                        //    index_inventory_move = 0;
                                        //    Item.Equip(SelectedMenuItem);
                                        //    break;
                                        //}
                                        //else if (SelectedMenuMove == " Выбросить")
                                        //{
                                        //    index_inventory_move = 0;
                                        //    is_exist = Item.Drop(false, "inventory.txt", SelectedMenuItem);
                                        //    Item.Drop(false, $"Equipment/equipped_{SelectedMenuItem.Kind}.txt", SelectedMenuItem);
                                        //    break;
                                        //}
                                        //else if (SelectedMenuMove == "Выбросить все")
                                        //{
                                        //    index_inventory_move = 0;
                                        //    is_exist = Item.Drop(true, "inventory.txt", SelectedMenuItem);
                                        //    Item.Drop(true, $"Equipment/equipped_{SelectedMenuItem.Kind}.txt", SelectedMenuItem);

                                        //    break;
                                        //}
                                        //else if (SelectedMenuMove == "Снять с себя")
                                        //{
                                        //    index_inventory_move = 0;
                                        //    Item.Drop(true, $"Equipment/equipped_{SelectedMenuItem.Kind}.txt", SelectedMenuItem);

                                        //    break;
                                        //}
                                        //else if (SelectedMenuMove == "Escape")
                                        //{
                                        //    index_inventory_move = 0;
                                        //    SelectedMenuMove = "";
                                        //    break;
                                        //}
                                    }
                                }





                            }

                        }
                    }
                    else if (index == 2)
                    {
                        Console.Clear();

                        im = File.ReadAllLines("SecretSeller/viiti.txt");
                        Graphics.DrawImage(im, true, 80, 2);

                        im = File.ReadAllLines("SecretSeller/no.txt");
                        Graphics.DrawImage(im, true, 155, 23);

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        im = File.ReadAllLines("SecretSeller/yes.txt");
                        Graphics.DrawImage(im, true, 25, 23);
                        Console.ResetColor();

                        im = File.ReadAllLines("SecretSeller/2.txt");
                        Graphics.DrawImage(im, true, 57, 13);


                        int index_exit = 0;

                        while (index_exit != -1)
                        {
                            ckey = Console.ReadKey();
                            if (ckey.Key == ConsoleKey.LeftArrow)
                            {
                                Console.Beep(500, 10);

                                im = File.ReadAllLines("SecretSeller/viiti.txt");
                                Graphics.DrawImage(im, true, 80, 2);

                                im = File.ReadAllLines("SecretSeller/no.txt");
                                Graphics.DrawImage(im, true, 155, 23);

                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                im = File.ReadAllLines("SecretSeller/yes.txt");
                                Graphics.DrawImage(im, true, 25, 23);
                                Console.ResetColor();

                                im = File.ReadAllLines("SecretSeller/2.txt");
                                Graphics.DrawImage(im, true, 57, 13);

                                index_exit = 0;

                            }
                            else if (ckey.Key == ConsoleKey.RightArrow)
                            {
                                Console.Beep(500, 10);

                                im = File.ReadAllLines("SecretSeller/viiti.txt");
                                Graphics.DrawImage(im, true, 80, 2);

                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                im = File.ReadAllLines("SecretSeller/no.txt");
                                Graphics.DrawImage(im, true, 155, 23);
                                Console.ResetColor();

                                im = File.ReadAllLines("SecretSeller/yes.txt");
                                Graphics.DrawImage(im, true, 25, 23);

                                im = File.ReadAllLines("SecretSeller/smoke_right.txt");
                                Graphics.DrawImage(im, true, 57, 13);

                                index_exit = 1;

                            }
                            else if (ckey.Key == ConsoleKey.Enter)
                            {
                                if (index_exit == 0)
                                {
                                    Console.Clear();
                                    return 0;
                                }
                                else if (index_exit == 1)
                                {
                                    Console.Clear();

                                    im = File.ReadAllLines("SecretSeller/left.txt");
                                    Graphics.DrawImage(im, true, 57, 13);

                                    im = File.ReadAllLines("SecretSeller/welcome.txt");
                                    Graphics.DrawImage(im, true, 35, 2);


                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    im = File.ReadAllLines("SecretSeller/buy.txt");
                                    Graphics.DrawImage(im, true, 15, 23);
                                    Console.ResetColor();

                                    im = File.ReadAllLines("SecretSeller/sell.txt");
                                    Graphics.DrawImage(im, true, 150, 23);

                                    Graphics.TextWriterForAnimation("Выйти", 190, 45, true, 40);

                                    index_exit = -1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
