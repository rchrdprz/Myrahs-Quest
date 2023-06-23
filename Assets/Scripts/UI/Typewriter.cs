using System.Collections;
using UnityEngine;
using TMPro;

namespace Richie.GameProject
{
    public class Typewriter : MonoBehaviour
    {
		[Header("Settings")]
		[SerializeField] private float _startDelay = 0f;
		[SerializeField] private float _typeDelay = 0.1f;

		[Header("References")]
		[SerializeField] private GameObject _button;
		[SerializeField] private Dialogue _dialogue;

		private Coroutine _coroutine;
		private TMP_Text _tmpProText;
		private string _text;

		public event Complete OnComplete;
		public delegate void Complete();

		private void Start()
		{
			_tmpProText = GetComponent<TMP_Text>();
            _dialogue.OnSkip += Dialogue_OnSkip;
			_button.SetActive(false);

			if (_tmpProText == null) return;
			_text = _tmpProText.text;
			_tmpProText.text = "";

			_coroutine = StartCoroutine(TypeWriterTMP());
		}

        private void Dialogue_OnSkip()
        {  // stop coroutines and shows all text //
			StopCoroutine(_coroutine);
			_tmpProText.text = _text;
			_button.SetActive(true);
			OnComplete?.Invoke();
		}

        private IEnumerator TypeWriterTMP()
		{   // type writing effect using Text Mesh Pro //
			yield return new WaitForSeconds(_startDelay);

            for (int i = 0; i < _text.Length; i++)
            {
				_tmpProText.text += _text[i];
				if (i == _text.Length - 1)
				{
					_button.SetActive(true);
					OnComplete?.Invoke();
				}

				yield return new WaitForSeconds(_typeDelay);
			}
		}
	}
}