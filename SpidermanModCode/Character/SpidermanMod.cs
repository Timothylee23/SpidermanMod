using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using SpidermanMod.SpidermanModCode.Cards;
using SpidermanMod.SpidermanModCode.Extensions;
using SpidermanMod.SpidermanModCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace SpidermanMod.SpidermanModCode.Character;

public class SpidermanMod : PlaceholderCharacterModel
{
    public const string CharacterId = "SpidermanMod";
    
    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 99;
    
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<WebShot>(),
        ModelDb.Card<WebShot>(),
        ModelDb.Card<WebShot>(),
        ModelDb.Card<WebShot>(),
        ModelDb.Card<WebShot>(),
        ModelDb.Card<WebBlock>(),
        ModelDb.Card<WebBlock>(),
        ModelDb.Card<WebBlock>(),
        ModelDb.Card<WebBlock>(),
        ModelDb.Card<WebBlock>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Sticky>()
    ];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<SpidermanModCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<SpidermanModRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<SpidermanModPotionPool>();
    
    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    // Uses a still picture for the character in battle instead of the placeholder's animated body.
    public override NCreatureVisuals? CreateCustomVisuals() =>
        NodeFactory<NCreatureVisuals>.CreateFromResource("combat_spiderman.png".CharacterUiPath());

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}