using Core.UI.Common.Interfaces;
using UnityEngine;

namespace Core.UI.Common
{
    public abstract class UIView<T> : MonoBehaviour, IUIView where T : UIBaseViewModel
    {
        protected T ViewModel;

        public void Bind(UIBaseViewModel viewModel)
        {
            ViewModel = (T)viewModel;
            OnBind(ViewModel);
        }

        public virtual void Close() => Destroy(gameObject);

        protected virtual void OnBind(T viewModel) { }
    }
}