using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using ROGALIK;

namespace ROGALIK
{
    class Map
    {
        public char[,] map1;
        public int count = 0;
        public int[] mapQueue = new int[7];
        public int coordPortalX;
        public int coordPortalY;
        public bool havePortal = false;

        public void ReadRandomMap()
        {
            if (this.count == 0)
            {
                Random rand = new Random();

                int num = 0;
                int[] arr = new int[5];
                bool is_here = false;
                int count1 = 0;

                while (count1 < arr.Length)
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
                        arr[count1] = num;
                        count1++;
                    }
                    is_here = false;

                }

                for (int i = 0; i < 5; i++)
                {
                    this.mapQueue[i] = arr[i];
                }
                this.mapQueue[5] = 6;
                this.mapQueue[6] = 7;


            }

            string[] file = File.ReadAllLines($"Maps/map{this.mapQueue[this.count]}.txt");

            if (this.count < 7)
            {
                this.count += 1;
            }

            char[,] map = new char[GetMaxLengthOfLine(file), file.Length];
            //char[,] map = new char[file.Length, GetMaxLengthOfLine(file)];

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = file[y][x];


            this.map1 = map;

        }


        public void DrawMap()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < map1.GetLength(0); i++)
            {
                for (int j = 0; j < map1.GetLength(1); j++)
                {
                    if (map1[i, j] == '$')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(map1[i, j]);
                    }
                    else if (map1[i, j] == '@')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(map1[i, j]);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(map1[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }


        public static int GetMaxLengthOfLine(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (var line in lines)
                if (line.Length > maxLength)
                    maxLength = line.Length;

            return maxLength;
        }


        public void Emerging_Portal(bool LifeEnemy)
        {
            if ((LifeEnemy == true) && (havePortal == false) && (this.count <= 6))
            {

                Random rand = new Random();
                while (true)
                {
                    coordPortalX = rand.Next(0, map1.GetLength(0));
                    coordPortalY = rand.Next(0, map1.GetLength(1));
                    if (map1[coordPortalX, coordPortalY] == ' ')
                    {
                        map1[coordPortalX, coordPortalY] = '@';
                        havePortal = true;
                        break;
                    }
                }

            }
        }


        public bool NewMap(ref int userX, ref int userY, ref bool haveChess, ref bool haveSeller, ref bool is_every_dead)
        {
            if (map1[userX, userY] == map1[coordPortalX, coordPortalY])
            {
                Console.Clear();
                ReadRandomMap();
                havePortal = false;
                haveChess = false;
                haveSeller = false;
                is_every_dead = false;

                Random rand = new Random();
                while (true)
                {
                    userX = rand.Next(0, map1.GetLength(0));
                    userY = rand.Next(0, map1.GetLength(1));
                    if (map1[userX, userY] == ' ')
                    {
                        break;
                    }
                }

                coordPortalX = 0;
                coordPortalY = 0;

                return true;
            }

            return false;

        }


    }


}