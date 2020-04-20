using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }


        public static List<Product> GetSampleData()
        {
            var urun1 = new Product
            {
                Id = 1,
                ImageUrl = "https://productimages.hepsiburada.net/s/23/280-413/10030249115698.jpg",
                Name = "SteelSeries Arctis 3 Siyah (2019 Edition) Oyuncu Kulaklık",
                Price = 499.00
            };

            var urun2 = new Product
            {
                Id = 2,
                ImageUrl = "https://productimages.hepsiburada.net/s/23/280-413/10030249443378.jpg",
                Name = "SteelSeries Arctis 5 Siyah (2019 Edition) RGB Oyuncu Kulaklık",
                Price = 749.00
            };

            var urun3 = new Product
            {
                Id = 3,
                ImageUrl = "https://productimages.hepsiburada.net/s/23/280-413/10030249771058.jpg",
                Name = "SteelSeries Arctis 7 Siyah (2019 Edition) Wireless Oyuncu Kulaklık",
                Price = 1.199

            };

            var urun4 = new Product
            {
                Id = 4,
                ImageUrl = "https://productimages.hepsiburada.net/s/19/280-413/9827671867442.jpg",
                Name = "SteelSeries Arctis Pro Wireless Hi-Res Oyuncu Kulaklığı",
                Price = 3.299
            };

            List<Product> sampleData = new List<Product>();
            sampleData.Add(urun1);
            sampleData.Add(urun2);
            sampleData.Add(urun3);
            return sampleData;
        }
    }
}
