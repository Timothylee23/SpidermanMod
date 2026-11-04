using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using SpidermanCharacter = global::SpidermanMod.SpidermanModCode.Character.SpidermanMod;

namespace SpidermanMod.SpidermanModCode.Patches;

/// <summary>
/// Spider-Man's battle picture has no death animation, so without this he'd keep standing after dying.
/// These patches make him topple over and dim when he dies, and stand back up if he's revived.
/// </summary>
[HarmonyPatch]
public static class StillImageDeathPatch
{
    private const float FallenRotationDegrees = -90f; // He faces right, so this tips him over backward.
    private const float FallenOpacity = 0.6f;
    private const double FallSeconds = 0.45;
    private const double StandUpSeconds = 0.2;

    private static bool IsStillImageSpiderman(NCreature creature) =>
        creature.Entity.Player?.Character is SpidermanCharacter && !creature.HasSpineAnimation;

    [HarmonyPatch(typeof(NCreature), nameof(NCreature.StartDeathAnim))]
    [HarmonyPostfix]
    private static void FallOverOnDeath(NCreature __instance)
    {
        if (!IsStillImageSpiderman(__instance))
        {
            return;
        }

        var tween = __instance.CreateTween().SetParallel();
        tween.TweenProperty(__instance.Visuals, "rotation", Mathf.DegToRad(FallenRotationDegrees), FallSeconds)
            .SetTrans(Tween.TransitionType.Bounce)
            .SetEase(Tween.EaseType.Out);
        tween.TweenProperty(__instance.Visuals, "modulate:a", FallenOpacity, FallSeconds);
    }

    [HarmonyPatch(typeof(NCreature), nameof(NCreature.StartReviveAnim))]
    [HarmonyPostfix]
    private static void StandUpOnRevive(NCreature __instance)
    {
        if (!IsStillImageSpiderman(__instance))
        {
            return;
        }

        // The game fades him back in by itself, so only the rotation needs undoing here.
        __instance.CreateTween().TweenProperty(__instance.Visuals, "rotation", 0f, StandUpSeconds);
    }
}
