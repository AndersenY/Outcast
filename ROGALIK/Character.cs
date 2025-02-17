using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics.Tracing;
using System.Data.SqlTypes;
using System.Threading;

namespace ROGALIK
{
    class Character
    {
        public string Role;
        public string Name;
        public string Sprite;
        public int HP;
        public int Damage;
        public int Armor;
        public int X;
        public int Y;
        public int Vision;
        public int CountSteps;
        public int Live;
 
        
        public Character(string[] heroStats) // Для героя
        {
            Role = heroStats[0];
            Sprite = heroStats[1];
            Name = heroStats[2];
            HP = Convert.ToInt32(heroStats[3]);
            Damage = Convert.ToInt32(heroStats[4]);
            Armor = Convert.ToInt32(heroStats[5]);
            X = Convert.ToInt32(heroStats[6]);
            Y = Convert.ToInt32(heroStats[7]);
            Vision = Convert.ToInt32(heroStats[8]);
        }

        public Character(string[] heroStats, int mapX, int mapY, char[,] map) // для врага =============================================================
        {
            Role = heroStats[0];
            Sprite = heroStats[1];
            Name = heroStats[2];
            HP = Convert.ToInt32(heroStats[3]);
            Damage = Convert.ToInt32(heroStats[4]);
            Armor = Convert.ToInt32(heroStats[5]);
            Vision = Convert.ToInt32(heroStats[6]);
            Live = Convert.ToInt32(heroStats[7]);
            
            while (true)
            {
                Thread.Sleep(1);
                Random rand = new Random();
                int coorX = rand.Next(1, mapX);
                int coorY = rand.Next(1, mapY);
                if (map[coorX, coorY] != '░' && map[coorX, coorY] != '║' && map[coorX, coorY] != '╚' && map[coorX, coorY] != '╔' &&  map[coorX, coorY] != '╗' && map[coorX, coorY] != '╝' && map[coorX, coorY] != '═' && map[coorX, coorY] != '╩' && map[coorX, coorY] != '╠' && map[coorX, coorY] != '╣' && map[coorX, coorY] != '╬' && map[coorX, coorY] != '╦')
                {
                    X = coorX;
                    Y = coorY;
                    break;
                }               
            }           
            Vision = Convert.ToInt32(heroStats[8]);
        }
        
