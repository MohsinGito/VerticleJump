using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Collections.Generic;
using Utilities;
using RestManager;
using RestClient.Classes;
using Newtonsoft.Json;

namespace MyHelper
{
    public static class HelperClass
    {

        public static string ComputeSha256Hash(string rawData)
        {
            /* -- CREATING A SHA255 -- */  
            using (SHA256 sha256Hash = SHA256.Create())
            {
                /* --  -- */ // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                /* -- CONVERTING BYTE ARRAY TO A STRING -- */
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static long GetUnixTimestamp()
        {
            /* -- CONVERTING E.G => "12/05/2022 18:39:27" TO "1652351967" -- */
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            /* -- CONVERTING E.G => "1652351967" TO "12/05/2022 18:39:27" -- */
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).
                    AddSeconds(unixTimeStamp).ToLocalTime();
        }

        public static bool CheckUserPlayed10Minutes(this int playedTime)
        {
            /* -- GETTING STARTUP IN SECONDS AND CONVERTING IT TO MINUTES -- */
            return (Time.realtimeSinceStartup / 60) > 10;
        }

        public static void ClearCache(this List<ApiRequest> objList, int itemsMaxCount = 20)
        {
            /* -- CHECKING IF ALL REQUESTS ARE COMPLETED THEN WE CLEAR THE LIST -- */
            if (objList.Find(n => n.isRequestCompleted == false) == null)  
            {
                if (objList.Count > itemsMaxCount) 
                { 
                    objList.Clear(); 
                }
            }
        }
    }

    public enum API
    {
        SCORES_URL = 0,
        ADD_PLAY_URL = 1
    }

}