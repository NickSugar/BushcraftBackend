using Microsoft.EntityFrameworkCore;
using bushcraftAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bushcraftAPI
{
    public class BushcraftApiContext : DbContext
    {
        public BushcraftApiContext(DbContextOptions options)
            : base() { }

        public DbSet<GearEntity> Gear { get; set; }
    }
}
