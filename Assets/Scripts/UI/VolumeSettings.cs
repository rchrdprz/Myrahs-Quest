using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

namespace Richie.GameProject
{
    public class VolumeSettings : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        [Header("Mixer")]
        const string MIXER_MUSIC = "MusicVolume";
        const string MIXER_SFX = "SFXVolume";

        [Header("Keys")]
        const string MUSIC_KEY = "musicVolume";
        const string SFX_KEY = "sfxVolume";

        // decided not to use a singleton, instead loads the volume useing the mixer at begin of scene, using PlayerPrefs //

        private void Awake()
        {
            _musicSlider.onValueChanged.AddListener(SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

            LoadVolume();
        }

        private void Start()
        {
            _musicSlider.value = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            _sfxSlider.value = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        }

        private void SetMusicVolume(float value)
        {
            _mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
        }

        private void SetSFXVolume(float value)
        {
            _mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        }

        private void LoadVolume()
        {
            float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 1f);

            _mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(musicVol) * 20);
            _mixer.SetFloat(MIXER_SFX, Mathf.Log10(sfxVol) * 20);
        }

        public void SaveVolume()
        {
            PlayerPrefs.SetFloat(MUSIC_KEY, _musicSlider.value);
            PlayerPrefs.SetFloat(SFX_KEY, _sfxSlider.value);
        }

        private void OnDisable() => SaveVolume();
    }
}