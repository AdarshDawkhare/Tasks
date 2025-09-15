namespace Tasks.CommonUtility.Models
{
    public class UpdateTaskRequest
    {
        public int UserId { get; set; }

        public int TaskId { get; set; }

        public string Heading { get; set; }

        public string TaskDescription { get; set; }

        public string Status { get; set; }
    }

    public class UpdateTaskResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
