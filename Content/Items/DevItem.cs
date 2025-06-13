using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Mechanica.Content.Contraptions;
using System;
using Microsoft.Xna.Framework;

namespace Mechanica.Content.Items
{
    public class DevItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true; 
            Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
		}

        public override void AddRecipes()
        {
            CreateRecipe(999)
                .AddIngredient(ItemID.None, 0)
                .Register();
        }

		public override bool? UseItem(Player player)
		{
            // Get cursor pos aligned to the tile grid
            Vector2 mouse_pos = Main.MouseWorld;
            Point mouse_pos_tile = mouse_pos.ToTileCoordinates();
            mouse_pos = mouse_pos_tile.ToWorldCoordinates();

            mouse_pos.X -= 8;
            mouse_pos.Y -= 8;

			NPC.NewNPC(NPC.GetSource_None(), Convert.ToInt32(mouse_pos.X), Convert.ToInt32(mouse_pos.Y), ModContent.NPCType<Contraption>());
			return true;
		}

		public override void UseAnimation(Player player)
		{
           // NPC.NewNPC(NPC.GetSource_None(), Convert.ToInt32(Main.MouseWorld.X), Convert.ToInt32(Main.MouseWorld.Y), ModContent.NPCType<Contraption>());
		}
	}
}