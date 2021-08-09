using Shifter;
using Shifter.Controllers;
using Shifter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shifter_v1
{
    class Program
    {
        static void Main(string[] args)
        {
            pager p = new pager();

            while (true)
            {
                Logo();
                p.mainMenu();
                p.mainMenu(p.reader()); 
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void Logo()
        {
            Console.WriteLine(@"  ____    _       _    __   _                 ");
            Console.WriteLine(@" / ___|  | |__   (_)  / _| | |_    ___   _ __ ");
            Console.WriteLine(@" \___ \  | '_ \  | | | |_  | __|  / _ \ | '__|");
            Console.WriteLine(@"  ___) | | | | | | | |  _| | |_  |  __/ | |   ");
            Console.WriteLine(@" |____/  |_| |_| |_| |_|    \__|  \___| |_|   ");
            Console.WriteLine(@"                     (c)2021 Vojtěch Pavlas   " + "\n");
        }
    }
}

//FileController fc = new FileController();
//List<WorkPlace> ps = new List<WorkPlace>();
//ps = fc.loadWorkPlaces_Alternative();
//foreach (WorkPlace item in ps)
//{
//    Console.WriteLine(item.Name + ":\n-----------");
//    foreach (Post_model post in item.posts)
//    {
//        Console.WriteLine(post.Place + " " + post.ShiftBegin.Show() + " - " + post.ShiftEnd.Show());
//    }
//}