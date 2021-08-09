using System;
using System.Text.RegularExpressions;

namespace Shifter.Models
{
    public class Time
    {
        public double Hours { get; set; }
        public byte Minutes { get; set; }
        public byte Seconds { get; set; }
        public Time() {
            this.Hours = 0;
            this.Minutes = 0;
            this.Seconds = 0;
        }
        public Time(double _hours, byte _minutes, byte _seconds)
        {
            this.Hours = _hours;
            this.Minutes = _minutes;
            this.Seconds = _seconds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">in Format "HH:MM:SS"</param>
        public Time(string time)
        {
            if (time !=null)
            {
                Regex regex = new Regex(@"^\d+:\d+:\d+$");
                if (!regex.IsMatch(time)) throw new FormatException("Wrong time format!");
                //Time parts -> hh:mm:ss -> timeParts[0] -> hours; timeParts[1] -> minutes; timeParts[2] -> seconds
                string[] timeParts = time.Split(':');
                try
                {
                    this.Hours = Convert.ToDouble(timeParts[0]);
                    this.Minutes = Convert.ToByte(timeParts[1]);
                    this.Seconds = Convert.ToByte(timeParts[2]);
                }
                catch (Exception)
                {
                    throw new FormatException("Not number in time format!");
                } 
            }
        }
        public Time AddSec()
        {
            if ((this.Seconds += 1) < 60) return this;
            if ((this.Minutes += 1) < 60) { this.Seconds = 2; return this; }
            this.Hours++; this.Minutes = 0; this.Seconds = 0; return this;
        }
        public Time Add(Time time)
        {
            //add times together
            this.Seconds += time.Seconds;
            this.Minutes += time.Minutes;
            this.Hours += time.Hours;

            //Redistribute Hours, minutes, seconds according to time rules
            //max addition is 60+59 = 119 -> so add {0,1}
            if (this.Seconds >= 60) { this.Seconds = (byte)(this.Seconds % 60); this.Minutes++; }
            if (this.Minutes >= 60) { this.Minutes = (byte)(this.Minutes % 60); this.Hours++; }
            return this;
        }
        public string Show()
        {
            string time = "";
            time = this.Hours <= 9 ? "0" + this.Hours.ToString() : this.Hours.ToString();
            time += ":";
            time += this.Minutes <= 9 ? "0" + this.Minutes.ToString() : this.Minutes.ToString();
            time += ":";
            time += this.Seconds <= 9 ? "0" + this.Seconds.ToString() : this.Seconds.ToString();

            return time;//this.Hours + ":" + this.Minutes + ":" + this.Seconds;
        }

        public Time subtract(Time t2) {
            //sub times together
            int s = (int)this.Seconds - (int)t2.Seconds;
            int m = (int)this.Minutes - (int)t2.Minutes;
            int h = (int)this.Hours - (int)t2.Hours;

            //Redistribute Hours, minutes, seconds according to time rules
            //max subtraction is 0-59 = -59 -> so sub {0,1}
            if (this.Seconds < 0) { this.Seconds = (byte)(this.Seconds % 60); this.Minutes--; }
            if (this.Minutes < 0) { this.Minutes = (byte)(this.Minutes % 60); this.Hours--; }
            return new Time(h + ":" + m + ":" + s);
        }

        /// <summary>
        /// Ignores seconds
        /// </summary>
        /// <returns>decimal value</returns>
        public double ToHours() {
            return (double)this.Hours + ((double)this.Minutes / 60.0);
        }
    }

    
}