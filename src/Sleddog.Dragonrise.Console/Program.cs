namespace Sleddog.Dragonrise.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gamepad = new Gamepad();

            gamepad.ReadInput();

            System.Console.ReadLine();
        }
    }
}