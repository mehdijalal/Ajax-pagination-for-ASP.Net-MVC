using MJApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MJApp.Context
{
    public class MYDBContext:DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<ProjectProvRel> ProRel { get; set; }
    }
}