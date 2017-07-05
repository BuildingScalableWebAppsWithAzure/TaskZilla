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
        public async Task<ActionResult> Create()
        {
            List<PriorityDTO> allPriorities = await _taskService.GetPriorities();
            List<UserDTO> allUsers = await _taskService.GetUsers();
            TaskDTO task = new TaskDTO();
            task.Priorities = allPriorities;
            task.Users = allUsers; 
            return View(task); 
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskDTO taskToCreate)
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

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            TaskDTO task = await _taskService.GetTaskById(id);
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
        public async Task<ActionResult> Edit(TaskDTO task)
        {
            try
            {
                await _taskService.UpdateTask(task);
                task.Result = OpResult.Success; 
            }
            catch (Exception ex)
            {
                task.Result = OpResult.Exception;
                task.ErrorMessage = ex.Message; 
            }
            return View(task);
        }

        public async Task<ActionResult> Delete(int id)
        {
            await _taskService.DeleteTask(id);
            return View();
        }

       
    }
}