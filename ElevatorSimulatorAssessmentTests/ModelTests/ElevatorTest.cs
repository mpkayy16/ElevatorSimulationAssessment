using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Models;

namespace ElevatorSimulatorAssessmentTests
{
    [TestClass]
    public class ElevatorTest
    {
        [TestMethod]
        [TestCategory("Elevator return false to having no passengers")]
        public void HasPassengersToOffload_ReturnFalse_When_It_HasNo_Passengers()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");

            var hasPassengersToOffload = elevator.HasPassengersToOffload();

            Assert.IsFalse(hasPassengersToOffload);
        }
        [TestMethod]
        [TestCategory("Elevator return true to having passengers")]
        public void HasPassengersToOffload_ReturnTrue_When_It_Has_Passengers_To_Offload()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");
            elevator.CurrentPassengers.Add(new(3, 5, 8));
            var hasPassengersToOffload = elevator.HasPassengersToOffload();

            Assert.IsTrue(hasPassengersToOffload);
        }

        [TestMethod]
        [TestCategory("Elevator moves and update current floor")]
        public void Move_Elevator_Updates_CurrentFloor_And_Moves_In_The_Direction()
        {
            int currentFloor = 5;
            Elevator elevator = new(1, 9, 10, currentFloor, DirectionType.Up, "DVT Passenger");
            elevator.Move(false);

            Assert.AreNotEqual(currentFloor, elevator.CurrentFloor);
            Assert.IsTrue(elevator.CurrentFloor > currentFloor);
        }
        [TestMethod]
        [TestCategory("Elevator changes direction when max floor reached")]
        public void Move_Elevator_Updates_CurrentFloor_And_Changes_Direction_OnMax_Floor()
        {
            int currentFloor = 8;
            Elevator elevator = new(1, 9, 10, currentFloor, DirectionType.Up, "DVT Passenger");
            elevator.Move(false);

            Assert.AreNotEqual(currentFloor, elevator.CurrentFloor);
            Assert.IsTrue(elevator.CurrentFloor > currentFloor);
            Assert.AreEqual(DirectionType.Down, elevator.Direction);
        }
        [TestMethod]
        [TestCategory("Elevator updates the capacity which enables it to load accordingly")]
        public void UpdateCapacity_Returns_Updated_CapacityValues()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");
            elevator.CurrentPassengers.Add(new(3, 5, 8));
            var previousAvailability = elevator.AvailableCapacity;
            var previousCapacity = elevator.CurrentCapacity;

            elevator.UpdateCapacity();

            Assert.AreNotEqual(previousCapacity, elevator.CurrentCapacity);
            Assert.AreNotEqual(previousAvailability, elevator.AvailableCapacity);
        }
        [TestMethod]
        [TestCategory("Elevator checks if it has the current passenger as so to print on console(which would be the sound made to the users in elevator")]
        public void IsCurrentPassengerInThisElevator_ReturnTrue_When_CurrentPassengerIsInElevator()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");
            Passenger passenger = new(3, 5, 8);
            elevator.CurrentPassengers.Add(passenger);

            var passengerInElevator = elevator.IsCurrentPassengerInThisElevator(passenger);

            Assert.IsTrue(passengerInElevator);
        }
        [TestMethod]
        [TestCategory("Elevator return false to having current passenger if passenger is marked as removed")]
        public void IsCurrentPassengerInThisElevator_ReturnFalse_When_CurrentPassengerIsInRemoved()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");
            Passenger passenger = new(3, 5, 8)
            {
                IsRemovedFromElevator = true
            };
            elevator.CurrentPassengers.Add(passenger);

            var passengerInElevator = elevator.IsCurrentPassengerInThisElevator(passenger);

            Assert.IsFalse(passengerInElevator);
        }

        [TestMethod]
        [TestCategory("Elevator removes all passengers who reached destination and updates capacity")]
        public void RemovePassengers_Updates_EachPassenger_As_Removed_From_Elevator_And_Update_Capacity()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");
            Passenger currentPassenger = new(3, 5, 7);
            elevator.CurrentPassengers.Add(currentPassenger);
            elevator.CurrentPassengers.Add(new(1, 5, 8));
            elevator.CurrentPassengers.Add(new(0, 5, 9));
            elevator.CurrentPassengers.Add(new(2, 6, 10));
            elevator.UpdateCapacity();
            var previousCapacity = elevator.CurrentCapacity;
            var previousAvailableCapacity = elevator.AvailableCapacity;

            var hasPassengersToOffload = elevator.HasPassengersToOffload();
            elevator.RemovePassengers(currentPassenger.QueueNumber, currentPassenger.PassengerRef);

            Assert.IsTrue(previousCapacity > elevator.CurrentCapacity);
            Assert.IsTrue(previousAvailableCapacity < elevator.AvailableCapacity);
            Assert.AreNotEqual(hasPassengersToOffload, elevator.HasPassengersToOffload());
            Assert.IsFalse(elevator.HasPassengersToOffload());
            Assert.IsTrue(elevator.CurrentPassengers.Any(a=>a.IsRemovedFromElevator));
        }

        [TestMethod]
        [TestCategory("Elevator adds all passengers eligible(same direction as elevator)")]
        public void AddAllFloorPassengers_Updates_EachPassenger_As_RemovedFromFloor_And_TakesOnlyEligible()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");
            elevator.UpdateCapacity();
            Passenger currentPassenger = new(5, 5, 7);
            List<Passenger> passengers = new()
            {
                currentPassenger,
                new(5, 7, 8),
                new(5, 8, 9),
                new(5, 0, 10)
            };
            var previousCapacity = elevator.CurrentCapacity;
            var previousAvailableCapacity = elevator.AvailableCapacity;

            elevator.AddAllFloorPassengers(passengers,currentPassenger);

            Assert.IsTrue(previousCapacity < elevator.CurrentCapacity);
            Assert.IsTrue(previousAvailableCapacity > elevator.AvailableCapacity);
            Assert.AreEqual(passengers.Count(a=>a.IsRemovedFromFloor), elevator.CurrentPassengers.Count(a=>a.IsRemovedFromFloor && a.CallingFloor == elevator.CurrentFloor));
        }
        [TestMethod]
        [TestCategory("Elevator adds passengers by available capacity")]
        public void AddPassengersByAvailableCapacity_Updates_EachPassenger_As_RemovedFromFloor_And_TakesByElevatorAvailability()
        {
            Elevator elevator = new(1, 9, 10, 5, DirectionType.Up, "DVT Passenger");
            elevator.CurrentPassengers.Add(new(1, 7, 8));
            elevator.CurrentPassengers.Add(new(0, 8, 9));
            elevator.CurrentPassengers.Add(new(2, 9, 10));
            elevator.CurrentPassengers.Add(new(3, 8, 11));
            elevator.CurrentPassengers.Add(new(2, 9, 12));
            elevator.CurrentPassengers.Add(new(4, 9, 13));
            elevator.UpdateCapacity();

            Passenger currentPassenger = new(5, 5, 7);
            List<Passenger> passengers = new()
            {
                currentPassenger,
                new(5, 7, 8),
                new(5, 8, 9),
                new(5, 9, 9),
                new(5, 0, 10)
            };


            var queueNumbersTaken = elevator.AddPassengersByAvailableCapacity(passengers, currentPassenger);

            Assert.AreEqual(elevator.MaxCapacity, elevator.CurrentCapacity);
            Assert.AreNotEqual(queueNumbersTaken.Count, passengers.Count);
        }

    }
}