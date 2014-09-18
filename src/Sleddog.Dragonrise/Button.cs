using System;

namespace Sleddog.Dragonrise
{
    [Flags]
    public enum Button
    {
        None = 0x00,
        Select = 0x01,
        Start = 0x02,
        L = 0x04,
        R = 0x08,
        A = 0x10,
        B = 0x20,
        X = 0x40,
        Y = 0x80
    }
}