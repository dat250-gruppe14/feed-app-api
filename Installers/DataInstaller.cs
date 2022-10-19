
using FeedAppApi.Proxies.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FeedAppApi.Installers
{
    public class DataInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
	    var connectionString = configuration["PostgreSql:ConnectionString"];
	    var dbPassword = configuration["PostgreSql:DbPassword"];

	    var builder = new NpgsqlConnectionStringBuilder(connectionString)
	    {
		Password = dbPassword
	    };

	    services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.ConnectionString));
	}
    }

}
