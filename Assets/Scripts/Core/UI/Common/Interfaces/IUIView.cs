namespace Core.UI.Common.Interfaces
{
    public interface IUIView
    {
        void Bind(UIBaseViewModel viewModel);
        void Close();
    }
}