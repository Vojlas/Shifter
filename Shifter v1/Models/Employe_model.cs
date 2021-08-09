using Shifter.Controllers;

namespace Shifter
{
    public class Employe_model
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }

        public void fill(string firstName, string lastName, string middleName = "") {
            this.firstName = firstName;
            this.middleName = middleName;
            this.lastName = lastName;

            this.id = System.Guid.NewGuid().ToString("B").ToUpper();
        }

        public void saveEmploye() {
            FileController fc = new FileController();
            fc.saveEmploye(this);
        }
    }
}