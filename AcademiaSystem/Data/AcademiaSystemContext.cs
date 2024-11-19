using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AcademiaSystem.Models;

namespace AcademiaSystem.Data
{
    public class AcademiaSystemContext : DbContext
    {
        public AcademiaSystemContext (DbContextOptions<AcademiaSystemContext> options)
            : base(options)
        {
        }

        public DbSet<AcademiaSystem.Models.ClienteModel> ClienteModel { get; set; } = default!;
    }
}
