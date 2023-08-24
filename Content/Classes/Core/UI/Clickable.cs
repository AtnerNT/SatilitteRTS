using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Core;

namespace UI
{
    

    public abstract class Clickable
    {
        public Vector2 Position { get; protected set; }
        private Vector2 localPosition = Vector2.Zero;

        public bool isEnabled = false;

        public int Width { get; private set; } = 20;
        public int Height { get; private set; } = 20;

        protected bool hasBeenClicked = false;
        public delegate void OnClickEvent();
        public OnClickEvent OnClick;

        public virtual Rectangle Bounds
        {
            get
            {
                int zoom = GameManager.ZoomLevel;
                return new Rectangle((int)(zoom * Position.X), (int)(zoom * Position.Y), zoom * Width, zoom * Height);
            }
        }

        protected virtual void PerformMouseClick(MouseState mouseState)
        {
            if (Bounds.Contains(mouseState.Position))
            {
                isEnabled = true;
                OnClick?.Invoke();
            }
        }

        public void MouseInteraction(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (hasBeenClicked == true) return;
                hasBeenClicked = true;

                PerformMouseClick(mouseState);
            }
            else hasBeenClicked = false;

        }
   
    }
}
