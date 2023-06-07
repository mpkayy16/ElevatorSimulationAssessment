using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Models;

namespace ElevatorSimulatorAssessment.Extensions
{
    public class FloorExtensions
    {

        public static List<Floor> GenerateMockFloors()
        {
            return new List<Floor>()
            {
                new() { FloorNumber = 0, Passengers = new() { new(0,5,1),new(0,5,2),new(0,9,3),new(0,5,4),new(0, 7,5),new(0, 6,6),new(0,8,7)}},
                new() { FloorNumber = 1, Passengers = new() { new(1,8,1),new(1,9,2),new(1,7,3),new(1,5,4),new(1, 9,5),new(1, 7,6),new(1,7,7)}},
                new() { FloorNumber = 2, Passengers = new() { new(2,9,1),new(2,8,2),new(2,8,3),new(2,5,4),new(2, 5,5),new(2, 0,6),new(2,9,7)}},
                new() { FloorNumber = 3, Passengers = new() { new(3,5,1),new(3,9,2),new(3,0,3),new(3,5,4),new(3, 6,5),new(3, 9,6),new(3,7,7)}},
                new() { FloorNumber = 4, Passengers = new() { new(4,1,1),new(4,9,2),new(4,9,3),new(4,5,4),new(4, 1,5),new(4, 7,6),new(4,6,7)}},
                new() { FloorNumber = 5, Passengers = new() { new(5,3,1),new(5,3,2),new(5,2,3),new(5,5,4),new(5, 2,5),new(5, 3,6),new(5,9,7)}},
                new() { FloorNumber = 6, Passengers = new() { new(6,8,1),new(6,1,2),new(6,1,3),new(6,5,4),new(6, 3,5),new(6, 2,6),new(6,9,7)}},
                new() { FloorNumber = 7, Passengers = new() { new(7,2,1),new(7,9,2),new(7,0,3),new(7,5,4),new(7, 4,5),new(7, 2,6),new(7,3,7)}},
                new() { FloorNumber = 8, Passengers = new() { new(8,1,1),new(8,5,2),new(8,0,3),new(8,5,4),new(8, 5,5),new(8, 2,6),new(8,2,7)}},
                new() { FloorNumber = 9, Passengers = new() { new(9,5,1),new(9,4,2),new(9,2,3),new(9,5,4),new(9, 6,5),new(9, 2,6),new(9,1,7)}},
            };
        }
        public static int GetMaximumNumberOfFloorsByFloornumber(List<Floor> floors)
        {
            return floors.OrderBy(a => a.FloorNumber).LastOrDefault().FloorNumber;
        }
        public static List<Elevator> GenerateMockElevators(int MaximumFloors)
        {
            return new List<Elevator>()
            {
                new(1,MaximumFloors,10,0,DirectionType.Up,"Elevator A"),
                new(2,MaximumFloors,10,6,DirectionType.Down, "Elevator B"),
                new(3,MaximumFloors,10,3,DirectionType.Up, "Elevator C"),
                new(4,MaximumFloors,10,2,DirectionType.Up, "Elevator D"),
                new(5,MaximumFloors,10,7,DirectionType.Down, "Elevator E"),
                new(6,MaximumFloors,10,9,DirectionType.Up, "Elevator F")
            };
        }

        public static Floor GetFloor(List<Floor> floors, int floorNumber)
        {
            return floors.FirstOrDefault(a => a.FloorNumber == floorNumber);
        }
        public static List<Floor> GetFloorsWithQueue(List<Floor> floors)
        {
            return floors.Where(a => a.Passengers.Any(p => !p.IsRemovedFromFloor)).ToList();
        }
        private static int GetLastFloor(List<Floor> floors, Elevator elevator)
        {
            int floorNumber = 0;
            var floor = floors.Where(a => a.Passengers.Any(p => !p.IsRemovedFromFloor)).OrderByDescending(a => a.FloorNumber).FirstOrDefault();
            if (floor != null)
            {
                floorNumber = floor.FloorNumber;
            }
            var elevatorLastFloor = elevator.CurrentPassengers.Where(p => !p.IsRemovedFromElevator).OrderByDescending(a => a.DestinationFloor).FirstOrDefault();

            if (floor == null && elevatorLastFloor != null)
            {
                floorNumber = elevatorLastFloor.DestinationFloor;
            }
            else if (floor != null && elevatorLastFloor != null)
            {
                if (floorNumber < elevatorLastFloor.DestinationFloor)
                {
                    floorNumber = elevatorLastFloor.DestinationFloor;
                }
            }
            else if (floor != null && elevatorLastFloor == null && floorNumber < elevator.GetLastFloor())
            {
                floorNumber = elevator.GetLastFloor();
            }

            if (floorNumber > elevator.GetLastFloor())
            {
                floorNumber = elevator.GetLastFloor();
            }
            return floorNumber;
        }
        private static int GetFirstFloor(List<Floor> floors, Elevator elevator)
        {
            int floorNumber = 0;
            var floor = floors.Where(a => a.Passengers.Any(p => !p.IsRemovedFromFloor)).OrderBy(a => a.FloorNumber).FirstOrDefault();
            if (floor != null)
            {
                floorNumber = floor.FloorNumber;
            }
            var elevatorLastStop = elevator.CurrentPassengers.Where(p => !p.IsRemovedFromElevator).OrderBy(a => a.DestinationFloor).FirstOrDefault();

            if (floor == null && elevatorLastStop != null)
            {
                floorNumber = elevatorLastStop.DestinationFloor;
            }

            if (elevatorLastStop != null)
            {
                if (floorNumber > elevatorLastStop.DestinationFloor)
                {
                    floorNumber = elevatorLastStop.DestinationFloor;
                }
            }

            if (elevator.CurrentFloor > elevator.GetLastFloor())
            {
                elevator.CurrentFloor = elevator.GetLastFloor();
            }
            return floorNumber;

        }
        public static int GetLastStop(List<Floor> floors, Elevator elevator)
        {
            int floorNumber = 0;
            if (elevator.Direction.Equals(DirectionType.Up))
            {
                floorNumber= GetLastFloor(floors, elevator);
            }
            else if(elevator.Direction.Equals(DirectionType.Down))
            {
                floorNumber= GetFirstFloor(floors, elevator);
            }
            return floorNumber;
        }
    }
}
