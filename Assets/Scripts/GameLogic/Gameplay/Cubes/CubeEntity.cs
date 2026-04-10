using TMPro;
using UnityEngine;

namespace GameLogic.Gameplay.Cubes
{
    public class CubeEntity : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _valueLabels;
        
        private int _value;

        public void Initialize(int value)
        {
            _value = value;
            RefreshLabels(_value);
        }

        private void RefreshLabels(int value)
        {
            foreach (TMP_Text label in _valueLabels)
                label.text = value.ToString();
        }
    }
}