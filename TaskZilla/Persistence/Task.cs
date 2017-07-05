namespace TaskZilla.Persistence
{
    using System.ComponentModel.DataAnnotations;

    public partial class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public string AssignedToUserId { get; set; }

        public int PriorityId { get; set; }

        public decimal? EstDurationInHours { get; set; }
    }
}
