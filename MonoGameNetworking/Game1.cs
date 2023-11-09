using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameNetworking.Authorization;
using MonoGameNetworking.Player;

namespace MonoGameNetworking;

public class Game1 : Game
{
    private Texture2D ballTexture;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private MovementData movementData;
    private IKeyInputRouter inputRouter;
    private IPlayerController playerController;
    private ClientDataProvider dataProvider;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        inputRouter = new KeyInputRouter(dataProvider);
        movementData = new MovementData(
            new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        
        playerController = new ClientController(movementData, inputRouter);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ballTexture = Content.Load<Texture2D>("ball");
    }
    
    
    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        inputRouter.ParseKeyboardState(keyboardState);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(
            ballTexture,
            movementData.Position,
            null,
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}