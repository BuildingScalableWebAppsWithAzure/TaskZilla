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
    /// <summary>
    /// Contains methods to handle views for all task-related activities. 
    /// Users must be logged in to access any methods in this controller. 
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private TaskService _taskService;

        /// <summary>
        /// Constructor. In a production app, we'd inject all dependencies like TaskService
        /// into this constructor using a DI framework like Autofac. 
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

        /// <summary>
        /// Handles the validation and creation of a new task. 
        /// </summary>
        /// <param name="taskToCreate">The task that the user wants to create</param>
        /// <returns>A viewModel indicating whether the create operation was successful</returns>
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

        /// <summary>
        /// Renders a view that shows a task's details. 
        /// </summary>
        /// <param name="id">The primary key of the task to view</param>
        /// <returns>A view containing the requested task's details</returns>
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            TaskDTO task = await _taskService.GetTaskById(id);
            return View(task);
        }

        /// <summary>
        /// Renders a view that will allow the user to edit a specific task. 
        /// </summary>
        /// <param name="id">The ID of the task that we want to edit</param>
        /// <returns>A view populated with the requested task's details.</returns>
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

        /// <summary>
        /// Writes the edits to a task back to the database, and tells the user
        /// if the update was successful. 
        /// </summary>
        /// <param name="task">The task with edits to be saved</param>
        /// <returns>A view informing the user if the update was successful</returns>
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

        /// <summary>
        /// Deletes the specified task and tells the user if the 
        /// deletion was successful. 
        /// </summary>
        /// <param name="id">The ID of the task to delete</param>
        /// <returns>A view telling the user if the deletion was successful</returns>
        public async Task<ActionResult> Delete(int id)
        {
            await _taskService.DeleteTask(id);
            return View();
        }

       
    }
}