using System;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Factories.UI
{
    public interface IUIFactory
    {
        UniTask<GameObject> Create(UIIdentifier identifier, Transform parent);
        UniTask<GameObject> Create(Enum typeEnum, Transform parent);
    }
}