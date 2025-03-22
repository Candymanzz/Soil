namespace server.DTOs
{
    public class CropDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PlantingSeason { get; set; }
        public int GrowthDuration { get; set; }
    }

    public class CreateCropDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PlantingSeason { get; set; }
        public int GrowthDuration { get; set; }
    }

    public class UpdateCropDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PlantingSeason { get; set; }
        public int? GrowthDuration { get; set; }
    }
}