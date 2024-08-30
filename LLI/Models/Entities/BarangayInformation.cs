namespace LLI.Models.Entities
{
    public class BarangayInformation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Birthday { get; set; }
        public string Address { get; set; }

        public string Phone { get; set; }

        public string EmergencyContactName { get; set; }
        public string EmergencyContact { get; set; }
        public string Email { get; set; }
    }
}
