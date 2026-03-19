using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Infrastructure.DBconnect
{
    public class AppDBconnect(DbContextOptions<AppDBconnect> options) : DbContext(options)
    {
       public DbSet<User> Users {  get; set; }
        public DbSet<EUserNotification> EUsers { get; set; }
    }
}
