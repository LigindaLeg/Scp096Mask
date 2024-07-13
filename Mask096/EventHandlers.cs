using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace Mask096
{
	public class EventHandlers
	{
		public void OnUsingItemCompleted(UsingItemCompletedEventArgs ev)
		{
			bool flag = this.Serials.Contains(ev.Item.Serial);
			if (flag)
			{
				ev.IsAllowed = false;
				IEnumerable<Player> enumerable = Enumerable.Where<Player>(Player.List, (Player p) => p.Role is Scp096Role && Vector3.Distance(p.Position, ev.Player.Position) < 5f && !this.MaskEquipped.Contains(p));
				bool flag2 = Enumerable.Count<Player>(enumerable) != 0;
				if (flag2)
				{
					Player player = Enumerable.First<Player>(enumerable);
					player.EnableEffect(Exiled.API.Enums.EffectType.Ensnared, 999f, true);
					ev.Player.EnableEffect(Exiled.API.Enums.EffectType.Ensnared, 999f, true);
					ev.Usable.Destroy();
					this.Serials.Remove(ev.Usable.Serial);
					ev.Player.CustomInfo = MainPlugin.Instance.Translation.Scp096Hint;
					Timing.RunCoroutine(this.Scp096Corountine(ev.Player, player));
				}
			}
		}

		public void OnHurt(HurtEventArgs ev)
		{
			bool flag = MainPlugin.Instance.Config.IsMaskOffByDamage && this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				this.MaskEquipped.Remove(ev.Player);
				ev.Player.CustomInfo = "";
			}
		}

		public void AddingTarget(AddingTargetEventArgs ev)
		{
			bool flag = this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnEnragind(EnragingEventArgs ev)
		{
			bool flag = this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				ev.IsAllowed = false;
			}
		}
		public void Died(DiedEventArgs ev)
		{
			bool flag = this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				this.MaskEquipped.Remove(ev.Player);
			}
		}

		public void Spawned(SpawnedEventArgs ev)
		{
			bool flag = ev.Player.Role.Type == RoleTypeId.NtfCaptain;
			if (flag)
			{
				Item item = ev.Player.AddItem(ItemType.SCP268);
				this.Serials.Add(item.Serial);
				ev.Player.ShowHint(MainPlugin.Instance.Translation.PlayerSpawnHint);
			}
		}

		public IEnumerator<float> Scp096Corountine(Player cuffer, Player scp096)
		{
			int num;
			for (int i = MainPlugin.Instance.Config.SecondsToUse; i >= 0; i = num - 1)
			{
				bool flag = i == 0;
				if (flag)
				{
					this.MaskEquipped.Add(scp096);
					scp096.Role.As<Scp096Role>().ClearTargets();
					scp096.DisableEffect(Exiled.API.Enums.EffectType.Ensnared);
					cuffer.DisableEffect(Exiled.API.Enums.EffectType.Ensnared);
					yield break;
				}
				bool flag2 = i > 0;
				if (flag2)
				{
					cuffer.ShowHint(string.Format(MainPlugin.Instance.Translation.PlayerMaskHint, i), 1f);
					scp096.ShowHint(string.Format(MainPlugin.Instance.Translation.Scp096MaskHint, i), 1f);
					
				}
				yield return Timing.WaitForSeconds(1f);
				num = i;
			}
			yield break;
		}

		public List<Player> MaskEquipped = new List<Player>();

		public List<ushort> Serials = new List<ushort>();
	}
}
