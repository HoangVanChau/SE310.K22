namespace HRM.Models.Requests
{
    public class ChangeRoleRequest
    {
        public string UserId { get; set; }
        public string NewRole { get; set; }
    }
}