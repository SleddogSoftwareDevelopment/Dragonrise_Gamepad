namespace Sleddog.Dragonrise
{
    public struct GamepadEvent
    {
        public Direction Directions { get; private set; }
        public Button Buttons { get; private set; }

        public GamepadEvent(Direction directions, Button buttons) : this()
        {
            Directions = directions;
            Buttons = buttons;
        }

        public override string ToString()
        {
            return string.Format("Directions: {0} - Buttons: {1}", Directions, Buttons);
        }
    }
}