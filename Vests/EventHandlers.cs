// <copyright file="eventHandlers.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Vests
{
    using System.Collections.Generic;
    using AdvancedHints;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using InventorySystem.Items.Armor;
    using MEC;

    public class EventHandlers : Config
    {
        private readonly Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        private readonly Dictionary<ushort, float> bodyArmors = new Dictionary<ushort, float>();

        public void OnRoundStarted()
        {
            Timing.RunCoroutine(UpdateBodyArmors());
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines();
            bodyArmors.Clear();
        }

        public IEnumerator<float> UpdateBodyArmors()
        {
            while (true)
            {
                foreach (Player player in Player.List)
                {
                    if (player.Inventory.TryGetBodyArmorAndItsSerial(out BodyArmor bodyArmor, out ushort serial))
                    {
                        if (!this.bodyArmors.ContainsKey(serial))
                        {
                            switch (bodyArmor.ItemTypeId)
                            {
                                case ItemType.ArmorLight:
                                    bodyArmors[serial] = plugin.Config.LightArmorValue;
                                    break;
                                case ItemType.ArmorCombat:
                                    bodyArmors[serial] = plugin.Config.CombatArmorValue;
                                    break;
                                case ItemType.ArmorHeavy:
                                    bodyArmors[serial] = plugin.Config.HeavyArmorValue;
                                    break;
                            }
                        }
                        player.ArtificialHealth = bodyArmors[serial];
                    }
                }
                yield return Timing.WaitForOneFrame;
            }
        }

        public void OnItemAdded(ItemAddedEventArgs ev)
        {
            if (bodyArmors.TryGetValue(ev.Item.Serial, out float armorValue))
            {
                ev.Player.ArtificialHealth = armorValue;

                if (armorValue <= 0)
                {
                    ev.Player.ShowManagedHint("Бронежилет нуждается в починке.");
                }
            }
        }

        public void OnItemDropping(DroppingItemEventArgs ev)
        {
            if (bodyArmors.ContainsKey(ev.Item.Serial))
            {
                ev.Player.ArtificialHealth = 0;
            }
        }

        public void OnUsingItem(UsingItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Adrenaline)
            {
                if (ev.Player.Inventory.TryGetBodyArmorAndItsSerial(out _, out ushort serial) && bodyArmors.TryGetValue(serial, out float armorValue) && armorValue <= 0)
                {
                    ev.Player.ShowManagedHint("Снимите бронежилет.");
                    ev.IsAllowed = false;
                }
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Player.ArtificialHealth > 0)
            {
                if (ev.Player.Inventory.TryGetBodyArmorAndItsSerial(out _, out ushort serial) && bodyArmors.TryGetValue(serial, out float armorValue))
                {
                    bodyArmors[serial] -= ev.Amount;

                    if (bodyArmors[serial] < 0)
                    {
                        bodyArmors[serial] = 0;
                        ev.Player.ShowManagedHint("Бронежилет нуждается в починке.");
                    }
                }
            }
        }
    }
}
