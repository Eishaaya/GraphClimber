using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace HillClimber
{
    class Slider : Button
    {
        Sprite bar;
        Label laby;
        Button[] points;

        bool freeSlider { get; }

        public int Value { get; private set; }
        public bool Done { get; private set; }

        string[] texts;
        bool prevTouched = false;

        public Slider(Texture2D image, Vector2 location, Vector2 origin, float scale, Sprite bottom, Button point, int pointCount, bool free, SpriteFont font = null, string text = "", string[] labels = null)
            : this(image, location, Color.White, Color.DarkGray, Color.Gray, origin, scale, bottom, point, pointCount, free, font, text, labels) { }


        public Slider(Texture2D image, Vector2 location, Color color, Color hoverColor, Color clickedColor, Vector2 origin, float scale, Sprite bottom, Button point, int pointCount, bool free, SpriteFont font = null,
                      string text = "", string[] labels = null)

            : this(image, location, color, 0, SpriteEffects.None, origin, scale, 1, hoverColor, clickedColor, bottom, point, pointCount, free, font, text, labels) { }


        public Slider(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effect, Vector2 origin, float scale, float depth, Color hovercolor, Color clickedcolor,
                      Sprite Bottom, Button point, int pointCount, bool free, SpriteFont font = null, string text = "", string[] labels = null, float stringH = 50,
                      float offx = 0, float offy = 0, int value = 0)

            : base(image, location, color, rotation, effect, origin, scale, depth, hovercolor, clickedcolor)
        {
            var buffers = new Vector2(offx, offy) * Scale;
            freeSlider = free;
            Done = true;
            bar = Bottom;
            Value = value;

            points = new Button[pointCount];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = point.Clone();
                points[i].Location = bar.Location - new Vector2(-i * (bar.Image.Width - buffers.X * 2) / (pointCount - 1) + bar.Origin.X - buffers.X, 0) * Scale;
            }

            texts = labels;
            if (font != null)
            {
                laby = new Label(font, Color, new Vector2(bar.Location.X - (int)font.MeasureString(text).X / 2, bar.Location.Y + stringH), text, TimeSpan.Zero);
            }

            Location = points[value].Location;
        }

        public override bool Check(Vector2 cursor, bool isclicked)
        {
            var ballPressed = base.Check(cursor, isclicked);
            Done = !ballPressed & !(prevTouched & isclicked);

            if (!ballPressed && !prevTouched)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].Check(cursor, isclicked))
                    {
                        SetValueTo(i);
                    }
                }
            }
            prevTouched = ballPressed | isclicked & prevTouched;

            Move(cursor);            

            return ballPressed;
        }
        public void Move(Vector2 cursor)
        {
            if (!Done)
            {
                Location = new Vector2(MathHelper.Clamp(cursor.X, points[0].Location.X, points[points.Length - 1].Location.X), Location.Y);

                int closestIndex = 0;
                float minDistance = int.MaxValue;
                for (int i = 0; i < points.Length; i++)
                {
                    var tempDistance = Vector2.Distance(Location, points[i].Location);
                    if (tempDistance < minDistance)
                    {
                        minDistance = tempDistance;
                        closestIndex = i;
                    }
                }
                Value = closestIndex;
                if (texts != null)
                {
                    laby.SetText(texts[Value]);
                    laby.Location = new Vector2(bar.Location.X - (int)laby.Font.MeasureString(laby.Text).X / 2, laby.Location.Y);
                }
                else if (laby != null)
                {
                    laby.SetText(Value);
                    laby.Location = new Vector2(bar.Location.X - (int)laby.Font.MeasureString(laby.Text).X / 2, laby.Location.Y);
                }
                laby.ChangeColor(Color.Yellow);
            }
            else
            {
                var targetLocation = points[Value].Location;

                Location = Vector2.Lerp(Location, targetLocation, .1f);
                if (Vector2.Distance(Location, targetLocation) <= .1f)
                {
                    Location = targetLocation;
                }
                laby.ChangeColor(Color.White);
            }
        }

        public void SetValueTo(int value)
        {
            Value = value;
            Done = true;
            if (texts != null)
            {
                laby.SetText(texts[Value]);
                laby.Location = new Vector2(bar.Location.X - (int)laby.Font.MeasureString(laby.Text).X / 2, laby.Location.Y);
            }
            else if (laby != null)
            {
                laby.SetText(Value);
                laby.Location = new Vector2(bar.Location.X - (int)laby.Font.MeasureString(laby.Text).X / 2, laby.Location.Y);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            bar.Draw(batch);            
            if (!freeSlider)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].Draw(batch);
                }
            }
            base.Draw(batch);
            //batch.Draw(Image, new Vector2(540, 797), Color.Red);

            if (laby != null)
            {
                laby.Print(batch);
            }
        }
    }
}
