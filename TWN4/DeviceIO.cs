using Microsoft.Win32;
using System;

namespace TWN4Api
{
    public class DeviceIO
    {
        public string RegHKLMQuery(string subKey, string valueName)
        {
            string data = string.Empty;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey);
            if (key?.GetValue(valueName) == null) return string.Empty;
            if (key.GetValue(valueName) != null)
                data = key.GetValue(valueName).ToString();
            
            if (string.IsNullOrEmpty(data) || key.GetValueKind(valueName) != RegistryValueKind.String)
                data = string.Empty;
            key.Close();
            return data;
        }
        
        public string FindUSBDevice(string driver, string devicePath)
        {
            int port = 0;
            while (true)
            {
                var data = RegHKLMQuery($"SYSTEM\\CurrentControlSet\\Services\\{driver}\\Enum", port.ToString());
                port++;
                if (data == string.Empty) return string.Empty;
                var substr = data.Substring(0, devicePath.Length).ToUpper();
                if (substr == devicePath)
                    return data;
            }
        }
        
        public int GetCOMPort(string device)
        {
            string path = "SYSTEM\\CurrentControlSet\\Enum\\" + device + "\\device Parameters";
            string data = RegHKLMQuery(path, "PortName");
            if (data == string.Empty) return 0;
            if (data.Length < 4) return 0;
            int port = Convert.ToUInt16(data.Substring(3));
            if (port < 1 || port > 256) return 0;
            return port;
        }

        public string COMPort(int port) => port < 1 || port > 256 ? string.Empty : $"COM{port}";
    }
}