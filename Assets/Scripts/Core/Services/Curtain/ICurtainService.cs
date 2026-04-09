using Core.Services.Initialization;

namespace Core.Services.Curtain
{
    public interface ICurtainService : IInitializableAsync
    {
        void Show(string text = "Loading...");
        void Hide();
        void SetProgress(float value);
        void SetText(string text);
    }
}