using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Networking;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game.Crewmate;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using Reactor.Utilities.Extensions;

namespace TownOfUs.Buttons.Modifiers;

public sealed class IllegalGunKillButton : TownOfUsTargetButton<PlayerControl>, IKillButton
{
    public override string Name => TranslationController.Instance.GetStringWithDefault(StringNames.KillLabel, "Kill");
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfUsColors.Crewmate;
    public override float Cooldown => Math.Clamp(30f + MapCooldown, 0.5f, 120f);
    public override LoadableAsset<Sprite> Sprite => TouAssets.KillSprite;

    public bool Usable { get; set; } = true;

    public override bool Enabled(RoleBehaviour? role)
    {
        return PlayerControl.LocalPlayer != null &&
               PlayerControl.LocalPlayer.HasModifier<IllegalGunOwnerModifier>() &&
               !PlayerControl.LocalPlayer.Data.IsDead;
    }

    public override bool CanUse()
    {
        return base.CanUse() && Usable && Minigame.Instance == null && !HudManager.Instance.Chat.IsOpenOrOpening;
    }

    public override void SetOutline(bool active)
    {
        if (Target != null && !PlayerControl.LocalPlayer.HasDied())
        {
            Target.cosmetics.currentBodySprite.BodySprite.SetOutline(active ? TownOfUsColors.Crewmate : null);
        }
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            Error("Illegal Gun Kill: Target is null");
            return;
        }

        if (Target.HasModifier<FirstDeadShield>())
        {
            return;
        }

        if (Target.HasModifier<BaseShieldModifier>())
        {
            return;
        }

        PlayerControl.LocalPlayer.RpcCustomMurder(Target);
    }

    public override PlayerControl? GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false, x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }
}
