using System;

namespace Core.Enums.Scene.Extensions
{
    public static class SceneNameExtensions
    {
        public static string ToSceneString(this SceneType scene)
        {
            return scene switch
            {
                SceneType.Core => nameof(SceneType.Core),
                _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
            };
        }
    }
}