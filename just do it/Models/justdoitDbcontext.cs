using Microsoft.EntityFrameworkCore;

namespace just_do_it.Models
{
    public class justdoitDbcontext : DbContext
    {
        public justdoitDbcontext(DbContextOptions<justdoitDbcontext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Journaling> Journalings { get; set; }
        public virtual DbSet<TasksDayslist> TasksDayslists { get; set; }
    }
}