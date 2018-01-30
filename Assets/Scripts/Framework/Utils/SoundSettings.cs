//using DarkTonic.MasterAudio;
using UnityEngine;

public struct SoundSettings
{
    public static int Volume;
    
    public static void MuteSounds(bool flag)
    {
        if (flag) {
//            MasterAudio.MuteEverything();
            Volume = 0;
        }
        else
        {
//            MasterAudio.UnmuteEverything();
            Volume = 1;
        }
        PlayerPrefs.SetInt("SoundVolume", 1);
    }

    public static void SwitchSounds()
    {
        if (Volume == 0)
        {
            Volume = 1;
//            MasterAudio.UnmuteEverything();
            D.Log("Sound ON");
        }
        else
        {
            Volume = 0;
//            MasterAudio.MuteEverything();
            D.Log("Sound OFF");
        }
        
        PlayerPrefs.SetInt("SoundVolume", 0);
    }
}