using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using Tasks.CommonUtility.Models;
using Tasks.Models;
using Tasks.ServiceLayer;

namespace Tasks.RepositoryLayer
{
    public class TasksRL : ITasksRL
    {
        private readonly IConfiguration _configuration;

        private readonly MySqlConnection _mySqlConnection;

        public TasksRL(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MySqlDBString"]);
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            RegisterUserResponse response = new RegisterUserResponse();
            response.IsSuccess = true;
            response.Message = "Successfull";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string query = "INSERT INTO Tasks.User (UserName, Email, Mobile, UserPassword) VALUES (@UserName, @Email, @Mobile, @UserPassword);";

                using (MySqlCommand command = new MySqlCommand(query, _mySqlConnection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = 180;

                    command.Parameters.AddWithValue("@UserName", request.UserName);
                    command.Parameters.AddWithValue("@Email", request.Email);
                    command.Parameters.AddWithValue("@Mobile", request.Mobile);
                    command.Parameters.AddWithValue("@UserPassword", request.UserPassword);

                    int Status = await command.ExecuteNonQueryAsync();

                    if (Status <= 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Query Not Executed";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
        {
            LoginUserResponse response = new LoginUserResponse();
            response.IsSuccess = true;
            response.Message = "Successfull";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string query = "SELECT * FROM Tasks.User where Email=@Email and UserPassword = @UserPassword";

                using (MySqlCommand sqlCommand = new MySqlCommand(query, _mySqlConnection))
                {
                    try
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandTimeout = 180;

                        sqlCommand.Parameters.AddWithValue("@Email", request.Email);
                        sqlCommand.Parameters.AddWithValue("@UserPassword", request.UserPassword);

                        using (MySqlDataReader dataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                        {
                            response.user = new LoginUser();

                            if (await dataReader.ReadAsync())
                            {
                                response.user.UserId = dataReader[name: "UserId"] != DBNull.Value ? Convert.ToInt32(dataReader[name: "UserId"]) : 0;
                                response.user.UserName = dataReader[name: "UserName"] != DBNull.Value ? Convert.ToString(dataReader[name: "UserName"]) : string.Empty;
                                response.user.Email = dataReader[name: "Email"] != DBNull.Value ? Convert.ToString(dataReader[name: "Email"]) : string.Empty;
                                response.user.UserRole = dataReader[name: "UserRole"] != DBNull.Value ? Convert.ToString(dataReader[name: "UserRole"]) : string.Empty;
                            }
                            else
                            {
                                response.IsSuccess = true;
                                response.Message = "Record Not Found / Database Empty";
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                    }
                    finally
                    {
                        await sqlCommand.DisposeAsync();
                    }
                } }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request)
        {
            CreateTaskResponse response = new CreateTaskResponse();
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {
                // First check is mysqlconnection is open or not
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string query = "INSERT INTO Tasks.Task (UserId,Heading,TaskDescription,StartDateTime,Status,TaskType) VALUES (@UserId,@Heading,@TaskDescription,@StartDateTime,@Status,@TaskType);";

                if (request.TaskType == "Daily")
                {
                    query = "INSERT INTO Tasks.DailyTasks (UserId,Heading,TaskDescription,Status) VALUES (@UserId,@Heading,@TaskDescription,@Status);";
                }

                using (MySqlCommand command = new MySqlCommand(query, _mySqlConnection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = 180;

                    command.Parameters.AddWithValue("@UserId", request.UserId);
                    command.Parameters.AddWithValue("@Heading", request.Heading);
                    command.Parameters.AddWithValue("@TaskDescription", request.TaskDescription);

                    if (request.TaskType == "Normal")
                    {
                        command.Parameters.AddWithValue("@StartDateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@TaskType", request.TaskType);
                    }

                    command.Parameters.AddWithValue("@Status", request.Status);

                    int Status = await command.ExecuteNonQueryAsync();

                    if (Status <= 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Query Not Executed";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<UpdateTaskResponse> UpdateTask(UpdateTaskRequest request)
        {
            UpdateTaskResponse response = new UpdateTaskResponse();
            response.IsSuccess = true;
            response.Message = "Successfull";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string query = "UPDATE Task SET Heading = @Heading, TaskDescription = @TaskDescription, Status = @Status WHERE TaskId = @TaskId and UserId = @UserId";

                if (request.Status == "Completed")
                {
                    query = "UPDATE Task SET Heading = @Heading, TaskDescription = @TaskDescription, EndDateTime = @EndDateTime ,Status = @Status WHERE TaskId = @TaskId and UserId = @UserId";
                }

                using (MySqlCommand command = new MySqlCommand(query, _mySqlConnection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = 180;

                    command.Parameters.AddWithValue("@UserId", request.UserId);
                    command.Parameters.AddWithValue("@TaskId", request.TaskId);
                    command.Parameters.AddWithValue("@Heading", request.Heading);
                    command.Parameters.AddWithValue("@TaskDescription", request.TaskDescription);

                    if (request.Status == "Completed")
                    {
                        command.Parameters.AddWithValue("@EndDateTime", DateTime.Now);
                    }

                    command.Parameters.AddWithValue("@Status", request.Status);

                    int Status = await command.ExecuteNonQueryAsync();

                    if (Status <= 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Query Not Executed";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetAllTasksResponse> GetAllTasks(int UserId)
        {
            GetAllTasksResponse response = new GetAllTasksResponse();
            response.IsSuccess = true;
            response.Message = "Successfull";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string query = "SELECT * FROM Tasks.Task WHERE UserId = @UserId";

                using (MySqlCommand command = new MySqlCommand(query, _mySqlConnection))
                {
                    try
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 180;
                        command.Parameters.AddWithValue("@UserId", UserId);

                        using (MySqlDataReader dataReader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            if (dataReader.HasRows)
                            {
                                response.Tasks = new List<GetTask>();

                                while (await dataReader.ReadAsync())
                                {
                                    GetTask task = new GetTask();

                                    task.TaskId = dataReader[name: "TaskId"] != DBNull.Value ? Convert.ToInt32(dataReader[name: "TaskId"]) : 0;
                                    task.Heading = dataReader[name: "Heading"] != DBNull.Value ? Convert.ToString(dataReader[name: "Heading"]) : string.Empty;
                                    task.TaskDescription = dataReader[name: "TaskDescription"] != DBNull.Value ? Convert.ToString(dataReader[name: "TaskDescription"]) : string.Empty;
                                    task.Status = dataReader[name: "Status"] != DBNull.Value ? Convert.ToString(dataReader[name: "Status"]) : string.Empty;
                                    task.TaskType = dataReader[name: "TaskType"] != DBNull.Value ? Convert.ToString(dataReader[name: "TaskType"]) : string.Empty;

                                    DateTime startdateTime = dataReader[name: "StartDateTime"] != DBNull.Value ? Convert.ToDateTime(dataReader[name: "StartDateTime"]) : DateTime.MinValue;
                                    DateTime enddateTime = dataReader[name: "EndDateTime"] != DBNull.Value ? Convert.ToDateTime(dataReader[name: "EndDateTime"]) : DateTime.MinValue;

                                    if (startdateTime != DateTime.MinValue)
                                    {
                                        DateOnly startDate = DateOnly.FromDateTime(startdateTime);
                                        TimeOnly startTime = TimeOnly.FromDateTime(startdateTime);

                                        task.StartDate = Convert.ToString(startDate);
                                        task.StartTime = Convert.ToString(startTime);

                                    }

                                    if (enddateTime != DateTime.MinValue)
                                    {
                                        DateOnly endDate = DateOnly.FromDateTime(enddateTime);
                                        TimeOnly endTime = TimeOnly.FromDateTime(enddateTime);

                                        task.EndDate = Convert.ToString(endDate);
                                        task.EndTime = Convert.ToString(endTime);
                                    }

                                    response.Tasks.Add(task);
                                }
                            }
                            else
                            {
                                response.IsSuccess = true;
                                response.Message = "Record Not Found / Database Empty";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                    }
                    finally
                    {
                        await command.DisposeAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<DeleteTaskResponse> DeleteTask(int UserId, int TaskId)
        {
            DeleteTaskResponse response = new DeleteTaskResponse();
            response.IsSuccess = true;
            response.Message = "Successfull";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string query = "Delete from Tasks.Task WHERE TaskId = @TaskId and UserId = @UserId";

                using (MySqlCommand command = new MySqlCommand(query, _mySqlConnection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = 180;

                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@TaskId", TaskId);
                    

                    int Status = await command.ExecuteNonQueryAsync();

                    if (Status <= 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Query Not Executed";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }
    }
}
