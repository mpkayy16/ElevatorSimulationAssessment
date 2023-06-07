using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Extensions;

namespace ElevatorSimulatorAssessment.Models
{
    public class Elevator
    {
        public int ElevatorNumber { get; set; }
        public string ElevatorName { get; set; }
        public int CurrentFloor { get; set; }
        public int AvailableCapacity { get; internal set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }
        public DirectionType Direction { get; set; }
        private int MaximumNumberOfFloors { get; }
        private bool IsDoorOpen { get; set; }
        private bool CurrentUserInThisElevator { get; set; }
        public List<Passenger> CurrentPassengers { get; set; }
        public Elevator(int elevatorNumber, int floors, int maxCapacity, int currentFloor, DirectionType directionType, string elevatorName)
        {
            ElevatorNumber = elevatorNumber;
            IsDoorOpen = false;
            MaximumNumberOfFloors = floors;
            MaxCapacity = maxCapacity;
            CurrentFloor = currentFloor;
            ElevatorName = elevatorName;
            if (CurrentFloor >= MaximumNumberOfFloors && directionType.Equals(DirectionType.Up))
            {
                CurrentFloor = MaximumNumberOfFloors;
                Direction = DirectionType.Down;
            }
            else
            {
                Direction = directionType;
            }
            CurrentPassengers = new List<Passenger>();
        }

        public void OpenDoors(bool print = true)
        {
            IsDoorOpen = true;
            if (print)
            {
                ConsolePrinter.Print($"{ElevatorName} \n\t --- Door opening --- \n \t Current Floor\t    : {CurrentFloor} \n \t Maximum Capacity   : {MaxCapacity}  \n \t Current Capacity   : {CurrentCapacity} \n \t Available Capacity : {AvailableCapacity}",
                    MessageType.Info, CurrentUserInThisElevator);
            }
        }
        public void CloseDoors(bool print = true)
        {
            IsDoorOpen = false;
            if (print)
            {
                ConsolePrinter.Print($"{ElevatorName} \n\t --- Door Closing --- \n \t Current Floor\t    : {CurrentFloor} \n \t Maximum Capacity   : {MaxCapacity}  \n \t Current Capacity   : {CurrentCapacity} \n \t Available Capacity : {AvailableCapacity}"
                    , MessageType.Info, CurrentUserInThisElevator);
            }

        }
        public void Move(bool currentUserInThisElevator)
        {
            CurrentUserInThisElevator = currentUserInThisElevator;
            if (IsDoorOpen)
            {
                CloseDoors(CurrentUserInThisElevator);
            }

            if (Direction.Equals(DirectionType.Up))
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
        }

