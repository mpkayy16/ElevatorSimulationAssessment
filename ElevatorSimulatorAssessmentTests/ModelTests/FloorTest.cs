using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Extensions;
using ElevatorSimulatorAssessment.Models;

namespace ElevatorSimulatorAssessmentTests
{
    [TestClass]
    public class FloorTest
    {
        [TestMethod]
        [TestCategory("Floor to return the next number by passengers count")]
        public void Floor_GetPassengersWaitingNumber_Returns_New_Number()
        {
            var floor = FloorExtensions.GenerateMockFloors().FirstOrDefault();
            var passengerNumber = floor.GetPassengersWaitingNumber();

            Assert.IsTrue(passengerNumber > 0);
            Assert.IsFalse(floor.Passengers.Any(a => a.QueueNumber == passengerNumber));

        }

        [TestMethod]
        [TestCategory("Floor to return the next passengers in the queue going to the available elevator direction")]
        public void Floor_GetPassengersToLoad_Returns_Passengers_To_Load_For_Elevator()
        {
            var floor = FloorExtensions.GenerateMockFloors().FirstOrDefault(a => a.FloorNumber == 5);
            var passengersGoingUp = floor.Passengers.Where(a => a.Direction.Equals(DirectionType.Up) && !a.IsRemovedFromFloor && a.DestinationFloor > floor.FloorNumber).ToList();

            var passengersToLoad = floor.GetPassengersToLoad(DirectionType.Up, 5);

            Assert.AreNotEqual(floor.Passengers.Count, passengersToLoad.Count);
            Assert.AreEqual(passengersGoingUp.Count, passengersToLoad.Count);
        }

        [TestMethod]
        [TestCategory("Floor adds new passenger in the queuea and return new passenger object")]
        public void Floor_AddPassengerToQueue_Creates_A_Passenger_AddsToQue_Returns_NewPassenger()
        {
            Floor floor = FloorExtensions.GenerateMockFloors().FirstOrDefault(a => a.FloorNumber == 5);
            int nextQueueNumber = floor.GetPassengersWaitingNumber();
            int currentPassengersCount = floor.Passengers.Where(a => !a.IsRemovedFromFloor).ToList().Count;

            Passenger passenger = floor.AddPassengerToQueue(8);
            int updatedPassengersCount = floor.Passengers.Where(a => !a.IsRemovedFromFloor).ToList().Count;

            Assert.IsNotNull(passenger);
            Assert.AreEqual(passenger.QueueNumber, nextQueueNumber);
            Assert.AreNotEqual(currentPassengersCount, updatedPassengersCount);
            Assert.IsTrue(currentPassengersCount < updatedPassengersCount);
        }
    }
}