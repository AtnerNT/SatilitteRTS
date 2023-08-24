using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Core;

public partial class GameManager : Game
{
    private const int QT_MAXLEVELS = 3;
    private const int QT_MAXOBJECTS = 3;
    private Rectangle GAME_BOUNDS = new Rectangle(-1000, -10000, 1000 ,1000);

    private static int initialtime = 0;
    public static int ProgramTime = 0;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static SpriteBatch SpriteBatch { get; private set; }
    public static SpriteFont Font { get; private set; }

    private Texture2D _texture;

    private PhysicsObject objectA;
    private PhysicsObject objectB;

    public static int ZoomLevel { get; private set; } = 1;

    public static readonly List<IDrawMe> Drawables = new List<IDrawMe>();

    public GameManager()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        initialtime = Environment.TickCount;
        Collider.QuadTree = new QuadTree(QT_MAXOBJECTS, QT_MAXLEVELS, 0, GAME_BOUNDS);

        objectA = new PhysicsObject(10, new Vector2(10, 10), new Vector2(0.05f, 0), _hasGravity: false);
        objectB = new PhysicsObject(15, new Vector2(200, 200), Vector2.Zero, _hasGravity: true, _affectedByGravity: false);

        BoxCollider boxColliderA = new BoxCollider();
        objectA.AttachCollider(boxColliderA);
        BoxCollider boxColliderB = new BoxCollider();
        objectB.AttachCollider(boxColliderB);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteBatch = _spriteBatch;
        _texture = Content.Load<Texture2D>(@"Sprites\SpriteEpicCool");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        ProgramTime = Environment.TickCount - initialtime;

        foreach(PhysicsObject attractor in PhysicsObject.Attractors)
        {
            attractor.Pull();
        }
        foreach (PhysicsObject obj in PhysicsObject.PhysicsObjects)
        {
            obj.PhysicsUpdate();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, new Rectangle((int)objectA.Position.X, (int)objectA.Position.Y, 20, 20), Color.White);
        _spriteBatch.Draw(_texture, new Rectangle((int)objectB.Position.X, (int)objectB.Position.Y, 20, 20), Color.Green);
        _spriteBatch.End();
 
        base.Draw(gameTime);
    }
}
