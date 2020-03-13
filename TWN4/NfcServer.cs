using System;

namespace TWN4Api
{
    /// <summary>
    /// SNEP protocol server
    /// </summary>
    public class NfcServer
    {
        private readonly DeviceReader _reader;
        public NfcServer()
        {
            _reader = new DeviceReader();
        }

        public bool Init()
        {
            byte[] request = { 0x18, 0x00 };
            byte[] response = { 0x00, 0x01 };
            byte[] buffer = _reader.Transmit(request);
            if (!response.CompareSegments(0, buffer, 0, 2))
                return false;
            return true;
        }

        public bool Begin(int val)
        {
            byte[] request = { 0x18, 0x03 };
            request = request.Add(BitConverter.GetBytes(val));
            byte[] response = { 0x00, 0x01 };
            byte[] buffer = _reader.Transmit(request);
            if (!response.CompareSegments(0, buffer, 0, 2)) return false;
            return true;
        }

        public bool SearchTag()
        {
            byte[] request = { 0x05, 0x00, 0x10 };
            byte[] response = { 0x00, 0x01 };
            byte[] buffer = _reader.Transmit(request);
            if (!response.CompareSegments(0, buffer, 0, 2)) return false;
            return true;
        }

        private bool SetTagTypeNFC()
        {
            byte[] request = { 0x05, 0x02, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00 };
            byte[] response = { 0x00 };
            byte[] buffer = _reader.Transmit(request);
            if (!response.CompareSegments(0, buffer, 0, 1)) return false;
            return true;
        }

        public byte GetConnectionState()
        {
            byte[] request = { 0x18, 0x01 };
            byte[] response = { 0x00 };
            byte[] buffer = _reader.Transmit(request);
            if (!response.CompareSegments(0, buffer, 0, 1)) return 0;
            return buffer[1];
        }

        public bool SendMessageFragment(byte[] message)
        {
            byte[] request = { 0x18, 0x04 };
            request = request.Add(BitConverter.GetBytes(Convert.ToUInt16(message.Length)));
            request = request.Add(message);
            byte[] respnonse = { 0x00, 0x01 };
            byte[] buffer = _reader.Transmit(request);
            if (!respnonse.CompareSegments(0, buffer, 0, 2)) return false;
            return true;
        }

        public bool ReceiveMessageFragment(UInt16 fragment, out byte[] message)
        {
            message = null;
            byte[] request = { 0x18, 0x06 };
            request = request.Add(BitConverter.GetBytes(fragment));
            byte[] response = { 0x00, 0x01 };
            byte[] buffer = _reader.Transmit(request);
            if (!response.CompareSegments(0, buffer, 0, 2)) return false;
            message = buffer.Segment(4, buffer.Length - 4);
            return true;
        }

        public bool GetFragmentByteCount(byte direction, out int maxFragmentSize)
        {
            maxFragmentSize = 0;
            byte[] request = { 0x18, 0x02 };
            request = request.AddByte(direction);
            byte[] response = { 0x00 };
            byte[] buffer = _reader.Transmit(request);
            if (!response.CompareSegments(0, buffer, 0, 1)) return false;
            maxFragmentSize = BitConverter.ToUInt16(buffer.Segment( 1, 2), 0);
            return true;
        }
    }
}