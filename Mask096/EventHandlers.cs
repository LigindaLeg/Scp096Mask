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
	// Token: 0x02000004 RID: 4
	public class EventHandlers
	{
		// Token: 0x0600000D RID: 13 RVA: 0x0000224C File Offset: 0x0000044C
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
					ev.Player.CustomInfo = "Надета маска";
					Timing.RunCoroutine(this.Scp096Corountine(ev.Player, player));
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000233C File Offset: 0x0000053C
		public void OnHurt(HurtEventArgs ev)
		{
			bool flag = MainPlugin.Instance.Config.IsMaskOffByDamage && this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				this.MaskEquipped.Remove(ev.Player);
				ev.Player.CustomInfo = "";
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002388 File Offset: 0x00000588
		public void AddingTarget(AddingTargetEventArgs ev)
		{
			bool flag = this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				ev.IsAllowed = false;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023B8 File Offset: 0x000005B8
		public void OnEnragind(EnragingEventArgs ev)
		{
			bool flag = this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				ev.IsAllowed = false;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023E8 File Offset: 0x000005E8
		public void Died(DiedEventArgs ev)
		{
			bool flag = this.MaskEquipped.Contains(ev.Player);
			if (flag)
			{
				this.MaskEquipped.Remove(ev.Player);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002420 File Offset: 0x00000620
		public void Spawned(SpawnedEventArgs ev)
		{
			bool flag = ev.Player.Role.Type == RoleTypeId.NtfCaptain;
			if (flag)
			{
				Item item = ev.Player.AddItem(ItemType.SCP268);
				this.Serials.Add(item.Serial);
				ev.Player.ShowHint("Вам была выдана маска для SCP-096");
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002468 File Offset: 0x00000668
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
					cuffer.ShowHint(string.Format("Вы надеваете маску на SCP-096, осталось <color=yellow> {0} </color> секунд", i), 1f);
					scp096.ShowHint(string.Format("На вас надевают маску, осталось <color=yellow> {0} </color> секунд", i), 1f);
					
				}
				yield return Timing.WaitForSeconds(1f);
				num = i;
			}
			yield break;
		}

		// Token: 0x04000007 RID: 7
		public List<Player> MaskEquipped = new List<Player>();

		// Token: 0x04000008 RID: 8
		public List<ushort> Serials = new List<ushort>();
	}
}
