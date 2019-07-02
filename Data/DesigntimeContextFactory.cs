using System;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace byappt_identity.Data
{
    public static class CTX
    {
        public static string Migrations = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        public static string GetPwd()
        {
            var pwd = Environment.GetEnvironmentVariable("POSTGRES_PWD");
            if (string.IsNullOrEmpty(pwd))
                throw new InvalidOperationException("POSTGRES_PWD needs to be set.");

            return pwd;
        }

        public static string ToCs(string pwd, string db) => $"User ID=ids;Password={pwd};Database={db};Host=localhost;Port=5432;";

        public static DbContextOptions<T> ToOpts<T>(string pwd, string db) where T : DbContext =>
            new DbContextOptionsBuilder<T>()
                .UseNpgsql(ToCs(pwd, db), opts => opts.MigrationsAssembly(Migrations))
                .Options;
    }

    public class AppContextFactory : IDesignTimeDbContextFactory<UserStoreDbContext>
    {
        public AppContextFactory()
        {
            // A parameter-less constructor is required by the EF Core CLI tools.
        }

        public UserStoreDbContext CreateDbContext(string[] args)
        {
            return new UserStoreDbContext(CTX.ToOpts<UserStoreDbContext>(CTX.GetPwd(), "users"));
        }
    }

    public class ConfigurationContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationContextFactory()
        {
            // A parameter-less constructor is required by the EF Core CLI tools.
        }

        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            return new ConfigurationDbContext(
                CTX.ToOpts<ConfigurationDbContext>(CTX.GetPwd(), "clients"), 
                new ConfigurationStoreOptions());
        }
    }

    public class GrantsContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public GrantsContextFactory()
        {
            // A parameter-less constructor is required by the EF Core CLI tools.
        }

        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            return new PersistedGrantDbContext(
                CTX.ToOpts<PersistedGrantDbContext>(CTX.GetPwd(), "grants"),
                new OperationalStoreOptions() 
                );
        }
    }
}