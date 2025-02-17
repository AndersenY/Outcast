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
using static System.Net.Mime.MediaTypeNames;

namespace ROGALIK
{
    internal class Player
    {
        public int Health { get; set; }
        public int OwnDamage { get; set; }
        public int OwnDefense { get; set; }
        public int RadVision { get; set; }
        public int RadDamage { get; set; }
        public int Money { get; set; }


        public string Sprite = "☻";
        public int X;
        public int Y;

        public static int DefaultPlayerHealth = 100;
        public static int DefaultPlayerDamage = 30;
        public static int DefaultPlayerDefense = 50;
        public static int DefaultPlayerVision = 20;
        public static int DefaultPlayerRadDamage = 30;
        public static int DefaultPlayerMoney = 0;



        public Player(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            Health = DefaultPlayerHealth;
            OwnDamage = DefaultPlayerDamage;
            OwnDefense = DefaultPlayerDefense;
            RadVision = DefaultPlayerVision;
            RadDamage = DefaultPlayerRadDamage;
            Money = DefaultPlayerMoney;

        }
        public Player(int health, int own_damage, int rad_vision, int own_defense, int rad_damage, int money)
        {
            Health = health;
            OwnDamage = own_damage;
            OwnDefense = own_defense;
            RadVision = rad_vision;
            RadDamage = rad_damage;
            Money = money;
        }
        public Player()
        {
            Health = DefaultPlayerHealth;
            OwnDamage = DefaultPlayerDamage;
            OwnDefense = DefaultPlayerDefense;
            RadVision = DefaultPlayerVision;
            RadDamage = DefaultPlayerRadDamage;
            Money = DefaultPlayerMoney;

        }
        public Player(int X, int Y, int health, int money)
        {
            this.X = X;
            this.Y = Y;

            int defense_bib = 0;
            int defense_helm = 0;
            int defense = 0;
            int damage = DefaultPlayerDamage;
            int radVision = DefaultPlayerVision;
            int radDamage = DefaultPlayerRadDamage;

            string[] eq = File.ReadAllLines("Equipment/equipped_Bib.txt");
            if (eq.Length != 0)
            {
                defense_bib = Convert.ToInt32(eq[0]);
                defense += defense_bib;
            }

            eq = File.ReadAllLines("Equipment/equipped_Helm.txt");
            if (eq.Length != 0)
            {
                defense_helm = Convert.ToInt32(eq[0]);
                radVision = DefaultPlayerVision + Convert.ToInt32(eq[1]);

                defense += defense_helm;


            }

            defense += DefaultPlayerDefense;

            eq = File.ReadAllLines("Equipment/equipped_Weapon.txt");
            if (eq.Length != 0)
            {
                damage = DefaultPlayerDamage + Convert.ToInt32(eq[0]);
                radDamage = DefaultPlayerRadDamage + Convert.ToInt32(eq[1]);

            }


            Health = health;
            OwnDamage = damage;
            OwnDefense = defense;
            RadVision = radVision;
            RadDamage = radDamage;
            Money = money;

        }

