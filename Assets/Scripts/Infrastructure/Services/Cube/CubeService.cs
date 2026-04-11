using System;
using Core.Services.Cube;
using Core.Services.Input;
using Core.Services.StaticData;
using Domain.StaticData.Gameplay.Cubes;
using GameLogic.Gameplay.Cubes;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Services.Cube
{
    public class CubeService : ICubeService, IInitializable, IDisposable
    {
        private readonly IPointerInput _inputService;
        private readonly IStaticDataService _staticDataService;
        
        public CubeEntity ActiveCube { get; private set; }
        public bool IsDragging { get; private set; }
        private Rigidbody _rigidbody;
        private Camera _mainCamera;
        
        private Camera MainCamera => _mainCamera ??= Camera.main;
        
        private float BoardHalfWidth => _staticDataService.BoardConfig?.BoardHalfWidth ?? 1.5f;

        public CubeService(
            IPointerInput inputService,
            IStaticDataService staticDataService)
        {
            _inputService = inputService;
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            _inputService.PointerDown += OnPointerDown;
            _inputService.PointerMove += OnPointerMove;
            _inputService.PointerUp += OnPointerUp;
        }

        public void Dispose()
        {
            _inputService.PointerDown -= OnPointerDown;
            _inputService.PointerMove -= OnPointerMove;
            _inputService.PointerUp -= OnPointerUp;
        }

        public void RegisterCube(CubeEntity cube) { }
        public void UnregisterCube(CubeEntity cube) 
        { 
            if (ActiveCube == cube)
                ActiveCube = null;
        }

        public void SetActiveCube(CubeEntity cube) => ActiveCube = cube;

        private void OnPointerDown(Vector3 screenPosition)
        {
            if (ActiveCube == null) 
                return;
            
            StartDrag(screenPosition);
        }

        private void OnPointerMove(Vector3 screenPosition)
        {
            if (!IsDragging) 
                return;
            
            Drag(screenPosition);
        }

        private void OnPointerUp(Vector3 screenPosition)
        {
            if (!IsDragging) 
                return;
            
            EndDrag();
        }

        public void StartDrag(Vector3 screenPosition)
        {
            if (ActiveCube == null) 
                return;

            IsDragging = true;
            _rigidbody = ActiveCube.Rigidbody;
            _rigidbody.isKinematic = true;
        }

        public void Drag(Vector3 screenPosition)
        {
            if (ActiveCube == null || !IsDragging) 
                return;

            Ray ray = MainCamera.ScreenPointToRay(screenPosition);
            Plane plane = new Plane(Vector3.up, ActiveCube.transform.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                float boardHalfWidth = BoardHalfWidth;
                float newX = Mathf.Clamp(worldPoint.x, -boardHalfWidth, boardHalfWidth);

                Vector3 newPosition = ActiveCube.transform.position;
                newPosition.x = newX;
                ActiveCube.transform.position = newPosition;
            }
        }

        public void EndDrag()
        {
            if (ActiveCube == null) 
                return;

            IsDragging = false;
            _rigidbody.isKinematic = false;
                
            CubeStaticData config = _staticDataService.CubeConfig;
            float force = Random.Range(config.MinLaunchForce, config.MaxLaunchForce);
            _rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
            
            ActiveCube = null;
            _rigidbody = null;
        }

    }
}
