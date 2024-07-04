using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pfe.models;

namespace pfe.config
{
    public class DBContext : IdentityDbContext<User,Role,string>
    {
        public DBContext(DbContextOptions<DBContext>options): base(options)
        {}
        public DbSet<Chambre> chambres { get; set; }
        public DbSet<Commentaire> commentaires { get; set; }
        public DbSet<Destination> destinations { get; set; }
        public DbSet<Maison>  maisons { get; set; }
        public DbSet<Reservation> reservations { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Message> Messages { get; set; }   
    }
}
