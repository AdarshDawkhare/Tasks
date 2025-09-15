using System.ComponentModel.DataAnnotations;

namespace Tasks.CommonUtility.Models
{
    public class LoginUserRequest
    {
        public string Email { get; set; }
        public string UserPassword { get; set; }
    }

    public class LoginUserResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public LoginUser user { get; set; }

    }

    public class LoginUser
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string UserRole { get; set; }

    }
}
