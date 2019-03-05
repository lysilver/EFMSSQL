using Autofac;
using Autofac.Integration.Mvc;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;

namespace EFMSSQL
{
	public class AutofacConfig
	{
		public static void Register()
		{
			//ContainerBuilder builder = new ContainerBuilder();
			//Assembly controllerAss = Assembly.Load("EFMSSQL");
			////builder.RegisterAssemblyTypes(controllerAss);
			//builder.RegisterControllers(controllerAss);
			////builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
			////builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

			//Assembly repositoryAssR = Assembly.Load("Repository");
			//Type[] rtypesR = repositoryAssR.GetTypes();
			//builder.RegisterTypes(rtypesR).AsImplementedInterfaces().InstancePerLifetimeScope();
			//Assembly repositoryAssS = Assembly.Load("Services");
			//Type[] rtypesS = repositoryAssS.GetTypes();
			//builder.RegisterTypes(rtypesS).AsImplementedInterfaces().InstancePerLifetimeScope();
			//var container = builder.Build();
			//DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			//var builder = new ContainerBuilder();
			//var assembly = Assembly.GetExecutingAssembly();
			//var repository = Assembly.Load("Repository");
			//builder.RegisterAssemblyTypes(repository, repository)
			//    .AsImplementedInterfaces();
			//var service = Assembly.Load("Services");
			//builder.RegisterAssemblyTypes(service, service)
			//    .AsImplementedInterfaces();
			//builder.RegisterControllers(typeof(MvcApplication).Assembly);
			//容器
			//var container = builder.Build();
			//注入改为Autofac注入
			//DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			//ContainerBuilder builder = new ContainerBuilder();
			//builder.RegisterControllers(Assembly.GetExecutingAssembly());
			//builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
			//	.AsImplementedInterfaces();
			//var container = builder.Build();
			//DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			ContainerBuilder builder = new ContainerBuilder();
			var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
			builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
			builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("Services")).AsImplementedInterfaces();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
}