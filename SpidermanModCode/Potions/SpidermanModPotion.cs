using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using SpidermanMod.SpidermanModCode.Character;
using SpidermanMod.SpidermanModCode.Extensions;

namespace SpidermanMod.SpidermanModCode.Potions;

[Pool(typeof(SpidermanModPotionPool))]
public abstract class SpidermanModPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}