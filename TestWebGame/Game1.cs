﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static Retyped.dom;
using Console = System.Console;

namespace TestWebGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        HTMLDivElement divdata;
        GraphicsDeviceManager graphics;
        Song song;
        MouseState prevstate;
        bool playing;
        KeyboardState prevkstate;
        SpriteBatch spriteBatch;
        Texture2D texBall;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            divdata = document.getElementById("testoutput") as HTMLDivElement;

            song = Content.Load<Song>("awake");
            MediaPlayer.Play(song);
            MediaPlayer.Pause(); 
            MediaPlayer.Volume = 0.1f;

            // texBall = Content.Load<Texture2D>("hacker");
            texBall = Texture2D.FromURL(GraphicsDevice, "Content/hacker.png");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var state = Mouse.GetState();
            var kstate = Keyboard.GetState();

            if (prevstate.RightButton == ButtonState.Released && state.RightButton == ButtonState.Pressed)
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            if (prevkstate.IsKeyUp(Keys.M) && kstate.IsKeyDown(Keys.M))
            {
                if (playing)
                    MediaPlayer.Pause();
                else
                    MediaPlayer.Resume();

                playing = !playing;
            }

            divdata.innerHTML = "Left: " + state.LeftButton + "<br>Right: " + state.RightButton + "<br>Mouse pos: " + state.Position + "<br>A: " + kstate.IsKeyDown(Keys.A) + "<br>Caps: " + kstate.CapsLock;

            prevstate = state;
            prevkstate = kstate;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texBall, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}