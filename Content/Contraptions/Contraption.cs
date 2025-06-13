using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Filters = Terraria.Graphics.Effects.Filters;

namespace Mechanica.Content.Contraptions
{
    class Contraption : ModNPC
    {
        Vector2 mStartPos;
		Vector2 mDefaultStep = new Vector2(0f, .2f);

        RenderTarget2D mTileTexture;

        const int cTileWidth = 10;
        const int cTileHeight = 10;
        const int cTileSize = 16;

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1; // make sure to set this for your modnpcs.
        }

        public override void SetDefaults()
        {
            NPC.width = cTileWidth * cTileSize;
            NPC.height = cTileHeight * cTileSize;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
            NPC.lifeMax = 999;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
			mStartPos = NPC.position;

            // Determine Capture Region

			Vector2 start_pos = mStartPos;
			int capture_width = NPC.width;
            int capture_height = NPC.height;

          //  start_pos.X += capture_width / 2;
            start_pos.Y += capture_height / 2;

			mStartPos = start_pos;
            NPC.position = start_pos;

			Point start_pos_tile = start_pos.ToTileCoordinates();

			// Init Render targets and capture settings
			RenderTarget2D render_target = new RenderTarget2D(Main.instance.GraphicsDevice, capture_width, capture_height);
			RenderTarget2D screen_target1 = new RenderTarget2D(Main.instance.GraphicsDevice, capture_width, capture_height);
			RenderTarget2D screen_target2 = new RenderTarget2D(Main.instance.GraphicsDevice, capture_width, capture_height);
			CaptureSettings settings = new CaptureSettings();
			settings.CaptureBackground = false;
			settings.CaptureEntities = false;
			settings.CaptureMech = false;

			Rectangle capture_area = new Rectangle(start_pos_tile.X, start_pos_tile.Y, cTileWidth, cTileHeight);

            // Render region to texture
			Main.instance.TilesRenderer.PrepareForAreaDrawing(capture_area.Left, capture_area.Right, capture_area.Top, capture_area.Bottom, false);
			Main.instance.TilePaintSystem.PrepareAllRequests();
			Filters.Scene.BeginCapture(screen_target1, Color.Transparent);
			Main.instance.DrawCapture(capture_area, settings);
			Filters.Scene.EndCapture(render_target, screen_target1, screen_target2, Color.Transparent);

			mTileTexture = render_target;
		}

        public override void AI()
        {
            Vector2 current_delta = NPC.position - mStartPos;

            if (Math.Abs(current_delta.Length()) > 120f)
            {
				mDefaultStep *= -1;
            }

            NPC.position += mDefaultStep;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, NPC.width, NPC.height);

            Vector2 draw_pos = NPC.Center - Main.screenPosition;
            Vector2 centre = new Vector2(mTileTexture.Width / 2, mTileTexture.Height / 2);

           // drawColor = Color.Red;

			spriteBatch.Draw(mTileTexture, draw_pos, null, drawColor, NPC.rotation, centre, 1, SpriteEffects.None, 0);
		}

		public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            /*
            // We can use ModifyCollisionData to customize collision damage.
            // Here we double damage when this npc is in the falling state and the victim is almost directly below the npc
            if (AI_State == (float)ActionState.Fall)
            {
                // We can modify npcHitbox directly to implement a dynamic hitbox, but in this example we make a new hitbox to apply bonus damage
                // This math creates a hitbox focused on the bottom center of the original 36x36 hitbox:
                // --> ☐☐☐
                //     ☐☒☐
                Rectangle extraDamageHitbox = new Rectangle(npcHitbox.X + 12, npcHitbox.Y + 18, npcHitbox.Width - 24, npcHitbox.Height - 18);
                if (victimHitbox.Intersects(extraDamageHitbox))
                {
                    damageMultiplier *= 2f;
                }
            }
            */
            return true;
        }
    }
}
