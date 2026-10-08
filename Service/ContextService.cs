using trpo6_var2.Models;

namespace trpo6_var2.Service
{
    public class ContextService
    {
        private WebApiDatabaseContext _context;
        public WebApiDatabaseContext Context => _context;

        private static ContextService instance;
        public static ContextService Instance
        {
            get
            {
                if (instance == null) instance = new ContextService();
                return instance;
            }
        }
        private ContextService()
        {
            _context = new WebApiDatabaseContext();
        }
    }
}
