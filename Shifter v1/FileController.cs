using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Shifter.Models;
using Shifter_v1.Models;

namespace Shifter.Controllers
{
    class FileController
    {
        public const string basePath = @"C:\Users\Vojtech\Desktop\shifterV1\";

        public FileController() {
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
            if (!Directory.Exists(basePath+"employes")) Directory.CreateDirectory(basePath + "employes");
            if (!Directory.Exists(basePath+"shifts")) Directory.CreateDirectory(basePath + "shifts");
            if (!Directory.Exists(basePath + "workplaces")) Directory.CreateDirectory(basePath + "workplaces"); 
        }

        public void saveEmploye(Employe_model employe) {
            List<Employe_model> employes = new List<Employe_model>();

            List<Employe_model> emp = loadEmployes();
            if (emp != null) {
                foreach (Employe_model item in emp)
                {
                    employes.Add(item);
                }                             
            }
            employes.Add(employe);

            var xOrders = new XElement("employes",
            from o in employes
            select new XElement("employe",
                new XAttribute("id", o.id),
                new XAttribute("firstName", o.firstName),
                new XAttribute("middleName", o.middleName),
                new XAttribute("lastName", o.lastName)));
                    xOrders.Save(basePath+ @"employes\employes.xml");
        }

        public List<Employe_model> loadEmployes() {
            if (!File.Exists(basePath + @"employes\employes.xml")) return null;
            var xml = XDocument.Load(basePath + @"employes\employes.xml");

            IEnumerable<Employe_model> query = from c in xml.Root.Descendants("employe")
                        select new Employe_model()
                        {
                            id = (string)c.Attribute("id"),
                            firstName = (string)c.Attribute("firstName"),
                            middleName = (string)c.Attribute("middleName"),
                            lastName = (string)c.Attribute("lastName")
                        };

            List<Employe_model> emp = query.ToList();
            return emp;
        }

