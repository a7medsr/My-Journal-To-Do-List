using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace just_do_it.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? DueDate { get; set; }

        public int TasksDayslistId { get; set; }
        [ForeignKey("TasksDayslistId")]
        public virtual TasksDayslist TasksDayslist { get; set; }
    }
}
