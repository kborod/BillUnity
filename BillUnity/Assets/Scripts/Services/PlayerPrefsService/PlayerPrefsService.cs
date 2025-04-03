using Newtonsoft.Json;
using UnityEngine;

namespace Kborod.Services.LocalCache
{
    public class PlayerPrefsService
    {
        public void Save<T>(T data, PlayerPrefKey key) =>
            PlayerPrefs.SetString(key.ToString(), JsonConvert.SerializeObject(data));

        public T LoadOrDefault<T>(PlayerPrefKey key)
        {
            string prefsString = PlayerPrefs.GetString(key.ToString());
            return string.IsNullOrEmpty(prefsString) ? default : JsonConvert.DeserializeObject<T>(prefsString);
        }
        
        public T LoadOrDefault<T>(PlayerPrefKey key, T defaultValue)
        {
            string prefsString = PlayerPrefs.GetString(key.ToString());
            return string.IsNullOrEmpty(prefsString) ? defaultValue : JsonConvert.DeserializeObject<T>(prefsString);
        }

        public void Clear(PlayerPrefKey key) =>
            PlayerPrefs.DeleteKey(key.ToString());

        public bool HasKey(PlayerPrefKey key) =>
            PlayerPrefs.HasKey(key.ToString());
    }
}

/* Copyright: Made by Appfox */