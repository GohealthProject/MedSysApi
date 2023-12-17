namespace MedSysApi.Models
{
    public class CEmailDto
    {
        public string Address { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
