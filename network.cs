// Created by Ctrl-Alt-Tea (Dylan Rose)
// https://github.com/Ctrl-Alt-Tea

// This code is a network interface enumerator that displays information about active network interfaces.
// It filters out boring interfaces such as virtual, loopback, and tunnel adapters.

// The code is designed to run on Windows and Linux but should work on a Mac too,
// This code has been tested on a Windows 11 and a Ubuntu laptop via an OpenWrt router.

// Installers for both Windows and Linux (Debian/Ubuntu) are available at:
// https://github.com/Ctrl-Alt-Tea/Network-Information/releases

// For further information you can refer to the official Microsoft documentation:
// https://learn.microsoft.com/en-us/dotnet/api/system.net.networkinformation.networkinterface?view=net-9.0


using System;
using System.Net.NetworkInformation;
using RestSharp;

class Program
{
    static async Task Main()
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

            // Filtering logic to remove boring interfaces
            if (hasIpAddress &&
                (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                 ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                 ni.NetworkInterfaceType == NetworkInterfaceType.Ppp) &&
                ni.OperationalStatus == OperationalStatus.Up &&
                !ni.Description.ToLower().Contains("virtual") &&
                !ni.Description.ToLower().Contains("loopback") &&
                !ni.Description.ToLower().Contains("tunnel") &&
                !ni.Name.ToLower().Contains("local area connection"))
            {
                Console.WriteLine($"Name: {ni.Name}");
                Console.WriteLine($"Description: {ni.Description}");
                Console.WriteLine($"Status: {ni.OperationalStatus}");
                Console.WriteLine($"Speed: {ni.Speed / 1_000_000} Mbps");
                Console.WriteLine($"MAC Address: {ni.GetPhysicalAddress()}");
                foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) continue; // Skip IPv6
                    Console.WriteLine($"Local IP Address: {ip.Address}");

                    string ipAddress = ip.Address.ToString(); // Assign the IP address as a string
                    string apiKey = "eae488345f1265"; // Replace with your IPinfo API key

                    var client = new RestClient($"https://ipinfo.io/{ipAddress}/json?token={apiKey}");
                    var request = new RestRequest();
                    var response = await client.GetAsync(request);
                }
            }
        }

        // Fetch and display external IP information
        string publicIpAddress = await GetPublicIpAddress();
        if (!string.IsNullOrEmpty(publicIpAddress))
        {
            Console.WriteLine($"\nPublic IP Address: {publicIpAddress}");

            string apiKey = Environment.GetEnvironmentVariable("IPINFO_API_KEY");
            //string apiKey = "ENTER_API_KEY"; // Replace with your IPinfo API key
            var client = new RestClient($"https://ipinfo.io/{publicIpAddress}/json?token={apiKey}");
            var request = new RestRequest();
            var response = await client.GetAsync(request);

            if (response != null)
            {
                Console.WriteLine("Public IP Geolocation Data:");
                Console.WriteLine(response.Content);
            }
            else
            {
                Console.WriteLine("Error retrieving geolocation data for public IP.");
            }
        }
        else
        {
            Console.WriteLine("Failed to retrieve public IP address.");
        }

        Console.WriteLine();
        Console.WriteLine("Network Information by Ctrl-Alt-Tea");

        // Pause the program to prevent it from closing immediately
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    // Method to fetch the public IP address
    static async Task<string> GetPublicIpAddress()
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Use a public IP service
                return await httpClient.GetStringAsync("https://api.ipify.org");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching public IP: {ex.Message}");
            return null;
        }
    }
}
