using ElevatorSimulatorAssessment.Controller;
using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Extensions;
using ElevatorSimulatorAssessment.Models;

namespace ElevatorSimulatorAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Floor> mockFloors = FloorExtensions.GenerateMockFloors();
            int maxBuildingFloorNumber = FloorExtensions.GetMaximumNumberOfFloorsByFloornumber(mockFloors);
            List<Elevator> mockElevators = FloorExtensions.GenerateMockElevators(maxBuildingFloorNumber);

            Building building = new("Menzi Dvt Building",mockFloors,mockElevators);
            int CallingFloor, DestinationFloor;
            bool hasIncorrectFormat;

            ConsolePrinter.Print($" \t -------- {building.BuildingName} -------- \n \n \t ********** \"DVT elevator\" **********\n \t {building.BuildingDescription} ", MessageType.Info, true);

            try
            {
                do
                {
                    hasIncorrectFormat = false;

                    ConsolePrinter.Print("Insert current floor \t\t : ", MessageType.Prompt, true);
                    CallingFloor = Convert.ToInt32(Console.ReadLine());

                    ConsolePrinter.Print("Insert destination floor \t : ", MessageType.Prompt, true);
                    DestinationFloor = Convert.ToInt32(Console.ReadLine());

                    if (DestinationFloor > building.MaximumFloors || CallingFloor > building.MaximumFloors || CallingFloor == DestinationFloor)
                    {
                        hasIncorrectFormat = true;
                        var message = "An error occured while processing this request! \n\n" +
                                      $"* Ensure destination floor is not greater than maximum number of floors - [{building.MaximumFloors}] \n" +
                                      $"* Ensure current floor is not greater than maximum number of floors - [{building.MaximumFloors}] \n" +
                                      "* Ensure current floor is not your destination floor \n";
                        ConsolePrinter.Print($"{message}", MessageType.Error, true);
                    }
                } while (hasIncorrectFormat);

                var passenger = FloorExtensions.GetFloor(building.Floors, CallingFloor).AddPassengerToQueue(DestinationFloor);

                ConsolePrinter.Print(" \n ********** Calling \"DVT elevator\" ********** \n \t   .... please wait .... \n", MessageType.Info, true);
                ElevatorController.StartSimulation(building.Floors, building.Elevators, passenger);
            }
            catch (Exception ex)
            {
                var message = "An error occured while processing this request! \n\n" +
                              "* Please insert correct Current and destination floor  \n";
                ConsolePrinter.Print($"{message}", MessageType.Error, true);
            }
            ConsolePrinter.Print(" \n ********** \" ELEVATOR SIMULATION COMPLETE \"  ********** \n", MessageType.Success, true);

            Console.ReadKey();
        }
    }

}

