using Microsoft.EntityFrameworkCore;
using Project01.Entities;

namespace Project01.Database {
    public class MysqlContext : DbContext {
        public MysqlContext(DbContextOptions<MysqlContext> context) : base (context){}

        /*================[Set Db]================*/
        public DbSet<User> Users {get;set;}
        public DbSet<Role> Roles {get;set;}
        public DbSet<Page> Pages {get;set;}
        public DbSet<Access> Accesses {get;set;}
        /*================[Set Db]================*/

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Access>().HasKey(x => new {
                x.PageCode,
                x.RoleCode
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}