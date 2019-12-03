using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Riders.Data
{
    public class RidersContext : DbContext
    {
        public RidersContext (DbContextOptions<RidersContext> options): base(options)
        {

        }

        public DbSet<Riders.Models.Ride> Ride { get; set; }
    }
}
