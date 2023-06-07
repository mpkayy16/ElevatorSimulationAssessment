using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Extensions;
using ElevatorSimulatorAssessment.Models;

namespace ElevatorSimulatorAssessmentTests
{
    [TestClass]
    public class FloorExtensionsTest
    {

        [TestMethod]
        [TestCategory("FloorExtensions_ Get a list of mock Floors")]
        public void GenerateMockFloors_Gets_A_List_Of_Mock_Floors()
        {
            Assert.IsNotNull(FloorExtensions.GenerateMockFloors());
        }

        [TestMethod]
        [TestCategory("FloorExtensions")]
        public void GetFloorsWithQueue_Gets_A_List_Of_Floors_With_Queue_Only()
        {
            var mockFloors = FloorExtensions.GenerateMockFloors();
            for (int i = 0; i < 2; i++)
            {
                var queueNumbersToRemove = mockFloors[i].Passengers.Select(a => a.QueueNumber).ToList();
                mockFloors[i].RemovePassengersFromQueue(7, DirectionType.Up, queueNumbersToRemove);
            }

            var floorsWithQueues = FloorExtensions.GetFloorsWithQueue(mockFloors);

            Assert.AreNotEqual(mockFloors.Count, floorsWithQueues.Count);
            Assert.IsFalse(floorsWithQueues.Any(a => a.Passengers.Any(p => p.IsRemovedFromFloor)));
        }

        [TestMethod]
        [TestCategory("FloorExtensions gets a floor")]
        public void Gets_Floor_By_FloorNumber_In_A_List()
        {
            var floors = FloorExtensions.GenerateMockFloors();

            var floor = FloorExtensions.GetFloor(floors, 1);
            Assert.IsNotNull(floor);
        }

        [TestMethod]
        [TestCategory("FloorExtensions get elevators with set maximum floor it can reach")]
        public void GenerateMockElevators_Gets_A_List_Of_Mock_Elevators_And_Set_MaximumFloor_10()
        {
            var elevators = FloorExtensions.GenerateMockElevators(10);

            Assert.IsNotNull(elevators);
            Assert.IsFalse(elevators.Any(a=>a.GetLastFloor() != 10));
        }


        [TestMethod]
        [TestCategory("FloorExtensions gets a last stop in the direction")]
        public void GetLastStop_Gets_Last_Floor_By_Direction_Down()
        {

            var floors = FloorExtensions.GenerateMockFloors();
            Elevator elevator = new(7,9,10,5,DirectionType.Down, "Test Going Down Last Floor");//1,9, 10, 4, DirectionType.Up, "Test Going Down Last Floor"
            
            Assert.AreEqual(FloorExtensions.GetLastStop(floors, elevator),0);
        }

        [TestMethod]
        [TestCategory("FloorExtensions gets a last stop in the direction")]
        public void GetLastStop_Gets_Last_Floor_By_Direction_Up()
        {

            var floors = FloorExtensions.GenerateMockFloors();
            Elevator elevator = new(7, 9, 10, 5, DirectionType.Up, "Test Going Down Last Floor");//1,9, 10, 4, DirectionType.Up, "Test Going Down Last Floor"

            Assert.AreEqual(FloorExtensions.GetLastStop(floors, elevator), 9);
        }
    }
}