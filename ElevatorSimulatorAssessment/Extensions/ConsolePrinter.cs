using ElevatorSimulatorAssessment.Enums;

namespace ElevatorSimulatorAssessment.Extensions
{
    public static class ConsolePrinter
    {
        public static void Print(string message, MessageType messageType, bool mustPrint)
        {
            if (mustPrint)
            {
                Console.ForegroundColor = messageType switch
                {
                    MessageType.Info => ConsoleColor.Blue,
                    MessageType.Error => ConsoleColor.Red,
                    MessageType.Success => ConsoleColor.Green,
                    _ => ConsoleColor.White,
                };

                if (messageType == MessageType.Prompt)
                {
                    Console.Write(message);
                }
                else
                {
                    Console.WriteLine(message);
                }
                Console.ForegroundColor = ConsoleColor.White;
                if (messageType != MessageType.Prompt)
                {
                    Console.WriteLine("\n********************************************************\n");
                }
            }
        }
    }
}
