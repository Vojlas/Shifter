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
    class shift_model
    {
        //public int post { get; set; }
        //public DateTime shiftStarts { get; set; }
        //public DateTime shiftEnds { get; set; }
        public Post_model post {get; set;}
        public Date date { get; set; }
        public double hours{ get; set; }
        public string location { get; set; }
        public string employeID { get; set; }

        public shift_model() { }
        public shift_model(Post_model post, Date date, double hours, string location, string employeID) {
            this.post = post;
            this.date = date;
            this.hours = hours;
            this.location = location;
            this.employeID = employeID;
        }
        public void fill(Post_model post, Date date, double hours, string location, string employeID) {
            this.post = post;
            this.date = date;
            this.hours = hours;
            this.location = location;
            this.employeID = employeID;
        }

     
    }
}
