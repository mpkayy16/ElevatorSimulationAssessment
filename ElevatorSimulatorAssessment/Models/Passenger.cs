using ElevatorSimulatorAssessment.Enums;

namespace ElevatorSimulatorAssessment.Models
{
    public class Passenger
    {
        public int QueueNumber { get; set; }
        public int CallingFloor { get; set; }
        public int DestinationFloor { get; set; }
        public DirectionType Direction { get; set; }
        public bool IsRemovedFromFloor { get; set; } 
        public bool IsRemovedFromElevator { get; set; }
        public string PassengerRef { get; set; }
        public Passenger(int callingFloor, int destinationFloor,int queueNumber)
        {
            CallingFloor = callingFloor;
            DestinationFloor = destinationFloor;
            QueueNumber = queueNumber;
            Direction = CallingFloor > DestinationFloor ? DirectionType.Down : DirectionType.Up;
            PassengerRef = $"cf{callingFloor}_df{destinationFloor}_qn{queueNumber}";
        }
    }
}