using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.Features;
using Exiled.Events.Handlers;

namespace Mask096
{
	// Token: 0x02000003 RID: 3
	public class MainPlugin : Plugin<Config>
	{
		public override string Name => "SCP096Mask";
		public override string Author => "Liginda & NEGRajdanin";
		public override Version Version => new Version(1, 0, 0);
		public override Version RequiredExiledVersion => new Version(8, 9, 6);
		// Token: 0x0600000A RID: 10 RVA: 0x000020BC File Offset: 0x000002BC
		public override void OnEnabled()
		{
			MainPlugin.Instance = this;
			this.eventHandlers = new EventHandlers();
			Exiled.Events.Handlers.Player.UsingItemCompleted += new CustomEventHandler<UsingItemCompletedEventArgs>(this.eventHandlers.OnUsingItemCompleted);
			Exiled.Events.Handlers.Player.Hurt += new CustomEventHandler<HurtEventArgs>(this.eventHandlers.OnHurt);
			Exiled.Events.Handlers.Player.Spawned += new CustomEventHandler<SpawnedEventArgs>(this.eventHandlers.Spawned);
			Scp096.Enraging += new CustomEventHandler<EnragingEventArgs>(this.eventHandlers.OnEnragind);
			Scp096.AddingTarget += new CustomEventHandler<AddingTargetEventArgs>(this.eventHandlers.AddingTarget);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002180 File Offset: 0x00000380
		public override void OnDisabled()
		{
			Exiled.Events.Handlers.Player.UsingItemCompleted -= new CustomEventHandler<UsingItemCompletedEventArgs>(this.eventHandlers.OnUsingItemCompleted);
			Exiled.Events.Handlers.Player.Hurt -= new CustomEventHandler<HurtEventArgs>(this.eventHandlers.OnHurt);
			Exiled.Events.Handlers.Player.Spawned -= new CustomEventHandler<SpawnedEventArgs>(this.eventHandlers.Spawned);
			Scp096.Enraging -= new CustomEventHandler<EnragingEventArgs>(this.eventHandlers.OnEnragind);
			Scp096.AddingTarget -= new CustomEventHandler<AddingTargetEventArgs>(this.eventHandlers.AddingTarget);
			this.eventHandlers = null;
			MainPlugin.Instance = null;
		}

		// Token: 0x04000005 RID: 5
		public static MainPlugin Instance;

		// Token: 0x04000006 RID: 6
		public EventHandlers eventHandlers;
	}
}
