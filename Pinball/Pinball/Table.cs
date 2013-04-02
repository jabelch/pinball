using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Project03
{

    public class Table : Sprite
    {
        static ArrayList spriteArray;

        Game1 game;
		public Game1 Game { get { return game; } }

        Ball ball;
        Vector2 ballStartLocation;

        Texture2D PINBALL;
        Texture2D FAN, CAP1, CHIP, HHD;
		Texture2D LFLIP, RFLIP, LTRIG, RTRIG;
		Texture2D FANLIGHT;
        RoundBumper fan, cap1, cap2, cap3, hhd;
		Sprite lflip, rflip, ltrig, rtrig, chip1, chip2;
		Sprite fanlight;

        float fanSpeed, hhdSpeed = 0;
        bool isTilt = false;
        int tiltCount = 0;
        bool gameOver = false;
        bool downKey, leftKey, rightKey, wKey, aKey, dKey;
        int ballCount = 5;
		Vector2 ballStartVelocity = Vector2.Zero;
		SoundEffect roll;
		SoundEffect gameOverSound;
		SoundEffect feeble, flip, rebound;
		SoundEffectInstance rollInstance = null;
		Vector4 fanColor = new Vector4(1, 1, 1, 0);

		float elapsedGametime = 0;
		float songVolume = .4f;
		SpriteFont font;
        //Matrix transform;

		public SoundEffect Rebound { get { return rebound; } }

        public Table(Texture2D texture, String name, Vector2 location, Vector2 origin, float rotation, Game1 game) : base(texture, name, location, origin, rotation)
		{
            this.game = game;
            spriteArray = new ArrayList();
		}


        public void LoadContent()
        {
			gameOverSound = game.Content.Load<SoundEffect>("game_over");
			feeble = game.Content.Load<SoundEffect>("feeble");
			roll = game.Content.Load<SoundEffect>("rolling");
			flip = game.Content.Load<SoundEffect>("flip");
			rebound = game.Content.Load<SoundEffect>("rebound");
			font = game.Content.Load<SpriteFont>("SpriteFont1");

			PINBALL = game.Content.Load<Texture2D>("pinBall");
            ballStartLocation = new Vector2(this.Width * .17f, this.Height * .135f);

            FAN = game.Content.Load<Texture2D>("fan");
            CAP1 = game.Content.Load<Texture2D>("Cap1");
            HHD = game.Content.Load<Texture2D>("HD");
			LFLIP = game.Content.Load<Texture2D>("lFlip");
			RFLIP = game.Content.Load<Texture2D>("lFlip");
			LTRIG = game.Content.Load<Texture2D>("leftBumper1");
			RTRIG = game.Content.Load<Texture2D>("rightBumper1");
			FANLIGHT = game.Content.Load<Texture2D>("fanlight");
			CHIP = game.Content.Load<Texture2D>("chip");

            float radius = PINBALL.Width / 2;
            ball = new Ball(PINBALL, "PINBALL", ballStartLocation, new Vector2(radius, radius), 0, radius, rebound);
            fan = new RoundBumper(FAN, "FAN", new Vector2(345, 325), new Vector2(75.4f, 75.4f), 0, 75);
            cap1 = new RoundBumper(CAP1, "RB", new Vector2(625, 625), new Vector2(40, 40), 0, 40);
			cap2 = new RoundBumper(CAP1, "RB", new Vector2(75, 625), new Vector2(40, 40), 0, 40);
			cap3 = new RoundBumper(CAP1, "RB", new Vector2(625, 200), new Vector2(40, 40), 0, 40);
            hhd = new RoundBumper(HHD, "START", new Vector2(137f, 96f), new Vector2(87.5f, 87.5f), 0, HHD.Width / 2);
			lflip = new Sprite(LFLIP, "LFLIP", new Vector2(225, 555), new Vector2(12, 12), 0);
			rflip = new Sprite(RFLIP, "RFLIP", new Vector2(475, 555), new Vector2(88, 12), 0);
			ltrig = new Sprite(LTRIG, "BUMP", new Vector2(25, 660), Vector2.Zero, 0);
			rtrig = new Sprite(RTRIG, "BUMP", new Vector2(500, 660), Vector2.Zero, 0);
			fanlight = new Sprite(FANLIGHT, "LIGHT", fan.Location, new Vector2(23.5f, 23.5f), 0);
			chip1 = new Sprite(CHIP, "CHIP", new Vector2(110, 490), Vector2.Zero, 0);
			chip2 = new Sprite(CHIP, "CHIP", new Vector2(555, 460), Vector2.Zero, 0);

            this.InitializeBoundaries();
            
        }

        private void InitializeBoundaries()
        {
            // Table Boundaries
            this.boundaries.Add(new Line(new Vector2(this.Width, 20), new Vector2(0, 20), .9f)); // TOP
            this.boundaries.Add(new Line(new Vector2(25, 0), new Vector2(25, this.Height), .95f)); // Left Wall
           // this.boundaries.Add(new Line(new Vector2(0, this.Height -90), new Vector2(703, this.Height), 1.8f)); // TAKE OUT: Bottom
            this.boundaries.Add(new Line(new Vector2(673, this.Height), new Vector2(673, 0), .95f)); // Right Wall

            //
            fan.Boundaries = (new Circle(Vector2.Zero, fan.Radius, 1.4f));
            cap1.Boundaries = (new Circle(Vector2.Zero, cap1.Radius, 1.2f));
			cap2.Boundaries = (new Circle(Vector2.Zero, cap1.Radius, 1.2f));
			cap3.Boundaries = (new Circle(Vector2.Zero, cap1.Radius, 1.2f));
            hhd.Boundaries = (new Circle(Vector2.Zero, hhd.Radius, 1.2f));

			// Flippers
			lflip.Boundaries = (new Line(new Vector2(-12, -12), new Vector2(88, -12), 1.1f)); //TOP
			lflip.Boundaries = (new Line(new Vector2(88, 12), new Vector2(-12, 12), .7f));
			lflip.Boundaries = (new Circle(Vector2.Zero, 12, .7f));
			lflip.Boundaries = (new Circle(new Vector2(76, 0), 12, .7f));
			
			rflip.Boundaries = (new Line(new Vector2(-88, -12), new Vector2(12, -12), 1.1f));
			rflip.Boundaries = (new Line(new Vector2(12, 12), new Vector2(-88, 12), .7f));
			rflip.Boundaries = (new Circle(Vector2.Zero, 12, .7f));
			rflip.Boundaries = (new Circle(new Vector2(-76, 0), 12, .7f));
			
			// Lower Triangle Bumpers
			ltrig.Boundaries = (new Line(Vector2.Zero, new Vector2(ltrig.Width, ltrig.Height), 1.5f));
			rtrig.Boundaries = (new Line(new Vector2(0, rtrig.Height), new Vector2(ltrig.Width, 0), 1.5f));

			// Chips
			chip1.Boundaries = (new Line(Vector2.Zero, new Vector2(48, 0), 1.4f));
			chip1.Boundaries = (new Line(new Vector2(48,0), new Vector2(48, 100), 1.4f));
			chip1.Boundaries = (new Line(new Vector2(48, 100), new Vector2(0, 100), .6f));
			chip1.Boundaries = (new Line(new Vector2(0, 100), Vector2.Zero, .6f));

			chip2.Boundaries = (new Line(Vector2.Zero, new Vector2(48, 0), 1.4f));
			chip2.Boundaries = (new Line(new Vector2(48, 0), new Vector2(48, 100), 1.4f));
			chip2.Boundaries = (new Line(new Vector2(48, 100), new Vector2(0, 100), .6f));
			chip2.Boundaries = (new Line(new Vector2(0, 100), Vector2.Zero, .6f));

			chip1.Rotation += (float)-Math.PI / 4;
			chip2.Rotation += (float)Math.PI / 4;
			// Add Sprites to SpriteArray
			spriteArray.Add(this); //Table
			spriteArray.Add(fan);
			spriteArray.Add(cap1);
			spriteArray.Add(cap2);
			spriteArray.Add(cap3);
			spriteArray.Add(hhd);
			spriteArray.Add(lflip);
			spriteArray.Add(rflip);
			spriteArray.Add(ltrig);
			spriteArray.Add(rtrig);
			spriteArray.Add(chip1);
			spriteArray.Add(chip2);
			

			// Add this last so it is drawn last.
			spriteArray.Add(ball);
        }

        public void Update(float seconds)
        {
			MediaPlayer.Volume = (songVolume);
			elapsedGametime += seconds;
            KeyUpdate(seconds);
            if (ball.Location.X > this.Width || ball.Location.Y > this.Height ||
                ball.Location.X < -20 || ball.Location.Y < -20 && !gameOver)
            {
				if (rollInstance != null && !rollInstance.IsDisposed)
				{
					rollInstance.Stop();
				}

				if (ballCount == 0 && !gameOver)
				{
					gameOver = true;
					songVolume = .09f;
					feeble.Play();
				}
				if (!gameOver)
				{
					
					ball.NewGame = true;
					isTilt = false;
					tiltCount = 0;
					ball.Location = ballStartLocation;
					ball.Velocity = Vector2.Zero;
				}
            }

           
            if (!isTilt && fanSpeed < 50.0f)
                fanSpeed += 5.5f * seconds;
            if (fanSpeed > 0 && (isTilt || gameOver))
                fanSpeed -= 20 * seconds;

			if (hhdSpeed > 0 && gameOver)
				hhdSpeed -= 10f * seconds;

			if (!isTilt)
			{
				if (leftKey && lflip.Rotation > -Math.PI / 4)
				{
					lflip.Rotation -= 15f * seconds;
					ball.LFlipVel = (float)(-Math.PI / 4);
				}
				else if (!leftKey && lflip.Rotation < .5f)
				{
					lflip.Rotation += 15f * seconds;
					ball.LFlipVel = 0;
				}

				if (rightKey && rflip.Rotation < Math.PI / 4)
				{
					rflip.Rotation += 15f * seconds;
					ball.RFlipVel = (float)(Math.PI / 4);
				}
				else if (!rightKey && rflip.Rotation > -.5f)
				{
					rflip.Rotation -= 15f * seconds;
					ball.RFlipVel = 0;
				}
			}

			if (rollInstance != null && !rollInstance.IsDisposed)
			{
                rollInstance.Volume = .5f;// ball.Velocity.Length();
			}

			ball.Update(spriteArray, seconds);
				
            fan.Rotation += fanSpeed * seconds;
			fanlight.Rotation = fan.Rotation;
            hhd.Rotation += hhdSpeed * seconds;


			if ((int)(elapsedGametime % 3) == 0)// || !fade)
				fanColor += new Vector4(0, 0, 0, 1) * seconds;
			else
			{
				fanColor += new Vector4(0, 0, 0, -1) * seconds;
				if (fanColor.W < .1f)
					elapsedGametime = 0;
			}
            //else
            //    elapsedGametime = 0;
            //if (fanColor.Z > 1f || fade)
            //{
            //    fanColor -= new Vector4(0, 0, 0, 1) * seconds;
            //    fade = true;
            //}
			
		}
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite s in spriteArray)
            {
                spriteBatch.Draw(s.Texture, s.Location, null, Color.White, s.Rotation, s.Origin, s.Scale, SpriteEffects.None, 0);
            }
			spriteBatch.Draw(FANLIGHT, fanlight.Location, null, new Color(fanColor), fanlight.Rotation, fanlight.Origin, 1, SpriteEffects.None, 0);
			if (!gameOver)
				spriteBatch.DrawString(font, "Ball Count: " + ballCount, new Vector2(300, 0), Color.BlanchedAlmond);
			else
				spriteBatch.DrawString(font, "       Game Over!\n    Press Enter for\n       new game.", hhd.Location, Color.Red, hhd.Rotation, new Vector2(87.5f, 37.5f), 1, SpriteEffects.None, 0);

		}


        private void KeyUpdate(float seconds)
        {
			if (gameOver)
			{
				if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter) == true)
					reset();
				return;
			}

			if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape) == true)
                game.Exit();
			if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down) == true && ball.NewGame == true)
			{
				downKey = true;
				ballStartVelocity += new Vector2(1000,0) * seconds;
				hhdSpeed += 15f * seconds;
			}
			else if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.Down) == true && downKey == true)
			{
				ballCount--;
				downKey = false;
				ball.NewGame = false;
				ball.Velocity = ballStartVelocity;
				ballStartVelocity = Vector2.Zero;
                rollInstance = roll.CreateInstance();
                rollInstance.Volume = .7f;
                rollInstance.Play();
            }
			if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left) == true && !leftKey)
			{
				leftKey = true;
				flip.Play();
			}
			else if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.Left) == true)
			{
				leftKey = false;
			}
			if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right) == true && !rightKey)
			{
				rightKey = true;
				flip.Play();
			}
			else if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.Right) == true)
				rightKey = false;
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up) == true && ball.NewGame == false)
            {
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A) == true && aKey == false && !isTilt)
                {
                    aKey = true;
                    ball.Tilt("a");
                    tiltCount++;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.A) == true && aKey == true)
                    aKey = false;

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D) == true && dKey == false && !isTilt)
                {
                    dKey = true;
                    ball.Tilt("d");
                    tiltCount++;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.D) == true && dKey == true)
                    dKey = false;

				if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W) == true && wKey == false && !isTilt)
                {
                    wKey = true;
                    ball.Tilt("w");
                    tiltCount++;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.W) == true && wKey == true)
                    wKey = false;
            }

            if (tiltCount > 3)
                isTilt = true;
            
        }

		private void reset()
		{
			gameOver = false;
			ball.NewGame = true;
			ballCount = 5;
			songVolume = .4f;
		}

    }
}
