using System;
using R3;

namespace Core.UI.Common
{
    public abstract class UIBaseViewModel : IDisposable
    {
        public abstract UIIdentifier UIIdentifier { get; }

        public Observable<UIBaseViewModel> CloseRequested => _closeRequested;
        
        private readonly Subject<UIBaseViewModel> _closeRequested = new();

        public void RequestClose() => 
            _closeRequested.OnNext(this);

        public virtual void Dispose() { }
    }
}