using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Hud;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using TownOfUs.Buttons.Modifiers;
using TownOfUs.Interfaces;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class IllegalGunOwnerModifier : BaseModifier, IWikiDiscoverable, IButtonModifier
{
    public override string ModifierName => "Illegal Gun Owner";
    public override bool HideOnUi => true;

    public override void OnActivate()
    {
        CustomButtonSingleton<IllegalGunKillButton>.Instance.Usable = true;
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Illegal Gun", "Kill your target with a hidden pistol.", TouAssets.KillSprite)
            };
        }
    }
}