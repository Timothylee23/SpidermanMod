using BaseLib.Abstracts;
using SpidermanMod.SpidermanModCode.Extensions;
using Godot;

namespace SpidermanMod.SpidermanModCode.Character;

public class SpidermanModPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => SpidermanMod.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}