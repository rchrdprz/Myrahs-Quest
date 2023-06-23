using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Richie.GameProject
{
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField] private AsyncLoader _asyncLoader;
        [SerializeField] private GameObject _button;
        [SerializeField] private GameObject _percent;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _amount = 0.25f;
        public float _progress = 0;

        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            _slider.value = 0;
            _button.SetActive(false);
            _percent.SetActive(true);
            _asyncLoader.OnProgress += AsyncLoader_OnProgress;
        }

        private void Update()
        {   // exactly the same as the loading bar from "Menu Structure" assignment //
            if (_slider.value >= 1)
            {
                _percent.SetActive(false);
                _button.SetActive(true);
            }
            if (_slider.value < _progress)
            {
                _slider.value += _amount * Time.deltaTime;
                _text.text = $"{Mathf.RoundToInt(_slider.value * 100f)}%";
            }
        }

        private void AsyncLoader_OnProgress(float progress) => _progress = progress;
    }
}
