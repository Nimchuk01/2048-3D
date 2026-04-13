using Core.Services.Merge;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameLogic.Gameplay.Cubes
{
    public class CubeEntity : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _valueLabels;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        
        private int _value;
        private IMergeService _mergeService;
        private bool _isMerging;
        
        public Rigidbody Rigidbody => _rigidbody;
        public Collider Collider => _collider;
        public int Value => _value;
        public bool IsMerging => _isMerging;

        [Inject]
        public void Construct(IMergeService mergeService)
        {
            _mergeService = mergeService;
        }

        public void Initialize(int value)
        {
            _value = value;
            RefreshLabels(_value);
            _isMerging = false;
        }

        public void MarkAsMerging()
        {
            _isMerging = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isMerging) 
                return;

            CubeEntity otherCube = collision.gameObject.GetComponent<CubeEntity>();
            if (otherCube == null || otherCube.IsMerging) 
                return;
            
            float relativeVelocity = collision.relativeVelocity.magnitude;
            
            if (_mergeService.CanMerge(this, otherCube, relativeVelocity))
            {
                MarkAsMerging();
                otherCube.MarkAsMerging();
                _mergeService.Merge(this, otherCube);
            }
        }

        private void RefreshLabels(int value)
        {
            foreach (TMP_Text label in _valueLabels)
                label.text = value.ToString();
        }
    }
}