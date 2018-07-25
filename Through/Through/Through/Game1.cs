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
using System.IO;

namespace Through
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 512;
            graphics.PreferredBackBufferWidth = 1024;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            levelnum = 1;
            newMap(levelnum);
            base.Initialize();
            SoundEffect.MasterVolume = 0.25f;
        }
        int levelnum = 1;
        SoundEffect collidSound0;
        SoundEffect collidSound1;
        SoundEffect loseSound;
        SoundEffect winSound;
        Texture2D playerTexture;
        Texture2D playerTexture0;
        Texture2D playerTexture1;
        Texture2D playerTexture2;
        Texture2D powerTexture;
        Texture2D firstTexture;
        Vector2 playerPos = new Vector2(50f, 256f);
        Vector2 playerSpeed = new Vector2(0f, 0f);
        Vector2 origin;
        Rectangle powerMeter = new Rectangle(0, 0, 12, 512);
        Rectangle powerMeter1 = new Rectangle(1012, 0, 12, 512);
        Texture2D[] tile = new Texture2D[32];
        bool first = true;
        
        float time = 0f;
        
        float playerScale = 1f; 
        float mapScale = 2;
        float playerDeg = 0f;
        
        
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            playerTexture0 = Content.Load<Texture2D>("player1");
            playerTexture1 = Content.Load<Texture2D>("player2");
            playerTexture2 = Content.Load<Texture2D>("player3");
            playerTexture = playerTexture0;
            for (int i = 0; i < Convert.ToInt32(File.ReadAllText("not.txt")); i++)
            {
                string currentTile = "tile" + i;
                tile[i] = Content.Load<Texture2D>(currentTile);



            }
            firstTexture = Content.Load<Texture2D>("first");
            collidSound0 = Content.Load<SoundEffect>("collid1");
            collidSound1 = Content.Load<SoundEffect>("collid2");
            loseSound = Content.Load<SoundEffect>("lose");
            winSound = Content.Load<SoundEffect>("win");
            powerTexture = Content.Load<Texture2D>("power");
            origin = new Vector2(playerTexture.Width / 2, playerTexture.Height / 2);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        bool collided = false;
        Vector2 playerPrevPos;
        float stopPower = 100f;

        protected override void Update(GameTime gameTime)
        {
            
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 5f)
            {
                
                first = false;
                time = 0;
            }
            
           
            playerPrevPos = playerPos;
            float MaxX = graphics.GraphicsDevice.Viewport.Width - (playerTexture.Width*Convert.ToInt32(playerScale))-16;
            float MinX = 16*mapScale;
            float MaxY = graphics.GraphicsDevice.Viewport.Height - (playerTexture.Height*Convert.ToInt32(playerScale))-16;
            float MinY = 16*mapScale;
            
            
            if (playerPos.X > MaxX)
            {
                collided = true;
            }

            else if (playerPos.X < MinX)
            {
                collided = true;
            }

            if (playerPos.Y > MaxY)
            {
                collided = true;
            }

            else if (playerPos.Y < MinY)
            {
                collided = true;
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
            {
                newMap(levelnum);
            }
            if (!collided)
            {
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
            {
                if (playerSpeed.X > -2000) { playerSpeed.X += -20; }
                
            }


            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
            {
                if (playerSpeed.X < 2000) { playerSpeed.X += +20; }
                
            }

            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
            {

                if (playerSpeed.Y > -2000) { playerSpeed.Y += -20; }
                    
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S))
            {

                if (playerSpeed.Y < 2000) { playerSpeed.Y += +20; }

            }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
            {
                Vector2 dif = playerSpeed - new Vector2(0, 0);
                if (stopPower > 0)
                {
                    playerSpeed.X = dif.X / 1.1f;
                    playerSpeed.Y = dif.Y / 1.1f;
                    stopPower -= 2;
                }



            }
            else if (stopPower < 101)
            {
                stopPower += 2*(float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            powerMeter.Height = (Convert.ToInt32(stopPower) * 5)+12;
            powerMeter1.Height = (Convert.ToInt32(stopPower) * 5) + 12;
            
            
                
           
            
            Rectangle playerRect = new Rectangle(Convert.ToInt32(playerPos.X)-((playerTexture.Width * Convert.ToInt32(playerScale))/2),Convert.ToInt32(playerPos.Y)-((playerTexture.Height *Convert.ToInt32(playerScale))/2), playerTexture.Width * Convert.ToInt32(playerScale), playerTexture.Height *Convert.ToInt32(playerScale));
            collided = false; 
            foreach (Rectangle currentRect in collRects)
            {
                if (playerRect.Intersects(currentRect)) 
                {
                    
                    collided = true;
                   
                }
            }
            foreach (Rectangle currentRect in loseRects)
            {
                if (playerRect.Intersects(currentRect))
                {
                    loseSound.Play();
                    newMap(levelnum);
                    collided = false;

                }
            }
            if (playerTexture == playerTexture1)
            {
                foreach (Rectangle currentRect in winRects)
                {
                    if (playerRect.Intersects(currentRect))
                    {


                        playerSpeed = new Vector2(0, 0);
                        playerPos.X = currentRect.X + (currentRect.Width / 2);
                        playerPos.Y = currentRect.Y + (currentRect.Height / 2);
                        playerDeg = 0f;
                        stopPower = 100;
                        
                        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (time > 1f)
                        {
                            winSound.Play();                        
                            newMap(levelnum + 1);
                            levelnum += 1;
                            
                            time = 0;
                        }

                    }
                } 
                
            }
            
           
            //Checking the collision with tiles ends
            if (collided)
            {

                playerPos = playerPrevPos;
                if (playerTexture == playerTexture0)
                {
                    collidSound1.Play();
                    playerTexture = playerTexture1;
                }
                else
                {
                    collidSound0.Play();
                    playerTexture = playerTexture0;
                }
                playerSpeed = -playerSpeed;

            }
            
            playerPos += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            playerDeg = ((playerSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds) - (playerSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds))/10;
            
            
            
           
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            GraphicsDevice.Clear(Color.Black);

            for (int y = 0; y < (64/mapScale); y++)
            {
                for (int x = 0; x < (32/mapScale); x++)
                {

                    spriteBatch.Draw(tile[map[x, y]], new Vector2(y * 16*mapScale, x * 16*mapScale), null, Color.White, 0, Vector2.Zero, mapScale, SpriteEffects.None, 0.5f);

                }
            }
            
            spriteBatch.Draw(playerTexture, playerPos, null, Color.White, playerDeg, origin, playerScale, SpriteEffects.None, 0.4f);
            spriteBatch.Draw(playerTexture2, playerPos, null, Color.White, 0f, origin, playerScale, SpriteEffects.None, 0.2f);
            spriteBatch.Draw(powerTexture, powerMeter,null, Color.White,0f,Vector2.Zero,SpriteEffects.None,0.3f);
            spriteBatch.Draw(powerTexture, powerMeter1, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.3f);
            spriteBatch.End();
            if (first)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(firstTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(1.28f,1.28f), SpriteEffects.None, 0.1f);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        string mapFile;
        int[,] map = new int[64, 64];
        int[,] collMap = new int[64, 64]; //collision map: 0 doesn't collide, 1 collides
        string[] tinfo = File.ReadAllLines("tinfo.txt"); //tileinfo file
        Rectangle[] collRects = new Rectangle[2048];
        Rectangle[] loseRects = new Rectangle[2048];
        Rectangle[] winRects = new Rectangle[2048];
        void newMap(int level)
        {
            collRects = new Rectangle[2048];
            winRects = new Rectangle[2048];
            loseRects = new Rectangle[2048];
            if (File.Exists("level" + level + ".txt"))
            {
                mapFile = File.ReadAllText("level" + level + ".txt");
                int i = 0, j = 0; int a = 0; int b = 0; int c = 0;

                foreach (string row in mapFile.Split('\n'))
                {
                    j = 0;
                    foreach (string col in row.Split(','))
                    {
                        map[i, j] = Convert.ToInt32(col);
                        collMap[i, j] = Convert.ToInt32(tinfo[map[i, j]]);
                        switch (collMap[i, j])
                        {
                            case 1:
                                collRects[a] = new Rectangle(Convert.ToInt32(j * 16 * mapScale), Convert.ToInt32(i * 16 * mapScale), Convert.ToInt32(16 * mapScale), Convert.ToInt32(16 * mapScale));
                                a++;
                                break;
                            case 2:
                                loseRects[b] = new Rectangle(Convert.ToInt32(j * 16 * mapScale), Convert.ToInt32(i * 16 * mapScale), Convert.ToInt32(16 * mapScale), Convert.ToInt32(16 * mapScale));
                                b++;
                                break;
                            case 3:
                                winRects[c] = new Rectangle(Convert.ToInt32(j * 16 * mapScale), Convert.ToInt32(i * 16 * mapScale), Convert.ToInt32(16 * mapScale), Convert.ToInt32(16 * mapScale));
                                c++;
                                break;
                            default:
                                break;

                        }

                        j++;
                    }
                    i++;
                }
                playerPos = new Vector2(50f, 256f);
                playerSpeed = new Vector2(0, 0);
                stopPower = 100;
                playerTexture = playerTexture0;
            }
            else
            {
                levelnum = 0;
                newMap(1);
                
            }
            

            
            
        }

        

        
    }
}
