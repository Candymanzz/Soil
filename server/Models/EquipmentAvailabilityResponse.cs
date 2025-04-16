using System;

namespace server.Models
{
    public class EquipmentAvailabilityResponse
    {
        public bool IsAvailable { get; set; }
        public List<ConflictingTaskInfo> ConflictingTasks { get; set; } = new();
    }

    public class ConflictingTaskInfo
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public DateTime PlannedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
