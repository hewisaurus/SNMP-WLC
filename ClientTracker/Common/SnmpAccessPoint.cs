using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SnmpAccessPoint
    {
        public string Index { get; set; }
        public string BaseRadioMacAddress { get; set; }
        public string EthernetMacAddress { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }

        public SnmpAccessPoint()
        {

        }

        public SnmpAccessPoint(string index)
        {
            Index = index;
        }

        public SnmpAccessPoint(string index, string baseRadiomac, string ethernetMac, string ipAddress, string name, 
            string location, string model, string serialNumber)
        {
            Index = index;
            BaseRadioMacAddress = baseRadiomac.Replace(" ", "");
            EthernetMacAddress = ethernetMac.Replace(" ", "");
            IpAddress = ipAddress;
            Name = name.Trim();
            Location = location;
            Model = model;
            SerialNumber = serialNumber;
        }

        public override string ToString()
        {
            return string.Format(
                "Name: {1}{0}" +
                "Location: {2}{0}" +
                "Model: {3}{0}" +
                "Serial number: {4}{0}" +
                "IP Address: {5}{0}" +
                "Base radio MAC Address: {6}{0}" +
                "Ethernet MAC Address: {7}{0}",
                Environment.NewLine, Name, Location, Model, SerialNumber, IpAddress, BaseRadioMacAddress, EthernetMacAddress);
        }
    }
}
