/*
 * ---------------------------------------------------------------
 * Author      : Amit Pandey
 * Email       : ap3400568@gmail.com
 * Role        : Unity Developer
 * 
 * Summary     : This script handles persistent Storage to save and load game progress or status
 *               
 *
 * ---------------------------------------------------------------
 */

using UnityEngine;
using System.IO;

namespace CardMatchGame.Storage
{
    public class PersistentStorage : MonoBehaviour
    {
        public static PersistentStorage Instance 
        { 
            get;
            private set; 
        }

        private string basePath;

        private void Awake()
        {            
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            basePath = Application.persistentDataPath;
        }

        private string GetFilePath(string key) => Path.Combine(basePath, key + ".json");

        
        public void Save<T>(string key, T data)
        {
            string path = GetFilePath(key);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);            
        }

        
        public T Load<T>(string key)
        {
            string path = GetFilePath(key);            
            if (!File.Exists(path))
            {
                return default;
            }

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }

        
        public void Delete(string key)
        {
            string path = GetFilePath(key);

            if (File.Exists(path))
            {
                File.Delete(path);                
            }
        }
        
        public bool Exists(string key)
        {
            return File.Exists(GetFilePath(key));
        }
    }
}