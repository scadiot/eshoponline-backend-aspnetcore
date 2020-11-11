using AutoMapper;
using eshoponline;
using eshoponline.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace eshoponline_test
{
    public class SliceFixture : IDisposable
    {
        static readonly IConfiguration Config;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ServiceProvider _provider;
        private readonly string DbName = Guid.NewGuid() + ".db";

        static SliceFixture()
        {
            Config = new ConfigurationBuilder()
               .AddEnvironmentVariables()
               .Build();
        }

        public SliceFixture()
        {
            var startup = new Startup(Config);
            var services = new ServiceCollection();

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlite(connection);
            services.AddSingleton(new EshoponlineContext(builder.Options));

            startup.ConfigureServices(services);

            _provider = services.BuildServiceProvider();

            GetDbContext().Database.EnsureCreated();
            _scopeFactory = _provider.GetService<IServiceScopeFactory>();
        }

        public EshoponlineContext GetDbContext()
        {
            return _provider.GetRequiredService<EshoponlineContext>();
        }

        public IMapper GetMapper()
        {
            return _provider.GetRequiredService<IMapper>();
        }

        public ICurrentUserAccessor GetCurrentUserAccessor()
        {
            return _provider.GetRequiredService<ICurrentUserAccessor>();
        }

        public void Dispose()
        {
            File.Delete(DbName);
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                return await action(scope.ServiceProvider);
            }
        }

        public Task ExecuteDbContextAsync(Func<EshoponlineContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<EshoponlineContext>()));
        }

        public Task<T> ExecuteDbContextAsync<T>(Func<EshoponlineContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<EshoponlineContext>()));
        }
    }
}
