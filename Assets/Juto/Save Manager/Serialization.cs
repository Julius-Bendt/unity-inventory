using System.IO;
using UnityEngine;

namespace Juto
{
    public class Serialization
    {

        /// <summary>
        /// Loads a object from json save file.
        /// </summary>
        /// <typeparam name="T">The object to load</typeparam>
        /// <param name="path">Path to file</param>
        /// <param name="encryption">Wether or not a encryption was used.</param>
        /// <returns></returns>
        public static T Load<T>(string path, Crypto.Encryption encryption = Crypto.Encryption.none)
        {

            if (File.Exists(path))
            {
                //Load json
                string json = File.ReadAllText(path);

                //decrypt if needed
                json = Crypto.Decrypt(json, encryption);

                // settings = JsonUtility.FromJson<Settings>(settingsJson);
                return JsonUtility.FromJson<T>(json);
            }

            throw new System.Exception("File " + path + " couldn't be found!");
        }

        /// <summary>
        /// Creates a save file.
        /// </summary>
        /// <param name="path">end destination</param>
        /// <param name="obj">obj to save</param>
        /// <param name="encryption">wether or not to use encryption</param>
        public static void Save(string path, object obj, Crypto.Encryption encryption = Crypto.Encryption.none)
        {
            //Get Json string
            string json = JsonUtility.ToJson(obj,true);

            //Encrypt if needed
            json = Crypto.Encrypt(json, encryption);

            //Save to file
            File.WriteAllText(path, json);
        }

    }

}

