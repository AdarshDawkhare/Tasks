using Org.BouncyCastle.Asn1.Ocsp;
using Tasks.CommonUtility.Models;
using Tasks.Models;
using Tasks.RepositoryLayer;

namespace Tasks.ServiceLayer
{
    public class TasksSL : ITasksSL
    {
        
        private readonly ITasksRL _iTasksRL;

        public TasksSL(ITasksRL iTasksRL) 
        {
            _iTasksRL = iTasksRL;
        }
        
        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            return await _iTasksRL.RegisterUser(request);
        }

        public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
        {
            return await _iTasksRL.LoginUser(request);

        }

        public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request)
        {
            return await _iTasksRL.CreateTask(request);
        }

        public async Task<UpdateTaskResponse> UpdateTask(UpdateTaskRequest request)
        {
            return await _iTasksRL.UpdateTask(request);
        }

        public async Task<GetAllTasksResponse> GetAllTasks(int UserId, string Filter)
        {
            GetAllTasksResponse response =  await _iTasksRL.GetAllTasks(UserId);

            if(Filter == "Daily")
            {
                response.Tasks.RemoveAll(x => x.StartDate != DateOnly.FromDateTime(DateTime.Now).ToString() );
            }

            if(Filter == "Completed")
            {
                response.Tasks.RemoveAll(x => x.Status != "Completed");
            }

            return response;
        }

        public async Task<DeleteTaskResponse> DeleteTask(int UserId, int TaskId)
        {
            return await _iTasksRL.DeleteTask(UserId,TaskId);
        }
    }
}
