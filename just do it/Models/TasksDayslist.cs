using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace just_do_it.Models
{
    public class TasksDayslist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual List<Task> Tasks { get; set; } = new List<Task>();

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

}