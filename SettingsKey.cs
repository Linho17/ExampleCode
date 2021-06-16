using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsKey 
{
    const string MUTE_KEY = "isMute";
    const string DISTANCE = "distance";
    const string FISH = "fish";
    const string DISTANCE_ALL_TIME = "distanceAllTime";
    const string DISTANCE_HIGHSCORE = "distanceHighscore";
    const string FISH_COLLECT = "fishCollect";


    const bool MUTE_DEFAULT = false;
    public static void SetMute(bool mute)
    {
        if (mute)
            PlayerPrefs.SetString(MUTE_KEY, "true");
        else
            PlayerPrefs.SetString(MUTE_KEY, "false");

       
    }

    public static bool GetMute()
    {
        string temp = PlayerPrefs.GetString(MUTE_KEY);
        if (temp == "true")
            return true;
        else
            return false;
    }



    public static void SetDistance(int _distance)
    {
        PlayerPrefs.SetInt(DISTANCE, _distance);
    }

    public static void SetFish(int _fish)
    {
        PlayerPrefs.SetInt(FISH, _fish);
    }

    public static void SetDistanceAndFish(int _distance, int _fish)
    {
        SetFish(_fish);
        SetDistance(_distance);
        PlayerPrefs.Save();
    }

    public static void SetFishCollect(int _fish)
    {
        PlayerPrefs.SetInt(FISH_COLLECT, GetFishCollect() + _fish);
    }

    public static int GetDistance()
    {
       return PlayerPrefs.GetInt(DISTANCE);
    }

    public static int GetFish()
    {
        return PlayerPrefs.GetInt(FISH);
    }
    public static int GetFishCollect()
    {
        return PlayerPrefs.GetInt(FISH_COLLECT);
    }


    public static int GetDistanceAllTime()
    {
        return PlayerPrefs.GetInt(DISTANCE_ALL_TIME);
    }

    public static int GetDistanceHighscore()
    {
        return PlayerPrefs.GetInt(DISTANCE_HIGHSCORE);
    }


    public static void SetDistanceHighscore(int _distance)
    {
         PlayerPrefs.SetInt(DISTANCE_HIGHSCORE, _distance);
    }


    public static void SetDistanceAllTime(int _distance)
    {
        PlayerPrefs.SetInt(DISTANCE_ALL_TIME, GetDistanceAllTime()+_distance);
    }




}
