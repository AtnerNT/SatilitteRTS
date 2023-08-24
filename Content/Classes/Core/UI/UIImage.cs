using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UI;

internal class UIImage : UIElement
{
    public UIImage(Vector2 position, int width, int height) : base(position, width, height) { }
    /*
    public void Draw()
    {
        int zoom = GameManager.ZoomLevel;
        Rectangle destRect = new Rectangle((int)Position.X * zoom, (int)Position.Y * zoom, (int)Size.X * zoom, (int)Size.Y * zoom);

        GameManager.SpriteBatch.Draw(GameManager.Atlas, destRect, sourceRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
    }*/
}
