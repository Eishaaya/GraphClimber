using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    public class Sprite : VisualObject
    {
        public Texture2D Image { get; set; }
        public virtual Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)(Location.X - Origin.X * Scale), (int)(Location.Y - Origin.Y * Scale), (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            }
        }

        public Sprite(Texture2D image, Vector2 location)
            : this(image, location, Vector2.Zero) { }
        public Sprite(Texture2D image, Vector2 location, Vector2 origin)
            : this(image, location, origin, 1) { }
        public Sprite(Texture2D image, Vector2 location, Vector2 origin, float scale)
    : this(image, location, Color.White, 0, SpriteEffects.None, origin, scale, 1) { }
        public Sprite(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Vector2 origin, float scale, float depth)
        : base(location, color, origin, rotation, effects, scale, depth)
        {
            Image = image;
        }

        #region clone

        public new Sprite Clone()
        {
            var copy = new Sprite(Image, Location, Color, rotation, effect, Origin, Scale, Depth);
            CloneLogic(copy);

            return copy;
        }
        protected new void CloneLogic<T>(T copy) where T : Sprite
        {
            base.CloneLogic(copy);
            copy.Image = Image;
        }

        #endregion

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Location + offset, null, Color, rotation, Origin, Scale, effect, Depth);
        }
    }
}
