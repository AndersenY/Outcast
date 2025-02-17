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
    internal class Music
    {
        public static void Timer(object Sound)
        {
            bool sound = Convert.ToBoolean(Sound);
            string[] file_time = File.ReadAllLines("Music/time.txt");

            Random rand = new Random();
            int num_of_file = rand.Next(0, file_time.Length);
            SoundPlayer sound1;
            int time = 0;
            int timer = 0;
            var sw = new Stopwatch();

            while (true)
            {
                int temp = num_of_file;

                while (num_of_file == temp)
                    num_of_file = rand.Next(0, file_time.Length);

                if (sound == true)
                    sound1 = new SoundPlayer($"Music/{num_of_file}.wav");
                else
                    sound1 = new SoundPlayer($"Music/nothing.wav");

                sound1.Play();
                time = Convert.ToInt32(file_time[num_of_file]);
                timer = 0;

                while (timer < time)
                {
                    sw.Restart();
                    Thread.Sleep(1);
                    sw.Stop();

                    timer += Convert.ToInt32(sw.ElapsedMilliseconds);
                }
                sound1.Stop();


            }

        }
    }
}
