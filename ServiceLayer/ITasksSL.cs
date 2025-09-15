using Tasks.CommonUtility.Models;
using Tasks.Models;

namespace Tasks.ServiceLayer
{
    public interface ITasksSL
    {
        public Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);

        public Task<LoginUserResponse> LoginUser(LoginUserRequest request);

        public Task<CreateTaskResponse> CreateTask(CreateTaskRequest request);

        public Task<UpdateTaskResponse> UpdateTask(UpdateTaskRequest request);

        public Task<GetAllTasksResponse> GetAllTasks(int UserId, string Filter );

        public Task<DeleteTaskResponse> DeleteTask(int UserId, int TaskId);   
    }
}
