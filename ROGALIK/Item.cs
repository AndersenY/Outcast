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
    internal class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
        public string CountThings { get; set; }
        public string Property1 { get; set; }
        public string Property2 { get; set; }

        public Item(string property1, string property2, string name, string description, string kind, string countThings)
        {
            Name = name;
            Description = description;
            Kind = kind;
            CountThings = countThings;
            Property1 = property1;
            Property2 = property2;
        }
        public Item()
        {

        }



        public static void Use(ref Player player, Item item)
        {
            int buf = 0;
            int MovesNumber = 0;

            string[] file = File.ReadAllLines("inventory.txt");

            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] + "(" + item.CountThings + ")" == item.Name || file[i] == item.Name)
                {
                    buf = Convert.ToInt32(file[i - 2]);
                    MovesNumber = Convert.ToInt32(file[i - 1]);
                    break;
                }

            }
            string[] file3 = File.ReadAllLines("buf.txt");
            StreamWriter fs = new StreamWriter("buf.txt", true);

            int temp = 0;
            bool is_exist = false;

            if (item.Kind == "PotionH")
            {
                player.Health += buf;
                if (player.Health > 100)
                    player.Health = 100;
                fs.Close();
            }
            else if (item.Kind == "PotionD")
            {
                if (is_exist)
                    player.OwnDamage += buf;

                for (int i = 0; i < file3.Length; i++)
                {
                    if (file3[i] == "DamageBuf")
                    {
                        temp = Convert.ToInt32(file3[i + 1]) + MovesNumber;
                        file3[i + 1] = Convert.ToString(temp);
                        is_exist = true;
                        break;
                    }
                }

                if (is_exist == false)
                {
                    if (file3.Length != 0)
                        fs.Write("\nDamageBuf");
                    else
                        fs.Write("DamageBuf");
                    fs.Write("\n" + MovesNumber);
                    fs.Close();

                }
                else
                {
                    fs.Close();
                    fs = new StreamWriter("buf.txt");

                    for (int i = 0; i < file3.Length; i++)
                    {
                        if (i == 0)
                            fs.Write(file3[i]);
                        else
                            fs.Write("\n" + file3[i]);
                    }
                    fs.Close();

                }

            }
            else if (item.Kind == "PotionDef")
            {
                if (is_exist)
                    player.OwnDefense += buf;


                for (int i = 0; i < file3.Length; i++)
                {
                    if (file3[i] == "DefenseBuf")
                    {
                        temp = Convert.ToInt32(file3[i + 1]) + MovesNumber;
                        file3[i + 1] = Convert.ToString(temp);
                        is_exist = true;
                        break;
                    }
                }
                if (is_exist == false)
                {
                    if (file3.Length != 0)
                        fs.Write("\nDefenseBuf");
                    else
                        fs.Write("DefenseBuf");
                    fs.Write("\n" + MovesNumber);
                    fs.Close();

                }
                else
                {
                    fs.Close();
                    fs = new StreamWriter("buf.txt");

                    for (int i = 0; i < file3.Length; i++)
                    {
                        if (i == 0)
                            fs.Write(file3[i]);
                        else
                            fs.Write("\n" + file3[i]);
                    }
                    fs.Close();

                }

            }
            else if (item.Kind == "PotionR")
            {
                if (is_exist)
                    player.RadVision += buf;


                for (int i = 0; i < file3.Length; i++)
                {
                    if (file3[i] == "VisionBuf")
                    {
                        temp = Convert.ToInt32(file3[i + 1]) + MovesNumber;
                        file3[i + 1] = Convert.ToString(temp);
                        is_exist = true;
                        break;
                    }
                }
                if (is_exist == false)
                {
                    if (file3.Length != 0)
                        fs.Write("\nVisionBuf");
                    else
                        fs.Write("VisionBuf");
                    fs.Write("\n" + MovesNumber);
                    fs.Close();

                }
                else
                {
                    fs.Close();
                    fs = new StreamWriter("buf.txt");

                    for (int i = 0; i < file3.Length; i++)
                    {
                        if (i == 0)
                            fs.Write(file3[i]);
                        else
                            fs.Write("\n" + file3[i]);
                    }
                    fs.Close();

                }

            }
            else if(item.Kind == "Coin")
            {
                fs.Close();
                player.Money += Convert.ToInt32(item.Property1);
            }

        }
        public static string[] Equip(Item item)
        {
            //count -= 1;

            string[] file = File.ReadAllLines("inventory.txt");
            string[] file1 = new string[file.Length - 6];
            string[] file2 = new string[6];
            string[] file3 = File.ReadAllLines($"Equipment/equipped_{item.Kind}.txt");
            int start = 0;
            int stop = 0;



            for (int i = 0; i < file.Length; i++)
            {
                if (item.CountThings == "1")
                {
                    if (file[i] == item.Name)
                    {
                        start = i - 2;
                        stop = i + 3;
                    }
                }
                else
                {
                    if (file[i] + "(" + item.CountThings + ")" == item.Name)
                    {
                        start = i - 2;
                        stop = i + 3;
                    }
                }

            }

            StreamWriter fs = new StreamWriter($"Equipment/equipped_{item.Kind}.txt");

            for (int i = 0; i < file.Length; i++)
            {
                if (i >= start && i <= stop)
                {
                    if (i != stop)
                    {
                        fs.WriteLine(file[i]);
                    }
                    else
                    {
                        fs.Write(file[i]);
                    }
                }

            }
            fs.Close();
            return file2;
        }
        public static bool Drop(bool is_all, string file_name, Item item)
        {

            string[] file = File.ReadAllLines(file_name);
            int start = 0;
            int stop = 0;
            bool is_exist = true;

            if (file.Length != 0)
            {


                if (is_all || item.CountThings == "1")
                {
                    for (int i = 0; i < file.Length; i++)
                    {

                        if (file[i] + "(" + item.CountThings + ")" == item.Name || file[i] == item.Name)
                        {
                            start = i - 2;
                            stop = i + 3;
                        }
                    }

                    StreamWriter sw = new StreamWriter(file_name);

                    for (int i = 0; i < file.Length; i++)
                    {
                        if (i < start || i > stop)
                        {
                            if (i != file.Length - 1)
                            {
                                if (i == file.Length - 7 && start == file.Length - 6)
                                    sw.Write(file[i]);
                                else
                                    sw.WriteLine(file[i]);
                            }
                            else
                            {
                                sw.Write(file[i]);
                            }
                        }
                    }
                    sw.Close();
                    is_exist = false;
                }
                else
                {
                    for (int i = 0; i < file.Length; i++)
                    {
                        if (file[i] + "(" + item.CountThings + ")" == item.Name || file[i] == item.Name)
                        {
                            stop = i + 3;
                            break;
                        }
                    }
                    int count = Convert.ToInt32(item.CountThings) - 1;
                    file[stop] = $"{count}";

                    StreamWriter sw = new StreamWriter(file_name);

                    for (int i = 0; i < file.Length; i++)
                    {
                        if (i != file.Length - 1)
                        {
                            if (i == file.Length - 7 && start == file.Length - 6)
                                sw.Write(file[i]);
                            else
                                sw.WriteLine(file[i]);
                        }
                        else
                        {
                            sw.Write(file[i]);
                        }
                    }
                    sw.Close();
                }
            }


            return is_exist;
        }

        public static void PutToInventory(Item item)
        {
            string[] inventory = File.ReadAllLines("inventory.txt");
            bool is_exist = false;
            int start = 0;
            int stop = 0;


            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == item.Name || inventory[i] + $"({item.CountThings})" == item.Name)
                {
                    is_exist = true;
                    break;
                }
            }


            if (is_exist == true)
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] == item.Name)
                    {
                        stop = i + 3;
                        break;
                    }
                }
                int count = Convert.ToInt32(inventory[stop]) + 1;
                inventory[stop] = $"{count}";

                StreamWriter sw = new StreamWriter("inventory.txt");

                for (int i = 0; i < inventory.Length; i++)
                {
                    if (i != inventory.Length - 1)
                    {
                        if (i == inventory.Length - 7 && start == inventory.Length - 6)
                            sw.Write(inventory[i]);
                        else
                            sw.WriteLine(inventory[i]);
                    }
                    else
                    {
                        sw.Write(inventory[i]);
                    }
                }
                sw.Close();
            }
            else
            {
                FileStream fs1 = new FileStream("inventory.txt", FileMode.Append);
                StreamWriter sw1 = new StreamWriter(fs1);

                if (inventory.Length == 0)
                    sw1.WriteLine(item.Property1);
                else
                    sw1.WriteLine("\n" + item.Property1);

                sw1.WriteLine(item.Property2);
                sw1.WriteLine(item.Name);
                sw1.WriteLine(item.Description);
                sw1.WriteLine(item.Kind);
                sw1.Write(item.CountThings);
                sw1.Close();
            }
        }

        public static void PutToSecretInventory(Item item)
        {
            string[] inventory = File.ReadAllLines("SecretSeller/SecretInventory.txt");
            bool is_exist = false;
            int start = 0;
            int stop = 0;


            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == item.Name || inventory[i] + $"({item.CountThings})" == item.Name)
                {
                    is_exist = true;
                    break;
                }
            }


            if (is_exist == true)
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] == item.Name)
                    {
                        stop = i + 3;
                        break;
                    }
                }
                int count = Convert.ToInt32(inventory[stop]) + 1;
                inventory[stop] = $"{count}";

                StreamWriter sw = new StreamWriter("SecretSeller/SecretInventory.txt");

                for (int i = 0; i < inventory.Length; i++)
                {
                    if (i != inventory.Length - 1)
                    {
                        if (i == inventory.Length - 7 && start == inventory.Length - 6)
                            sw.Write(inventory[i]);
                        else
                            sw.WriteLine(inventory[i]);
                    }
                    else
                    {
                        sw.Write(inventory[i]);
                    }
                }
                sw.Close();
            }
            else
            {
                FileStream fs1 = new FileStream("SecretSeller/SecretInventory.txt", FileMode.Append);
                StreamWriter sw1 = new StreamWriter(fs1);

                if (inventory.Length == 0)
                    sw1.WriteLine(item.Property1);
                else
                    sw1.WriteLine("\n" + item.Property1);

                sw1.WriteLine(item.Property2);
                sw1.WriteLine(item.Name);
                sw1.WriteLine(item.Description);
                sw1.WriteLine(item.Kind);
                sw1.Write(item.CountThings);
                sw1.Close();
            }
        }
    }
}
