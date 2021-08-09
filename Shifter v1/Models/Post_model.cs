using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shifter.Models
{
    class Post_model
    {
        public string Place { get; set; }
        public Time ShiftBegin { get; set; }
        public Time ShiftEnd { get; set; }

        public Post_model(string place, Time shiftBegin, Time shiftEnd) {
            this.Place = place;
            this.ShiftBegin = shiftBegin;
            this.ShiftEnd = shiftEnd;
        }

        public Post_model(Post_model p)
        {
            this.Place = p.Place;
            this.ShiftBegin = p.ShiftBegin;
            this.ShiftEnd = p.ShiftEnd;
        }
        public Post_model()
        {
        }
    }
}