        public void UpdateCapacity()
        {
            var currentPassengers = CurrentPassengers.Where(a => !a.IsRemovedFromElevator).ToList();
            CurrentCapacity = currentPassengers.Count;
            AvailableCapacity = MaxCapacity - CurrentCapacity;
            if (AvailableCapacity < 0)
            {
                ConsolePrinter.Print($"{ElevatorName} an error capacity < 0", MessageType.Error, true);
            }
        }
        public bool IsCurrentPassengerInThisElevator(Passenger passenger)
        {
            CurrentUserInThisElevator = CurrentPassengers.Any(a => a.DestinationFloor == passenger.DestinationFloor
                                              && a.PassengerRef == passenger.PassengerRef
                                              && a.QueueNumber == passenger.QueueNumber && !a.IsRemovedFromElevator);
            return CurrentUserInThisElevator;
        }
        public List<int> AddAllFloorPassengers(List<Passenger> passengers, Passenger currentPassenger)
        {
            var passengersEligibleToBeLoaded = passengers.Where(a => a.CallingFloor == CurrentFloor && !a.IsRemovedFromFloor && !a.IsRemovedFromElevator && a.Direction == Direction).ToList();
            if (passengersEligibleToBeLoaded.Count > 0 && AvailableCapacity>0 && AvailableCapacity>= passengersEligibleToBeLoaded.Count)
            {
                OpenDoors(CurrentUserInThisElevator);
                CurrentPassengers.AddRange(passengersEligibleToBeLoaded);
                RemovePassengersFromFloor();
                UpdateCapacity();
                CloseDoors(IsCurrentPassengerInThisElevator(currentPassenger));
            }

            return CurrentPassengers.Select(a => a.QueueNumber).ToList();
        }
        public List<int> AddPassengersByAvailableCapacity(List<Passenger> passengersToLoad, Passenger currentPassenger)
        {
            var passengerQNumbers = passengersToLoad.Select(a => a.QueueNumber)
                .Take(AvailableCapacity).ToList();

            List<Passenger> passengersToTake = new();
            foreach (var queueNumber in passengerQNumbers)
            {
                passengersToTake.Add(passengersToLoad.First(a => a.QueueNumber == queueNumber));
            }
            OpenDoors(CurrentUserInThisElevator);
            CurrentPassengers.AddRange(passengersToTake);
            UpdateCapacity();
            CloseDoors(IsCurrentPassengerInThisElevator(currentPassenger));
            return passengerQNumbers;
        }
        public int GetLastFloor()
        {
            return MaximumNumberOfFloors;
        }
        public bool HasPassengersToOffload()
        {
            return CurrentPassengers.Where(a => !a.IsRemovedFromElevator && a.DestinationFloor == CurrentFloor).Any();
        }
        public void RemovePassengers(int currentPassengerQueueNumber, string PassengerRef)
        {
            List<Passenger> passengersWhoHaveArrived = GetPassengersToOffload();

            if (passengersWhoHaveArrived.Any())
            {
                if (CurrentUserInThisElevator && passengersWhoHaveArrived.Any(a => a.QueueNumber == currentPassengerQueueNumber && a.PassengerRef == PassengerRef))
                {
                    CurrentPassengers.FirstOrDefault(a => a.QueueNumber == currentPassengerQueueNumber).IsRemovedFromFloor = true;
                    ConsolePrinter.Print($"{ElevatorName} \n\t Floor {CurrentFloor} \n \t --- You have arrived ---", MessageType.Success, true);
                }
                OpenDoors(CurrentUserInThisElevator);
                OffloadPassengers();
            }
            UpdateCapacity();
        }
        private void OffloadPassengers()
        {
            var passengerToOffload = CurrentPassengers.Where(a => a.DestinationFloor == CurrentFloor && a.IsRemovedFromElevator == false).ToList();
            foreach (var passenger in passengerToOffload)
            {
                passenger.IsRemovedFromElevator = true;
            }
            UpdateCapacity();
        }
        private void RemovePassengersFromFloor()
        {
            foreach (var passenger in CurrentPassengers)
            {
                passenger.IsRemovedFromFloor = true;
            }
        }
        private void MoveDown()
        {
            ConsolePrinter.Print($"{ElevatorName} \n\t --- Going Down --- \n \t Current Floor\t    : {CurrentFloor} \n \t Maximum Capacity   : {MaxCapacity} \n \t Current Capacity   : {CurrentCapacity} \n \t Available Capacity : {AvailableCapacity}", MessageType.Info, CurrentUserInThisElevator);

            if (CurrentFloor != 0)
            {
                CurrentFloor--;
            }

            if (CurrentFloor == 0)
            {
                Direction = DirectionType.Up;
            }
        }
        private void MoveUp()
        {
            ConsolePrinter.Print($"{ElevatorName} \n\t --- Going UP --- \n \t Current Floor\t    : {CurrentFloor} \n \t Maximum Capacity   : {MaxCapacity}  \n \t Current Capacity   : {CurrentCapacity} \n \t Available Capacity : {AvailableCapacity}", MessageType.Info, CurrentUserInThisElevator);

            if (CurrentFloor != MaximumNumberOfFloors)
            {
                CurrentFloor++;
            }

            if (MaximumNumberOfFloors == CurrentFloor)
            {
                Direction = DirectionType.Down;
            }
        }
        private List<Passenger> GetPassengersToOffload()
        {
            return CurrentPassengers.Where(a => a.DestinationFloor == CurrentFloor && !a.IsRemovedFromElevator).ToList();
        }
    }

}