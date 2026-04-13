using System;
using Core.Factories;
using Core.Services.Cube;
using Core.Services.Input;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Cubes;
using GameLogic.Gameplay.Cubes;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Services.Cube
{
    public class CubeService : ICubeService, IInitializable, IDisposable
    {
        private readonly IPointerInput _inputService;
        private readonly IStaticDataService _staticDataService;
        private readonly ICubeFactory _cubeFactory;

        private bool _isDragging;
        private CubeEntity _activeCube;
        private Rigidbody _rigidbody;
        private Camera _mainCamera;
        private Transform _cubesParent;
        
        private Camera MainCamera => _mainCamera ??= Camera.main;
        
        private float BoardHalfWidth => _staticDataService.BoardConfig.BoardHalfWidth;

        public CubeService(IPointerInput inputService, IStaticDataService staticDataService, ICubeFactory cubeFactory)
        {
            _inputService = inputService;
            _staticDataService = staticDataService;
            _cubeFactory = cubeFactory;
        }

        public void Initialize()
        {
            _inputService.PointerDown += OnPointerDown;
            _inputService.PointerMove += OnPointerMove;
            _inputService.PointerUp += OnPointerUp;
        }
        
        public void SetCubesParent(Transform cubesParent) => 
            _cubesParent = cubesParent;
        
        public async UniTask SpawnPlayerCube()
        {
            CubeEntity cube = await _cubeFactory.CreateCube(_cubesParent, isBoardCube: false);
            SetActiveCube(cube);
        }

        public void Dispose()
        {
            _inputService.PointerDown -= OnPointerDown;
            _inputService.PointerMove -= OnPointerMove;
            _inputService.PointerUp -= OnPointerUp;
        }

        private void SetActiveCube(CubeEntity cube) 
            => _activeCube = cube;

        private void OnPointerDown(Vector3 screenPosition)
        {
            if (_activeCube == null) 
                return;
            
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            StartDrag();
        }

        private void OnPointerMove(Vector3 screenPosition)
        {
            if (!_isDragging) 
                return;
            
            Drag(screenPosition);
        }

        private void OnPointerUp(Vector3 screenPosition)
        {
            if (!_isDragging) 
                return;
            
            EndDrag();
        }

        private void StartDrag()
        {
            if (_activeCube == null) 
                return;

            _isDragging = true;
            _rigidbody = _activeCube.Rigidbody;
            _rigidbody.isKinematic = true;
        }

        private void Drag(Vector3 screenPosition)
        {
            if (_activeCube == null || !_isDragging) 
                return;

            Ray ray = MainCamera.ScreenPointToRay(screenPosition);
            Plane plane = new Plane(Vector3.up, _activeCube.transform.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                float boardHalfWidth = BoardHalfWidth;
                float newX = Mathf.Clamp(worldPoint.x, -boardHalfWidth, boardHalfWidth);

                Vector3 newPosition = _activeCube.transform.position;
                newPosition.x = newX;
                _activeCube.transform.position = newPosition;
            }
        }

        private void EndDrag()
        {
            if (_activeCube == null) 
                return;

            _isDragging = false;
            _rigidbody.isKinematic = false;
                
            CubeStaticData config = _staticDataService.CubeConfig;
            float force = Random.Range(config.MinLaunchForce, config.MaxLaunchForce);
            _rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
            
            _activeCube = null;
            _rigidbody = null;
            
            SpawnNextCubeAfterCooldown().Forget();
        }
        
        private async UniTask SpawnNextCubeAfterCooldown()
        {
            CubeStaticData config = _staticDataService.CubeConfig;
            await UniTask.Delay((int)(config.SpawnCooldown * 1000));
            await SpawnPlayerCube();
        }

    }
}