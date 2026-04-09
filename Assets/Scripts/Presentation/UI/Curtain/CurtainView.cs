using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Curtain
{
    public class CurtainView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _loadingText;
        
        public void SetVisible(bool visible) => 
            _canvasGroup.alpha = visible ? 1 : 0;

        public void SetProgress(float progress) => 
            _progressBar.value = progress;

        public void SetText(string text) => 
            _loadingText.text = text;
    }
}