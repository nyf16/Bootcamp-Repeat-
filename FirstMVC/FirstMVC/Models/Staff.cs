using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstMVC.Models
{
    public class Staff
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool Gender { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        public static List<Staff> GetSampleData()
        {
            List<Staff> sampleData = new List<Staff>();
            sampleData.Add(new Staff
            {
                Id = 1,
                Name = "Nejat",
                LastName = "Biyediç",
                Gender = false,
                JobTitle = "Legend",
                Address = "Mostar/Bosna-Hersek",
                StartDate = Convert.ToDateTime("06.01.1986"),
                BirthDate = Convert.ToDateTime("09.24.1959"),
                Salary = 16000,
                ImageUrl = "https://holiganspor.com/wp-content/uploads/2019/08/4bd4540e-9435-48c5-8446-3e6993e1fe83.jpg"
            });

            sampleData.Add(new Staff
            {
                Id = 2,
                Name = "Battalla",
                LastName = "Pablo Martin",
                Gender = false,
                JobTitle = "Captain",
                Address = "Nilüfer/Bursa",
                StartDate = Convert.ToDateTime("06.01.2009"),
                BirthDate = Convert.ToDateTime("01.16.1984"),
                Salary = 160000,
                ImageUrl = "https://iasbh.tmgrup.com.tr/532283/752/395/0/0/800/420?u=https://isbh.tmgrup.com.tr/sbh/2018/05/11/bursasporda-batalladan-flas-karar-1526027461860.jpg"

            });

            sampleData.Add(new Staff
            {
                Id = 3,
                Name = "Belluschi",
                LastName = "Fernando",
                Gender = false,
                JobTitle = "Midfielder",
                Address = "Osmangazi/Bursa",
                StartDate = Convert.ToDateTime("06.01.2012"),
                BirthDate = Convert.ToDateTime("09.10.1983"),
                Salary = 16000,
                ImageUrl = "https://i2.milimaj.com/i/milliyet/75/0x410/5c8d3f4245d2a05010d4c7b2.jpg"
            });

            return sampleData;

        }
    }
}
