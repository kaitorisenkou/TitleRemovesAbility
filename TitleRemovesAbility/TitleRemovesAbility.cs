using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TitleRemovesAbility {
    [StaticConstructorOnStartup]
    public class TitleRemovesAbility {
        static TitleRemovesAbility() {
            Log.Message("[TitleRemovesAbility] Now active");
            var harmony = new Harmony("kaitorisenkou.BoomerangAbility");
            harmony.Patch(
                AccessTools.Method(typeof(Pawn_RoyaltyTracker), "OnPostTitleChanged", null, null),
                    null,
                    new HarmonyMethod(typeof(TitleRemovesAbility), nameof(Patch_OnPostTitleChanged), null),
                    null,
                    null
                    );
            Log.Message("[TitleRemovesAbility] Harmony patch complete!");

        }
        public static void Patch_OnPostTitleChanged(RoyalTitleDef newTitle, ref Pawn ___pawn) {
            var modEx = newTitle.GetModExtension<ModExtension_TitleRemovesAbility>();
            if (modEx == null || modEx.abilityDefs.NullOrEmpty()) return;
            foreach(var i in modEx.abilityDefs) {
                ___pawn.abilities.RemoveAbility(i);
            }
        }
    }

    public class ModExtension_TitleRemovesAbility : DefModExtension {
        public List<AbilityDef> abilityDefs = null;
    }
}
