using System;
using Core.Enums.UI;

namespace Core.UI
{
    [Serializable]
    public readonly struct UIIdentifier : IEquatable<UIIdentifier>
    {
        public readonly UICategory Category;
        public readonly int Id; 

        public UIIdentifier(UICategory category, int id)
        {
            Category = category;
            Id = id;
        }

        public static UIIdentifier From(CanvasType type) => new(UICategory.Canvas, (int)type);
        public static UIIdentifier From(RootType type) => new(UICategory.Root, (int)type);
        public static UIIdentifier From(ScreenType type) => new(UICategory.Screen, (int)type);
        public static UIIdentifier From(PopupType type) => new(UICategory.Popup, (int)type);

        public override string ToString() => $"{Category}:{Id}";
        public bool Equals(UIIdentifier other) => Category == other.Category && Id == other.Id;
        public override bool Equals(object obj) => obj is UIIdentifier other && Equals(other);
        public override int GetHashCode() => HashCode.Combine((int)Category, Id);
    }

}