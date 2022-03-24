using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System;
using UnityEngine.Audio;

public class SettingsSave
{
    public float volume;
    public System.DateTime lastEdited;
}

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI lastEdited;
    public AudioMixer mixer;

    public SettingsSave loadedConfig;

    private bool changedStuff;

    public void onChange() => changedStuff = true;

    //https://stackoverflow.com/questions/11/calculate-relative-time-in-c-sharp
    public string ConvertTimeToWords(DateTime theDate)
    {
        Dictionary<long, string> thresholds = new Dictionary<long, string>();
        int minute = 60;
        int hour = 60 * minute;
        int day = 24 * hour;
        thresholds.Add(60, "{0} seconds ago");
        thresholds.Add(minute * 2, "a minute ago");
        thresholds.Add(45 * minute, "{0} minutes ago");
        thresholds.Add(120 * minute, "an hour ago");
        thresholds.Add(day, "{0} hours ago");
        thresholds.Add(day * 2, "yesterday");
        thresholds.Add(day * 30, "{0} days ago");
        thresholds.Add(day * 365, "{0} months ago");
        thresholds.Add(long.MaxValue, "{0} years ago");
        long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
        foreach (long threshold in thresholds.Keys)
        {
            if (since < threshold)
            {
                TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
                return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
            }
        }

        return "";
    }


    public void SetVolume(string key, float sliderValue)
    {
        mixer.SetFloat(key, Mathf.Log10(sliderValue) * 20); ;
    }

    public void SaveSettings()
    {
        SettingsSave save = new SettingsSave();
        save.volume = volumeSlider.value;
        save.lastEdited = System.DateTime.Now;

        if (!changedStuff) return;

        string json = JsonConvert.SerializeObject(save);
        File.WriteAllText(Application.persistentDataPath + "/settings.txt", json);
        SetVolume("Music", volumeSlider.value);
    }

    public void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.txt"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/settings.txt");
            SettingsSave save = JsonConvert.DeserializeObject<SettingsSave>(json);
            loadedConfig = save;
            volumeSlider.value = save.volume;
            if (save.lastEdited != System.DateTime.MinValue)
            {
                lastEdited.text = "Last updated: " + ConvertTimeToWords(save.lastEdited);
            }
            else
            {
                lastEdited.text = "Last updated: never";
            }

            if (save.volume == 0)
            {
                volumeSlider.value = 0.5f;
            }
        }
        else
        {
            volumeSlider.value = 0.5f;
            lastEdited.text = "Last updated: never";
        }

        SetVolume("Music", volumeSlider.value);
        changedStuff = false;
    }

    public void Start()
    {
        LoadSettings();
        SetVolume("Music", volumeSlider.value);
    }
}