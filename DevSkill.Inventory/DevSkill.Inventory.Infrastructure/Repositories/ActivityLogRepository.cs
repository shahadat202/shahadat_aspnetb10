using Blog.Infrastructure.Repositories;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ActivityLogRepository : Repository<ActivityLog, Guid>, IActivityLogRepository
    {
        private readonly InventoryDbContext _context;
        public ActivityLogRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<ActivityLog> GetRecentLogs()
        {
            return _context.ActivityLogs.OrderByDescending(log => log.ActionDate).ToList();
        }
    }
}
