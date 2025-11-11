using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPythone.AppData.Models;

namespace WpfAppPythone.AppData
{
    public class AppDBContext:DbContext
    {
        public AppDBContext()
        {

        }

        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options)
        {

        }
        public DbSet<UserApp> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<UserQuest> UserQuests  { get; set; }
        public DbSet<UserSections> UserSections { get; set; }
        public DbSet<UserItems> UserItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            if (!Path.Exists(path)) Directory.CreateDirectory(path);
            optionsBuilder.UseSqlite("Data Source=Data\\learning.db");
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\VS2010Test\\WPF_bannikova_pythone\\WpfAppPythone\\WpfAppPythone\\AppData\\Database\\AppDB.mdf;Integrated Security=True");
        //}

    }
}
