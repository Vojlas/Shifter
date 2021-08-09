using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shifter.Models
{
    class WorkPlace
    {
        public string Name { get; set; }
        public List<Post_model> posts { get; set; }

        public WorkPlace() { }
        public WorkPlace(string _name, List<Post_model> _posts) {
            this.Name = _name;
            this.posts = _posts;
        }
    }
}
