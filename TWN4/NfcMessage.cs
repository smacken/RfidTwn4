using System;
using System.Text;

namespace TWN4Api
{
    public class NfcMessage
    {
        byte _messageHeader = 0xD1;
        byte[] _messageType = new byte[] { 0x55 };
        byte[] _payload = new byte[] { 0x01 };
        private byte[] _message;

        public NfcMessage(string text)
        {
            _payload = _payload.Add(Encoding.UTF8.GetBytes(text));
            _message = _message.AddByte(_messageHeader);
            _message = _message.AddByte(Convert.ToByte(_messageType.Length));
            _message = _message.AddByte(Convert.ToByte(_payload.Length));
            _message = _message.Add(_messageType);
            _message = _message.Add(_payload);
        }
    }
}