/*using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MHLab.Web.Storage
{
    public class LocalStorageManager
    {
        /// <summary>
        /// Displays an alert popup in the browser.
        /// </summary>
        [DllImport("__Internal")]
        private static extern void BrowserAlert(string message);

        /// <summary>
        /// Synchronizes files in the browser's local storage.
        /// </summary>
        [DllImport("__Internal")]
        private static extern void BrowserSynchronizeFiles();

        private static readonly string DataPath = string.Format("{0}/LocalStorage.dat", Application.persistentDataPath);

        private static LocalStorage m_storage;

        public static LocalStorage Storage
        {
            get { return m_storage; }
        }

        static LocalStorageManager()
        {
            m_storage = new LocalStorage();
            Load(m_storage);
        }

        /// <summary>
        /// Saves the local data to the browser's local storage.
        /// </summary>
        public static void Save(LocalStorage localStorage)
        {
            try
            {
                if (File.Exists(DataPath))
                {
                    File.WriteAllText(DataPath, string.Empty);
                }

                File.WriteAllText(DataPath, localStorage.Serialize());

                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    BrowserSynchronizeFiles();
                }
            }
            catch (Exception e)
            {
#if UNITY_WEBGL
                BrowserAlert("Failed to store: " + e.Message);
#else
                Debug.Log("Failed to store: " + e.Message);
#endif
            }
        }

        /// <summary>
        /// Loads the saved data from the browser's local storage.
        /// </summary>
        /// <param name="localStorage"></param>
        public static void Load(LocalStorage localStorage)
        {
            try
            {
                if (!File.Exists(DataPath))
                {
                    return;
                }

                var content = File.ReadAllText(DataPath);

                localStorage.Deserialize(content);
            }
            catch (Exception e)
            {
#if UNITY_WEBGL
                BrowserAlert("Failed to load: " + e.Message);
#else
                Debug.Log("Failed to load: " + e.Message);
#endif
            }
        }
    }
}*/