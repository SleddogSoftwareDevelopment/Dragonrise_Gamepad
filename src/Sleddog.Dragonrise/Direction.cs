using System;

namespace Sleddog.Dragonrise
{
    [Flags]
    public enum Direction
    {
        None = 0x00,
        Up = 0x01,
        Right = 0x02,
        Down = 0x04,
        Left = 0x08
    }
}