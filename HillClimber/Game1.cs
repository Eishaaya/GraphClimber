﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using System.Collections.Generic;

namespace HillClimber
{
    public class Game1 : Game
    {
        Climber climber;

        enum RunType
        {
            Climber,
            Dorito,
        }

        bool shouldRun;
        int placements = 0;
        RunType value = 0;
        public static Vector2 bounds = new Vector2(940, 1080);
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Button> points;
        Texture2D draggedTexture;
        Button draggedPoint;
        Button selectedPoint;


        Texture2D MakeButttxt;
        Button pointMaker;
        Button clear;
        Button delete;
        Button run;

        ButtonLabel xLabel;
        ButtonLabel yLabel;
        ButtonLabel arrangementLabel;
        ButtonLabel timeLabel;

        Slider slider;
        
        //Texture2D LinearButttxt;
        //Texture2D TimeXPosButttxt;

        //Texture2D darkLinetxt;

        MouseState ms;
        KeyboardState ks;
        bool prevDown;
        int grabbedIndex;
        ScalableSprite graphBackGround;
        ScalableSprite midBeam;
        List<ScalableSprite> gridLines;
        Sprite drawnLine;
        ScalableSprite drawnSegment;
        Vector2 offSet;
        float gridWidth;
        Sprite settingsBox;




        Timer time = new Timer(50);

        Label indexLabel;

        bool anyKeyPressed;

        List<Sprite> drawnPoints;
        List<Sprite> drawnCheckPoints;

        float newNumber;

        //Sprite numberLine;        

        Dictionary<ButtonLabel, Func<bool>> mapping;
        Dictionary<Keys, string> keyStrings;

        Timer OneSecTimer;

