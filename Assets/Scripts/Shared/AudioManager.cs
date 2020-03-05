using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClips
{
    //Explorer sounds
    NewQuest,
    Stairs,
    ButtonSound,
    ToggleCrystal,
    MovePillar,
    ExplorerDeath,
    Flamethrower,

    //Shooter sounds
    GunClick,
    GunShoot,
    KeyAccepted,
    DoorSlide,
    EmptyGun,
    WindowSmash,
    GlassSmash1,
    GlassSmash2,
    GlassSmash3,
    GlassHit,
    CollectItem,
    Pickup,
    PowerDown,
    WrongSequence,
    Alert,
    LaserGun
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Dictionary<AudioClips, AudioClip> audioClips;

    public List<AudioSource> gunAudioSources;

    //Explorer sounds
    public AudioClip newQuest;
    public AudioClip stairs;
    public AudioClip buttonSound;
    public AudioClip movePillar;
    public AudioClip toggleCrystal;
    public AudioClip explorerDeath;
    public AudioClip flamethrower;

    //Shooter sounds
    public AudioClip gunClick;
    public AudioClip gunShoot;
    public AudioClip keyAccepted;
    public AudioClip doorSlide;
    public AudioClip emptyGun;
    public AudioClip windowSmash;
    public AudioClip glassSmash1;
    public AudioClip glassSmash2;
    public AudioClip glassSmash3;
    public AudioClip glassHit;
    public AudioClip collectItem;
    public AudioClip pickup;
    public AudioClip powerDown;
    public AudioClip wrongSequence;
    public AudioClip alert;
    public AudioClip laserGun;

    private AudioSource audioSource;

    public bool useAudio = true;

    private void Awake()
    {
        instance = this;

        audioClips = new Dictionary<AudioClips, AudioClip>();
        gunAudioSources = new List<AudioSource>();

        //Explorer
        audioClips.Add(AudioClips.NewQuest, newQuest);
        audioClips.Add(AudioClips.Stairs, stairs);
        audioClips.Add(AudioClips.ButtonSound, buttonSound);
        audioClips.Add(AudioClips.ToggleCrystal, toggleCrystal);
        audioClips.Add(AudioClips.MovePillar, movePillar);
        audioClips.Add(AudioClips.ExplorerDeath, explorerDeath);
        audioClips.Add(AudioClips.Flamethrower, flamethrower);

        //Shooter
        audioClips.Add(AudioClips.GunClick, gunClick);
        audioClips.Add(AudioClips.GunShoot, gunShoot);
        audioClips.Add(AudioClips.KeyAccepted, keyAccepted);
        audioClips.Add(AudioClips.DoorSlide, doorSlide);
        audioClips.Add(AudioClips.EmptyGun, emptyGun);
        audioClips.Add(AudioClips.WindowSmash, windowSmash);
        audioClips.Add(AudioClips.GlassSmash1, glassSmash1);
        audioClips.Add(AudioClips.GlassSmash2, glassSmash2);
        audioClips.Add(AudioClips.GlassSmash3, glassSmash3);
        audioClips.Add(AudioClips.GlassHit, glassHit);
        audioClips.Add(AudioClips.CollectItem, collectItem);
        audioClips.Add(AudioClips.Pickup, pickup);
        audioClips.Add(AudioClips.PowerDown, powerDown);
        audioClips.Add(AudioClips.WrongSequence, wrongSequence);
        audioClips.Add(AudioClips.Alert, alert);
        audioClips.Add(AudioClips.LaserGun, laserGun);


        audioSource = GetComponent<AudioSource>();
    }

    public void StopSounds()
    {
        audioSource.Stop();
    }

    public void PlayShootSound(AudioSource source, bool gunEmpty)
    {
        foreach (var item in gunAudioSources)
        {
            if (item == source)
            {
                if (!gunEmpty)
                {
                    if (WeaponManager.instance.currentWeapon.type == GunType.Rifle && source.gameObject.GetComponent<GunScript>() == WeaponManager.instance.currentWeapon)
                    {
                        item.PlayOneShot(laserGun);
                    }
                    else
                    {
                        item.PlayOneShot(gunShoot);
                    }
                }
                else
                {
                    item.PlayOneShot(emptyGun);
                }
            }
        }
    }

    public void PlaySound(AudioClips clipToPlay)
    {
        var clip = audioClips[clipToPlay];
        if (clip != null && useAudio)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public AudioClip GetAudioClip(AudioClips clipToPlay)
    {
        return audioClips[clipToPlay];
    }
}
