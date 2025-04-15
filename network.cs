// Created by Ctrl-Alt-Tea (Dylan Rose)
// https://github.com/Ctrl-Alt-Tea

// This code is a network interface enumerator that displays information about active network interfaces.
// It filters out boring interfaces such as virtual, loopback, and tunnel adapters.

// The code is designed to run on Windows and has been tested on a Windows 11 laptop and an OpenWrt router.

// For further information you can refer to the official Microsoft documentation:
// https://learn.microsoft.com/en-us/dotnet/api/system.net.networkinformation.networkinterface?view=net-9.0


using System;
using System.Net.NetworkInformation;

class Program
{
    static void Main()
    {
        Console.WriteLine("Displaying only active interfaces:");
        Console.WriteLine();

        // Get all network interfaces
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface ni in networkInterfaces)
        {
            // Filter to show only NICs with an assigned IP address
            IPInterfaceProperties ipProperties = ni.GetIPProperties();
            bool hasIpAddress = false;

            foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // IPv4 only
                {
                    hasIpAddress = true;
                    break;
                }
            }

            // Filtering logic to remove boring interfaces (Tested on Windows 11 laptop and an Openwrt router)
            if (hasIpAddress &&
                (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                 ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                 ni.NetworkInterfaceType == NetworkInterfaceType.Ppp) && // Include Bluetooth (needs more testing and work)
                ni.OperationalStatus == OperationalStatus.Up &&
                !ni.Description.ToLower().Contains("virtual") && // Exclude virtual adapters
                !ni.Description.ToLower().Contains("loopback") && // Exclude loopback adapters
                !ni.Description.ToLower().Contains("tunnel") && // Exclude tunnel adapters
                !ni.Name.ToLower().Contains("local area connection")) // Exclude specific names
            {
                Console.WriteLine($"Name: {ni.Name}");
                Console.WriteLine($"Description: {ni.Description}");
                Console.WriteLine($"Status: {ni.OperationalStatus}");
                Console.WriteLine($"Speed: {ni.Speed / 1_000_000} Mbps");
                Console.WriteLine($"MAC Address: {ni.GetPhysicalAddress()}");
                foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) continue; // Skip IPv6
                    Console.WriteLine($"IP Address: {ip.Address}");
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine("Network scrapper by @Ctrl-Alt-Tea");

        // Pause the program to prevent it from closing immediately
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(); 
    }
}