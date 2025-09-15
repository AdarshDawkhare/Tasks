namespace Tasks.Models
{
    public class GetAllTasksRequest
    {
        public int UserId { get; set; }
    }

    public class GetAllTasksResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<GetTask> Tasks { get; set; }
    }

    public class GetTask
    {
        public int TaskId { get; set; }
        public string Heading { get; set; }
        public string TaskDescription { get; set; }
        public string Status { get; set; }
        public string TaskType { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
    }
}
