using System;

namespace Core.Enums.UI.Extensions
{
    public static class CanvasNameExtensions
    {
        public static string ToCanvasString(this CanvasType canvas)
        {
            return canvas switch
            {
                CanvasType.Curtain => "CurtainCanvas",
                CanvasType.Overlay => "OverlayCanvas",
                _ => throw new ArgumentOutOfRangeException(nameof(canvas), canvas, null)
            };
        }
    }
}