        public static Player PlayerInformation(Player player1, int x, int y)
        {
            Player player = new Player();

            int defense_bib = 0;
            int defense_helm = 0;
            int damage = 0;
            int RadD = 0;
            int RadV = 0;
            int buf_defense = 0;
            int buf_damage = 0;
            int buf_RadV = 0;

            string[] equip_types = { "Bib", "Helm", "Weapon" };
            string[] buf_types = { "Def", "Dmg", "RadV", "RadD" };
            string[] equip = new string[3];

            string[] bufs = File.ReadAllLines("buf.txt");

            for (int i = 0; i < bufs.Length; i++)
            {
                for (int j = 0; j < bufs.Length; j++)
                {
                    if (bufs[i] == "DefenseBuf")
                        buf_defense = 10;
                    else if (bufs[i] == "DamageBuf")
                        buf_damage = 10;
                    else if (bufs[i] == "VisionBuf")
                        buf_RadV = 5;
                }
            }

            int first = 0;
            int second = 0;

            for (int i = 0; i < equip_types.Length; i++)
            {
                string[] file1 = File.ReadAllLines($"Equipment/equipped_{equip_types[i]}.txt");

                if (i == 0 && file1.Length != 0)
                {
                    first = 0;
                    defense_bib = Convert.ToInt32(file1[0]);
                }
                else if (i == 1 && file1.Length != 0)
                {
                    first = 0;
                    second = 2;
                    defense_helm = Convert.ToInt32(file1[0]);
                    RadV = Convert.ToInt32(file1[1]);
                }
                else if (i == 2 && file1.Length != 0)
                {
                    first = 1;
                    second = 3;
                    damage = Convert.ToInt32(file1[0]);
                    RadD = Convert.ToInt32(file1[1]);
                }

                if (file1.Length == 0)
                {
                    equip[i] = "-Пусто-";
                }
                else
                {
                    for (int j = 0; j < file1.Length; j++)
                    {
                        if (j == 2)
                        {
                            if (i == 0)
                                equip[i] = file1[j] + $" ({file1[j - 2]} {buf_types[first]})";
                            else
                                equip[i] = file1[j] + $" ({file1[j - 2]} {buf_types[first]}, {file1[j - 1]} {buf_types[second]})";

                        }
                    }

                }

            }
            player.Health = player1.Health;
            player.OwnDamage = DefaultPlayerDamage + damage + buf_damage;
            player.OwnDefense = DefaultPlayerDefense + defense_bib + defense_helm + buf_defense;
            if (player.RadVision + RadV + buf_RadV < 0)
                player.RadVision = 0;
            else
                player.RadVision = DefaultPlayerVision + RadV + buf_RadV;
            player.RadDamage = DefaultPlayerRadDamage + RadD;

            player.X = player1.X;
            player.Y = player1.Y;
            player.Money = player1.Money;




            Graphics.PrintText($"Защита: {player.OwnDefense} Def", x, y);
            Graphics.PrintText($"Урон: {player.OwnDamage} Dmg", x, y + 2);
            Graphics.PrintText($"Радиус атаки: {player.RadDamage} RadD", x, y + 4);
            Graphics.PrintText($"Радиус видимости: {player.RadVision} RadV", x, y + 6);
            //150,17

            Graphics.PrintText("Экипировка:  ", x, y + 8);

            for (int i = 0; i < equip.Length; i++)
            {
                Console.SetCursorPosition(x + 13, (y + 8) + 2 * i);
                Console.Write(equip[i]);
            }

            Graphics.PrintText("Бафы:  ", x, y + 15);

            Graphics.PrintText("                                     ", x + 10, y + 15);
            Graphics.PrintText("                                     ", x + 10, y + 16);
            Graphics.PrintText("                                     ", x + 10, y + 17);
            Graphics.PrintText("                                     ", x + 10, y + 18);
            Graphics.PrintText("                                     ", x + 10, y + 19);


            for (int i = 0; i < bufs.Length; i++)
            {
                if (bufs[i] == "DamageBuf")
                    Graphics.PrintText($"+10 к урону ({bufs[i + 1]} ходов)", x + 10, y + 15 + i);
                else if (bufs[i] == "DefenseBuf")
                    Graphics.PrintText($"+10 к защите ({bufs[i + 1]} ходов)", x + 10, y + 15 + i);
                else if (bufs[i] == "VisionBuf")
                    Graphics.PrintText($"+5 к радиусу видимости ({bufs[i + 1]} ходов)", x + 10, y + 15 + i);
            }

            Graphics.PrintText($"Ваш баланс: {player.Money} Coins", 150, 43);

            return player;
        }

