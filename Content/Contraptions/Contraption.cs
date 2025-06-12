using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Mechanica.Content.Contraptions
{
    class Contraption : ModNPC
    {
        Vector2 mStartPos;
		Vector2 mDefaultStep = new Vector2(1f, 0f);

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1; // make sure to set this for your modnpcs.
        }

        public override void SetDefaults()
        {
            NPC.width = 2;
            NPC.height = 2;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
            NPC.lifeMax = 999;
            NPC.noGravity = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            mStartPos = NPC.position;
        }

        public override void AI()
        {
            Vector2 current_delta = NPC.position - mStartPos;

            if (Math.Abs(current_delta.Length()) > 80f)
            {
				mDefaultStep *= -1;
            }

            NPC.position += mDefaultStep;
        }

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            int width = 20;
            int height = 20;
            Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, width, height);

			Color[] colorData = new Color[width * height];
			for (int i = 0; i < colorData.Length; i++)
				colorData[i] = Color.Cyan;

            texture.SetData(colorData);

		    spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, Color.Cyan);
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
