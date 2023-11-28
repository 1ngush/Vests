// <copyright file="VestsPlugin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Vests
{
    using Exiled.API.Features;

    public class Plugin : Plugin<Config>
    {
        internal static EventHandlers EventHandlers;
        public static Plugin Instance { get; private set; }

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.ItemAdded += EventHandlers.OnItemAdded;
            Exiled.Events.Handlers.Player.DroppingItem += EventHandlers.OnItemDropping;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurting;
            Exiled.Events.Handlers.Player.UsingItem += EventHandlers.OnUsingItem;
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.ItemAdded -= EventHandlers.OnItemAdded;
            Exiled.Events.Handlers.Player.DroppingItem -= EventHandlers.OnItemDropping;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurting;
            Exiled.Events.Handlers.Player.UsingItem -= EventHandlers.OnUsingItem;

            EventHandlers = null;
            Instance = null;
        }
    }
}