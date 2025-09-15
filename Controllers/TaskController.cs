using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Security.Claims;
using Tasks.CommonUtility.Models;
using Tasks.Models;
using Tasks.ServiceLayer;

namespace Tasks.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITasksSL _iTasksSL;

        public TaskController(ITasksSL iTasksSL)
        {
            _iTasksSL = iTasksSL;
        }

        [HttpGet]
        public IActionResult ReturnTaskView() 
        {
            return PartialView("_TaskView");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskRequest request)
        {
            CreateTaskResponse response = new CreateTaskResponse();
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.PrimarySid);

                if (userIdClaim == null)
                {
                    return Unauthorized(new { IsSucess = response.IsSuccess, Message = "User ID claim not found" });
                }

                int userId = Convert.ToInt32(userIdClaim.Value);

                request.UserId = userId;

                response = await _iTasksSL.CreateTask(request);

                if ( ! response.IsSuccess)
                {
                    return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex) 
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new { IsSucess = response.IsSuccess , Message = response.Message});
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTask(UpdateTaskRequest request)
        {
            UpdateTaskResponse response = new UpdateTaskResponse();
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.PrimarySid);

                if (userIdClaim == null)
                {
                    return Unauthorized(new { IsSucess = response.IsSuccess, Message = "User ID claim not found" });
                }

                int userId = Convert.ToInt32(userIdClaim.Value);

                request.UserId = userId;

                response = await _iTasksSL.UpdateTask(request);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new { IsSucess = response.IsSuccess, Message = response.Message });
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTasks(string Filter)
        {
            GetAllTasksResponse response = new GetAllTasksResponse();
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.PrimarySid);

                if (userIdClaim == null)
                {
                    return Unauthorized(new { IsSucess = response.IsSuccess, Message = "User ID claim not found" });
                }

                var userId = userIdClaim.Value;

                response = await _iTasksSL.GetAllTasks(Convert.ToInt32(userId) , Filter);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new { isSuccess = response.IsSuccess, message = response.Message , allTasks = response.Tasks });
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int TaskId)
        {
            DeleteTaskResponse response = new DeleteTaskResponse();
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.PrimarySid);

                if (userIdClaim == null)
                {
                    return Unauthorized(new { IsSucess = response.IsSuccess, Message = "User ID claim not found" });
                }

                var userId = userIdClaim.Value;

                response = await _iTasksSL.DeleteTask(Convert.ToInt32(userId), TaskId);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(new { IsSucess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new { isSuccess = response.IsSuccess, message = response.Message});
        }

    }
}
