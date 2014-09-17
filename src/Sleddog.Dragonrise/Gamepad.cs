using System.Diagnostics;
using System.Linq;
using System.Threading;
using HidLibrary;

namespace Sleddog.Dragonrise
{
    public class Gamepad
    {
        private static readonly int VendorId = 0x0079;
        private static readonly int ProductId = 0x0011;
        private readonly IHidDevice device;

        public Gamepad()
        {
            device = new HidEnumerator().Enumerate(VendorId, ProductId).First();

            device.MonitorDeviceEvents = true;
        }

        public void ReadInput()
        {
            device.ReadReport(OnRead);
        }

        private void OnRead(HidReport report)
        {
            Debug.WriteLine("Status: {0}", report.ReadStatus);

            if (report.ReadStatus == HidDeviceData.ReadStatus.Success)
            {
                Debug.WriteLine("ReportId: {0}", report.ReportId);

                foreach (var d in report.Data)
                {
                    Debug.Write(string.Format("{0:X} ", d));
                }

                Debug.WriteLine(string.Empty);
            }

            Thread.Sleep(1000);

            device.ReadReport(OnRead);
        }
    }
}