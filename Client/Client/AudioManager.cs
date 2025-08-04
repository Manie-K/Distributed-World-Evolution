using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

public class AudioManager
{
    private class SoundEffectData
    {
        public SoundEffect Effect;
        public float BaseVolume;

        public SoundEffectData(SoundEffect effect, float baseVolume)
        {
            Effect = effect;
            BaseVolume = MathHelper.Clamp(baseVolume, 0f, 1f);
        }
    }

    private Dictionary<string, SoundEffectData> soundEffects = new();
    private Dictionary<string, Song> songs = new();
    private ContentManager content;


    private float globalMusicVolume = 1.0f;
    private float globalEffectVolume = 1.0f;


    private bool isMuted = false;
    private float savedMusicVolume = 1.0f;
    private float savedEffectVolume = 1.0f;

    public AudioManager(ContentManager content)
    {
        this.content = content;
    }


    public void LoadSong(string name, string path)
    {
        if (!songs.ContainsKey(name))
        {
            Song song = content.Load<Song>(path);
            songs[name] = song;
        }
    }

    public void PlaySong(string name, bool isRepeating = true)
    {
        if (songs.ContainsKey(name))
        {
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Volume = isMuted ? 0f : globalMusicVolume;
            MediaPlayer.Play(songs[name]);
        }
    }

    public void StopSong() => MediaPlayer.Stop();
    public void PauseSong() => MediaPlayer.Pause();
    public void ResumeSong() => MediaPlayer.Resume();

    public void SetGlobalMusicVolume(float volume)
    {
        volume = MathHelper.Clamp(volume, 0f, 1f);

        if (isMuted)
        {
            savedMusicVolume = volume;
        }
        else
        {
            globalMusicVolume = volume;
            MediaPlayer.Volume = globalMusicVolume;
        }
    }

    public float GetGlobalMusicVolume()
    {
       if (isMuted) return  savedMusicVolume;
       else return globalMusicVolume;
    }


    public void LoadSoundEffect(string name, string path, float baseVolume = 1.0f)
    {
        if (!soundEffects.ContainsKey(name))
        {
            SoundEffect effect = content.Load<SoundEffect>(path);
            soundEffects[name] = new SoundEffectData(effect, baseVolume);
        }
    }

    public void PlaySound(string name)
    {
        if (soundEffects.ContainsKey(name))
        {
            var data = soundEffects[name];
            var instance = data.Effect.CreateInstance();
            float finalVolume = data.BaseVolume * (isMuted ? 0f : globalEffectVolume);
            instance.Volume = MathHelper.Clamp(finalVolume, 0f, 1f);
            instance.Play();
        }
    }

    public void SetGlobalEffectVolume(float volume)
    {
        volume = MathHelper.Clamp(volume, 0f, 1f);

        if (isMuted)
        {
            savedEffectVolume = volume;
        }
        else
        {
            globalEffectVolume = volume;
        }
    }

    public float GetGlobalEffectVolume()
    {
        if(isMuted) return savedEffectVolume;
        else return globalEffectVolume;
    }
    public void SetBaseEffectVolume(string name, float baseVolume)
    {
        if (soundEffects.ContainsKey(name))
        {
            soundEffects[name].BaseVolume = MathHelper.Clamp(baseVolume, 0f, 1f);
        }
    }



    public void MuteAll()
    {
        if (isMuted) return;

        savedMusicVolume = globalMusicVolume;
        savedEffectVolume = globalEffectVolume;

        MediaPlayer.Volume = 0f;
        globalEffectVolume = 0f;
        isMuted = true;
    }

    public void UnmuteAll()
    {
        if (!isMuted) return;

        globalMusicVolume = savedMusicVolume;
        globalEffectVolume = savedEffectVolume;

        MediaPlayer.Volume = globalMusicVolume;
        isMuted = false;
    }

    public void ToggleMute()
    {
        if (isMuted)
            UnmuteAll();
        else
            MuteAll();
    }

    public void IncreaseMusicVolume(float step = 0.1f)
    {
        float targetVolume = (isMuted ? savedMusicVolume : globalMusicVolume) + step;
        SetGlobalMusicVolume(targetVolume);
    }

    public void DecreaseMusicVolume(float step = 0.1f)
    {
        float targetVolume = (isMuted ? savedMusicVolume : globalMusicVolume) - step;
        SetGlobalMusicVolume(targetVolume);
    }

    public void IncreaseEffectVolume(float step = 0.1f)
    {
        float targetVolume = (isMuted ? savedEffectVolume : globalEffectVolume) + step;
        SetGlobalEffectVolume(targetVolume);
    }

    public void DecreaseEffectVolume(float step = 0.1f)
    {
        float targetVolume = (isMuted ? savedEffectVolume : globalEffectVolume) - step;
        SetGlobalEffectVolume(targetVolume);
    }

    public bool IsMuted => isMuted;
}