using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shifter_v1.Models
{
    class Date
    {
        public byte Day { get; set; }
        public byte Month { get; set; }
        public int Year { get; set; }

        public Date() { }
        public Date(byte day, byte month, int year) {
            this.Day = day;
            this.Month = month;
            this.Year = year;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">dd-mm-yyyy</param>
        /// <param name="delimiter"></param>
        public Date(string date, char delimiter = '-')
        {
            string[] d = date.Split(delimiter);
            this.Day = Convert.ToByte(d[0]);
            this.Month = Convert.ToByte(d[1]);
            this.Year = Convert.ToInt32(d[2]);
        }

        public string Show(string delimiter = "-") {
            return (this.Day <= 9 ? "0"+this.Day.ToString() : this.Day.ToString()) + delimiter + (this.Month <= 9 ? "0" + this.Month.ToString() : this.Month.ToString()) + delimiter + this.Year;
        }
    }
}
