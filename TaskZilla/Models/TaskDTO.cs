using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskZilla.Models
{
    /// <summary>
    /// Transports information about a task and 
    /// </summary>
    
    public class TaskDTO : BaseDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Display(Name = "Assigned to")]
        public string AssignedToUserId { get; set; }

        [Display(Name = "Assigned to")]
        public string AssignedToLabel { get; set; }

        [Display(Name = "Priority")]
        public int PriorityId { get; set; }

        [Display(Name = "Priority")]
        public string PriorityLabel { get; set; }

        [Display(Name = "Est. Duration")]
        public decimal? EstDurationInHours { get; set; }

        public List<PriorityDTO> Priorities { get; set; }
        public List<UserDTO> Users { get; set; }
    }

    /// <summary>
    /// Used to transport all priority types to our Views. This is mainly used
    /// in binding to dropdowns where the user will pick a priority
    /// </summary>
    public class PriorityDTO
    {
        public PriorityDTO() { }
        public PriorityDTO(int id, string priority)
        {
            this.Id = id;
            this.Priority = priority; 
        }

        public int Id { get; set; }
        public string Priority { get; set; }
    }

    /// <summary>
    /// Used to transport a list of users who are valid assignees for a task. This 
    /// is used by our Views and is mainly bound to dropdowns where a user needs to 
    /// be selected. 
    /// </summary>
    public class UserDTO
    {
        public UserDTO() { }
        public UserDTO(string id, string userName)
        {
            this.Id = id;
            this.UserName = userName; 
        }

        public string Id { get; set; }
        public string UserName { get; set; }
    }
}