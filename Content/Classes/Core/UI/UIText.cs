using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UI;

public enum HorizontalAlignment
{
    Left = 0,
    Center = 1,
    Right = 2,
}

public enum VerticalAlignment
{
    Top = 0,
    Center = 1,
    Bottom = 2,
}

internal class UIText : UIElement
{
    public UIText(Vector2 position, int width, int height) : base(position, width, height) { }

    public string Text { get; private set; }

    protected HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
    protected VerticalAlignment verticalAlignment = VerticalAlignment.Top;

    public void DrawString()
    {
        int zoom = GameManager.ZoomLevel;

        Vector2 position = new Vector2(Position.X * zoom, Position.Y * zoom);
        position = position + new Vector2((int)horizontalAlignment * Width * zoom / 2, (int)verticalAlignment * Height * zoom / 2);
        position = position - new Vector2(Text.Length * 3.5f, 8);

        GameManager.SpriteBatch.DrawString(GameManager.Font, Text, position, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
    }
}
