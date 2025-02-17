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
    internal class Animation
    {
        public string Name_of_files { get; set; }
        public int Iterations_count { get; set; }
        public List<string> Items { get; set; }

        public int Index { get; set; }

        public Animation(string name_of_files, int iteration_count, List<string> items, int index)
        {
            this.Name_of_files = name_of_files;
            this.Iterations_count = iteration_count;
            this.Items = items;
            Index = index;
        }
    }
}
