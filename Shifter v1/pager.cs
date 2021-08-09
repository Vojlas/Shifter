using Shifter.Controllers;
using Shifter.Models;
using Shifter_v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shifter
{
    class pager
    {
        public WorkPlace TMPWorkPlace { get; set; }
        public int reader() {
            Console.Write("::");
            string x = Console.ReadLine();
            try
            {
                return Convert.ToInt32(x);
            }
            catch (Exception)
            {
                Console.WriteLine("invalid!!");
                return this.reader();
            }
        }
        public void mainMenu() {
            Console.WriteLine("1) Login");
            Console.WriteLine("2) Create new employe");
        }
        public void mainMenu(int x)
        {
            
            if (x == 1) { Console.Clear(); this.loadEmployes(); return; }
            if (x == 2) { Console.Clear(); this.createEmploye(); return; }
        }

        private void loadEmployes()
        {
            FileController fc = new FileController();
            if (fc.loadEmployes() != null)
            {
                List<Employe_model> employes = new List<Employe_model>(fc.loadEmployes());
                int i = 1;
                foreach (Employe_model emp in employes)
                {
                    Console.WriteLine(i + ")" + emp.firstName + " " + emp.lastName);
                    i++;
                }
                EmployeMenu(employes.ElementAt(this.reader()-1));
            }            
        }

        private void createEmploye()
        {
            bool con = true; string fname = ""; string mname = ""; string lname = "";
            do
            {
                Console.Clear();
                Console.WriteLine("New Employe:\n-------");
                Console.Write("first name: "); fname = Console.ReadLine();
                Console.Write("middle name: "); mname = Console.ReadLine();
                Console.Write("last name: "); lname = Console.ReadLine();

                Console.WriteLine("\n Is "+ fname + (mname == "" ? "" : mname) + " " + lname + " your name (y/n)");
                string x = Console.ReadLine();
                x.ToLower();
                if (x == "y") con = false;
            } while (con);

            Employe_model employe = new Employe_model();
            employe.fill(fname, lname, mname);
            employe.saveEmploye();
            Console.Clear();
        }

        private void EmployeMenu(Employe_model employe_model)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1) New Shift");
                Console.WriteLine("2) Shift schedule");
                Console.WriteLine("3) Exit");

                int i = this.reader();
                switch (i)
                {
                    case 1:
                        NewShift(employe_model);
                        break;
                    case 2:
                        ShowShifts(employe_model);
                        break;
                    case 3:
                        Console.Clear();
                        return;
                } 
            } while (true);
        }

        private void ShowShifts(Employe_model employe_model)
        {
            Console.Clear();
            Console.WriteLine("Shifts:\n------------------");
            FileController fc = new FileController();

            List<shift_model> sf = fc.LoadAllShifts(employe_model); //fc.loadShifts(employe_model, new Date("19-06-2021"));

            foreach (shift_model item in sf)
            {
                Console.WriteLine(item.date.Show() + " - " + item.post.Place + "\t("+item.post.ShiftBegin.Show()+" => " + item.post.ShiftEnd.Show()+ ")");
            }
            Console.Write("END\n::");Console.ReadKey();
            Console.Clear();
        }

        public void NewShift(Employe_model e)
        {
            Console.Clear();
            Console.WriteLine("Date of your shift: dd-mm-YYYY");
            Console.Write("::"); string date = Console.ReadLine();
            Date shiftDate = new Date(date);

            Console.WriteLine("Select your work space:");
            FileController fc = new FileController();
            List<WorkPlace> wc = fc.loadWorkPlaces_Alternative();
            int i = 1;
            foreach (WorkPlace item in wc)
            {
                Console.WriteLine(i + ") " + item.Name);
                i++;
            }
            pager pager = new pager();
            i = pager.reader();
            WorkPlace work = wc.ElementAt(i - 1);
            Console.WriteLine("Selecet your post:");

            List<Post_model> ps = work.posts;
            i = 1;
            foreach (Post_model item in ps)
            {
                Console.WriteLine(i+") "+item.Place);
                i++;
            }
            i = pager.reader();
            Post_model post = ps.ElementAt(i - 1);
            Console.WriteLine("Is your shift standart?\nOpens: "+ post.ShiftBegin.Show() +"\nClosing: " + post.ShiftEnd.Show() + "\n(Y/N)");
            Console.Write("::");string x = Console.ReadLine();
            if (x.ToLower() == "y")
            {
                Time hours = post.ShiftEnd.subtract(post.ShiftBegin);
                fc.SaveShift(e, new shift_model(ps.ElementAt(i - 1), shiftDate, hours.ToHours(), work.Name, e.id));
            }
            else
            {
                Console.Write("Shift starts: (hh:mm)\n:: ");
                Time start = new Time(Console.ReadLine() + ":00");
                Console.Write("Shift ends: (hh:mm)\n:: ");
                Time end = new Time(Console.ReadLine() + ":00");


                Time hours = end.subtract(start);
                Post_model editedPost = new Post_model(ps.ElementAt(i - 1));
                editedPost.ShiftBegin = start;
                editedPost.ShiftEnd = end;
                fc.SaveShift(e, new shift_model(editedPost, shiftDate, hours.ToHours(), work.Name, e.id));
            }
        }
    }
}
