using System.Runtime.Remoting.Messaging;

namespace ORM
{
    public class ContextFactory
    {
        public static EfDbContext GetCurrentContext()
        {
            EfDbContext nContext = CallContext.GetData("EfDbContext") as EfDbContext;
            if (nContext == null)
            {
                nContext = new EfDbContext();
                CallContext.SetData("EfDbContext", nContext);
            }
            return nContext;
        }
    }
}