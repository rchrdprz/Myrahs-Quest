using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Richie.GameProject
{
    public class OptionSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _resolutionDrop;
        private Resolution[] _resolutions;

        private void Start()
        {
            FindResolutions();
        }

        private void FindResolutions()
        {   // makes a dropdown of all resolution available based on indivudal settings // 
            // this looks good, but does not change resolution most times (works sometimes //
            _resolutions = Screen.resolutions;
            _resolutionDrop.ClearOptions();

            List<string> options = new();

            int currentResolution = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height + " @ " + _resolutions[i].refreshRate + "hz";
                options.Add(option);

                if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                    currentResolution = i;
            }

            _resolutionDrop.AddOptions(options);
            _resolutionDrop.value = currentResolution;
            _resolutionDrop.RefreshShownValue();
        }

        public void SetResolution(int index)
        {
            Resolution resolution = _resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullScreen(bool toggle)
        {   // this should toggle fullscreen, but does not work majority of the time //
            if (toggle)
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            }
            else Screen.fullScreenMode = FullScreenMode.Windowed;
        }   
    }
}