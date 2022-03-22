using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class SettingsSave
{
    public int volume;
}

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;

    public void SaveSettings()
    {
        SettingsSave save = new SettingsSave();
        save.volume = (int)volumeSlider.value;
        string json = JsonConvert.SerializeObject(save);
        File.WriteAllText(Application.persistentDataPath + "/settings.txt", json);
    }
    
    public void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.txt"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/settings.txt");
            SettingsSave save = JsonConvert.DeserializeObject<SettingsSave>(json);
            volumeSlider.value = save.volume;
        }
        else
        {
            volumeSlider.value = 50;
        }
    }
}
