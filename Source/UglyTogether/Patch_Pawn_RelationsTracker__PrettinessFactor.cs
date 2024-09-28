using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace UglyTogether
{
    [HarmonyPatch(typeof(Pawn_RelationsTracker))]
    [HarmonyPatch("PrettinessFactor", new[] { typeof(Pawn) })]
    public static class Patch_Pawn_RelationsTracker__PrettinessFactor
    {
        internal static FieldInfo _pawn;

        public static void Postfix(ref Pawn_RelationsTracker __instance, ref float __result, ref Pawn otherPawn)
        {
            if (__result < 1f)
            {
                Pawn pawn = __instance.GetPawn();

                if (pawn == null || !pawn.RaceProps.Humanlike)
                {
                    return;
                }

                float num = pawn.GetStatValue(StatDefOf.PawnBeauty, true, -1);
                if (num < 0f)
                {
                    __result = 1f;
                }
            }
        }
        private static Pawn GetPawn(this Pawn_RelationsTracker _this)
        {
            var flag = _pawn == null;
            if (!flag)
            {
                return (Pawn)_pawn.GetValue(_this);
            }

            _pawn = typeof(Pawn_RelationsTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic);
            var flag2 = _pawn == null;
            if (flag2)
            {
                Log.ErrorOnce("Unable to reflect Pawn_RelationsTracker.pawn", 1874595483);
            }

            return (Pawn)_pawn?.GetValue(_this);
        }
    }
}
