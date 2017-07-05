namespace TaskZilla.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(128)]
        public string AssignedToUserId { get; set; }

        public int PriorityId { get; set; }

        public decimal? EstDurationInHours { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Priority Priority { get; set; }
    }
}
