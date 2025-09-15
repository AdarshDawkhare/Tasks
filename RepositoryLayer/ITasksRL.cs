using Tasks.CommonUtility.Models;
using Tasks.Models;

namespace Tasks.RepositoryLayer
{
    public interface ITasksRL
    {
        public Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);

        public Task<LoginUserResponse> LoginUser(LoginUserRequest request);

        public Task<CreateTaskResponse> CreateTask(CreateTaskRequest request);

        public Task<UpdateTaskResponse> UpdateTask(UpdateTaskRequest request);

        public Task<GetAllTasksResponse> GetAllTasks(int UserId);

        public Task<DeleteTaskResponse> DeleteTask(int UserId, int TaskId);
    }
}
