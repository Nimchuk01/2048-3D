using Core.Enums.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Factories.UI
{
    public interface ICanvasFactory
    {
        UniTask<Canvas> GetOrCreateByType(CanvasType type);
        UniTask<Canvas> GetOrCreateCurtain();
        void ClearByType(CanvasType type);
        void ClearAll();
    }
}