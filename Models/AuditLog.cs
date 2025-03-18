namespace EmployeeManagementSystemLoginHr.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }  

        public string ChangedData { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }  
        public string UserName { get; set; }
        public string UserRoles { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
