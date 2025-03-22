namespace server.DTOs
{
    public class FertilizerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Composition { get; set; }
        public string ApplicationMethod { get; set; }
    }

    public class CreateFertilizerDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Composition { get; set; }
        public string ApplicationMethod { get; set; }
    }

    public class UpdateFertilizerDto
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Composition { get; set; }
        public string? ApplicationMethod { get; set; }
    }
}