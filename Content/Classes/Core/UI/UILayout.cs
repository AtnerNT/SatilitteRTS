using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UI;

internal class UILayout
{
    public List<Clickable> ClickableElements { get; private set; } = new List<Clickable>();
    public List<UIText> TextElements { get; private set; } = new List<UIText>();
    public List<UIImage> ImageElements { get; private set; } = new List<UIImage>();

    public Vector2 Position { get; private set; }

    public void Draw()
    {
        foreach(UIText textElement in TextElements)
        {
            textElement.DrawString();
        }
        foreach (UIImage imageElement in ImageElements)
        {
            //imageElement.Draw();
        }
    }
}
