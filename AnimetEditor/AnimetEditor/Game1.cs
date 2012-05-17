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
using NibLib.Animations;
using NibLib.Frames;
using NibLib.IO;

namespace AnimetEditor
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D pixel;

        AnimationCollection collection;

        ListComponent<Animation> animList;
        ListComponent<Frame> frameList;
        ListComponent<KeyFrame> kfList;
        List<Button> buttons;
        TextBox textBox;
        TextureContainer texContainer;
        FrameContainer frameContainer;
        KeyFrameContainer kfContainer;
        AnimationComponent animCompo;

        string path = @"C:\Users\Erik\Desktop\" + "testmap.json";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            collection = new AnimationCollection();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            animList = new ListComponent<Animation>(16, 50, "Animations", collection.animations).Load(Content, GraphicsDevice);
            animList.OnDoubleClickedEvent += new ListComponent<Animation>.OnClicked(animList_OnDoubleClickedEvent);
            animList.OnClickedEvent += new ListComponent<Animation>.OnClicked(animList_OnClickedEvent);

            frameList = new ListComponent<Frame>(286, 50, "Frames", collection.frames).Load(Content, GraphicsDevice);
            frameList.OnClickedEvent += new ListComponent<Frame>.OnClicked(frameList_OnClickedEvent);
            frameList.OnDoubleClickedEvent += new ListComponent<Frame>.OnClicked(frameList_OnDoubleClickedEvent);

            kfList = new ListComponent<KeyFrame>(16, 200, "Keyframes", new List<KeyFrame>()).Load(Content, GraphicsDevice);
            kfList.OnClickedEvent += new ListComponent<KeyFrame>.OnClicked(kfList_OnClickedEvent);

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
            
            Button btnKeyFrame = new Button(286 + 286, 16, "KeyFrame").Load(Content, GraphicsDevice);
            btnKeyFrame.OnClickedEvent += new Button.OnClicked(btnKeyFrame_OnClickedEvent);
            buttons.Add(btnKeyFrame);

            Button btnDeleteKeyFrame = new Button(286 + 386, 16, "Del KF").Load(Content, GraphicsDevice);
            btnDeleteKeyFrame.OnClickedEvent += new Button.OnClicked(btnDeleteKeyFrame_OnClickedEvent);
            buttons.Add(btnDeleteKeyFrame);

            Button btnSave = new Button(1280 - 100, 16, "Save").Load(Content, GraphicsDevice);
            btnSave.OnClickedEvent += new Button.OnClicked(btnSave_OnClickedEvent);
            buttons.Add(btnSave);

            Button btnLoad = new Button(1280, 16, "Load").Load(Content, GraphicsDevice);
            btnLoad.OnClickedEvent += new Button.OnClicked(btnLoad_OnClickedEvent);
            buttons.Add(btnLoad);

            texContainer = new TextureContainer(16, 400, @"gfx/tex").Load(Content, GraphicsDevice);
            texContainer.OnNewSource += new TextureContainer.OnNewSourceDelegate(texContainer_OnNewSource);

            frameContainer = new FrameContainer(600, 200).Load(Content, GraphicsDevice);
            kfContainer = new KeyFrameContainer(16, 400).Load(Content, GraphicsDevice);

            textBox = new TextBox().LoadContent(Content, GraphicsDevice);

            animCompo = new AnimationComponent(900, 50).Load(Content, GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        void btnDeleteKeyFrame_OnClickedEvent()
        {
            kfList.DeleteSelected();
            kfContainer.SetKeyFrame(null);
        }

        void btnLoad_OnClickedEvent()
        {
            collection = AnimationIO.Load(path, null);
            LoadContent();
            collection.Load(Content);
        }

        void kfList_OnClickedEvent(KeyFrame item)
        {
            // select keyframe
            kfContainer.SetKeyFrame(item);
        }

        void btnSave_OnClickedEvent()
        {
            AnimationIO.Save(collection, path);
        }

        void animList_OnClickedEvent(Animation item)
        {
            animCompo.SetAnimation(item);
            AnimChanged(item);
        }

        void btnKeyFrame_OnClickedEvent()
        {
            Frame item = frameList.GetSelected();
            Animation anim = animList.GetSelected();
            anim.Keyframes.Add(new KeyFrame(item, 100f, new NibLib.Scripts.KeyFrameScript[] { }));
        }

        void texContainer_OnNewSource(Rectangle source)
        {
            frameContainer.SetSource(source);
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
            frameContainer.SetFrame(null);
        }

        void btnNewFrame_OnClickedEvent()
        {
            Frame f = new Frame(texContainer.texture, new List<FramePart>() { new FramePart(Vector2.Zero, 0f, 1f) }) { Name = "f" };
            if (frameList.selected != -1)
            {
                f = frameList.items[frameList.selected].Clone();
            }
            frameList.AddNew(f);
            frameContainer.SetFrame(f);
        }

        void btnDeleteAnim_OnClickedEvent()
        {
            animList.DeleteSelected();
            AnimChanged(null);
        }

        void btnNewAnimClicked()
        {
            Animation ani = new Animation(new List<NibLib.Frames.KeyFrame>()) { Name = "a" };
            animList.AddNew(ani);
            AnimChanged(ani);
        }

        void AnimChanged(Animation ani)
        {
            if (ani == null)
            {
                kfList.items = new List<KeyFrame>();
            }
            else
            {
                kfList.items = ani.Keyframes;
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
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
                kfList.Update(dt);
                frameContainer.Update(dt);
                animCompo.Update(dt);
                kfContainer.Update(dt);
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
            kfList.Draw(spriteBatch);
            foreach (var b in buttons)
            {
                b.Draw(spriteBatch);
            }
            texContainer.Draw(spriteBatch);
            frameContainer.Draw(spriteBatch);
            animCompo.Draw(spriteBatch);
            kfContainer.Draw(spriteBatch);
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