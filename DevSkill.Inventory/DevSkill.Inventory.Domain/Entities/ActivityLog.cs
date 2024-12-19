using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class ActivityLog : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Action { get; set; } 
        public string ItemName { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