        public bool LiveOrDie()
        {
            if (HP <= 0)
            {
                return false;
            } 
            else
            {
                return true;
            }               
        }
        public bool CheckWall(string direction, char[,] map) // проверка на движение
        {
            char[] walls =  {'║','╚','╔','╗','╝','.', '═'}; 
            bool flag = false;
            for (int i = 0 ; i < 7; i++)
            {
                if (direction == "left")
                    if (map[this.X-1, this.Y] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
                else if (direction == "right")
                    if (map[this.X+1, this.Y] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
                else if (direction == "up")
                    if (map[this.X, this.Y-1] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
                else if (direction == "down")
                    if (map[this.X, this.Y+1] != walls[i])
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
            }

            return flag;

        }
        public void ShowStats() // Показ статов
        {
            Console.WriteLine($"Role: {Role}, Name: {Name}, Hill points: {HP}, Damage: {Damage}, Armor: {Armor}, Vision: {Vision}, X:{X}, Y:{Y}");           
        }
        public void GetDamage(int damage) // Получениe урона
        {
            HP -= damage - (Armor / 100);
        }
    }

    class Hero : Character
    {
       
        public Hero(string[] heroStats)
        
            : base(heroStats)
            {

            }
  
        public void HeroAction(ConsoleKeyInfo charKey, char[,] map, int enemyX, int enemyY)
        {
            
                switch (charKey.Key)
                {
                    case ConsoleKey.LeftArrow:
                    if (CheckWall("left", map) == true)
                        {
                        this.X -= 1;
                        map[this.X+1, this.Y] = ' '; 
                        map[this.X, this.Y] = '.';
                        }
                    break;

                    case ConsoleKey.RightArrow:
                    if (CheckWall("right", map) == true)
                    {
                        this.X += 1;
                        map[this.X-1, this.Y] = ' '; 
                        map[this.X, this.Y] = '.';
                        }
                    break;
                    
                    case ConsoleKey.UpArrow:
                    if (CheckWall("up", map) == true)
                    {
                        this.Y -= 1;
                        map[this.X, this.Y+1] = ' '; 
                        map[this.X, this.Y] = '.';
                        }
                        
                    break;

                    case ConsoleKey.DownArrow:
                    if (CheckWall("down", map) == true)
                    {
                        this.Y += 1;
                        map[this.X, this.Y-1] = ' '; 
                        map[this.X, this.Y] = '.';
                        }
                    break;
                    

                }
        }
    }
    
    class EnemySad : Character // Легкий враг =======================================================
    {       
        public EnemySad(string[] heroStats, int mapX, int mapY, char[,] map)
            : base(heroStats, mapX, mapY, map)
            {
            }      

        public void YaZametilTebyaBegiTvar(int heroX, int heroY, char[,] map)
            {
                int MoveToHero= 0;
                double num = ((this.X-heroX)*(this.X-heroX) + (this.Y-heroY)*(this.Y-heroY));
                double dist = Math.Sqrt(num);
                double radius = Convert.ToDouble(this.Vision);

                if (this.X > heroX && this.Y > heroY )
                    MoveToHero = 1; // move left up
                else if (this.X > heroX && this.Y < heroY )
                    MoveToHero = 2; // move left down
                else if (this.X < heroX && this.Y > heroY )
                    MoveToHero = 3; // move right up
                else if (this.X < heroX && this.Y < heroY )
                    MoveToHero = 4; // move right down
                // Coordinate equal
                else if (this.X == heroX && this.Y < heroY )
                    MoveToHero = 5; // move down
                else if (this.X == heroX && this.Y > heroY )
                    MoveToHero = 6; // move up
                else if (this.X < heroX && this.Y == heroY )
                    MoveToHero = 7; // move right
                else if (this.X > heroX && this.Y == heroY )
                    MoveToHero = 8; // move left
               

                if (dist < radius)
                {
                Console.SetCursorPosition(X, Y - 1);
                Console.Write(HP);
                Console.SetCursorPosition(X, Y);

                switch (MoveToHero)
                    {
                        case 1:
                        if (CheckWall("left", map))
                        {
                            this.X -= 1;
                            map[this.X+1, this.Y] = ' '; 
                            map[this.X, this.Y] = '.';
                        } else if(CheckWall("up", map))
                        {
                            this.Y -= 1;
                            map[this.X, this.Y+1] = ' '; 
                            map[this.X, this.Y] = '.';
                        }
                        break;

                        case 2:
                        if (CheckWall("left", map))
                        {
                            this.X -= 1;
                            map[X+1, Y] = ' '; 
                            map[this.X, this.Y] = '.';
                        } else if(CheckWall("down", map))
                        {
                            this.Y += 1;
                            map[this.X, this.Y-1] = ' '; 
                            map[this.X, this.Y] = '.';
                        }
                        break;

                        case 3:
                        if (CheckWall("right", map))
                        {
                            this.X += 1;
                            map[this.X-1, this.Y] = ' '; 
                            map[this.X, this.Y] = '.';
                        } else if(CheckWall("up", map))
                        {
                            this.Y -= 1;
                            map[this.X, this.Y+1] = ' '; 
                            map[this.X, this.Y] = '.';
                        }
                        break ;

                        case 4:
                        if (CheckWall("right", map))
                        {
                            this.X += 1;
                            map[this.X-1, this.Y] = ' '; 
                            map[this.X, this.Y] = '.';
                        } else if(CheckWall("down", map))
                        {
                            this.Y += 1;
                            map[this.X, this.Y-1] = ' '; 
                            map[this.X, this.Y] = '.';
                        }
                        break;

                        case 5:
                        if (CheckWall("down", map))
                        {
                            this.Y += 1;
                            map[this.X, this.Y-1] = ' '; 
                            map[this.X, this.Y] = '.';
                        } 
                        break;

                        case 6:
                        if (CheckWall("up", map))
                        {
                            this.Y -= 1;
                            map[this.X, this.Y+1] = ' '; 
                            map[this.X, this.Y] = '.';
                        }
                        break;

                        case 7:
                        if (CheckWall("right", map))
                        {
                            this.X += 1;
                            map[this.X-1, this.Y] = ' '; 
                            map[this.X, this.Y] = '.';
                        } 
                        break;

                        case 8:
                        if (CheckWall("left", map))
                        {
                            this.X -= 1;
                            map[this.X+1, this.Y] = ' '; 
                            map[this.X, this.Y] = '.';
                        }
                        break;
                        
                    }
                }
                
            }
    }


    class EnemyAngry: Character
    {
        public EnemyAngry(string[] heroStats, int mapX, int mapY, char[,] map)
            : base(heroStats, mapX, mapY, map)
        {

        }

        
        public void YaZametilTebyaBegiTvar(int heroX, int heroY, char[,] map)
        {

            int MoveToHero = 0;
            double num = ((this.X - heroX) * (this.X - heroX) + (this.Y - heroY) * (this.Y - heroY));
            double dist = Math.Sqrt(num);
            double radius = Convert.ToDouble(this.Vision);

            if (this.X > heroX && this.Y > heroY)
                MoveToHero = 1; // move left up
            else if (this.X > heroX && this.Y < heroY)
                MoveToHero = 2; // move left down
            else if (this.X < heroX && this.Y > heroY)
                MoveToHero = 3; // move right up
            else if (this.X < heroX && this.Y < heroY)
                MoveToHero = 4; // move right down
                                // Coordinate equal
            else if (this.X == heroX && this.Y < heroY)
                MoveToHero = 5; // move down
            else if (this.X == heroX && this.Y > heroY)
                MoveToHero = 6; // move up
            else if (this.X < heroX && this.Y == heroY)
                MoveToHero = 7; // move right
            else if (this.X > heroX && this.Y == heroY)
                MoveToHero = 8; // move left


            if (dist < radius)
            {
                Console.SetCursorPosition(X, Y - 1);
                Console.Write(HP);
                Console.SetCursorPosition(X, Y);
                switch (MoveToHero)
                {
                    case 1:
                        if (CheckWall("left", map))
                        {
                            this.X -= 1;
                            map[this.X + 1, this.Y] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        else if (CheckWall("up", map))
                        {
                            this.Y -= 1;
                            map[this.X, this.Y + 1] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                    case 2:
                        if (CheckWall("left", map))
                        {
                            this.X -= 1;
                            map[X + 1, Y] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        else if (CheckWall("down", map))
                        {
                            this.Y += 1;
                            map[this.X, this.Y - 1] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                    case 3:
                        if (CheckWall("right", map))
                        {
                            this.X += 1;
                            map[this.X - 1, this.Y] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        else if (CheckWall("up", map))
                        {
                            this.Y -= 1;
                            map[this.X, this.Y + 1] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                    case 4:
                        if (CheckWall("right", map))
                        {
                            this.X += 1;
                            map[this.X - 1, this.Y] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        else if (CheckWall("down", map))
                        {
                            this.Y += 1;
                            map[this.X, this.Y - 1] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                    case 5:
                        if (CheckWall("down", map))
                        {
                            this.Y += 1;
                            map[this.X, this.Y - 1] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                    case 6:
                        if (CheckWall("up", map))
                        {
                            this.Y -= 1;
                            map[this.X, this.Y + 1] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                    case 7:
                        if (CheckWall("right", map))
                        {
                            this.X += 1;
                            map[this.X - 1, this.Y] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                    case 8:
                        if (CheckWall("left", map))
                        {
                            this.X -= 1;
                            map[this.X + 1, this.Y] = ' ';
                            map[this.X, this.Y] = '.';
                        }
                        break;

                }
            }

        }
    }
}
  

        


