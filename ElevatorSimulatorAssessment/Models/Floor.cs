using ElevatorSimulatorAssessment.Enums;

namespace ElevatorSimulatorAssessment.Models
{
    public class Floor
    {
        public int FloorNumber { get; set; }
        public List<Passenger>? Passengers { get; set; }
        public Floor()
        {
            Passengers = new List<Passenger>();
        }
        public int GetPassengersWaitingNumber()
        {
            if (Passengers == null)
            {
                return 0;
            }
            return Passengers.Count + 1;
        }
        public Passenger AddPassengerToQueue(int destionationFloor)
        {
            var queueNumber = GetPassengersWaitingNumber();
            Passenger passenger= new(FloorNumber, destionationFloor, queueNumber);
            Passengers.Add(passenger);
            return passenger;

        }
        public void RemovePassengersFromQueue(int take, DirectionType direction, List<int> passengerIds)
        {
            var passengersToRemove = Passengers.Where(a => a.Direction.Equals(direction) && !a.IsRemovedFromFloor).Take(take);
            foreach (var passenger in passengersToRemove)
            {
                if (passengerIds.Find(a => a == passenger.QueueNumber) != null)
                {
                    passenger.IsRemovedFromFloor = true;
                }
            }
        }

        //public List<Passenger> GetPassengersToLoad()
        //{
        //    return Passengers.Where(p => !p.IsRemovedFromFloor).ToList();
        //}
        public List<Passenger> GetPassengersToLoad(DirectionType elevatorDirection, int currentFloor)
        {
            if (elevatorDirection.Equals(DirectionType.Up))
            {
                return Passengers.Where(p => p.Direction == elevatorDirection && !p.IsRemovedFromFloor && p.DestinationFloor > currentFloor).ToList();
            }
            else
            {
                return Passengers.Where(p => p.Direction == elevatorDirection && !p.IsRemovedFromFloor && p.DestinationFloor < currentFloor).ToList();
            }
        }
    }
}