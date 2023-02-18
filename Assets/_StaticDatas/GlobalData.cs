using UnityEngine;
using System.Collections.Generic;

public static class GlobalData
{
    public static class Settings
    {
        public static Dictionary<string, PrefsData<float>> volumes = new Dictionary<string, PrefsData<float>>() {
            {KeyData.Settings.VOLUME_MASTER, new PrefsData<float>(KeyData.Settings.VOLUME_MASTER, 1.2f)},
            {KeyData.Settings.VOLUME_ME, new PrefsData<float>(KeyData.Settings.VOLUME_ME, 1.1f)},
            {KeyData.Settings.VOLUME_SE, new PrefsData<float>(KeyData.Settings.VOLUME_SE, 1.1f)},
            {KeyData.Settings.VOLUME_BGM, new PrefsData<float>(KeyData.Settings.VOLUME_BGM, 1.1f)},
        };
    }

    public static class IAP
    {
        public static Dictionary<string, PrefsData<bool>> non_consumable = new Dictionary<string, PrefsData<bool>>() {
            // {KeyData.KEY, new PrefsData<bool>(KeyData.KEY, false)},
        };
        public static Dictionary<string, PrefsData<int>> consumable = new Dictionary<string, PrefsData<int>>() {
            // {KeyData.KEY, new PrefsData<bool>(KeyData.KEY, false)},
        };
    }
}