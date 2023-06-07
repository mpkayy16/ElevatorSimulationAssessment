using ElevatorSimulatorAssessment.Enums;
using ElevatorSimulatorAssessment.Extensions;
using ElevatorSimulatorAssessment.Models;

namespace ElevatorSimulatorAssessment.Controller
{
    public class ElevatorController
    {
        public static void StartSimulation(List<Floor> floors, List<Elevator> elevators, Passenger currentPassenger)
        {
            var elevatorIsCalled = floors.Any(a => a.Passengers.Where(p => !p.IsRemovedFromFloor).ToList().Count > 0);

            while (elevatorIsCalled)
            {
                List<Floor> floorsWithQueues = new();
                List<Elevator> elevatorsWithPassengers = new();
                foreach (Elevator elevator in elevators)
                {
                    elevator.UpdateCapacity();
                    bool currentPassengerInThisElevator = elevator.IsCurrentPassengerInThisElevator(currentPassenger);
                    int lastStop = FloorExtensions.GetLastStop(floors, elevator);

                    if (elevator.Direction.Equals(DirectionType.Up))
                    {

                        while (elevator.CurrentFloor <= lastStop)
                        {
                            lastStop = FloorExtensions.GetLastStop(floors, elevator);
                            if (elevator.CurrentCapacity > 0 && elevator.HasPassengersToOffload())
                            {
                                elevator.RemovePassengers(currentPassenger.QueueNumber,currentPassenger.PassengerRef);
                            }

                            Floor currentFloor = FloorExtensions.GetFloor(floors, elevator.CurrentFloor);
                            List<Passenger> passengersToLoad = currentFloor.GetPassengersToLoad(elevator.Direction, elevator.CurrentFloor);
                            if (elevator.AvailableCapacity > 0)
                            {
                                if (passengersToLoad.Count > 0)
                                {
                                    if ((elevator.AvailableCapacity > passengersToLoad.Count || elevator.AvailableCapacity == passengersToLoad.Count) && passengersToLoad.Count != 0)
                                    {
                                        var passengerQNumbers = elevator.AddAllFloorPassengers(passengersToLoad.Where(a => a.Direction == elevator.Direction).ToList(), currentPassenger);
                                        currentFloor.RemovePassengersFromQueue(elevator.AvailableCapacity, elevator.Direction, passengerQNumbers);
                                    }
                                    else if ((elevator.AvailableCapacity < passengersToLoad.Count)
                                                    && elevator.AvailableCapacity != 0 && passengersToLoad.Count != 0)
                                    {
                                        var passengerQNumbers = elevator.AddPassengersByAvailableCapacity(passengersToLoad.Where(a => a.Direction == elevator.Direction).ToList(), currentPassenger);
                                        currentFloor.RemovePassengersFromQueue(elevator.AvailableCapacity, elevator.Direction, passengerQNumbers);
                                    }
                                }
                                else if (elevator.CurrentCapacity > 0 && elevator.HasPassengersToOffload())
                                {
                                    elevator.RemovePassengers(currentPassenger.QueueNumber, currentPassenger.PassengerRef);
                                }
                                currentPassengerInThisElevator = elevator.IsCurrentPassengerInThisElevator(currentPassenger);
                            }
                            elevator.Move(currentPassengerInThisElevator);

                            if (elevator.CurrentFloor > lastStop || elevator.CurrentCapacity == 0)
                            {
                                if (elevator.CurrentFloor >= lastStop)
                                {
                                    elevator.CurrentFloor = lastStop;
                                    elevator.Direction = DirectionType.Down;
                                    if (elevator.CurrentCapacity == 0)
                                    {
                                        break;
                                    }
                                }
                            }

                            lastStop = FloorExtensions.GetLastStop(floors, elevator);
                            if (elevator.CurrentFloor == lastStop && elevator.CurrentCapacity == 0)
                            {
                                break;
                            }
                            else if (elevator.CurrentFloor == lastStop && elevator.CurrentCapacity != 0 && elevator.HasPassengersToOffload())
                            {
                                elevator.RemovePassengers(currentPassenger.QueueNumber, currentPassenger.PassengerRef);
                                break;
                            }

                        }

                        if (elevator.CurrentCapacity == 0 && elevator.Direction.Equals(DirectionType.Up))
                        {
                            elevator.Direction = DirectionType.Down;
                        }
                    }
                    else if (elevator.Direction.Equals(DirectionType.Down))
                    {
                        while (elevator.CurrentFloor >= lastStop)
                        {
                            if (elevator.CurrentCapacity > 0 && elevator.HasPassengersToOffload())
                            {
                                elevator.RemovePassengers(currentPassenger.QueueNumber, currentPassenger.PassengerRef);
                            }

                            Floor currentFloor = FloorExtensions.GetFloor(floors, elevator.CurrentFloor);

                            List<Passenger> passengersToLoad = currentFloor.GetPassengersToLoad(elevator.Direction, elevator.CurrentFloor);
                            if (elevator.AvailableCapacity > 0)
                            {
                                if (passengersToLoad.Count > 0)
                                {
                                    if ((elevator.AvailableCapacity > passengersToLoad.Count || elevator.AvailableCapacity == passengersToLoad.Count) && passengersToLoad.Count != 0)
                                    {
                                        var passengerQNumbers = elevator.AddAllFloorPassengers(passengersToLoad, currentPassenger);
                                        currentFloor.RemovePassengersFromQueue(elevator.AvailableCapacity, elevator.Direction, passengerQNumbers);
                                    }
                                    else if ((elevator.AvailableCapacity < passengersToLoad.Count)
                                                    && elevator.AvailableCapacity != 0 && passengersToLoad.Count != 0)
                                    {
                                        var passengerQNumbers = elevator.AddPassengersByAvailableCapacity(passengersToLoad, currentPassenger);
                                        currentFloor.RemovePassengersFromQueue(elevator.AvailableCapacity, elevator.Direction, passengerQNumbers);
                                    }
                                }
                                currentPassengerInThisElevator = elevator.IsCurrentPassengerInThisElevator(currentPassenger);
                            }
                            elevator.Move(currentPassengerInThisElevator);

                            if (elevator.CurrentCapacity == 0)
                            {
                                if (elevator.CurrentFloor == lastStop)
                                {
                                    elevator.Direction = DirectionType.Up;
                                }
                            }
                            lastStop = FloorExtensions.GetLastStop(floors, elevator);

                            if (elevator.CurrentFloor == lastStop && elevator.CurrentCapacity == 0)
                            {
                                break;
                            }
                            else if (elevator.CurrentFloor == lastStop && elevator.CurrentCapacity != 0 && elevator.HasPassengersToOffload())
                            {
                                elevator.RemovePassengers(currentPassenger.QueueNumber, currentPassenger.PassengerRef);
                                break;
                            }
                        }
                        if (elevator.CurrentCapacity == 0 && elevator.Direction.Equals(DirectionType.Down))
                        {
                            elevator.Direction = DirectionType.Up;
                        }
                    }

                    floorsWithQueues = FloorExtensions.GetFloorsWithQueue(floors);
                    elevatorsWithPassengers = GetElevatorsWithPassengers(elevators);
                    if (floorsWithQueues.Count == 0 && elevatorsWithPassengers.Count == 0)
                    {
                        elevatorIsCalled = false;
                        break;
                    }

                }
            }
            ConsolePrinter.Print("Elevators are all empty and available now", MessageType.Success, true);
        }
        private static List<Elevator> GetElevatorsWithPassengers(List<Elevator> elevators)
        {
            return elevators.Where(a => a.CurrentPassengers.Where(p => p.IsRemovedFromElevator == false).Any()).ToList();
        }
    }
}
