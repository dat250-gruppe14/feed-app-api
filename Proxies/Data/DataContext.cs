using FeedAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedAppApi.Proxies.Data
{
	public class DataContext : DbContext
	{
		public DataContext (DbContextOptions<DataContext> options)
		    : base(options)
		{
		}
		public DbSet<Poll>? Polls {get;set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		    modelBuilder.Entity<Poll>().ToTable("poll");
		}
	}

}
