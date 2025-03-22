namespace server.DTOs
{
    public class FieldDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public string Location { get; set; }
        public int SoilTypeId { get; set; }
        public string SoilTypeName { get; set; }
    }

    public class CreateFieldDto
    {
        public string Name { get; set; }
        public double Area { get; set; }
        public string Location { get; set; }
        public int SoilTypeId { get; set; }
    }

    public class UpdateFieldDto
    {
        public string? Name { get; set; }
        public double? Area { get; set; }
        public string? Location { get; set; }
        public int? SoilTypeId { get; set; }
    }
}