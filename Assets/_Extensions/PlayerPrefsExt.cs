using UnityEngine;
using Newtonsoft.Json;

public static class PlayerPrefsExt
{
    private static AES128 aes256 = new AES128();

    public static void SetObject<T>(string key, T value, bool encryption = true)
    {
        string jsonValue = JsonConvert.SerializeObject(value, JsonSettings.Settings);

        string savingKey = encryption ? aes256.Encrypt(key) : key;
        string savingValue = encryption ? aes256.Encrypt(jsonValue) : jsonValue;

        PlayerPrefs.SetString(savingKey, savingValue);
    }

    public static T GetObject<T>(string key, T defaultValue = default(T), bool encryption = true)
    {
        string savedKey = encryption ? aes256.Encrypt(key) : key;

        if(!PlayerPrefs.HasKey(savedKey))
        {
            Debug.LogWarning($"NO PREFS OF KEY: {savedKey}");
            return defaultValue;
        }

        string savedValue = PlayerPrefs.GetString(savedKey, "");
        string originalValue = encryption ? aes256.Decrypt(savedValue) : savedValue;

        if(originalValue == "") return defaultValue;

        return JsonConvert.DeserializeObject<T>(originalValue);
    }

    public static bool HasKey(string key, bool encryption = false)
    {
        key = encryption ? aes256.Encrypt(key) : key;
        return PlayerPrefs.HasKey(key);
    }
}