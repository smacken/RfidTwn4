using System.IO.Ports;

namespace TWN4Api
{
    public class DeviceReader
    {
        private SerialPort _port;
        public void Connect(string portName)
        {
            _port.PortName = portName;
            _port.BaudRate = 115200;
            _port.DataBits = 8;
            _port.StopBits = StopBits.One;
            _port.Parity = Parity.None;
            
            // NFC functions are known to take less than 2 second to execute.
            _port.ReadTimeout = 2000;
            _port.WriteTimeout = 2000;
            _port.NewLine = "\r";
            
            _port.Open();
        }

        private string GetPortName(int port)
        {
            var device = new DeviceIO();
            string portName;
            if (port == 0)
            {
                var path = device.FindUSBDevice("usbser", "USB\\VID_09D8&PID_0420\\");
                if (path == string.Empty) return string.Empty;
                portName = device.COMPort(device.GetCOMPort(path));
                if (portName == string.Empty) return string.Empty;
            }
            else
            {
                portName = device.COMPort(port);
                if (portName == string.Empty) return string.Empty;
            }
            return portName;
        }

        public byte[] Transmit(byte[] cmd)
        {
            _port.DiscardInBuffer();
            _port.WriteLine(Protocol.ToPrs(cmd));
            return Protocol.FromPrs(_port.ReadLine());
        }
    }
}