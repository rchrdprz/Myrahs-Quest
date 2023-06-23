using UnityEngine;

namespace Richie.GameProject
{
    public class TextBox : MonoBehaviour
    {
        public event TextOpen OnOpen;
        public delegate void TextOpen();

        public event TextClosed OnClose;
        public delegate void TextClosed();

        // used via animation events and sends the event to the "Dialogue Script" //
        public void AsOpened() => OnOpen?.Invoke();

        public void AsClosed()
        {
            OnClose?.Invoke();
            gameObject.SetActive(false);
        }
    }
}