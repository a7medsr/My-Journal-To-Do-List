using System.ComponentModel.DataAnnotations;

namespace just_do_it.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Navigation properties
        public virtual List<TasksDayslist> TasksDayslists { get; set; } = new List<TasksDayslist>();
        public virtual List<Journaling> Journalings { get; set; } = new List<Journaling>();
    }

}
