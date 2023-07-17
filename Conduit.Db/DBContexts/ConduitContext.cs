using Conduit.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Db.DBContexts
{
    public class ConduitContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Follow> Follows { get; set; } = null!;
        public DbSet<FavoriteArticle> FavoriteArticles { get; set; } = null!;

        public ConduitContext(DbContextOptions<ConduitContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.User)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.Article)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                 .HasOne(c => c.Article)
                 .WithMany(u => u.Comments)
                 .HasForeignKey(c => c.ArticleId)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Follow>()
                 .HasKey(f => new { f.FollowerId, f.FolloweeId });

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followee)
                .WithMany(u => u.Followees)
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FavoriteArticle>()
                 .HasKey(bc => new { bc.UserId, bc.ArticleId });

            modelBuilder.Entity<FavoriteArticle>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.FavoriteArticles)
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<FavoriteArticle>()
                .HasOne(bc => bc.Article)
                .WithMany(c => c.FavoriteArticles)
                .HasForeignKey(bc => bc.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasData(
               new User()
               {
                   Id = 1,
                   UserName = "lenaSamaha",
                   Email = "lenasama7a@gmail.com",
                   Password = "13579L$l"
               },
               new User()
               {
                   Id = 2,
                   UserName = "saulBellow",
                   Email = "saulBellow@gmail.com",
                   Password = "246810Saul%"
               },
               new User()
               {
                   Id = 3,
                   UserName = "SidneySheldon",
                   Email = "SidneySheldon@gmail.com",
                   Password = "Sidney1&",
                   Bio = "I'm From Italy"
               },
               new User()
               {
                   Id = 4,
                   UserName = "ErnestHemingway",
                   Email = "ErnestHemingway@gmail.com",
                   Password = "12246E-r"
               });

            modelBuilder.Entity<Article>()
             .HasData(
               new Article()
               {
                   Id = 1,
                   Title = "Central Park",
                   Body = "The most visited urban park in the United States.",
                   Tag = "#central_Park",
                   UserId = 1
               },
               new Article()
               {
                   Id = 2,
                   Title = "Empire State Building",
                   Body = "A 102-story skyscraper located in Midtown Manhattan.",
                   Tag = "#Empire_State_Building",
                   UserId = 1
               },
                 new Article()
                 {
                     Id = 3,
                     Title = "Cathedral",
                     Body = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.",
                     Tag = "#Cathedral",
                     UserId = 2
                 },
               new Article()
               {
                   Id = 4,
                   Title = "Antwerp Central Station",
                   Body = "The the finest example of railway architecture in Belgium.",
                   Tag = "#Central_Station",
                   UserId = 2

               },
               new Article()
               {
                   Id = 5,
                   Title = "Eiffel Tower",
                   Body = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.",
                   Tag = "#Eiffel_Tower",
                   UserId = 3

               },
               new Article()
               {
                   Id = 6,
                   Title = "The Louvre",
                   Body = "The world's largest museum.",
                   Tag = "#Louvre",
                   UserId = 4
               }
               );

            modelBuilder.Entity<Comment>()
             .HasData(
               new Comment()
               {
                   Id = 1,
                   UserId = 1,
                   ArticleId = 1,
                   Body = "This is beautiful article"
               },
               new Comment()
               {
                   Id = 2,
                   UserId = 2,
                   ArticleId = 3,
                   Body = "Amazing!"
               },
               new Comment()
               {
                   Id = 3,
                   UserId = 3,
                   ArticleId = 5,
                   Body = "Eiffel Tower is the best"
               },
               new Comment()
               {
                   Id = 4,
                   UserId = 4,
                   ArticleId = 6,
                   Body = "I love this museum"
               }
               );


            modelBuilder.Entity<FavoriteArticle>()
             .HasData(
               new FavoriteArticle()
               {
                   ArticleId = 1,
                   UserId = 1
               },
               new FavoriteArticle()
               {
                   ArticleId = 3,
                   UserId = 1
               },
               new FavoriteArticle()
               {
                   ArticleId = 1,
                   UserId = 3
               },
               new FavoriteArticle()
               {
                   ArticleId = 2,
                   UserId = 4
               }
               );


            modelBuilder.Entity<Follow>()
             .HasData(
               new Follow()
               {
                   FollowerId = 1,
                   FolloweeId = 2
               },
               new Follow()
               {
                   FollowerId = 1,
                   FolloweeId = 3
               },
               new Follow()
               {
                   FollowerId = 2,
                   FolloweeId = 1
               },
               new Follow()
               {
                   FollowerId = 2,
                   FolloweeId = 3
               },
               new Follow()
               {
                   FollowerId = 3,
                   FolloweeId = 4
               }
               );
            base.OnModelCreating(modelBuilder);
        }
    }
}
