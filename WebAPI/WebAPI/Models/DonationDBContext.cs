using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    //public class DonationDBContext : DbContext
    public class DonationDBContext : IdentityDbContext<IdentityUser>
    {
        public DonationDBContext(DbContextOptions<DonationDBContext> options):base(options)
        {
            Program.Logger?.LogInformation("DonationDBContext model");
        }

        public DbSet<DCandidate> DCandidates { get; set; }
    }

}
