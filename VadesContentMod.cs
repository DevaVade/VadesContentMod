using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace VadesContentMod
{
    public class VadesContentMod : Mod
    {
        public static VadesContentMod instance;

        public enum MessageType
        {
            SyncVadePlayer
        }

        public static Effect TrailEffect;
        public static ModKeybind OneShothotKey;
        public Mod HerosMod;
        public const string HeroPerm = "VadesContentMod";
        public const string HeroPermDisplayName = "Infinite Realities";
        public bool Perm;

        public bool ThoriumLoaded;
        public bool CalamityLoaded;
        public bool SoALoaded;
        public bool DLCLoaded;
        public bool FargoLoaded;
        public bool SoulsLoaded;
        public bool CalValLoaded;

        public static string currentDate;
        public static int day;
        public static int month;

        public override void Load()
        {
            instance = this;
            
            OneShothotKey = KeybindLoader.RegisterKeybind(instance, "Destructor Armor One-Shot", "F");

            /*HerosMod = ModLoader.GetMod("HEROsMod");
            if (Main.netMode != NetmodeID.Server)
            {
                TrailEffect = ModContent.Request<Effect>("Effects/TrailShader").Value;

                Ref<Effect> invertRef = new Ref<Effect>(ModContent.Request<Effect>("Effects/Grayscale").Value);
                Ref<Effect> shockwaveRef = new Ref<Effect>(ModContent.Request<Effect>("Effects/Shockwave").Value);

                Filters.Scene["VadesContentMod:Grayscale"] = new Filter(new ScreenShaderData(invertRef, "Main"), EffectPriority.VeryHigh);
                Filters.Scene["VadesContentMod:Shockwave"] = new Filter(new ScreenShaderData(shockwaveRef, "Shockwave"), EffectPriority.VeryHigh);
            }

            ThoriumLoaded = ModLoader.GetMod("ThoriumMod") != null;
            CalamityLoaded = ModLoader.GetMod("CalamityMod") != null;
            DLCLoaded = ModLoader.GetMod("FargowiltasSoulsDLC") != null;
            SoulsLoaded = ModLoader.GetMod("FargowiltasSouls") != null;
            FargoLoaded = ModLoader.GetMod("Fargowiltas") != null;
            SoALoaded = ModLoader.GetMod("SacredTools") != null;
            CalValLoaded = ModLoader.GetMod("CalValEX") != null;*/

            DateTime dateTime = DateTime.Now;
            currentDate = dateTime.ToString("dd/MM/yyyy");
            day = dateTime.Day;
            month = dateTime.Month;

            //MusicLoader.AddMusicBox(VadesContentMod.instance, MusicLoader.GetMusicSlot(VadesContentMod.instance, "pathname"), ModContent.ItemType<musicBoxItem>(), ModContent.TileType<placedMusicVBox>());
        }

        public override void Unload()
        {
            instance = null;
            ThoriumLoaded = false;
            CalamityLoaded = false;
            DLCLoaded = false;
            SoulsLoaded = false;
            FargoLoaded = false;
            SoALoaded = false;
            CalValLoaded = false;
            HerosMod = null;

            OneShothotKey = null;
            TrailEffect = null;

        }

        public override void PostSetupContent()
        {
            /*//Census support
            Mod censusMod = ModLoader.GetMod("Census");
            if (censusMod != null)
            {
                censusMod.Call("TownNPCCondition", ModContent.NPCType<Wahtever fucking NPCs we add>(), "The uh the descroption ecks dee");
                censusMod.Call("TownNPCCondition", ModContent.NPCType<Wahtever fucking NPCs we add>(),
                    "How you find the mfer");
            }*/

            /*if (ModContent.GetInstance<VadeConfig>().DiscordRichPresence)
            {
                try
                {
                    var RichPresence = ModLoader.GetMod("DiscordRP");
                    if (RichPresence != null)
                    {
                        //RichPresence.Call("AddClient", "929973580178010152", "ecks dee");
                        //RichPresence.Call("AddBiome", (Func<bool>)(() => Main.LocalPlayer.GetModPlayer<VadePlayer>().Biome), "biome display name",
                        // "biome internal name", 50f, "uhhh");

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Something went wrong with Discord Rich Presence...", ex);
                }
            }*/
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType messageType = (MessageType)reader.ReadByte();
            byte playerNumber;
            VadePlayer VadePlayer;
            switch (messageType)
            {
                case MessageType.SyncVadePlayer:
                    playerNumber = reader.ReadByte();
                    VadePlayer = Main.player[playerNumber].GetModPlayer<VadePlayer>();
                    break;

                default:
                    Logger.WarnFormat("Infinite Realities: Unknown Message Type: {0}", messageType);
                    break;
            }
        }

        public override void AddRecipeGroups()
        {
            /*RecipeGroup dirt = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Dirt"]];
            dirt.ValidItems.Add(ModContent.ItemType<Whatever unique dirt we get>());*/

            if (RecipeGroup.recipeGroupIDs.ContainsKey("WingsGroup"))
            {
                int index = RecipeGroup.recipeGroupIDs["WingsGroup"];
                RecipeGroup groupe = RecipeGroup.recipeGroups[index];
                //groupe.ValidItems.Add(ModContent.ItemType<Any fcking wings we add!!!>());
            }

            //Æ: Example from CalVal's digger recipe
            RecipeGroup group = new RecipeGroup(() => "Any Hardmode Drill", new int[]
            {
                ItemID.CobaltDrill,
                ItemID.PalladiumDrill,
                ItemID.MythrilDrill,
                ItemID.OrichalcumDrill,
                ItemID.AdamantiteDrill,
                ItemID.TitaniumDrill,
            });
            RecipeGroup.RegisterGroup("AnyHardmodeDrill", group);
        }
        public override void AddRecipes()
        {
        }
        
        public void SetupHerosMod()
        {
            if (HerosMod != null)
            {
                HerosMod.Call("AddPermission", HeroPerm, HeroPermDisplayName);
            }
        }

        public override void PostAddRecipes()
        {
            SetupHerosMod();
        }

        public bool getPermission()
        {
            return Perm;
        }
    }
}