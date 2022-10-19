using FeedAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedAppApi.Proxies.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Poll>? Polls {get;set;}
	}

}
