namespace EmployeeManagementSystemLoginHr.Dtos
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}