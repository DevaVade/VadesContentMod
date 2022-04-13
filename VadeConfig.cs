using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace VadesContentMod
{
    [Label("Config")]
    [BackgroundColor(49, 32, 36, 216)]
    public class VadeConfig : ModConfig
    {
        public static VadeConfig Instance;
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Enable Discord Rich Presence (Requires DRP mod)")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        [ReloadRequired()]
        [Tooltip("Turn on or off the Discord Rich Presence mod by Purplefin Neptuna. Requires the mod")]
        public bool DiscordRichPresence { get; set; }

        [Header("Config Access")]
        [Label("Enable Config Access to server host only")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [Tooltip("If enabled, only the server owner can make changes to Infinite Realities' config")]
        public bool OwnerOnly { get; set; }

        [Label("Enable Hero's Mod Config changes")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [Tooltip("If enabled, only the server owner can make changes to Hero's mod config")]
        public bool HerosPerm { get; set; }

        /// <summary>
        /// Checks to see if the player is the current server host. Thanks Jopojelly.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsPlayerLocalServerOwner(Player player)
        {
            if (Main.netMode == 1)
            {
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }

            for (int plr = 0; plr < Main.maxPlayers; plr++)
                if (Netplay.Clients[plr].State == 10 && Main.player[plr] == player && Netplay.Clients[plr].Socket.GetRemoteAddress().IsLocalHost())
                    return true;
            return false;
        }

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            string AcceptMessage = "Changes succesfully accepted";

            if (OwnerOnly && IsPlayerLocalServerOwner(Main.player[whoAmI]))
            {
                message = AcceptMessage;
                return true;
            }
            else if (OwnerOnly && VadesContentMod.instance.HerosMod == null)
            {
                message = "Host only changes config is turned on";
                return false;
            }

            if (HerosPerm && VadesContentMod.instance.HerosMod != null)
            {
                if (VadesContentMod.instance.HerosMod.Call("HasPermission", whoAmI, VadesContentMod.HeroPerm) is bool result && result)
                {
                    message = AcceptMessage;
                    return true;
                }
                message = "You don't have the necessary permissions to make changes";
                return false;
            }

            message = AcceptMessage;
            return true;
        }
    }
}