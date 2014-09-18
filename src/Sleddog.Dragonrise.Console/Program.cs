using System;
using System.Diagnostics;

namespace Sleddog.Dragonrise.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gamepad = new Gamepad();

            gamepad.EventStream.Subscribe(e => { Debug.WriteLine(e); });

            System.Console.ReadLine();
        }
    }
}