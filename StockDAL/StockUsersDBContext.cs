using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StockPlaygroundMVC
{
    public partial class StockUsersDBContext : DbContext
    {
        public StockUsersDBContext()
        {
            CreateConnection();
        }

        static SqliteConnection CreateConnection()
        {
            string dbPath = @"D:\Sabina - Documente\projects\StockPlaygroundMVC\StockDAL\StockUsersDB.db";
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + dbPath);
            try
            {
                conn.Open();
            }
            catch { }
            return conn;
        }
        public StockUsersDBContext(DbContextOptions<StockUsersDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserWatchlist> UserWatchlists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlite("Data Source=D:\\Sabina - Documente\\projects\\StockPlaygroundMVC\\StockDAL\\StockUsersDB.db");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName, "IX_Users_UserName")
                    .IsUnique();

                entity.Property(e => e.UserName).IsRequired();

                entity.Property(e => e.UserPassword).IsRequired();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleName).IsRequired();
            });

            modelBuilder.Entity<UserWatchlist>(entity =>
            {
                entity.HasKey(e => e.WatchlistId);

                entity.ToTable("UserWatchlist");

                entity.Property(e => e.WatchlistId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
