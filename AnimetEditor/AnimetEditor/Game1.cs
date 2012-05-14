using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AnimetEditor.Forms;
using Animet.Animations;
using Animet.Frames;

namespace AnimetEditor
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D pixel;

        ListComponent<Animation> animList;
        ListComponent<Frame> frameList;
        List<Button> buttons;
        TextBox textBox;
        TextureContainer texContainer;
        FrameContainer frameContainer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            animList = new ListComponent<Animation>(16, 50, "Animations").Load(Content, GraphicsDevice);
            animList.OnDoubleClickedEvent += new ListComponent<Animation>.OnClicked(animList_OnDoubleClickedEvent);

            frameList = new ListComponent<Frame>(286, 50, "Frames").Load(Content, GraphicsDevice);
            frameList.OnClickedEvent += new ListComponent<Frame>.OnClicked(frameList_OnClickedEvent);
            frameList.OnDoubleClickedEvent += new ListComponent<Frame>.OnClicked(frameList_OnDoubleClickedEvent);

            buttons = new List<Button>();

            Button btnNewAnim = new Button(16, 16, "New Animation").Load(Content, GraphicsDevice);
            btnNewAnim.OnClickedEvent += new Button.OnClicked(btnNewAnimClicked);
            buttons.Add(btnNewAnim);

            Button btnDeleteAnim = new Button(136, 16, "Delete Animation").Load(Content, GraphicsDevice);
            btnDeleteAnim.OnClickedEvent += new Button.OnClicked(btnDeleteAnim_OnClickedEvent);
            buttons.Add(btnDeleteAnim);

            Button btnNewFrame = new Button(286, 16, "New Frame").Load(Content, GraphicsDevice);
            btnNewFrame.OnClickedEvent += new Button.OnClicked(btnNewFrame_OnClickedEvent);
            buttons.Add(btnNewFrame);

            Button btnDeleteFrame = new Button(286 + 126, 16, "Delete Frame").Load(Content, GraphicsDevice);
            btnDeleteFrame.OnClickedEvent += new Button.OnClicked(btnDeleteFrame_OnClickedEvent);
            buttons.Add(btnDeleteFrame);

            texContainer = new TextureContainer(16, 400, @"gfx/tex").Load(Content, GraphicsDevice);

            frameContainer = new FrameContainer(600, 100).Load(Content, GraphicsDevice);

            textBox = new TextBox().LoadContent(Content, GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        void frameList_OnClickedEvent(Frame item)
        {
            frameContainer.SetFrame(item);
        }        

        void frameList_OnDoubleClickedEvent(Frame item)
        {
            textBox.Show(item);
        }

        void animList_OnDoubleClickedEvent(Animation item)
        {
            textBox.Show(item);
        }

        void btnDeleteFrame_OnClickedEvent()
        {
            frameList.DeleteSelected();
        }

        void btnNewFrame_OnClickedEvent()
        {
            Frame f = new Frame(texContainer.texture, new List<FramePart>() { new FramePart(Vector2.Zero, 0f, 1f) }) { Name = "f" };
            frameList.AddNew(f);
            frameContainer.SetFrame(f);
        }

        void btnDeleteAnim_OnClickedEvent()
        {
            animList.DeleteSelected();
        }

        void btnNewAnimClicked()
        {
            animList.AddNew(new Animation(new List<Animet.Frames.KeyFrame>(), new List<Animet.Frames.Frame>()) { Name = "a" });
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (textBox.showing)
            {
                textBox.Update(dt);
            }
            else
            {
                texContainer.Update(dt);
                animList.Update(dt);
                frameList.Update(dt);
                frameContainer.Update(dt);
                foreach (var b in buttons)
                {
                    b.Update();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            animList.Draw(spriteBatch);
            frameList.Draw(spriteBatch);
            foreach (var b in buttons)
            {
                b.Draw(spriteBatch);
            }
            texContainer.Draw(spriteBatch);
            frameContainer.Draw(spriteBatch);
            if (textBox.showing)
            {
                spriteBatch.Draw(pixel, new Rectangle(0, 0, 1280, 720), Color.White * 0.5f);
                textBox.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}