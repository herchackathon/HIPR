using MHLab.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MHLab.Web.Storage
{
    [Serializable]
    public static class LocalStorage
    {
        /// <summary>
        /// Stores a key-value pair in local storage
        /// </summary>
        public static void Store(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        /// <summary>
        /// Stores a key-value pair in local storage
        /// </summary>
        public static void Store(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        /// <summary>
        /// Stores a key-value pair in local storage
        /// </summary>
        public static void Store(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// Retrieves the value associated to the given key.
        /// </summary>
        public static Result<int> GetInt(string key)
        {
            if(!PlayerPrefs.HasKey(key))
                return Result<int>.Create(0, new KeyNotFoundException(key));

            return Result<int>.Create(PlayerPrefs.GetInt(key), null);
        }

        /// <summary>
        /// Retrieves the value associated to the given key.
        /// </summary>
        public static Result<float> GetFloat(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return Result<float>.Create(0f, new KeyNotFoundException(key));

            return Result<float>.Create(PlayerPrefs.GetFloat(key), null);
        }

        /// <summary>
        /// Retrieves the value associated to the given key.
        /// </summary>
        public static Result<string> GetString(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return Result<string>.Create(null, new KeyNotFoundException(key));

            return Result<string>.Create(PlayerPrefs.GetString(key), null);
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}
