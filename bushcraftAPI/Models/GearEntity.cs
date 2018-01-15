using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bushcraftAPI.Models
{
    public class GearEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public int HitPoints { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Volume { get; set; }

        public int StorageVolume { get; set; }
    }
}
