using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace PendingSecretNotes
{
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod configuration from the player.</summary>
        private ModConfig Config;

        /// <summary>The mod ID used by the Content Patcher component.</summary>
        public const string ModId = "JohnPeters.PendingSecretNotesCP";

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            Helper.Events.GameLoop.GameLaunched += (e, a) => OnGameLaunched(e, a);

            ObjectPatches.ModInstance = this;
            ObjectPatches.Config = this.Config;

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
                original: typeof(StardewValley.Menus.CollectionsPage).GetConstructor(new[] { typeof(int), typeof(int) , typeof(int) , typeof(int) }),
                postfix: new HarmonyMethod(typeof(ObjectPatches), nameof(ObjectPatches.CollectionsPage_Constructor_Postfix))
            );
        }

        /// <summary>Add to Generic Mod Config Menu</summary>
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // placeholder for potential future use
        }
    }
}