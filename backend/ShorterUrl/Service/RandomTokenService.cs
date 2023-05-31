using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShorterUrl.Service
{
    public static class RandomTokenService
    {
        public static string generateValue()
        {
            Random res = new Random();

            String str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int size = 5;

            String randomstring = "";

            for (int i = 0; i < size; i++)
            {
                int x = res.Next(str.Length);
                randomstring = randomstring + str[x];
            }
            return randomstring;
        }
    }
}