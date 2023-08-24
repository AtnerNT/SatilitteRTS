using Microsoft.Xna.Framework;

namespace UI;

internal abstract class UIElement
{
    public Vector2 Position { get; protected set; }
    private Vector2 localPosition = Vector2.Zero;

    public int Width { get; private set; } = 20;
    public int Height { get; private set; } = 20;

    public void AnchorPosition(Vector2 anchorHead)
    {
        Position = localPosition + anchorHead;
    }

    public UIElement(Vector2 position, int width, int height)
    {
        localPosition = position;
        Position = localPosition;

        Width = width;
        Height = height;
    }
}
