using System;
using System.Collections.Generic;
using System.Text;

namespace TWN4Api
{
    public class Nfc
    {
        private NfcServer _server;

        public Nfc()
        {
            _server = new NfcServer();
        }

        public void Init()
        {
            if (!_server.SearchTag()) return;
            var start = DateTime.Now;
            var timeout = new TimeSpan(0, 0, 0, 500);
            while (_server.GetConnectionState() < 2)
            {
                var interval = DateTime.Now - start;
                if (interval > timeout) return;
            }

            // receive
            while (_server.GetConnectionState() > 1)
            {
                
            }

        }

        public bool SendMessage(byte[] buffer)
        {
            if (buffer.Length == 0) return true;

            if (!_server.Begin(buffer.Length)) return false;
            int remaining = buffer.Length;
            for (var offset = 0; offset < buffer.Length;)
            {
                int maxFragmentSize = 0;
                do
                {
                    // Is NFC device still connected?
                    if (_server.GetConnectionState() < 2 || !_server.GetFragmentByteCount(0, out maxFragmentSize))
                    {
                        // send failed
                        return false;
                    }
                }
                while (maxFragmentSize == 0);
                int fragmentSize = maxFragmentSize;
                if (fragmentSize > remaining) fragmentSize = remaining;
                var segment = buffer.Segment(offset, fragmentSize);
                if (!_server.SendMessageFragment(segment)) return false;
                offset += fragmentSize;
                remaining -= fragmentSize;
            }
            return true;
        }

        public byte[] ReceiveMessage()
        {
            int remainingMessageSize;
            byte[] receiveFragBuffer = null;
            byte[] receivedMessage = null;
            receivedMessage = null;
            if (!_server.TestMessage(out remainingMessageSize)) return null;

            do
            {
                int maxFragSize = 0;
                do
                {
                    // Is NFC device still connected?
                    if (_server.GetConnectionState() < 2)
                    {
                        // No, log Receiving message failed
                        return null;
                    }

                    if (!_server.GetFragmentByteCount(1, out maxFragSize))
                    {
                        // No, log Receiving message failed
                        return null;
                    }
                }
                while (maxFragSize == 0);
                int fragSize = maxFragSize;
                
                if (!_server.ReceiveMessageFragment(Convert.ToUInt16(fragSize), out receiveFragBuffer))
                {
                    // No, log Receiving message failed
                    return null;
                }
                receivedMessage = receivedMessage.Add(receiveFragBuffer);
                remainingMessageSize -= fragSize;
            }
            while (remainingMessageSize > 0);
            return receivedMessage;
        }
    }
}
