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
    internal class Program
    {
        static void Main(string[] args)
        {
            int index = 0;

            SoundPlayer sound = new SoundPlayer("Music/screensaver.wav");

            List<string> menuItem = new List<string>()
            {
                " Начать игру",
                "  Настройки",
                "   Авторы",
                "Выйти из игры"
            };
            Console.CursorVisible = false;
            Console.SetWindowSize(200, 50);

            string[] file = File.ReadAllLines("MainMenu/ebys.txt");
            Graphics.DrawImage(file, false, 0, 3);
            Thread.Sleep(2000);
            Console.Clear();
            sound.Play();


            Console.CursorVisible = false;

            bool is_ex = false;
            //Inventory.ClearFile("inventory");
            //Inventory.ClearFile("Equipment/equipped_Bib");
            //Inventory.ClearFile("Equipment/equipped_Helm");
            //Inventory.ClearFile("Equipment/equipped_Weapon");
            Inventory.ClearFile("ivents");


            while (true)
            {

                SecretSeller seller = new SecretSeller();

                Map mapp = new Map();
                if (mapp.count < 7)
                    mapp.ReadRandomMap();


                Chess chess = new Chess();

                Player player = new Player(5, 5, 100, 0);
                //SecretSeller.SecretShopWindow(ref player);

                Animation animation = new Animation("Outcast/outcast", 21, menuItem, index);
                ParameterizedThreadStart Anim = new ParameterizedThreadStart(Graphics.DrawAnimation);
                Thread thread = new Thread(Anim);
                thread.Start(animation);

                if (is_ex == true)
                {
                    sound.Play();
                    is_ex = false;
                }


                string selectedMenuItem = Graphics.DrawOutcast(menuItem, ref index);



                if (selectedMenuItem == " Начать игру")
                {
                    thread.Abort();
                    sound.Stop();

                    Graphics.ScreensaverAnimation();

                    Console.Clear();
                    Console.CursorVisible = false;

                    ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);



                    int wallet = 0;

                    int countEnemySad = 15;
                    List<EnemySad> enemySadList = new List<EnemySad>();
                    string[] enemySadStats = File.ReadAllLines("Enemy_sad.txt");
                    for (int i = 0; i < countEnemySad; i++)
                    {
                        enemySadList.Add(new EnemySad(enemySadStats, mapp.map1.GetLength(0), mapp.map1.GetLength(1), mapp.map1));
                    }


                    int countEnemyAngry = 10;
                    List<EnemyAngry> enemyAngryList = new List<EnemyAngry>();

                    string[] enemyAngryStats = File.ReadAllLines("Enemy_angry.txt");
                    for (int i = 0; i < countEnemyAngry; i++)
                    {
                        enemyAngryList.Add(new EnemyAngry(enemyAngryStats, mapp.map1.GetLength(0), mapp.map1.GetLength(1), mapp.map1));
                    }

                    bool is_exit = false;
                    bool can_turn = true;
                    var sw = new Stopwatch();

                    ParameterizedThreadStart timer = new ParameterizedThreadStart(Music.Timer);
                    Thread timer_thread = new Thread(timer);

                    ConsoleKeyInfo key = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

                    int[,] enemyCoord = new int[countEnemySad + countEnemyAngry, 2];
                    int enemyCount = 0;

                    bool is_every_dead = false;
                    bool is_new_map = false;


                    EnemySad Boss = new EnemySad(enemySadStats, mapp.map1.GetLength(0), mapp.map1.GetLength(1), mapp.map1);

                    while (mapp.count < 7)
                    {
                       
                        Graphics.PrintText("                                                            ", 10, 45);

                        Graphics.PrintSelectedText("Информация о персонаже: ", 158, 2, ConsoleColor.DarkCyan);
                        Graphics.PrintText("Здоровье: ", 150, 6);
                        Player.HealthBar(ConsoleColor.Green, player);
                        player = Player.PlayerInformation(player, 150, 8);

                        if (mapp.count == 6 && is_new_map == true)
                        {
                            Boss = new EnemySad(enemySadStats, mapp.map1.GetLength(0), mapp.map1.GetLength(1), mapp.map1);
                            is_new_map = false;
                        }


                        if ((enemyAngryList.Count == 0 && enemySadList.Count == 0) && mapp.count < 6) 
                            is_every_dead = true;
                        
                        if(Boss.HP<=0 && mapp.count==6)
                            is_every_dead = true;



                        mapp.Emerging_Portal(is_every_dead);
                        if (mapp.count < 6)
                        {
                            if (is_new_map == true)
                            {
                                enemySadList = new List<EnemySad>();
                                enemySadStats = File.ReadAllLines("Enemy_sad.txt");
                                for (int i = 0; i < countEnemySad; i++)
                                {
                                    enemySadList.Add(new EnemySad(enemySadStats, mapp.map1.GetLength(0), mapp.map1.GetLength(1), mapp.map1));
                                }


                                enemyAngryList = new List<EnemyAngry>();

                                enemyAngryStats = File.ReadAllLines("Enemy_angry.txt");
                                for (int i = 0; i < countEnemyAngry; i++)
                                {
                                    enemyAngryList.Add(new EnemyAngry(enemyAngryStats, mapp.map1.GetLength(0), mapp.map1.GetLength(1), mapp.map1));
                                }
                            }
                            chess.GenerateChess(ref mapp.map1);
                            seller.GenerateSeller(ref mapp.map1);
                        }

                        enemySadStats = File.ReadAllLines("Enemy_BOSS.txt");

                        Console.SetCursorPosition(0, 0);

                        char[,] pr_map = mapp.map1.Clone() as char[,];

                        int leftX = player.X - player.RadVision;
                        if (leftX < 0)
                            leftX = 0;

                        int rightX = player.X + player.RadVision;
                        if (rightX < 0)
                            rightX = 0;

                        int leftY = player.Y - player.RadVision;
                        if (leftY < 0)
                            leftY = 0;

                        int rightY = player.Y + player.RadVision;
                        if (rightY < 0)
                            rightY = 0;




                        for (int i = 0; i < pr_map.GetLength(1); i++)
                        {
                            for (int j = 0; j < pr_map.GetLength(0); j++)
                            {

                                if (i < leftY || i > rightY || j < leftX || j > rightX)
                                    pr_map[j, i] = ' ';

                            }
                        }

                        for (int i = 0; i < pr_map.GetLength(1); i++)
                        {
                            for (int j = 0; j < pr_map.GetLength(0); j++)
                            {
                                Console.Write(pr_map[j, i]);
                            }
                            Console.WriteLine();
                        }





                        Console.SetCursorPosition(0, 0);


                        if (can_turn == true)
                        {
                            timer_thread = new Thread(timer);
                            timer_thread.Start(true);
                            can_turn = false;
                        }

                        Console.ForegroundColor = ConsoleColor.Blue;

                        Console.SetCursorPosition(player.X, player.Y);
                        Console.Write(player.Sprite);

                        Inventory.PickUpThing(player.X, player.Y, ref enemyCoord);


                        //DrawMap(map);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;


                        if (mapp.count == 6)
                        {
                            if (Boss.HP > 0)
                            {

                                Boss.YaZametilTebyaBegiTvar(player.X, player.Y, mapp.map1, Boss.HP);
                                Console.SetCursorPosition(Boss.X, Boss.Y);
                                bool is_near = false;

                                if (Boss.Y < leftY || Boss.Y > rightY || Boss.X < leftX || Boss.X > rightX)
                                {

                                }
                                else
                                    is_near = true;
                                if (is_near == true)
                                    Console.Write(Boss.Sprite);
                                int dam = Boss.Atack(player.X, player.Y);
                                if (key.Key != ConsoleKey.I && key.Key != ConsoleKey.Escape)
                                {
                                    dam = dam - (dam * player.OwnDefense / 100);
                                    if (dam < 0)
                                        dam = 0;

                                    player.Health -= dam - (dam * player.OwnDefense / 100);

                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < enemySadList.Count; i++)
                            {

                                enemySadList[i].YaZametilTebyaBegiTvar(player.X, player.Y, mapp.map1, enemySadList[i].HP);
                                if (enemySadList[i].HP > 0)
                                {
                                    Console.SetCursorPosition(enemySadList[i].X, enemySadList[i].Y);
                                    bool is_near = false;

                                    for (int j = 0; j < enemySadList.Count; j++)
                                    {
                                        if (enemySadList[i].Y < leftY || enemySadList[i].Y > rightY || enemySadList[i].X < leftX || enemySadList[i].X > rightX)
                                        {

                                        }
                                        else
                                            is_near = true;
                                    }

                                    if (is_near == true)
                                        Console.Write(enemySadList[i].Sprite);
                                    int dam = enemySadList[i].Atack(player.X, player.Y);
                                    if (key.Key != ConsoleKey.I && key.Key != ConsoleKey.Escape)
                                    {
                                        dam = dam - (dam * player.OwnDefense / 100);
                                        if (dam < 0)
                                            dam = 0;

                                        player.Health -= dam - (dam * player.OwnDefense / 100);

                                    }
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.DarkRed;

                            for (int i = 0; i < enemyAngryList.Count; i++)
                            {
                                enemyAngryList[i].YaZametilTebyaBegiTvar(player.X, player.Y, mapp.map1, enemyAngryList[i].HP);
                                if (enemyAngryList[i].HP > 0)
                                {
                                    Console.SetCursorPosition(enemyAngryList[i].X, enemyAngryList[i].Y);
                                    bool is_near = false;


                                    for (int j = 0; j < enemySadList.Count; j++)
                                    {
                                        if (enemyAngryList[i].Y < leftY || enemyAngryList[i].Y > rightY || enemyAngryList[i].X < leftX || enemyAngryList[i].X > rightX)
                                        {

                                        }
                                        else
                                            is_near = true;
                                    }
                                    if (is_near == true)
                                        Console.Write(enemyAngryList[i].Sprite);
                                    int dam = enemyAngryList[i].Atack(player.X, player.Y);

                                    dam = dam - (dam * player.OwnDefense / 100);
                                    if (dam < 0)
                                        dam = 0;

                                    player.Health -= dam - (dam * player.OwnDefense / 100);
                                }
                            }




                        }
                        Console.ResetColor();




                        key = Console.ReadKey();


                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:

                                player.HeroAction(key, mapp.map1, player.X, player.Y);

                                Player.BufsTime();

                                break;
                            case ConsoleKey.DownArrow:

                                player.HeroAction(key, mapp.map1, player.X, player.Y);

                                Player.BufsTime();

                                break;
                            case ConsoleKey.LeftArrow:

                                player.HeroAction(key, mapp.map1, player.X, player.Y);

                                Player.BufsTime();

                                break;
                            case ConsoleKey.RightArrow:

                                player.HeroAction(key, mapp.map1, player.X, player.Y);

                                Player.BufsTime();

                                break;
                            case ConsoleKey.Enter:
                                int[] arr = new int[2];
                                arr = Player.Atack(player.X, player.Y, ref enemySadList, ref enemyAngryList, ref Boss, ref mapp.map1, player.OwnDamage, player.RadDamage);

                                chess.DamageChess(player, ref chess.health, chess.coordX, chess.coordY);

                                enemyCoord[enemyCount, 0] = arr[0];
                                enemyCoord[enemyCount, 1] = arr[1];
                                if (arr[0] != 0 && arr[1] != 0)
                                {
                                    for (int i = 0; i < mapp.map1.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < mapp.map1.GetLength(1); j++)
                                        {
                                            if (mapp.map1[i, j] == '.')
                                            {
                                                mapp.map1[i, j] = ' ';
                                            }
                                        }
                                    }
                                    enemyCount++;

                                }

                                Player.BufsTime();
                                break;

                            case ConsoleKey.M:
                                timer_thread.Abort();
                                timer_thread = new Thread(timer);
                                timer_thread.Start(false);
                                timer_thread.Abort();

                                SoundPlayer sound2 = new SoundPlayer("Music/shop.wav");
                                sound2.Play();
                                seller.OpenSeller(player.X, player.Y, ref player);

                                sound2.Stop();

                                can_turn = true;


                                break;


                            case ConsoleKey.Escape:
                                timer_thread.Abort();
                                timer_thread = new Thread(timer);
                                timer_thread.Start(false);

                                is_exit = Graphics.DrawMenu();
                                can_turn = !is_exit;
                                timer_thread.Abort();

                                break;
                            case ConsoleKey.I:
                                Inventory.ShowInventory(ref player);


                                break;

                        }

                        if (player.Health <= 0)
                        {
                            Console.Clear();
                            SoundPlayer pomerSound = new SoundPlayer("Music/POMER.wav");
                            SoundPlayer pomerMusic = new SoundPlayer("Music/musicPomer.wav");
                            pomerSound.Play();
                            Console.ForegroundColor = ConsoleColor.White;
                            string[] light = File.ReadAllLines("light.txt");
                            for (int i = 0; i < light.Length; i++)
                            {
                                Console.WriteLine(light[i]);
                            }
                            Thread.Sleep(2500);
                            Console.Clear();
                            string[] POMER = File.ReadAllLines("POMER.txt");
                            Console.ForegroundColor = ConsoleColor.Red;
                            for (int i = 0; i < POMER.Length; i++)
                            {
                                Console.SetCursorPosition(72, 18 + i);
                                Console.Write(POMER[i]);
                            }
                            Console.ForegroundColor = ConsoleColor.White;

                            Thread.Sleep(4000);
                            pomerMusic.Play();
                            Thread.Sleep(1000);

                            while (true)
                            {

                                Graphics.PrintText("Нажмите Enter, чтобы выйти...", 160, 45);
                                Console.CursorVisible = true;
                                ConsoleKeyInfo endKey = Console.ReadKey();
                                //mapp.map1 = mapp.ReadRandomMap("map1.txt");
                                if (endKey.Key == ConsoleKey.Enter)
                                {
                                    pomerMusic.Stop();
                                    pomerMusic = new SoundPlayer("Music/screensaver.wav");
                                    Console.CursorVisible = false;
                                    pomerMusic.Play();
                                    break;
                                }
                            }


                            break;
                        }




                        if (is_exit == true)
                        {
                            is_ex = true;
                            break;
                        }
                        int[] arr2 = new int[2];

                        arr2 = chess.DeadChess(ref mapp.map1);

                        enemyCoord[enemyCount, 0] = arr2[0];
                        enemyCoord[enemyCount, 1] = arr2[1];
                        if (arr2[0] != 0 && arr2[1] != 0)
                        {
                            enemyCount++;
                        }
                        is_new_map = mapp.NewMap(ref player.X, ref player.Y, ref chess.haveChess, ref seller.haveSeller, ref is_every_dead);

                    }
                    if(Boss.HP<0)
                        Graphics.EndAnimation();
                }
                else if (selectedMenuItem == "  Настройки")
                {
                    thread.Abort();
                    SoundPlayer sound1 = new SoundPlayer("Music/Op.wav");

                    Console.Clear();
                    string[] temp_file1 = File.ReadAllLines("MainMenu/coffin.txt");
                    string[] temp_file2 = File.ReadAllLines("MainMenu/settings.txt");

                    sound1.Play();
                    Graphics.DrawImage(temp_file1, false, 0, 0);
                    Graphics.DrawImage(temp_file2, false, 6, 9, 1000);
                    Console.ReadKey();
                    sound1.Stop();
                    sound.Play();
                }
                else if (selectedMenuItem == "   Авторы")
                {
                    thread.Abort();
                    SoundPlayer sound1 = new SoundPlayer("Music/Authors.wav");
                    sound1.Play();

                    Console.Clear();

                    Random rand = new Random();
                    int num = 0;
                    int[] arr = new int[5];
                    bool is_here = false;
                    int count = 0;

                    while (count < arr.Length)
                    {
                        num = rand.Next(1, 6);
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
                            arr[count] = num;
                            count++;
                        }
                        is_here = false;
                    }
                    string[] ballon = File.ReadAllLines("Authors/balloon.txt");
                    Graphics.DrawImage(ballon, false, 2, 32, 250);
                    Graphics.DrawImage(ballon, false, 174, 3, 250);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] author = File.ReadAllLines($"Authors/{arr[i]}.txt");
                        Graphics.DrawImage(author, false, i * 30, i * 10, 250);

                    }

                    Console.ReadKey();
                    sound1.Stop();
                    sound.Play();

                }
                else if (selectedMenuItem == "Выйти из игры")
                {
                    thread.Abort();
                    Console.Clear();
                    Environment.Exit(0);
                }
                thread.Abort();
            }

        }

    }
}