        public void HeroAction(ConsoleKeyInfo charKey, char[,] map, int enemyX, int enemyY)
        {

            switch (charKey.Key)
            {

                case ConsoleKey.LeftArrow:
                    if (CheckWall("left", map) == true)
                    {
                        this.X -= 1;
                        map[this.X + 1, this.Y] = ' ';
                        map[this.X, this.Y] = '.';
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (CheckWall("right", map) == true)
                    {
                        this.X += 1;
                        map[this.X - 1, this.Y] = ' ';
                        map[this.X, this.Y] = '.';
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (CheckWall("up", map) == true)
                    {
                        this.Y -= 1;
                        map[this.X, this.Y + 1] = ' ';
                        map[this.X, this.Y] = '.';
                    }

                    break;

                case ConsoleKey.DownArrow:
                    if (CheckWall("down", map) == true)
                    {
                        this.Y += 1;
                        map[this.X, this.Y - 1] = ' ';
                        map[this.X, this.Y] = '.';
                    }
                    break;


            }
        }

        public bool CheckWall(string direction, char[,] map) // проверка на движение
        {
            char[] walls = { '║', '╚', '╔', '╗', '╝', '.', '═' };
            bool flag = false;
            for (int i = 0; i < 7; i++)
            {
                if (direction == "left")
                    if (map[this.X - 1, this.Y] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
                else if (direction == "right")
                    if (map[this.X + 1, this.Y] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
                else if (direction == "up")
                    if (map[this.X, this.Y - 1] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
                else if (direction == "down")
                    if (map[this.X, this.Y + 1] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
            }

            return flag;

        }

        public static void HealthBar(ConsoleColor color, Player player)
        {
            ConsoleColor defaultColor = Console.BackgroundColor;
            string bar = "";

            for (int i = 0; i < player.Health / 5; i++)
                bar += " ";

            Console.Write('[');
            Console.BackgroundColor = color;
            Console.Write(bar);
            Console.ForegroundColor = defaultColor;

            bar = "";

            for (int i = player.Health / 5; i < DefaultPlayerHealth / 5; i++)
                bar += " ";
            Console.ResetColor();

            Console.Write(bar);


            Console.Write($"]  {player.Health}%");
        }

        public static void BufsTime()
        {
            string[] bufs = File.ReadAllLines("buf.txt");
            int temp = 0;
            int count = 0;

            StreamWriter tmp = new StreamWriter("temp.txt");

            for (int i = 0; i < bufs.Length; i++)
            {

                if (bufs[i] == "DamageBuf" || bufs[i] == "DefenseBuf" || bufs[i] == "VisionBuf")
                {
                    temp = Convert.ToInt32(bufs[i + 1]) - 1;
                    if (temp > 0)
                    {
                        bufs[i + 1] = Convert.ToString(temp);
                        if (count == 0)
                        {
                            tmp.Write(bufs[i]);
                            tmp.Write("\n" + bufs[i + 1]);
                            count++;
                        }
                        else
                        {
                            tmp.Write("\n" + bufs[i]);
                            tmp.Write("\n" + bufs[i + 1]);
                            count++;
                        }
                    }
                    else
                        i += 1;

                }
            }
            tmp.Close();

            string[] file_temp = File.ReadAllLines("temp.txt");
            StreamWriter fs = new StreamWriter("buf.txt");

            for (int i = 0; i < file_temp.Length; i++)
            {
                if (i == 0)
                    fs.Write(file_temp[i]);
                else
                    fs.Write("\n" + file_temp[i]);
            }
            fs.Close();
        }

        private static int GetMaxLengthOfLine(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (var line in lines)
                if (line.Length > maxLength)
                    maxLength = line.Length;

            return maxLength;
        }

        public static int[] Atack(int x, int y, ref List<EnemySad> enemySadList, ref List<EnemyAngry> enemyAngryList, ref EnemySad Boss, ref char[,] map, int defaultDamage, int radDamage)
        {

            int ImpactRadius = radDamage;
            int[] enemyCoord = new int[2];


            for (int i = (x - ImpactRadius); i <= (x + ImpactRadius); i++)
            {
                for (int j = (y - ImpactRadius); j <= (y + ImpactRadius); j++)
                {

                    for (int k = 0; k < enemySadList.Count; k++)
                    {

                        if (enemySadList[k].X == i && enemySadList[k].Y == j)
                        {
                            enemySadList[k].HP -= defaultDamage;

                            if (enemySadList[k].HP <= 0)
                            {
                                Random rand = new Random();
                                if (rand.Next(1, 3) == 1)
                                {
                                    //map[enemySadList[k].X, enemySadList[k].Y] = '$';
                                    enemyCoord[0] = enemySadList[k].X;
                                    enemyCoord[1] = enemySadList[k].Y;
                                }
                                else
                                    map[enemySadList[k].X, enemySadList[k].Y] = ' ';

                                Console.SetCursorPosition(enemySadList[k].X, enemySadList[k].Y);
                                enemySadList.RemoveAt(k);

                            }
                            break;

                        }

                    }
                    for (int l = 0; l < enemyAngryList.Count; l++)
                    {
                        if (enemyAngryList[l].X == i && enemyAngryList[l].Y == j)
                        {
                            enemyAngryList[l].HP -= defaultDamage;
                            if (enemyAngryList[l].HP <= 0)
                            {
                                Random rand = new Random();
                                if (rand.Next(1, 3) == 1)
                                {
                                    //map[enemyAngryList[l].X, enemyAngryList[l].Y] = '$';
                                    enemyCoord[0] = enemyAngryList[l].X;
                                    enemyCoord[1] = enemyAngryList[l].Y;
                                }
                                else
                                    map[enemyAngryList[l].X, enemyAngryList[l].Y] = ' ';

                                Console.SetCursorPosition(enemyAngryList[l].X, enemyAngryList[l].Y);
                                enemyAngryList.RemoveAt(l);

                            }
                            break;
                        }
                    }

                    if (Boss.X == i && Boss.Y == j)
                    {
                        Boss.HP -= defaultDamage;
                    }

                }
            }



            map[enemyCoord[0], enemyCoord[1]] = '$';


            return enemyCoord;

        }

        public void DamageChess(Player player, ref int[] healthChess, int[] coordX, int[] coordY)
        {
            for (int i = player.X - player.RadDamage; i < player.X + player.RadDamage; i++)
            {
                for (int j = player.Y - player.RadDamage; j < player.Y + player.RadDamage; j++)
                {
                    for (int k = 0; k < healthChess.Length; k++)
                    {
                        if (j == coordX[k] && i == coordY[k])
                        {
                            healthChess[k] = healthChess[k] - player.OwnDamage;
                        }
                    }
                }
            }
        }
    }


}
