using System;
using System.IO;
using UnityEngine;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    [System.Serializable]
    public class SaveSettings
    {
        /// <summary>
        /// Change the file name if something else floats your boat
        /// </summary>
        public string fileName = "GameSettings.json";
        /// <summary>
        /// The string that will be saved.
        /// </summary>
        static string jsonString;

        /// <summary>
        /// Create the JSON object needed to save settings.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static object createJSONOBJ(string jsonString)
        {
            return JsonUtility.FromJson<SaveSettings>(jsonString);

        }

        /// <summary>
        /// Read the game settings from the file
        /// </summary>
        /// <param name="readString"></param>
        public void LoadGameSettings(String readString)
        {

        }

        /// <summary>
        /// Get the quality/music settings before saving 
        /// </summary>
        public void SaveGameSettings()
        {
        }
    }
}