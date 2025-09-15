using System.Security.Cryptography.Xml;

namespace Tasks.Models
{
    public class CreateTaskRequest
    {
        public int UserId { get; set; }   

        public string Heading { get; set; }

        public string TaskDescription{ get; set; }

        public string Status { get; set; }

        public string TaskType {  get; set; }
        
    }

    public class CreateTaskResponse
    {
        public bool IsSuccess { get; set; }   
        public string Message { get; set; }
    }
}
