using FeedApp.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeedApp.Api.Proxies.Data
{
	public class DataContext : DbContext
	{
		public DataContext (DbContextOptions<DataContext> options)
		    : base(options)
		{}

		public DbSet<Poll> Polls { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Vote> Votes { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Poll
		    modelBuilder.Entity<Poll>().ToTable("poll");
		    modelBuilder.Entity<Poll>()
			    .HasIndex(p => p.Pincode)
			    .IsUnique();
		    
		    // User
		    modelBuilder.Entity<User>().ToTable("user");
		    modelBuilder.Entity<User>()
			    .HasIndex(u => u.Email)
			    .IsUnique();
		    
		    // Vote
		    modelBuilder.Entity<Vote>().ToTable("vote");
		}
	}

}
