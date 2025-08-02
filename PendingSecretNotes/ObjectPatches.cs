using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace PendingSecretNotes
{
    internal class ObjectPatches
    {
        // initialized by ModEntry.cs
        public static ModEntry ModInstance;
        public static ModConfig Config;

        public static void CollectionsPage_Constructor_Postfix(int x, int y, int width, int height, StardewValley.Menus.CollectionsPage __instance)
        {
            if (!__instance.collections.ContainsKey(StardewValley.Menus.CollectionsPage.secretNotesTab))
            {
                ModInstance.Monitor.Log("[Pending Secret Notes] Secret Notes collection not yet unlocked", StardewModdingAPI.LogLevel.Trace);
                return;
            }

            var texturePending = Game1.content.Load<Texture2D>($"Mods\\{ModEntry.ModId}\\PendingNoteOrScrap");;
            var srPendingSecretNote = Game1.getSourceRectForStandardTileSheet(texturePending, 0, 16, 16);
            var srPendingJournalScrap = Game1.getSourceRectForStandardTileSheet(texturePending, 1, 16, 16);

            foreach (var ctcSecretNote in __instance.collections[StardewValley.Menus.CollectionsPage.secretNotesTab][0])
            {
                ModInstance.Monitor.Log($"[Pending Secret Notes] Checking {ctcSecretNote.name}", StardewModdingAPI.LogLevel.Trace);

                // name = e.g. "1 True", "2 False"
                var noteData = ctcSecretNote.name.Split(" ");
                var noteKey = int.Parse(noteData[0]);
                var noteSeen = bool.Parse(noteData[1]);
                if (noteSeen && IsNotePending(noteKey))
                {
                    var isJournalScrap = (noteKey >= StardewValley.GameLocation.JOURNAL_INDEX);
                    var noteDescription = isJournalScrap ? $"Journal Scrap #{noteKey - 1000}" : $"Secret Note #{noteKey}";
                    ModInstance.Monitor.Log($"[Pending Secret Notes] Highlighting {noteDescription}", StardewModdingAPI.LogLevel.Debug);
                    ctcSecretNote.texture = texturePending;
                    ctcSecretNote.sourceRect = isJournalScrap ? srPendingJournalScrap : srPendingSecretNote;
                }
            }
        }

        private static bool IsNotePending(int noteKey)
        {
            switch (noteKey)
            {
                // Secret Notes: 1 to 27

                case 10:
                    return !Game1.player.mailReceived.Contains("qiCave");
                case 13:
                    return !Game1.player.mailReceived.Contains("junimoPlush");
                case 14:
                    return !Game1.RequireLocation("Town").CanItemBePlacedHere(new Vector2(57f, 16f));
                case 15:
                    return !Game1.player.mailReceived.Contains("gotPearl");
                case 16:
                    return !Game1.player.mailReceived.Contains("SecretNote16_done");
                case 17:
                    return !Game1.player.mailReceived.Contains("SecretNote17_done");
                case 18:
                    return !Game1.player.mailReceived.Contains("SecretNote18_done");
                case 19:
                    return !Game1.player.mailReceived.Contains("SecretNote19_done");
                case 20:
                    return !Game1.player.mailReceived.Contains("SecretNote20_done");
                case 21:
                    return !Game1.player.mailReceived.Contains("SecretNote21_done");
                case 22:
                    return !Game1.player.hasOrWillReceiveMail("TH_Tunnel");
                case 23:
                    return !Game1.player.eventsSeen.Contains("2120303");
                case 25:
                    return Game1.player.hasOrWillReceiveMail(GameLocation.CAROLINES_NECKLACE_MAIL);
                case 27:
                    return !Game1.player.locationsVisited.Contains("MasteryCave");

                // Journal Scraps: 1001 to 1011

                case 1004:
                    return !Game1.player.hasOrWillReceiveMail("Island_W_BuriedTreasure");
                case 1006:
                    return !Game1.player.hasOrWillReceiveMail("Island_W_BuriedTreasure2");
                case 1010:
                    return !Game1.player.hasOrWillReceiveMail("Island_N_BuriedTreasure");

                // Possible future improvement: add support for mods using Secret Note Framework

                default:
                    return false;
            }
        }
    }
}
