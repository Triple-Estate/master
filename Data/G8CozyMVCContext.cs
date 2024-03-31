using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using G8Cozy.Models;
using G8CozyMVC.Models;

namespace G8CozyMVC.Data
{
    public class G8CozyMVCContext : DbContext
    {
        public G8CozyMVCContext (DbContextOptions<G8CozyMVCContext> options)
            : base(options)
        {
        }

        public DbSet<G8Cozy.Models.user> user { get; set; } = default!;
        public DbSet<G8Cozy.Models.home> home { get; set; } = default!;
        public DbSet<G8Cozy.Models.agent> agent { get; set; } = default!;
        public DbSet<G8Cozy.Models.agentuser> agentuser { get; set; } = default!;
        public DbSet<G8Cozy.Models.notification> notification { get; set; } = default!;
        public DbSet<G8CozyMVC.Models.buy> buy { get; set; } = default!;
    }
}
