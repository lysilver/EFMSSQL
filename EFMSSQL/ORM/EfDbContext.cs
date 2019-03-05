using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using ORM.Migrations;

namespace ORM
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))] //启用mysql方法一，需要重新启动数据库迁移技术，否则迁移的依然是上次的数据库连接
    public class EfDbContext : DbContext
    {
        public EfDbContext()
            : base("mssql")
        {
            //取消EF的延迟加载
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            //Database.SetInitializer<EfDbContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfDbContext, Configuration>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EfDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new SystemUserMapping());
            Assembly pAssembly = Assembly.Load("Mapping");
            var typesToRegister = pAssembly.GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }
    }
}