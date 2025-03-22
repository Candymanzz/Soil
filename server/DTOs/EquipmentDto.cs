namespace server.DTOs
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }

    public class CreateEquipmentDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }

    public class UpdateEquipmentDto
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}