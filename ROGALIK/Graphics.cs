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
    internal class Graphics
    {
        //public static string DrawList(List<string> list, int x, int y, int count_endl, ref int index)
        //{

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        if (i == index)
        //        {
        //            Console.SetCursorPosition(x, y + i * (count_endl + 1));
        //            Console.ForegroundColor = ConsoleColor.DarkCyan;
        //            Console.WriteLine(list[i]);
        //        }
        //        else
        //        {
        //            Console.SetCursorPosition(x, y + i * (count_endl + 1));
        //            Console.WriteLine(list[i]);
        //        }
        //        Console.ResetColor();
        //    }

        //    ConsoleKeyInfo ckey = Console.ReadKey();

        //    if (ckey.Key == ConsoleKey.DownArrow)
        //    {
        //        Console.Beep(500, 10);
        //        if (index == list.Count - 1) { }
        //        else
        //            index++;
        //    }
        //    else if (ckey.Key == ConsoleKey.UpArrow)
        //    {
        //        Console.Beep(500, 10);

        //        if (index <= 0) { }
        //        else
        //            index--;
        //    }
        //    else if (ckey.Key == ConsoleKey.Escape || ckey.Key == ConsoleKey.LeftArrow)
        //    {
        //        return "Escape";
        //    }
        //    else if (ckey.Key == ConsoleKey.Enter)
        //        return list[index];
        //    else
        //        return "";



        //    return "";

        //}

        public static void DrawImage(string[] image, bool is_instantly, int x, int y, int timeSleep)
        {
            int time = 0;
            if (timeSleep <= 50)
                time = 150;
            else
                time = timeSleep;

            for (int i = 0; i < image.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.WriteLine(image[i]);

                Random rand = new Random();
                if (!is_instantly)
                    Thread.Sleep(rand.Next(time - 200, time));
            }
        }
        public static void DrawImage(string[] image, bool is_instantly, int x, int y)
        {

            for (int i = 0; i < image.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.WriteLine(image[i]);

                Random rand = new Random();
                if (!is_instantly)
                    Thread.Sleep(rand.Next(50, 150));
            }
        }

        public static string DrawOutcast(List<string> items, ref int index)
        {
            Console.Clear();

            string[] file = File.ReadAllLines("Outcast/outcast_0.txt");
            DrawImage(file, true, 0, 1);



            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                {
                    Console.SetCursorPosition(95, 23 + i);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(items[i]);
                }
                else
                {
                    Console.SetCursorPosition(95, 23 + i);
                    Console.WriteLine(items[i]);
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo ckey = Console.ReadKey();

            if (ckey.Key == ConsoleKey.DownArrow)
            {
                Console.Beep(500, 10);
                if (index == items.Count - 1) { }
                else
                    index++;
            }
            else if (ckey.Key == ConsoleKey.UpArrow)
            {
                Console.Beep(500, 10);

                if (index <= 0) { }
                else
                    index--;
            }
            else if (ckey.Key == ConsoleKey.Enter)
                return items[index];
            else
                return "";



            return "";

        }

        public static string DrawMenu(List<string> list, int x, int y, int count_endl, ref int index)
        {

            for (int i = 0; i < list.Count; i++)
            {
                if (i == index)
                {
                    Console.SetCursorPosition(x, y + i * (count_endl + 1));
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(list[i]);
                }
                else
                {
                    Console.SetCursorPosition(x, y + i * (count_endl + 1));
                    Console.WriteLine(list[i]);
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo ckey = Console.ReadKey();

            if (ckey.Key == ConsoleKey.DownArrow)
            {
                Console.Beep(500, 10);
                if (index == list.Count - 1) { }
                else
                    index++;
            }
            else if (ckey.Key == ConsoleKey.UpArrow)
            {
                Console.Beep(500, 10);

                if (index <= 0) { }
                else
                    index--;
            }
            else if (ckey.Key == ConsoleKey.Escape || ckey.Key == ConsoleKey.LeftArrow)
            {
                return "Escape";
            }
            else if (ckey.Key == ConsoleKey.Enter)
                return list[index];
            else
                return "";



            return "";

        }

        public static bool DrawMenu()
        {
            int index = 0;
            bool is_exit;
            List<string> menu = new List<string>()
            {
                "  Продолжить",
                "Вернуться в меню"

            };

            string Choice;
            Console.Clear();
            while (true)
            {
                Choice = DrawMenu(menu, 90, 23, 1, ref index);
                if (Choice == "  Продолжить")
                {
                    index = 0;
                    is_exit = false;
                    break;

                }
                else if (Choice == "Вернуться в меню")
                {
                    index = 0;
                    is_exit = true;
                    break;
                }

            }
            return is_exit;
        }

        public static Item DrawMenu(List<Item> inventory, int x, int y, int count_endl, ref int index, ref int selectedY)
        {
            int stop = 0;
            if (inventory.Count > 18)
                stop = 18;
            else
                stop = inventory.Count;


            Item item = new Item();
            item.Kind = "";

            string[] image = new string[100];
            string cnt;

            if (Convert.ToInt32(inventory[index].CountThings) == 1)
                cnt = "";
            else
                cnt = $"({inventory[index].CountThings})";



            if (inventory[index].Name == $"Обычная одежда{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_0.txt");

            else if (inventory[index].Name == $"Кожаная броня{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_1.txt");

            else if (inventory[index].Name == $"Стальная броня{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_2.txt");

            else if (inventory[index].Name == $"Эбонитовая броня{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_3.txt");

            else if (inventory[index].Name == $"Броня берсерка{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_4.txt");

            else if (inventory[index].Name == $"Венок из одуванчиков{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_5.txt");

            else if (inventory[index].Name == $"Бандана{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_6.txt");

            else if (inventory[index].Name == $"Сыромятный шлем{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_7.txt");

            else if (inventory[index].Name == $"Стальной шлем{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_8.txt");

            else if (inventory[index].Name == $"Эбонитовый шлем{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_9.txt");

            else if (inventory[index].Name == $"Ведро{cnt}")
                image = File.ReadAllLines("Items_images/armor_images/armor_image_10.txt");



            else if (inventory[index].Name == $"Камень{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_0.txt");

            else if (inventory[index].Name == $"Палка{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_1.txt");

            else if (inventory[index].Name == $"Обычный меч{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_2.txt");

            else if (inventory[index].Name == $"Кинжал{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_3.txt");

            else if (inventory[index].Name == $"Посох{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_4.txt");

            else if (inventory[index].Name == $"Копье{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_5.txt");

            else if (inventory[index].Name == $"Катана{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_6.txt");

            else if (inventory[index].Name == $"Убийца драконов{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_7.txt");

            else if (inventory[index].Name == $"Зангэцу{cnt}")
                image = File.ReadAllLines("Items_images/weapon_images/weapon_image_8.txt");




            else if (inventory[index].Name == $"Малое зелье здоровья{cnt}")
                image = File.ReadAllLines("Items_images/potion_images/potion_image_0.txt");

            else if (inventory[index].Name == $"Среднее зелье здоровья{cnt}")
                image = File.ReadAllLines("Items_images/potion_images/potion_image_1.txt");

            else if (inventory[index].Name == $"Большое зелье здоровья{cnt}")
                image = File.ReadAllLines("Items_images/potion_images/potion_image_2.txt");

            else if (inventory[index].Name == $"Зелье увеличения урона{cnt}")
                image = File.ReadAllLines("Items_images/potion_images/potion_image_3.txt");

            else if (inventory[index].Name == $"Зелье увеличения защиты{cnt}")
                image = File.ReadAllLines("Items_images/potion_images/potion_image_4.txt");

            else if (inventory[index].Name == $"Зелье увеличения радиуса видимости{cnt}")
                image = File.ReadAllLines("Items_images/potion_images/potion_image_5.txt");


            else if (inventory[index].Name == $"Малый мешок денег{cnt}")
                image = File.ReadAllLines("Items_images/coin_images/coin.txt");

            else if (inventory[index].Name == $"Средний мешок денег{cnt}")
                image = File.ReadAllLines("Items_images/coin_images/coin.txt");

            else if (inventory[index].Name == $"Большой мешок денег{cnt}")
                image = File.ReadAllLines("Items_images/coin_images/coin.txt");

            DrawImage(image, true, 57, 4);


            Graphics.PrintSelectedText("Краткая информация:", 160, 4, ConsoleColor.DarkCyan);

            TextWriter(inventory[index].Description, 150, 7, true, 44);
            int start = index - 17;


            if (index >= 18)
            {
                for (int i = start; i < stop + start; i++)
                {

                    if (i == index)
                    {
                        Console.SetCursorPosition(x, y + (i - start) * (count_endl + 1));
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(inventory[i].Name);
                        selectedY = y + (i - start) * (count_endl + 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(x, y + (i - start) * (count_endl + 1));
                        Console.Write(inventory[i].Name);

                    }
                    Console.ResetColor();
                }
            }
            else
            {
                for (int i = 0; i < stop; i++)
                {
                    if (i == index)
                    {
                        Console.SetCursorPosition(x, y + i * (count_endl + 1));
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(inventory[i].Name);
                        selectedY = y + i * (count_endl + 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(x, y + i * (count_endl + 1));
                        Console.Write(inventory[i].Name);

                    }
                    Console.ResetColor();
                }
            }


            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.DownArrow)
            {
                Console.Beep(500, 10);
                if (index == inventory.Count - 1) { }
                else
                    index++;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                Console.Beep(500, 10);
                if (index <= 0) { }
                else
                    index--;
            }
            else if
            (key.Key == ConsoleKey.Escape)
            {
                item.Kind = "Escape";
                return item;
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.RightArrow)
                return inventory[index];
            else
                return item;

            return item;
        }

        public static void DrawAnimation(object text_file)
        {
            Animation animation = text_file as Animation;
            while (true)
            {
                for (int i = 0; i < animation.Iterations_count; i++)
                {
                    Thread.Sleep(3000);
                    Console.Clear();

                    string[] file = File.ReadAllLines(animation.Name_of_files + $"_{i}.txt");
                    DrawImage(file, true, 0, 1);

                    for (int j = 0; j < animation.Items.Count; j++)
                    {
                        if (j == animation.Index)
                        {
                            Console.SetCursorPosition(95, 23 + j);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine(animation.Items[j]);
                        }
                        else
                        {
                            Console.SetCursorPosition(95, 23 + j);
                            Console.WriteLine(animation.Items[j]);
                        }
                        Console.ResetColor();
                    }
                }


            }
        }

        public static void TextWriter(string text, int x, int y, bool is_instantly, int Size)
        {
            if (is_instantly == true)
            {
                int count = 0;
                int warn_count = 0;
                bool is_here = false;
                int[] warning = new int[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == ' ' || text[i] == '.' || text[i] == '!')
                        warning[count] = i;
                    warn_count++;
                }

                for (int i = 0; i < text.Length; i++)
                {
                    for (int j = 0; j < warning.Length; j++)
                    {
                        if (i == warning[j])
                        {
                            is_here = true;
                            break;
                        }
                        else
                            is_here = false;

                    }

                    if (i != 0 && i % Size == 0 && is_here == false)
                    {
                        count = i;
                        y++;
                    }
                    Console.SetCursorPosition(x + i - count, y);

                    if (i == count && text[i] == ' ')
                    {
                        //i++;
                        //Console.Write(text[i]);
                    }
                    else
                        Console.Write(text[i]);
                }
            }
            else
            {
                int count = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    Random rand = new Random();

                    int time = rand.Next(50, 150);

                    if (text[i] != ' ')
                    {
                        Console.Beep(rand.Next(100, 200), rand.Next(10, 50));
                    }

                    if (i + 1 < text.Length)
                    {
                        if ((text[i] == '.' && text[i + 1] != '.') || (text[i] == '!' && text[i + 1] != '!') || (text[i] == '?' && text[i + 1] != '?'))
                            time += 1000;
                        else if (text[i] == ',')
                            time += 350;
                    }


                    if (i != 0 && i % Size == 0)
                    {
                        count = i;
                        y++;
                    }
                    Console.SetCursorPosition(x + i - count, y);

                    Console.Write(text[i]);

                    Thread.Sleep(time);
                }
            }
        }

        public static void TextWriterForAnimation(string text, int x, int y, bool is_instantly, int Size)
        {
            if (is_instantly == true)
            {
                int count = 0;
                int warn_count = 0;
                bool is_here = false;
                int[] warning = new int[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == ' ' || text[i] == '.' || text[i] == '!')
                        warning[count] = i;
                    warn_count++;
                }

                for (int i = 0; i < text.Length; i++)
                {
                    for (int j = 0; j < warning.Length; j++)
                    {
                        if (i == warning[j])
                        {
                            is_here = true;
                            break;
                        }
                        else
                            is_here = false;

                    }

                    if (i != 0 && i % Size == 0 && is_here == false)
                    {
                        count = i;
                        y++;
                    }
                    Console.SetCursorPosition(x + i - count, y);

                    if (i == count && text[i] == ' ')
                    {
                        //i++;
                        //Console.Write(text[i]);
                    }
                    else
                        Console.Write(text[i]);
                }
            }
            else
            {
                int count = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    Random rand = new Random();

                    int time = rand.Next(50, 70);

                    if (text[i] != ' ')
                    {
                        Console.Beep(rand.Next(100, 200), rand.Next(10, 50));
                    }

                    if (i + 1 < text.Length)
                    {
                        if ((text[i] == '.' && text[i + 1] != '.') || (text[i] == '!' && text[i + 1] != '!') || (text[i] == '?' && text[i + 1] != '?'))
                            time += 500;
                        else if (text[i] == ',')
                            time += 250;
                    }


                    if (i != 0 && i % Size == 0)
                    {
                        count = i;
                        y++;
                    }
                    Console.SetCursorPosition(x + i - count, y);

                    Console.Write(text[i]);

                    Thread.Sleep(time);
                }
            }
        }

        public static void PrintSelectedText(string text, int x, int y, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(text);
            Console.ResetColor();
        }
        public static void PrintText(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public static void ScreensaverAnimation()
        {
            Console.Clear();
            Thread.Sleep(1000);
            Console.SetWindowSize(200, 50);
            ThreadStart text = new ThreadStart(Text_Object);
            Thread thread1 = new Thread(text);

            ThreadStart animation = new ThreadStart(Anim_Object);
            Thread thread2 = new Thread(animation);

            thread1.Start();
            thread2.Start();

            SoundPlayer sound = new SoundPlayer("Music/Op.wav");
            sound.Play();

            Thread.Sleep(10);
            Graphics.PrintText("Нажмите Enter, чтобы пропустить заставку...", 150, 45);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    thread1.Abort();
                    thread2.Abort();
                    Console.Clear();
                    sound.Stop();
                    break;
                }

            }




        }

        private static void Text_Object()
        {
            string text = File.ReadAllText("Text/1.txt");
            TextWriterForAnimation(text, 115, 5, false, 80);
        }

        private static void Text_Object1()
        {
            string text = File.ReadAllText("end/kontsovka.txt");
            TextWriterForAnimation(text, 115, 5, false, 80);
        }

        private static void Anim_Object()
        {
            string[] arr = File.ReadAllLines("Screensaver/drink_pivo.txt");
            for (int j = 0; j < 3; j++)
            {
                arr = File.ReadAllLines("Screensaver/drink_pivo.txt");
                DrawImage(arr, true, 5, 0);
                Thread.Sleep(8000);

                arr = File.ReadAllLines("Screensaver/take_pivo.txt");
                DrawImage(arr, true, 5, 0);
                Thread.Sleep(8000);

            }

            arr = File.ReadAllLines("Screensaver/otbirayut_pivo.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(4000);

            arr = File.ReadAllLines("Screensaver/dovolny_gosha.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(3000);

            arr = File.ReadAllLines("Screensaver/negodyay.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(3000);

            arr = File.ReadAllLines("Screensaver/okhuevshiy_gosha.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(3000);

            arr = File.ReadAllLines("Screensaver/gosha_v_pogone.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(3500);

            arr = File.ReadAllLines("Screensaver/pereulok.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(13000);

            arr = File.ReadAllLines("Screensaver/prikosnovenie_k_kamnyu.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(10000);

            arr = File.ReadAllLines("Screensaver/spiral.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(5000);

            arr = File.ReadAllLines("Screensaver/padenie_goshi.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(2000);

            arr = File.ReadAllLines("Screensaver/lezhachiy_gosha.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(2000);

            arr = File.ReadAllLines("Screensaver/4.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(2000);

            arr = File.ReadAllLines("Screensaver/gosha_prosnulsya.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(13000);

            arr = File.ReadAllLines("Screensaver/pryamoy_gosha.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(2000);

            arr = File.ReadAllLines("Screensaver/pravy_gosha.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(2500);

            arr = File.ReadAllLines("Screensaver/levy_gosha.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(2500);

            arr = File.ReadAllLines("Screensaver/voprositelny_pryamoy_gosha.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(6000);

            arr = File.ReadAllLines("Screensaver/gollum.txt");
            DrawImage(arr, true, 5, 0);
            Thread.Sleep(6000);

            Console.ReadKey();

        }


        public static void PrintIvents(string ivent, ConsoleColor color)
        {
            string[] strr = File.ReadAllLines("ivents.txt");
            StreamWriter str = new StreamWriter("ivents.txt", true);
            if (strr.Length == 0)
                str.Write(ivent);
            else
                str.Write("\n" + ivent);

            Graphics.PrintText("Событие: ", 10, 45);
            Graphics.PrintSelectedText(ivent, 20, 45, color);
            str.Close();

        }

        public static void AnimationEnd()
        {
            string[] arr1 = File.ReadAllLines("end/GOShAN_VIN.txt");
            for (int i = 0; i < arr1.Length; i++)
            {
                Console.SetCursorPosition(5, i);
                Console.WriteLine(arr1[i]);
            }
            Thread.Sleep(15000);

            arr1 = File.ReadAllLines("end/gosha_sidit_na_skameyke.txt");
            for (int i = 0; i < arr1.Length; i++)
            {
                Console.SetCursorPosition(5, i);
                Console.WriteLine(arr1[i]);
            }
            Thread.Sleep(10000);
            arr1 = File.ReadAllLines("end/goshan_na_ulitse.txt");
            for (int i = 0; i < arr1.Length; i++)
            {
                Console.SetCursorPosition(5, i);
                Console.WriteLine(arr1[i]);
            }
            Console.ReadKey();
        }

        public static void EndAnimation()
        {
            Console.Clear();
            Thread.Sleep(1000);
            Console.SetWindowSize(200, 50);
            ThreadStart text = new ThreadStart(Text_Object1);
            Thread thread1 = new Thread(text);

            ThreadStart animation = new ThreadStart(AnimationEnd);
            Thread thread2 = new Thread(animation);

            thread1.Start();
            thread2.Start();

            SoundPlayer sound = new SoundPlayer("Music/end.wav");
            sound.Play();

            Thread.Sleep(10);
            Graphics.PrintText("Нажмите Enter, чтобы пропустить заставку...", 150, 45);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    thread1.Abort();
                    thread2.Abort();
                    Console.Clear();
                    sound.Stop();
                    break;
                }

            }




        }

    }
}
