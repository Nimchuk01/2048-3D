using Core.Services.Progress;
using Infrastructure.Services.Progress;
using UnityEditor;

namespace Utility
{
    public static class ProgressEditorReset
    {
        [MenuItem("Tools/Reset Progress %#r")] // Ctrl+Shift+R
        public static void ResetProgress()
        {
            IProgressStorage storage = new ProgressStorage(progressService: null);
            storage.Reset();
        }
    }
}