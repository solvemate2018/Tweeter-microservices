using GatewayService.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace GatewayService
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
    }
}
