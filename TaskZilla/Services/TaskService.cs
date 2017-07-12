using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskZilla.Persistence;
using TaskZilla.Models;
using System.Data.Entity;

namespace TaskZilla.Services
{
    /// <summary>
    /// This is our "business logic" layer. It is responsible for working with EF
    /// to handle CRUD operations and translate results into viewmodels for consumption
    /// by our controllers. 
    /// </summary>
    public class TaskService
    {
        private TaskZillaContext _context;

        /// <summary>
        /// Constructor. Note that in a production app, we'd inject dependencies such as 
        /// the TaskZillaContext into this constructor using a dependency injection
        /// framework like Autofac
        /// </summary>
        public TaskService()
        {
            _context = new TaskZillaContext();
        }

        /// <summary>
        /// Retrieves all priorities defined in the database. 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<PriorityDTO>> GetPriorities()
        {
            List<Priority> prioritiesList = await _context.Priorities.ToListAsync();
            List<PriorityDTO> priorityDTOs = DTOHelpers.CopyPriorities(prioritiesList);
            return priorityDTOs; 
        }

        /// <summary>
        /// Returns the task identified by taskId. 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<TaskDTO> GetTaskById(int taskId)
        {
            Task t = await _context.Tasks.FindAsync(taskId);
            var taskDto = new TaskDTO
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                EstDurationInHours = t.EstDurationInHours,
                AssignedToUserId = t.AssignedToUserId,
                PriorityId = t.PriorityId,
                PriorityLabel = t.Priority.Priority1,
                AssignedToLabel = t.AspNetUser.UserName
            };
            return taskDto; 
        }

        /// <summary>
        /// returns all users defined in the system. 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<UserDTO>> GetUsers()
        {
            //we'll use a projection since we don't need to retrieve fields such as password...
            var users = await _context.AspNetUsers.Select(p => new { Id = p.Id, UserName = p.UserName }).ToListAsync();
            List<UserDTO> userDTOs = new List<UserDTO>(); 
            foreach (var u in users)
            {
                userDTOs.Add(new UserDTO(u.Id, u.UserName));
            }
            return userDTOs; 
        }

        /// <summary>
        /// Updates a task in the database. 
        /// </summary>
        public async System.Threading.Tasks.Task UpdateTask(TaskDTO task)
        {
            var taskToUpdate = await _context.Tasks.FindAsync(task.Id);
            taskToUpdate.Name = task.Name;
            taskToUpdate.Description = task.Description;
            taskToUpdate.PriorityId = task.PriorityId;
            taskToUpdate.AssignedToUserId = task.AssignedToUserId;
            taskToUpdate.EstDurationInHours = task.EstDurationInHours;
            await _context.SaveChangesAsync(); 
        }

        /// <summary>
        /// returns all tasks in the system. This demonstrates an inner join between
        /// Tasks, Priorities, and AspNetUsers. 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<TaskDTO>> GetAllTasksAsync()
        {
            var tasks = await (from t in _context.Tasks join p in _context.Priorities on t.PriorityId equals p.Id
                               join u in _context.AspNetUsers on t.AssignedToUserId equals u.Id 
                               select new { Priority = p, Task = t, User = u }).ToListAsync();
            List<TaskDTO> taskDTOs = new List<TaskDTO>(); 
            foreach (var t in tasks)
            {
                taskDTOs.Add(new TaskDTO {
                    Id = t.Task.Id, 
                    Name = t.Task.Name, 
                    Description = t.Task.Description, 
                    PriorityId = t.Task.PriorityId,
                    PriorityLabel = t.Priority.Priority1, 
                    AssignedToUserId = t.Task.AssignedToUserId,
                    EstDurationInHours = t.Task.EstDurationInHours,
                    AssignedToLabel = t.User.UserName
                });
            }

            return taskDTOs; 
        }

        /// <summary>
        /// Adds a new task to the database. 
        /// </summary>
        /// <param name="newTask">The task to add</param>
        public async System.Threading.Tasks.Task CreateTask(TaskDTO newTaskDTO)
        {
            Task newTask = new Task
            {
                Name = newTaskDTO.Name,
                Description = newTaskDTO.Description,
                AssignedToUserId = newTaskDTO.AssignedToUserId,
                PriorityId = newTaskDTO.PriorityId,
                EstDurationInHours = newTaskDTO.EstDurationInHours
            }; 
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync(); 
        }

        /// <summary>
        /// Removes a task from the database. 
        /// </summary>
        public async System.Threading.Tasks.Task DeleteTask(int id)
        {
            Task taskToDelete = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync(); 
        }
    }

    /// <summary>
    /// Contains helper methods to copy EF Models into ViewModels. 
    /// </summary>
    public class DTOHelpers
    {
        public static List<PriorityDTO> CopyPriorities(List<Priority> priorities)
        {
            List<PriorityDTO> dtos = new List<PriorityDTO>(); 
            foreach (Priority p in priorities)
            {
                dtos.Add(new PriorityDTO(p.Id, p.Priority1));
            }
            return dtos; 
        }
    }
}