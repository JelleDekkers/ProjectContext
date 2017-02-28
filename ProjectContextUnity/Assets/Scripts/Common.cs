using UnityEngine;
using System.Collections;
using System.Net;

public static class Common {

    public static bool IsValidIpAddress(string ipAddress) {
        if (ipAddress.Length < 4)
            return false;

        System.Net.IPAddress address;
        if (System.Net.IPAddress.TryParse(ipAddress, out address)) {
            switch (address.AddressFamily) {
                case System.Net.Sockets.AddressFamily.InterNetwork:
                    //IPv4
                    return true;
                    break;
                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                    //IPv6
                    return true;
                    break;
                default:
                    return false;
                    break;
            }
        }
        return false;
    }

    public static string GetDNSIP() {
        string strHostName = "";
        strHostName = System.Net.Dns.GetHostName();
        IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
        IPAddress[] addr = ipEntry.AddressList;

        return addr[addr.Length - 1].ToString();
    }

    public static string GenerateRandomServerCode() {
        uint codeLength = 4;
        System.Random random = new System.Random();
        string generatedString = "";

        for (int i = 0; i < codeLength; i++) {
            int num = random.Next(0, 26);
            char rndLetter = (char)('a' + num);
            generatedString += rndLetter;
        }

        return generatedString;
    }

    public static bool IsValidName(string name) {
        if (name == "")
            return false;
        else
            return true;
    }
}
