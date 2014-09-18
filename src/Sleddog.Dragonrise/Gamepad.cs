using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using HidLibrary;

namespace Sleddog.Dragonrise
{
    public class Gamepad
    {
        private const int VendorId = 0x0079;
        private const int ProductId = 0x0011;

        private readonly IHidDevice device;
        private readonly IObservable<GamepadEvent> eventStream;

        private const byte Up = 0x00;
        private const byte Down = 0xFF;
        private const byte Left = 0x00;
        private const byte Right = 0xFF;

        private const byte Select = 0x10;
        private const byte Start = 0x20;

        private const byte L = 0x01;
        private const byte R = 0x02;

        private const byte X = 0x1F;
        private const byte A = 0x2F;
        private const byte B = 0x4F;
        private const byte Y = 0x8F;

        public IObservable<GamepadEvent> EventStream
        {
            get { return eventStream; }
        }

        public Gamepad()
        {
            device = new HidEnumerator().Enumerate(VendorId, ProductId).First();

            eventStream = Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Select(_ => ReadReport())
                .DistinctUntilChanged(new HidReportComparer())
                .Select(MapReport)
                .Publish()
                .RefCount();
        }

        private HidReport ReadReport()
        {
            return device.ReadReport(99);
        }

        private GamepadEvent MapReport(HidReport report)
        {
            if (report.ReadStatus != HidDeviceData.ReadStatus.Success)
            {
                return new GamepadEvent();
            }

            var sslrButtons = MapStartSelect(report.Data[6]) | MapLR(report.Data[6]);
            var abxyButtons = MapAB(report.Data[5]) | MapXY(report.Data[5]);

            var buttons = sslrButtons | abxyButtons;

            var directions = MapUpDown(report.Data[4]) | MapLeftRight(report.Data[3]);

            return new GamepadEvent(directions, buttons);
        }

        private Button MapAB(byte ab)
        {
            var buttons = Button.None;

            if (ab == A)
            {
                buttons |= Button.A;
            }

            if (ab == B)
            {
                buttons |= Button.B;
            }

            return buttons;
        }

        private Button MapXY(byte xy)
        {
            var buttons = Button.None;

            if (xy == X)
            {
                buttons |= Button.X;
            }

            if (xy == Y)
            {
                buttons |= Button.Y;
            }

            return buttons;
        }

        private Button MapLR(byte lr)
        {
            var buttons = Button.None;

            if (lr == L)
            {
                buttons |= Button.L;
            }

            if (lr == R)
            {
                buttons |= Button.R;
            }

            return buttons;
        }

        private Button MapStartSelect(byte startSelect)
        {
            var buttons = Button.None;

            if (startSelect == Start)
            {
                buttons |= Button.Start;
            }

            if (startSelect == Select)
            {
                buttons |= Button.Select;
            }

            return buttons;
        }

        private Direction MapUpDown(byte upDown)
        {
            var directions = Direction.None;

            if (upDown == Up)
            {
                directions |= Direction.Up;
            }

            if (upDown == Down)
            {
                directions |= Direction.Down;
            }

            return directions;
        }

        public Direction MapLeftRight(byte leftRight)
        {
            var directions = Direction.None;

            if (leftRight == Left)
            {
                directions |= Direction.Left;
            }

            if (leftRight == Right)
            {
                directions |= Direction.Right;
            }

            return directions;
        }

        private class HidReportComparer : IEqualityComparer<HidReport>
        {
            public bool Equals(HidReport x, HidReport y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return Equals(x.ReadStatus, y.ReadStatus) && x.Data.SequenceEqual(y.Data);
            }

            public int GetHashCode(HidReport obj)
            {
                unchecked
                {
                    return (obj.ReadStatus.GetHashCode()*397) ^ (obj.Data.GetHashCode());
                }
            }
        }
    }
}