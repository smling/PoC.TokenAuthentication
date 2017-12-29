using PoC.TokenAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Context
{
    public class LocalContext : DbContext
    {
        public LocalContext() : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        //public DbSet<TokensManager> TokensManager { get; set; }
        public DbSet<ClientKey> ClientKeys { get; set; }
        //public DbSet<MusicStore> MusicStore { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LocalContext>(new DropCreateDatabaseIfModelChanges<LocalContext>());
            base.OnModelCreating(modelBuilder);
        }
    }
}