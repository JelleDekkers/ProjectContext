using UnityEngine;
using System.Collections.Generic;
using System;
using System.Net;
using System.Linq;

public static class Common {

    public static string ConvertToString(List<int> list) {
        List<string> stringList = list.ConvertAll<string>(x => x.ToString());
        string str = string.Join(",", stringList.ToArray());
        return str;
    }

    public static List<int> ConvertToIntList(string str) {
        List<int> list = str.Split(',').Select(Int32.Parse).ToList();
        return list;
    }

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
