using System.Threading;
using Core.Services.Boosters;
using Core.Services.Cube;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Services.Timing;
using Presentation.Gameplay.Cubes;
using R3;
using UnityEngine;

namespace Infrastructure.Services.Boosters
{
    public class BoosterService : IBoosterService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICubeTracker _cubeTracker;
        private readonly ReactiveProperty<bool> _isOnCooldown = new(false);
        private readonly ReactiveProperty<float> _remainingCooldown = new(0f);

        public ReadOnlyReactiveProperty<bool> IsOnCooldown => _isOnCooldown;
        public ReadOnlyReactiveProperty<float> RemainingCooldown => _remainingCooldown;

        public BoosterService(IStaticDataService staticDataService, ICubeTracker cubeTracker)
        {
            _staticDataService = staticDataService;
            _cubeTracker = cubeTracker;
        }

        public async UniTask RunAutoMerge(CancellationToken token)
        {
            if (_isOnCooldown.CurrentValue)
                return;

            var pair = FindMergeablePair();
            if (pair == default)
                return;

            var (source, target) = pair.Value;
            
            await PerformMerge(source, target, token);
            _ = RunCooldownAsync(token);
        }

        private async UniTask RunCooldownAsync(CancellationToken token)
        {
            float cooldown = _staticDataService.CubeConfig.AutoMergeCooldown;
            _isOnCooldown.Value = true;
            _remainingCooldown.Value = cooldown;

            while (_remainingCooldown.CurrentValue > 0)
            {
                await Wait.Seconds(1f, token);
                _remainingCooldown.Value = Mathf.Max(0, _remainingCooldown.CurrentValue - 1f);
            }

            _isOnCooldown.Value = false;
        }

        private (CubeEntity source, CubeEntity target)? FindMergeablePair()
        {
            var activeCubes = _cubeTracker.Cubes;
            
            for (int i = 0; i < activeCubes.Count; i++)
            {
                for (int j = i + 1; j < activeCubes.Count; j++)
                {
                    if (activeCubes[i].Value == activeCubes[j].Value)
                        return (activeCubes[i], activeCubes[j]);
                }
            }
            
            return null;
        }

        private async UniTask PerformMerge(CubeEntity source, CubeEntity target, CancellationToken token)
        {
            var sourceRb = source.Rigidbody;
            var targetRb = target.Rigidbody;

            sourceRb.isKinematic = true;
            targetRb.isKinematic = true;
            
            Physics.IgnoreCollision(source.Collider, target.Collider, true);

            Vector3 sourceStartPos = source.transform.position;
            Vector3 targetStartPos = target.transform.position;
            Vector3 sourceLiftPos = sourceStartPos + Vector3.up * 3f;
            Vector3 targetLiftPos = targetStartPos + Vector3.up * 3f;
            
            await AnimateLift(source, target, sourceStartPos, targetStartPos, sourceLiftPos, targetLiftPos, token);
            await AnimateSwing(source, target, sourceLiftPos, targetLiftPos, token);
            Physics.IgnoreCollision(source.Collider, target.Collider, false);
            
            sourceRb.isKinematic = false;
            targetRb.isKinematic = false;
            Vector3 sourceToTarget = (targetLiftPos - sourceLiftPos).normalized;
            Vector3 targetToSource = -sourceToTarget;
            
            sourceRb.AddForce(sourceToTarget * 10f + Vector3.down * 3f, ForceMode.Impulse);
            targetRb.AddForce(targetToSource * 10f + Vector3.down * 3f, ForceMode.Impulse);
        }

        private async UniTask AnimateLift(CubeEntity source, CubeEntity target, Vector3 sourceStart, Vector3 targetStart, Vector3 sourceEnd, Vector3 targetEnd, CancellationToken token)
        {
            float duration = 0.4f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                
                source.transform.position = Vector3.Lerp(sourceStart, sourceEnd, t);
                target.transform.position = Vector3.Lerp(targetStart, targetEnd, t);
                
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, token);
            }
        }

        private async UniTask AnimateSwing(CubeEntity source, CubeEntity target, Vector3 sourcePos, Vector3 targetPos, CancellationToken token)
        {
            const float shakeDuration = 0.3f;
            var sourceTween = source.transform.DOShakePosition(shakeDuration, strength: 0.3f, vibrato: 8, randomness: 45f);
            var targetTween = target.transform.DOShakePosition(shakeDuration, strength: 0.3f, vibrato: 8, randomness: 45f);

            await Wait.Seconds(shakeDuration, token);

            sourceTween.Kill();
            targetTween.Kill();
            source.transform.position = sourcePos;
            target.transform.position = targetPos;
        }
    }
}