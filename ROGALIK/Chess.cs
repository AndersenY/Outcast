using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGALIK
{
    internal class Chess
    {
        public bool haveChess = false;
        private int coordChessX;
        private int coordChessY;
        public int[] coordX = new int[6];
        public int[] coordY = new int[6];
        public int[] health = new int[6];
        public void GenerateChess(ref char[,] map)
        {
            if (haveChess == false)
            {
                Random rand = new Random();
                int random = rand.Next(3, 7);
                int numb = 0;
                for (int i = 0; i < health.Length; i++)
                {
                    coordX[i] = 0;
                    coordY[i] = 0;
                    health[i] = 5;
                }
                while (numb < random)
                {
                    coordChessX = rand.Next(0, map.GetLength(0));
                    coordChessY = rand.Next(0, map.GetLength(1));
                    if (map[coordChessX, coordChessY] == ' ')
                    {
                        coordX[numb] = coordChessX;
                        coordY[numb] = coordChessY;
                        map[coordChessX, coordChessY] = 'O';
                        haveChess = true;
                        numb++;
                    }
                }

            }
        }
        public int[] DeadChess(ref char[,] map)
        {
            int[] arr = new int[2];
            for (int i = 0; i < health.Length; i++)
            {
                if (health[i] <= 0)
                {
                    map[coordX[i], coordY[i]] = '$';
                    arr[0] = coordX[i];
                    arr[1] = coordY[i];
                    coordX[i] = 0;
                    coordY[i] = 0;
                }
            }
            return arr;
        }

        public void DamageChess(Player player, ref int[] healthChess, int[] coordX, int[] coordY)
        {
            for (int i = player.Y - player.RadDamage; i < player.Y + player.RadDamage; i++)
            {
                for (int j = player.X - player.RadDamage; j < player.X + player.RadDamage; j++)
                {
                    for (int k = 0; k < healthChess.Length; k++)
                    {
                        if (j == coordX[k] && i == coordY[k])
                        {
                            //Console.SetCursorPosition(120, 37);
                            //Console.Write(coordX);

                            //Console.SetCursorPosition(140, 37);
                            //Console.Write(coordY);
                            healthChess[k] = healthChess[k] - player.OwnDamage;
                            //Graphics.PrintText(Convert.ToString(healthChess[k]), 100, 35);
                        }
                    }
                }
            }
        }


    }
}
