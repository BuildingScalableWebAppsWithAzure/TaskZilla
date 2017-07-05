using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskZilla.Services;
using TaskZilla.Models;
using System.Threading.Tasks; 

namespace TaskZilla.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private TaskService _taskService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HomeController()
        {
            _taskService = new TaskService(); 
        }

        /// <summary>
        /// Shows the user all tasks in the system. 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            List<TaskDTO> tasks = await _taskService.GetAllTasksAsync();
            return View(tasks);
        }

        /// <summary>
        /// Shows the create task screen. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CreateTask()
        {
            List<PriorityDTO> allPriorities = await _taskService.GetPriorities();
            List<UserDTO> allUsers = await _taskService.GetUsers();
            TaskDTO task = new TaskDTO();
            task.Priorities = allPriorities;
            task.Users = allUsers; 
            return View(task); 
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            TaskDTO task = await _taskService.GetTaskById(id);
            List<UserDTO> allUsers = await _taskService.GetUsers();
            List<PriorityDTO> allPriorities = await _taskService.GetPriorities();
            task.Priorities = allPriorities;
            task.Users = allUsers;
            return View(task);
        }

       

        [HttpPost]
        public async Task<ActionResult> CreateTask(TaskDTO taskToCreate)
        {
            try
            {
                await _taskService.CreateTask(taskToCreate);
                taskToCreate.Result = OpResult.Success; 
            }
            catch (Exception ex)
            {
                taskToCreate.Result = OpResult.Exception;
                taskToCreate.ErrorMessage = ex.Message; 
            }
            return View(taskToCreate); 
        }
    }
}