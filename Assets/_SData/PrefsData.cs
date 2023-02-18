using UnityEngine;

public class PrefsData<T> : Data<T>
{
    protected string key;
    protected bool encryption;

    public override T value
    {
        get
        {
            return PlayerPrefsExt.GetObject<T>(key, defaultValue, encryption);
        }
        set
        {
            PlayerPrefsExt.SetObject<T>(key, value, encryption);
            if(!blockChangeEvent) this.onChange?.Invoke(value);
        }
    }

    public PrefsData(string key, T defaultValue = default(T), bool encryption = false)
    {
        this.encryption = encryption;
        this.key = key;
        this.defaultValue = defaultValue;
        if(!PlayerPrefsExt.HasKey(key, encryption)) this.value = defaultValue;
    }

    public void DeletePrefs()
    {
        this.value = defaultValue;
    }
}