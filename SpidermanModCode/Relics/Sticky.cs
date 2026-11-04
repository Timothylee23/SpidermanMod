using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace SpidermanMod.SpidermanModCode.Relics;

/// <summary>
/// Just before your hand is discarded at the end of your turn, lets you pick one card to keep, or skip.
/// </summary>
public class Sticky : SpidermanModRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override async Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner)
        {
            return;
        }

        // Don't ask if your whole hand is being kept anyway (for example, by Runic Pyramid).
        var combatState = player.Creature.CombatState;
        if (combatState == null || !Hook.ShouldFlush(combatState, player))
        {
            return;
        }

        // Only offer cards that would otherwise be discarded. If there are none, there's nothing to ask.
        Func<CardModel, bool> wouldBeDiscarded = card => !card.ShouldRetainThisTurn;
        if (!PileType.Hand.GetPile(player).Cards.Any(wouldBeDiscarded))
        {
            return;
        }

        var combat = CombatManager.Instance;
        var wasReadyToEndTurn = combat.IsPlayerReadyToEndTurn(player);

        // Picking 0 or 1 cards is what gives the player the option to skip.
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, 1);
        var chosen = await CardSelectCmd.FromHand(choiceContext, player, prefs, wouldBeDiscarded, this);

        foreach (var card in chosen)
        {
            Flash();
            CardCmd.ApplySingleTurnRetain(card);
        }

        // Asking for a card marks the turn as "not ended" and re-enables your controls.
        // The turn really is ending, so put that back or you could click cards during the enemy's turn.
        if (wasReadyToEndTurn)
        {
            combat.SetReadyToEndTurn(player, false);
            if (LocalContext.IsMe(player))
            {
                combat.OnEndedTurnLocally();
            }
        }
    }
}
