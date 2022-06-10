using Verse;
using HarmonyLib;
using System.Reflection;
using System;

namespace MSE.HSKFix
{
    [StaticConstructorOnStartup]
    [HarmonyPatch]
    public static class Start
    {
        static bool Prepare() => LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Equals("skyarkhangel.HSK", StringComparison.CurrentCultureIgnoreCase));

        static MethodBase TargetMethod() => AccessTools.Method("SK.StargateUtilities:GenerateHediffs");

        static Start()
        {
            new Harmony("pirateby.mse2.hskfix").PatchAll();
        }

        [HarmonyPrefix]
        static bool GenerateHediffs(Pawn pawn, Pawn sourcePawn)
		{
			foreach (Hediff hediff in sourcePawn.health.hediffSet.hediffs)
			{
				hediff.ageTicks = 0;
				hediff.pawn = pawn;
				pawn.health.hediffSet.hediffs.Add(hediff);
				pawn.health.hediffSet.DirtyCache();
                // hsk - fail on missed parts
				// pawn.health.AddHediff(hediff, null, null, null);
			}
			return false;
		}
    }

}