        public Game1()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)bounds.X;
            graphics.PreferredBackBufferHeight = (int)bounds.Y;
        }
        protected override void Initialize()
        {
            //tell chris to remember the number
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var rightFont = Content.Load<SpriteFont>("RightFont");
            ms = new MouseState();
            ks = new KeyboardState();
            points = new List<Button>();
            gridLines = new List<ScalableSprite>();
            drawnPoints = new List<Sprite>();
            drawnCheckPoints = new List<Sprite>();
            draggedPoint = null;
            prevDown = false;
            anyKeyPressed = false;
            shouldRun = false;
            grabbedIndex = -1;
            OneSecTimer = new Timer(1000);

            gridWidth = bounds.Y / 2;
            offSet = new Vector2(0, bounds.Y / 4);
            draggedTexture = Content.Load<Texture2D>("Point");

            graphBackGround = new ScalableSprite(Content.Load<Texture2D>("Pixel"), Vector2.Zero, Color.LightGray, 0, SpriteEffects.None, Vector2.Zero, new Vector2(540, 1080), 1);
            midBeam = new ScalableSprite(Content.Load<Texture2D>("Pixel"), new Vector2(0, offSet.Y), Color.Black, 0, SpriteEffects.None, new Vector2(0, .5f), new Vector2(540, 10), 1);
            gridLines.Add(midBeam);
            midBeam = new ScalableSprite(Content.Load<Texture2D>("Pixel"), new Vector2(0, bounds.Y * 3 / 4), Color.Black, 0, SpriteEffects.None, new Vector2(0, .5f), new Vector2(540, 10), 1);
            gridLines.Add(midBeam);
            midBeam = new ScalableSprite(Content.Load<Texture2D>("Pixel"), new Vector2(5, 0), Color.Black, 0, SpriteEffects.None, new Vector2(.5f, 0), new Vector2(10, 1080), 1);
            gridLines.Add(midBeam);
            midBeam = new ScalableSprite(Content.Load<Texture2D>("Pixel"), new Vector2(535, 0), Color.Black, 0, SpriteEffects.None, new Vector2(.5f, 0), new Vector2(10, 1080), 1);
            gridLines.Add(midBeam);

            //numberLine = new Sprite(Content.Load<Texture2D>("Linear"), new Vector2(0, 0));

            int lineAmount = 20;



            for (int i = 0; i <= lineAmount; i++)
            {
                gridLines.Add(new ScalableSprite(Content.Load<Texture2D>("Pixel"), new Vector2(0, i * bounds.Y / lineAmount), Color.Black, 0, SpriteEffects.None, new Vector2(0, .5f), new Vector2(540, 2), 1));
                if (i < lineAmount / 2)
                {
                    gridLines.Add(new ScalableSprite(Content.Load<Texture2D>("Pixel"), new Vector2(i * bounds.Y / lineAmount, 0), Color.Black, 0, SpriteEffects.None, new Vector2(.5f, 0), new Vector2(2, 1080), 1));
                }
            }

            settingsBox = new Sprite(Content.Load<Texture2D>("Box"), new Vector2(gridWidth, 47), Color.White, 0, SpriteEffects.None, Vector2.Zero, 1, 1);
            drawnLine = new Sprite(Content.Load<Texture2D>("Line"), new Vector2(-1), Color.Gold, 0, SpriteEffects.None, new Vector2(7.5f), 1, 1);
            drawnSegment = new ScalableSprite(Content.Load<Texture2D>("LineSegment"), Vector2.Zero, Color.Gold, 0, SpriteEffects.None, new Vector2(0, 7.5f), new Vector2(1));

            var sliderPos = new Vector2(gridWidth + 200, settingsBox.Location.Y + settingsBox.Image.Height + 50);
            slider = new Slider(Content.Load<Texture2D>("SliderBall"), Vector2.Zero, new Vector2(20, 20), 
                                new Sprite(Content.Load<Texture2D>("Slider"), sliderPos, new Vector2(200, 12.5f)),
                                new Button(Content.Load<Texture2D>("SliderPoint"), Vector2.Zero), 2, false, rightFont, "Sussy Baka", new string[] {
                                    "Hill Climber",
                                    "Brain Dorito"
                                });

            MakeButttxt = Content.Load<Texture2D>("MakeButton");
           // LinearButttxt = Content.Load<Texture2D>("LinearButton");
            //TimeXPosButttxt = Content.Load<Texture2D>("2DButton");

            //darkLinetxt = Content.Load<Texture2D>("DarkLine");

            run = new Button(Content.Load<Texture2D>("RunButton"), new Vector2(bounds.X - 400, 0));
            pointMaker = new Button(MakeButttxt, new Vector2(bounds.X - 300, 0));
            delete = new Button(Content.Load<Texture2D>("DeleteButton"), new Vector2(bounds.X - 200, 0));
            clear = new Button(Content.Load<Texture2D>("ClearButton"), new Vector2(bounds.X - 100, 0));


            arrangementLabel = new ButtonLabel(new Button(Content.Load<Texture2D>("Short Highlight"), new Vector2(gridWidth + 259, run.Image.Height + 300),
                                                          Color.Transparent, 0, SpriteEffects.None, Vector2.Zero, 1, 1, Color.White, Color.LightGray),
                                               new Label(rightFont, Color.White, new Vector2(gridWidth + 269, run.Image.Height + 324 - 18), ""));

            xLabel = new ButtonLabel(new Button(Content.Load<Texture2D>("Highlight"), new Vector2(gridWidth + 50, run.Image.Height + 50),
                                                Color.Transparent, 0, SpriteEffects.None, Vector2.Zero, 1, 1, Color.White, Color.LightGray),
                                     new Label(rightFont, Color.White, new Vector2(gridWidth + 50, run.Image.Height + 74 - 18), ""));

            yLabel = new ButtonLabel(new Button(Content.Load<Texture2D>("Highlight"), new Vector2(gridWidth + 50, run.Image.Height + 150),
                                                Color.Transparent, 0, SpriteEffects.None, Vector2.Zero, 1, 1, Color.White, Color.LightGray),
                                    new Label(rightFont, Color.White, new Vector2(gridWidth + 50, run.Image.Height + 174 - 18), ""));

            timeLabel = new ButtonLabel(new Button(Content.Load<Texture2D>("Short Highlight"), new Vector2(gridWidth + 259, run.Image.Height + 600),
                                                          Color.Transparent, 0, SpriteEffects.None, Vector2.Zero, 1, 1, Color.White, Color.LightGray),
                                               new Label(rightFont, Color.White, new Vector2(gridWidth + 269, run.Image.Height + 624 - 18), "0.05"));

            indexLabel = new Label(rightFont, Color.White, new Vector2(gridWidth + 272, settingsBox.Location.Y + 474 - 18), "NaN");


            mapping = new Dictionary<ButtonLabel, Func<bool>>()
            {
                [arrangementLabel] = SwapArrangment,
                [xLabel] = SetX,
                [yLabel] = SetY,
                [timeLabel] = SetTime
            };

            keyStrings = new Dictionary<Keys, string>()
            {
                [Keys.D1] = "1",
                [Keys.D2] = "2",
                [Keys.D3] = "3",
                [Keys.D4] = "4",
                [Keys.D5] = "5",
                [Keys.D6] = "6",
                [Keys.D7] = "7",
                [Keys.D8] = "8",
                [Keys.D9] = "9",
                [Keys.D0] = "0",

                [Keys.OemMinus] = "-",
                [Keys.Decimal] = ".",
                [Keys.OemPeriod] = "."
            };

            //Func<int, int, int> sumFunction = mapping[0];
            //sumFunction(5, 5);
        }

        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            #region updateSetup

            base.Update(gameTime);
            ms = Mouse.GetState();
            ks = Keyboard.GetState();

            var mousePos = new Vector2(ms.Position.X, ms.Position.Y);
            var mouseDown = ms.LeftButton == ButtonState.Pressed;
            var midDown = ms.MiddleButton == ButtonState.Pressed;

            #endregion

            #region drawingPoints
            if (shouldRun)
            {
                time.Tick(gameTime);
                if (time.Ready() || time.GetMillies() == 0)
                {
                    climber.Update(points, gridWidth);

                    var tempOffset = new Vector2(0, gridWidth / 2); ;
                    drawnLine.Location = new Vector2(0, climber.getY(0)) * gridWidth;
                    drawnPoints.Add(drawnLine.Clone());
                    drawnLine.Location = new Vector2(1, climber.getY(1)) * gridWidth;


                    drawnSegment.Location = drawnPoints[drawnPoints.Count - 1].Location;
                    drawnSegment.rotation = (float)Math.Atan(climber.M);
                    drawnSegment.Scale2D = new Vector2(Vector2.Distance(drawnSegment.Location, drawnLine.Location), 1);


                    drawnPoints.Add(drawnSegment.Clone());
                    drawnPoints.Add(drawnLine.Clone());

                    drawnPoints.ColorPoints(Color.DarkOrange);
                }
            }
            #endregion

            #region selectionSettings

            if (timeLabel.Clicked || timeLabel.Check(mousePos, mouseDown))
            {
                if (!InputLogic(timeLabel, xLabel, yLabel, arrangementLabel))
                {
                    timeLabel.Label.SetText((double)time.GetMillies() / 1000, 3);
                }
            }

            if (selectedPoint != null)
            {

                if (arrangementLabel.Clicked || arrangementLabel.Check(mousePos, mouseDown))
                {
                    InputLogic(arrangementLabel, xLabel, yLabel, timeLabel);
                }
                else if (xLabel.Clicked || xLabel.Check(mousePos, mouseDown))
                {
                    InputLogic(xLabel, arrangementLabel, yLabel, timeLabel);
                }
                else if (yLabel.Clicked || yLabel.Check(mousePos, mouseDown))
                {
                    InputLogic(yLabel, arrangementLabel, xLabel, timeLabel);
                }
                else if (mouseDown && mousePos.X < gridWidth)
                {
                    CancelSelection();
                }

                else
                {
                    var coords = selectedPoint.Location.ConvertPos(new Vector2(gridWidth), offSet);
                    if (!arrangementLabel.Clicked)
                    {
                        arrangementLabel.Label.SetText(grabbedIndex, 0);
                    }
                    if (!xLabel.Clicked)
                    {
                        xLabel.Label.SetText(coords.X, 5);
                    }
                    if (!yLabel.Clicked)
                    {
                        yLabel.Label.SetText(coords.Y, 5);
                    }
                }
            }

            else
            {
                xLabel.Label.Clear();
                yLabel.Label.Clear();
                arrangementLabel.Label.Clear();
            }

            #endregion

            #region topButtons
            if (pointMaker.Check(mousePos, mouseDown) && !prevDown)
            {
                if (draggedPoint == null)
                {
                    SetDraggedPoint(mousePos);
                }
                selectedPoint = null;
                grabbedIndex = -1;
                prevDown = true;
            }
            else if (delete.Check(mousePos, mouseDown))
            {
                if (shouldRun)
                {
                    drawnPoints.Clear();
                }
                DeletePoint();
            }
            else if (clear.Check(mousePos, mouseDown))
            {
                shouldRun = false;
                drawnPoints.Clear();
                points.Clear();
                indexLabel.SetText("NaN");
            }
            else if (run.Check(mousePos, mouseDown) & !prevDown)
            {
                shouldRun = !shouldRun;
                RunBlock(mousePos, mouseDown, midDown, gameTime);
                prevDown = true;
            }
            else if (!slider.Check(mousePos, mouseDown))
            {
                value = (RunType)slider.Value;
            }

            #endregion

            #region pointManagment

            else
            {
                if (draggedPoint != null)
                {
                    DragLogic(mousePos, mouseDown);
                }
                CheckPoints(mousePos, mouseDown, midDown);

            }
            prevDown = mouseDown;
        }
        #endregion

        #region PointPlacingFunctions

        void CancelSelection()
        {
            selectedPoint = null;
            grabbedIndex = -1;
            xLabel.Clicked = false;
            yLabel.Clicked = false;
            arrangementLabel.Clicked = false;
            timeLabel.Clicked = false;
        }

        void DeletePoint()
        {
            if (draggedPoint != null)
            {
                if (grabbedIndex >= 0)
                {
                    points.RemoveAt(grabbedIndex);
                }
                draggedPoint = null;

                ColorPoints();
            }
            else if (selectedPoint != null)
            {
                points.RemoveAt(grabbedIndex);
                selectedPoint = null;
                grabbedIndex = -1;
            }
        }

        void PlacePoint()
        {
            if (draggedPoint.Location.X > gridWidth || draggedPoint.Location.X < 0 || draggedPoint.Location.Y > bounds.Y || draggedPoint.Location.Y < 0)
            {
                return;
            }
            draggedPoint.Color = Color.White;
            if (ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))
            {
                draggedPoint.Location = new Vector2((float)Math.Round(draggedPoint.Location.X / gridWidth, 1) * gridWidth, (float)Math.Round(draggedPoint.Location.Y / gridWidth, 1) * gridWidth);
            }
            if (grabbedIndex < 0)
            {
                points.Add(draggedPoint);
            }
            else
            {
                points[grabbedIndex] = draggedPoint;
            }
            if (ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift))
            {
                draggedPoint = draggedPoint.Clone();
                grabbedIndex = -1;
            }
            else
            {
                draggedPoint = null;
            }
            ColorPoints();
            placements++;
        }

        void ColorPoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                var degree = (float)Math.Pow(i / (points.Count != 1 ? (float)points.Count - 1 : 1), 1.75);
                points[i].NormalColor = Color.Lerp(Color.CornflowerBlue, Color.Red, degree);
            }
            if (points.Count > 0)
            {
                indexLabel.SetText(points.Count - 1);
            }
            else
            {
                indexLabel.SetText("NaN");
            }
        }

        void SetDraggedPoint(Vector2 mousePos, int index = -1)
        {
            draggedPoint = new Button(draggedTexture, mousePos, Color.White, 0, SpriteEffects.None, new Vector2(12, 12), 1, 1, Color.DarkGray, Color.Gray);
            grabbedIndex = index;
        }

        #endregion

        #region seperatedBlocks

        void RunBlock(Vector2 mousePos, bool mouseDown, bool midDown, GameTime gameTime)
        {
            climber = new Climber();
            CheckPoints(mousePos, mouseDown, midDown);
            drawnPoints = new List<Sprite>();
        }

        void DragLogic(Vector2 mousePos, bool mouseDown)
        {
            draggedPoint.Location = mousePos;
            if (ms.RightButton == ButtonState.Pressed && !prevDown)
            {
                DeletePoint();
            }
            else if (mouseDown && !prevDown)
            {
                prevDown = true;
                PlacePoint();
            }
        }

        bool InputLogic(ButtonLabel label, params ButtonLabel[] others)
        {
            if (ks.GetPressedKeys().Length > 0)
            {
                if (!anyKeyPressed)
                {
                    anyKeyPressed = true;
                    var input = ks.GetPressedKeys()[0];

                    var numberInputed = keyStrings.ContainsKey(input);

                    if (!numberInputed)
                    {
                        if (ks.IsKeyDown(Keys.Back))
                        {
                            label.Clicked = false;
                        }
                        if (ks.IsKeyDown(Keys.Enter))
                        {
                            var parseable = float.TryParse(label.Label.Text, out newNumber);

                            if (!parseable || !mapping[label]())
                            {
                                label.Clicked = false;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        label.Label.Add(keyStrings[input][0]);
                    }

                }
            }
            else
            {
                anyKeyPressed = false;
            }
            foreach (var button in others)
            {
                button.Clicked = false;
            }
            return true;
        }

        bool SwapArrangment()
        {
            var intNumber = (int)newNumber;
            if (intNumber >= 0 && intNumber < points.Count)
            {
                for (int i = grabbedIndex; i < points.Count - 1; i++)
                {
                    points[i] = points[i + 1];
                }

                for (int i = points.Count - 1; i > intNumber; i--)
                {
                    points[i] = points[i - 1];
                }

                points[intNumber] = selectedPoint;

                grabbedIndex = intNumber;
                selectedPoint = points[intNumber];
                arrangementLabel.Clicked = false;

                ColorPoints();
                return true;
            }
            return false;
        }

        bool SetX()
        {
            if (newNumber >= 0 && newNumber <= 1)
            {
                selectedPoint.Location = new Vector2(newNumber * gridWidth, selectedPoint.Location.Y);
                xLabel.Clicked = false;
            }
            return false;
        }
        bool SetY()
        {
            if (newNumber >= -.5 && newNumber <= 1.5)
            {
                selectedPoint.Location = new Vector2(selectedPoint.Location.X, newNumber * gridWidth + offSet.Y);
                yLabel.Clicked = false;
            }
            return false;
        }

        bool SetTime()
        {
            if (newNumber <= 999)
            {
                time = new Timer((int)(newNumber * 1000));
                timeLabel.Clicked = false;
                return true;
            }
            timeLabel.Label.SetText((double)time.GetMillies() / 1000, 3);
            return false;
        }

        void CheckPoints(Vector2 mousePos, bool mouseDown, bool midDown)
        {
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                if (i != grabbedIndex || selectedPoint == null)
                {
                    if (point.Check(mousePos, mouseDown) && !prevDown)
                    {
                        selectedPoint = null;
                        SetDraggedPoint(mousePos, i);
                    }
                    else if (point.Check(mousePos, midDown))
                    {
                        selectedPoint = points[i];
                        grabbedIndex = i;

                        xLabel.Clicked = false;
                        yLabel.Clicked = false;
                        arrangementLabel.Clicked = false;
                    }
                }
            }
        }
        #endregion

        #region drawing
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();            


            graphBackGround.Draw(spriteBatch);
            //numberLine.Draw(spriteBatch);

            if (shouldRun)
            {
                DrawLine(0, 3);
            }

            foreach (ScalableSprite line in gridLines)
            {
                line.Draw(spriteBatch);
            }

            if (shouldRun && drawnPoints.Count > 0)
            {
                DrawLine(drawnPoints.Count - 3, 0);
            }
            DrawPoints();

            if (draggedPoint != null)
            {
                draggedPoint.Draw(spriteBatch);
            }

            delete.Draw(spriteBatch);
            clear.Draw(spriteBatch);
            run.Draw(spriteBatch);
            pointMaker.Draw(spriteBatch);

            xLabel.Draw(spriteBatch);
            yLabel.Draw(spriteBatch);
            arrangementLabel.Draw(spriteBatch);

            settingsBox.Draw(spriteBatch);
            GraphicsDevice.Clear(Color.SlateGray);

            indexLabel.Print(spriteBatch);
            timeLabel.Draw(spriteBatch);

            slider.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void DrawPoints()
        {
            foreach (Sprite point in points)
            {
                point.Draw(spriteBatch);
            }
        }
        void DrawLine(int start, int endOffset)
        {
            for (int i = start; i < drawnPoints.Count - endOffset; i++)
            {
                drawnPoints[i].Draw(spriteBatch);
            }
            //foreach (var point in drawnCheckPoints)
            //{
            //    point.Draw(spriteBatch);
            //}
        }
        #endregion
    }
}
