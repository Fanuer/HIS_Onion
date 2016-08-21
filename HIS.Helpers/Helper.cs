﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HIS.Helpers
{
    public static class Helper
    {
        public static string GetHash(string input)
        {
            var hashAlgorithm = SHA256.Create();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        /// <summary>
        /// Returns an objects displayname or the string-representation
        /// </summary>
        /// <param name="obj">calling object</param>
        /// <returns></returns>
        public static string GetDisplayName(this object obj)
        {
            var fieldInfo = obj.GetType().GetField(obj.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].GetName() : obj.ToString();
        }
    }
}
