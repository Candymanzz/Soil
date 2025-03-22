namespace server.DTOs
{
    public class SoilTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
    }

    public class CreateSoilTypeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
    }

    public class UpdateSoilTypeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Characteristics { get; set; }
    }
}