        public void saveWorkPlace(WorkPlace workPlace) {
            var xOrders = new XElement("Workplace",
                new XAttribute("name", workPlace.Name),
                new XElement("Posts",
                from o in workPlace.posts
                select new XElement("Post",
                    new XAttribute("name", o.Place),
                    new XAttribute("ShiftStart", o.ShiftBegin.Show()),
                    new XAttribute("ShiftEnd", o.ShiftEnd.Show()))));

            xOrders.Save(basePath + @"workplaces\" + workPlace.Name.Replace(" ", String.Empty).Trim() + ".xml");
        }

        public List<WorkPlace> loadWorkPlaces() {
            throw new Exception("Forbidden Method");
            string[] files = Directory.GetFiles(basePath + @"workplaces\");

            List<WorkPlace> places = new List<WorkPlace>();

            foreach (string path in files)
            {
                var xml = XDocument.Load(path);
                var query = from c in xml.Root.Descendants("Workplace")
                                  select new WorkPlace()
                                  {
                                      Name = (string)c.Attribute("name"),
                                      posts = (from x in c.Descendants("Post")
                                              select new Post_model
                                              {
                                                  Place = (String)x.Attribute("name"),
                                                  ShiftBegin = new Time((String)x.Attribute("ShiftStart")),
                                                  ShiftEnd = new Time((String)x.Attribute("ShiftEnd"))
                                              }).ToList()                                                   
                                  };
                places = query.ToList();
            }
            return places;
        }

        public List<WorkPlace> loadWorkPlaces_Alternative()
        {
            string[] files = Directory.GetFiles(basePath + @"workplaces\");
            List<WorkPlace> wp = new List<WorkPlace>();

            foreach (string path in files)
            {
                var xml = XDocument.Load(path);
                var queryNames = (from c in xml.Root.Descendants("Post")
                                  select new Post_model
                                  {
                                      Place = (String)c.Attribute("name"),
                                      ShiftBegin = new Time((String)c.Attribute("ShiftStart")),
                                      ShiftEnd = new Time((String)c.Attribute("ShiftEnd"))
                                  }).ToList();

                string queryWorkName = (string)xml.Elements("Workplace").Select(x => x.Attribute("name")).SingleOrDefault().Value;
                wp.Add(new WorkPlace(queryWorkName, queryNames.ToList()));
            }
            return wp;
        }

        public void SaveShift(Employe_model employe, shift_model shift)
        {
            if (!Directory.Exists(basePath + @"shifts\" + employe.id)) { Directory.CreateDirectory(basePath + @"shifts\" + employe.id); }

            List<shift_model> AllShifts = new List<shift_model>();
            List<shift_model> emp = loadShifts(employe, shift.date);
            if (emp != null)
            {
                foreach (shift_model item in emp)
                {
                    AllShifts.Add(item);
                }
            }
            AllShifts.Add(shift);

            var xOrders = new XElement("Shifts",
            from o in AllShifts
            select new XElement("shiftInfo",
                new XAttribute("date", o.date.Day+"-"+ o.date.Month+"-"+ o.date.Year),
                new XAttribute("hours", o.hours),
                new XAttribute("location", o.location),
                new XAttribute("EmployeID", o.employeID),
                    
                new XElement("post",
                    new XAttribute("name", o.post.Place),
                    new XAttribute("start", o.post.ShiftBegin.Show()),
                    new XAttribute("end", o.post.ShiftEnd.Show()))));
            string path = basePath + @"shifts\" + employe.id + @"\" + "Shifts_" + shift.date.Year +"-"+ shift.date.Month + ".xml";
            xOrders.Save(path);
            Console.WriteLine("saved");
            System.Threading.Thread.Sleep(1);
            Console.Clear();
        }

        public List<shift_model> loadShifts(Employe_model employe, Date date)
        {
            if (!File.Exists(basePath + @"shifts\" + employe.id + @"\" + "Shifts_" + date.Year + "-" + date.Month + ".xml")) return null;
            List<shift_model> wp = new List<shift_model>();

            var xml = XDocument.Load(basePath + @"shifts\" + employe.id + @"\" + "Shifts_" + date.Year + "-" + date.Month + ".xml");

            var queryNames = (from c in xml.Root.Descendants("shiftInfo")
                                select new shift_model
                                {
                                    date = new Date((string)c.Attribute("date")),
                                    hours = (double)c.Attribute("hours"),
                                    location = (string)c.Attribute("location"),
                                    employeID = (string)c.Attribute("EmployeID"),
                                    
                                    post = (from p in c.Elements()
                                           select new Post_model { 
                                                Place = (string)p.Attribute("name"),
                                                ShiftBegin = new Time((string)p.Attribute("start")),
                                                ShiftEnd = new Time((string)p.Attribute("end")),
                                           }).First()
                                }).ToList();

            wp = (queryNames.ToList());

            return wp;
        }
        public List<shift_model> loadShifts(string path)
        {
            List<shift_model> wp = new List<shift_model>();

            var xml = XDocument.Load(path);

            var queryNames = (from c in xml.Root.Descendants("shiftInfo")
                              select new shift_model
                              {
                                  date = new Date((string)c.Attribute("date")),
                                  hours = (double)c.Attribute("hours"),
                                  location = (string)c.Attribute("location"),
                                  employeID = (string)c.Attribute("EmployeID"),

                                  post = (from p in c.Elements()
                                          select new Post_model
                                          {
                                              Place = (string)p.Attribute("name"),
                                              ShiftBegin = new Time((string)p.Attribute("start")),
                                              ShiftEnd = new Time((string)p.Attribute("end")),
                                          }).First()
                              }).ToList();

            wp = (queryNames.ToList());

            return wp;
        }

        public List<shift_model> LoadAllShifts(Employe_model e, Date from = null, Date to = null) {
            string[] all = Directory.GetFiles(basePath + @"shifts\" + e.id + @"\");

            List<shift_model> allShifts = new List<shift_model>();
            List<shift_model> tmp = new List<shift_model>();
            foreach (string shiftFile in all)
            {
                allShifts.AddRange(this.loadShifts(shiftFile));
            }
            if (from == null && to == null) return allShifts;

            //To logic
            if (from == null)
            {
                foreach (shift_model item in allShifts)
                {
                    if (item.date.Day <= to.Day && item.date.Month <= to.Month && item.date.Year <= to.Year)
                    {
                        tmp.Add(item); 
                    }
                }
                allShifts.Clear(); allShifts.AddRange(tmp); tmp.Clear(); 
            }

            //From logic
            if (to == null)
            {
                foreach (shift_model item in allShifts)
                {
                    if (item.date.Day >= from.Day && item.date.Month >= from.Month && item.date.Year >= from.Year)
                    {
                        tmp.Add(item);
                    }
                }
                allShifts.Clear(); allShifts.AddRange(tmp); tmp.Clear();
            }

            return allShifts;
        }

        //public List<shift_model> shifts(Employe_model e) {
        //    string[] files = Directory.GetFiles(basePath + @"shifts\" + e.id);

        //    foreach (string pat in files)
        //    {
        //        var xml = XDocument.Load(pat);
        //        IEnumerable<Employe_model> query = from c in xml.Root.Descendants("employe")
        //                                           select new Employe_model()
        //                                           {
        //                                               id = (string)c.Attribute("id"),
        //                                               firstName = (string)c.Attribute("firstName"),
        //                                               middleName = (string)c.Attribute("middleName"),
        //                                               lastName = (string)c.Attribute("lastName")
        //                                           };
        //    }
        //}



    }
}
