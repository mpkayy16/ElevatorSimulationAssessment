using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Extensions;

namespace ElevatorSimulatorAssessment.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string BuildingDescription { get; set; }
        public int MaximumFloors { get; set; }
        public List<Floor>? Floors { get; set; }
        public List<Elevator>? Elevators { get; set; }

        public Building(string buildingName,List<Floor> floors,List<Elevator> elevators)
        {
            BuildingName = buildingName;
            BuildingDescription = "DVT Elevator similation assessment";
            Floors = floors;
            MaximumFloors = FloorExtensions.GetMaximumNumberOfFloorsByFloornumber(Floors);
            Elevators = elevators;
        }
    }
}