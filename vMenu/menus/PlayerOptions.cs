using System;
using System.Collections.Generic;
using System.Linq;

using ScaleformUI.Menu;

using vMenuClient.data;

namespace vMenuClient.menus
{
    public class PlayerOptions
    {
        // Menu variable, will be defined in CreateMenu()
        private UIMenu menu;

        // Public variables (getters only), return the private variables.
        public bool PlayerGodMode { get; private set; } = UserDefaults.PlayerGodMode;
        public bool PlayerInvisible { get; private set; } = false;
        public bool PlayerStamina { get; private set; } = UserDefaults.UnlimitedStamina;
        public bool PlayerFastRun { get; private set; } = UserDefaults.FastRun;
        public bool PlayerFastSwim { get; private set; } = UserDefaults.FastSwim;
        public bool PlayerSuperJump { get; private set; } = UserDefaults.SuperJump;
        public bool PlayerNoRagdoll { get; private set; } = UserDefaults.NoRagdoll;
        public bool PlayerNeverWanted { get; private set; } = UserDefaults.NeverWanted;
        public bool PlayerIsIgnored { get; private set; } = UserDefaults.EveryoneIgnorePlayer;
        public bool PlayerStayInVehicle { get; private set; } = UserDefaults.PlayerStayInVehicle;
        public bool PlayerFrozen { get; private set; } = false;
        private readonly UIMenu CustomDrivingStyleMenu = new("Driving Style", "Custom Driving Style");

        /// <summary>
        /// Creates the menu.
        /// </summary>
        private void CreateMenu()
        {
            #region create menu and menu items
            // Create the menu.
            menu = new UIMenu(Game.Player.Name, "Player Options");

            // Create all checkboxes.
            UIMenuCheckboxItem playerGodModeCheckbox = new UIMenuCheckboxItem("Godmode", PlayerGodMode, "Makes you invincible.");
            UIMenuCheckboxItem invisibleCheckbox = new UIMenuCheckboxItem("Invisible", PlayerInvisible, "Makes you invisible to yourself and others.");
            UIMenuCheckboxItem unlimitedStaminaCheckbox = new UIMenuCheckboxItem("Unlimited Stamina", PlayerStamina, "Allows you to run forever without slowing down or taking damage.");
            UIMenuCheckboxItem fastRunCheckbox = new UIMenuCheckboxItem("Fast Run", PlayerFastRun, "Get ~g~Snail~s~ powers and run very fast!");
            SetRunSprintMultiplierForPlayer(Game.Player.Handle, PlayerFastRun && IsAllowed(Permission.POFastRun) ? 1.49f : 1f);
            UIMenuCheckboxItem fastSwimCheckbox = new UIMenuCheckboxItem("Fast Swim", PlayerFastSwim, "Get ~g~Snail 2.0~s~ powers and swim super fast!");
            SetSwimMultiplierForPlayer(Game.Player.Handle, PlayerFastSwim && IsAllowed(Permission.POFastSwim) ? 1.49f : 1f);
            UIMenuCheckboxItem superJumpCheckbox = new UIMenuCheckboxItem("Super Jump", PlayerSuperJump, "Get ~g~Snail 3.0~s~ powers and jump like a champ!");
            UIMenuCheckboxItem noRagdollCheckbox = new UIMenuCheckboxItem("No Ragdoll", PlayerNoRagdoll, "Disables player ragdoll, makes you not fall off your bike anymore.");
            UIMenuCheckboxItem neverWantedCheckbox = new UIMenuCheckboxItem("Never Wanted", PlayerNeverWanted, "Disables all wanted levels.");
            UIMenuCheckboxItem everyoneIgnoresPlayerCheckbox = new UIMenuCheckboxItem("Everyone Ignore Player", PlayerIsIgnored, "Everyone will leave you alone.");
            UIMenuCheckboxItem playerStayInVehicleCheckbox = new UIMenuCheckboxItem("Stay In Vehicle", PlayerStayInVehicle, "When this is enabled, NPCs will not be able to drag you out of your vehicle if they get angry at you.");
            UIMenuCheckboxItem playerFrozenCheckbox = new UIMenuCheckboxItem("Freeze Player", PlayerFrozen, "Freezes your current location.");

            // Wanted level options
            List<dynamic> wantedLevelList = new List<dynamic> { "No Wanted Level", "1", "2", "3", "4", "5" };
            UIMenuListItem setWantedLevel = new UIMenuListItem("Set Wanted Level", wantedLevelList, GetPlayerWantedLevel(Game.Player.Handle), "Set your wanted level by selecting a value, and pressing enter.");
            UIMenuListItem setArmorItem = new UIMenuListItem("Set Armor Type", new List<dynamic> { "No Armor", GetLabelText("WT_BA_0"), GetLabelText("WT_BA_1"), GetLabelText("WT_BA_2"), GetLabelText("WT_BA_3"), GetLabelText("WT_BA_4"), }, 0, "Set the armor level/type for your player.");

            UIMenuItem healPlayerBtn = new UIMenuItem("Heal Player", "Give the player max health.");
            UIMenuItem cleanPlayerBtn = new UIMenuItem("Clean Player Clothes", "Clean your player clothes.");
            UIMenuItem dryPlayerBtn = new UIMenuItem("Dry Player Clothes", "Make your player clothes dry.");
            UIMenuItem wetPlayerBtn = new UIMenuItem("Wet Player Clothes", "Make your player clothes wet.");
            UIMenuItem suicidePlayerBtn = new UIMenuItem("~r~Commit Suicide", "Kill yourself by taking the pill. Or by using a pistol if you have one.");

            UIMenu vehicleAutoPilot = new UIMenu("Auto Pilot", "Vehicle auto pilot options.");

            UIMenuItem vehicleAutoPilotBtn = new UIMenuItem("Vehicle Auto Pilot Menu", "Manage vehicle auto pilot options.");
            vehicleAutoPilotBtn.SetRightLabel("→→→");

            List<dynamic> drivingStyles = new List<dynamic>() { "Normal", "Rushed", "Avoid highways", "Drive in reverse", "Custom" };
            UIMenuListItem drivingStyle = new UIMenuListItem("Driving Style", drivingStyles, 0, "Set the driving style that is used for the Drive to Waypoint and Drive Around Randomly functions.");

            // Scenarios (list can be found in the PedScenarios class)
            UIMenuListItem playerScenarios = new UIMenuListItem("Player Scenarios", PedScenarios.Scenarios.Cast<dynamic>().ToList(), 0, "Select a scenario and hit enter to start it. Selecting another scenario will override the current scenario. If you're already playing the selected scenario, selecting it again will stop the scenario.");
            UIMenuItem stopScenario = new UIMenuItem("Force Stop Scenario", "This will force a playing scenario to stop immediately, without waiting for it to finish it's 'stopping' animation.");
            #endregion

            #region add items to menu based on permissions
            // Add all checkboxes to the menu. (keeping permissions in mind)
            if (IsAllowed(Permission.POGod))
            {
                menu.AddItem(playerGodModeCheckbox);
            }
            if (IsAllowed(Permission.POInvisible))
            {
                menu.AddItem(invisibleCheckbox);
            }
            if (IsAllowed(Permission.POUnlimitedStamina))
            {
                menu.AddItem(unlimitedStaminaCheckbox);
            }
            if (IsAllowed(Permission.POFastRun))
            {
                menu.AddItem(fastRunCheckbox);
            }
            if (IsAllowed(Permission.POFastSwim))
            {
                menu.AddItem(fastSwimCheckbox);
            }
            if (IsAllowed(Permission.POSuperjump))
            {
                menu.AddItem(superJumpCheckbox);
            }
            if (IsAllowed(Permission.PONoRagdoll))
            {
                menu.AddItem(noRagdollCheckbox);
            }
            if (IsAllowed(Permission.PONeverWanted))
            {
                menu.AddItem(neverWantedCheckbox);
            }
            if (IsAllowed(Permission.POSetWanted))
            {
                menu.AddItem(setWantedLevel);
            }
            if (IsAllowed(Permission.POIgnored))
            {
                menu.AddItem(everyoneIgnoresPlayerCheckbox);
            }
            if (IsAllowed(Permission.POStayInVehicle))
            {
                menu.AddItem(playerStayInVehicleCheckbox);
            }
            if (IsAllowed(Permission.POMaxHealth))
            {
                menu.AddItem(healPlayerBtn);
            }
            if (IsAllowed(Permission.POMaxArmor))
            {
                menu.AddItem(setArmorItem);
            }
            if (IsAllowed(Permission.POCleanPlayer))
            {
                menu.AddItem(cleanPlayerBtn);
            }
            if (IsAllowed(Permission.PODryPlayer))
            {
                menu.AddItem(dryPlayerBtn);
            }
            if (IsAllowed(Permission.POWetPlayer))
            {
                menu.AddItem(wetPlayerBtn);
            }

            menu.AddItem(suicidePlayerBtn);

            if (IsAllowed(Permission.POVehicleAutoPilotMenu))
            {
                menu.AddItem(vehicleAutoPilotBtn);
                vehicleAutoPilotBtn.Activated += async (a, b) => await a.SwitchTo(vehicleAutoPilot, 0, true);

                vehicleAutoPilot.AddItem(drivingStyle);

                UIMenuItem startDrivingWaypoint = new UIMenuItem("Drive To Waypoint", "Make your player ped drive your vehicle to your waypoint.");
                UIMenuItem startDrivingRandomly = new UIMenuItem("Drive Around Randomly", "Make your player ped drive your vehicle randomly around the map.");
                UIMenuItem stopDriving = new UIMenuItem("Stop Driving", "The player ped will find a suitable place to stop the vehicle. The task will be stopped once the vehicle has reached the suitable stop location.");
                UIMenuItem forceStopDriving = new UIMenuItem("Force Stop Driving", "This will stop the driving task immediately without finding a suitable place to stop.");
                UIMenuItem customDrivingStyle = new UIMenuItem("Custom Driving Style", "Select a custom driving style. Make sure to also enable it by selecting the 'Custom' driving style in the driving styles list.");
                customDrivingStyle.SetRightLabel("→→→");
                vehicleAutoPilot.AddItem(customDrivingStyle);
                customDrivingStyle.Activated += async (a, b) => await a.SwitchTo(CustomDrivingStyleMenu, 0, true);
                Dictionary<int, string> knownNames = new Dictionary<int, string>()
                {
                    { 0, "Stop before vehicles" },
                    { 1, "Stop before peds" },
                    { 2, "Avoid vehicles" },
                    { 3, "Avoid empty vehicles" },
                    { 4, "Avoid peds" },
                    { 5, "Avoid objects" },

                    { 7, "Stop at traffic lights" },
                    { 8, "Use blinkers" },
                    { 9, "Allow going wrong way" },
                    { 10, "Go in reverse gear" },

                    { 18, "Use shortest path" },

                    { 22, "Ignore roads" },

                    { 24, "Ignore all pathing" },

                    { 29, "Avoid highways (if possible)" },
                };
                for (int i = 0; i < 31; i++)
                {
                    string name = "~r~Unknown Flag";
                    if (knownNames.ContainsKey(i))
                    {
                        name = knownNames[i];
                    }
                    UIMenuCheckboxItem checkbox = new UIMenuCheckboxItem(name, false, "Toggle this driving style flag.");
                    CustomDrivingStyleMenu.AddItem(checkbox);
                }
                CustomDrivingStyleMenu.OnCheckboxChange += (sender, item, _checked) =>
                {
                    int style = GetStyleFromIndex(drivingStyle.Index);
                    CustomDrivingStyleMenu.Subtitle = $"custom style: {style}";
                    if (drivingStyle.Index == 4)
                    {
                        Notify.Custom("Driving style updated.");
                        SetDriveTaskDrivingStyle(Game.PlayerPed.Handle, style);
                    }
                    else
                    {
                        Notify.Custom("Driving style NOT updated because you haven't enabled the Custom driving style in the previous menu.");
                    }
                };

                vehicleAutoPilot.AddItem(startDrivingWaypoint);
                vehicleAutoPilot.AddItem(startDrivingRandomly);
                vehicleAutoPilot.AddItem(stopDriving);
                vehicleAutoPilot.AddItem(forceStopDriving);

                vehicleAutoPilot.OnItemSelect += async (sender, item, index) =>
                {
                    if (Game.PlayerPed.IsInVehicle() && item != stopDriving && item != forceStopDriving)
                    {
                        if (Game.PlayerPed.CurrentVehicle != null && Game.PlayerPed.CurrentVehicle.Exists() && !Game.PlayerPed.CurrentVehicle.IsDead && Game.PlayerPed.CurrentVehicle.IsDriveable)
                        {
                            if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed)
                            {
                                if (item == startDrivingWaypoint)
                                {
                                    if (IsWaypointActive())
                                    {
                                        int style = GetStyleFromIndex(drivingStyle.Index);
                                        DriveToWp(style);
                                        Notify.Info("Your player ped is now driving the vehicle for you. You can cancel any time by pressing the Stop Driving button. The vehicle will stop when it has reached the destination.");
                                    }
                                    else
                                    {
                                        Notify.Error("You need a waypoint before you can drive to it!");
                                    }

                                }
                                else if (item == startDrivingRandomly)
                                {
                                    int style = GetStyleFromIndex(drivingStyle.Index);
                                    DriveWander(style);
                                    Notify.Info("Your player ped is now driving the vehicle for you. You can cancel any time by pressing the Stop Driving button.");
                                }
                            }
                            else
                            {
                                Notify.Error("You must be the driver of this vehicle!");
                            }
                        }
                        else
                        {
                            Notify.Error("Your vehicle is broken or it does not exist!");
                        }
                    }
                    else if (item != stopDriving && item != forceStopDriving)
                    {
                        Notify.Error("You need to be in a vehicle first!");
                    }
                    if (item == stopDriving)
                    {
                        if (Game.PlayerPed.IsInVehicle())
                        {
                            Vehicle veh = GetVehicle();
                            if (veh != null && veh.Exists() && !veh.IsDead)
                            {
                                Vector3 outPos = new Vector3();
                                if (GetNthClosestVehicleNode(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 3, ref outPos, 0, 0, 0))
                                {
                                    Notify.Info("The player ped will find a suitable place to park the car and will then stop driving. Please wait.");
                                    ClearPedTasks(Game.PlayerPed.Handle);
                                    TaskVehiclePark(Game.PlayerPed.Handle, veh.Handle, outPos.X, outPos.Y, outPos.Z, Game.PlayerPed.Heading, 3, 60f, true);
                                    while (Game.PlayerPed.Position.DistanceToSquared2D(outPos) > 3f)
                                    {
                                        await BaseScript.Delay(0);
                                    }
                                    SetVehicleHalt(veh.Handle, 3f, 0, false);
                                    ClearPedTasks(Game.PlayerPed.Handle);
                                    Notify.Info("The player ped has stopped driving.");
                                }
                            }
                        }
                        else
                        {
                            ClearPedTasks(Game.PlayerPed.Handle);
                            Notify.Alert("Your ped is not in any vehicle.");
                        }
                    }
                    else if (item == forceStopDriving)
                    {
                        ClearPedTasks(Game.PlayerPed.Handle);
                        Notify.Info("Driving task cancelled.");
                    }
                };

                vehicleAutoPilot.OnListSelect += (sender, item, itemIndex) =>
                {
                    if (item == drivingStyle)
                    {
                        int style = GetStyleFromIndex(sender.MenuItems.IndexOf(item));
                        SetDriveTaskDrivingStyle(Game.PlayerPed.Handle, style);
                        Notify.Info($"Driving task style is now set to: ~r~{drivingStyles[sender.MenuItems.IndexOf(item)]}~s~.");
                    }
                };
            }

            if (IsAllowed(Permission.POFreeze))
            {
                menu.AddItem(playerFrozenCheckbox);
            }
            if (IsAllowed(Permission.POScenarios))
            {
                menu.AddItem(playerScenarios);
                menu.AddItem(stopScenario);
            }
            #endregion

            #region handle all events
            // Checkbox changes.
            menu.OnCheckboxChange += (sender, item, _checked) =>
            {
                // God Mode toggled.
                if (item == playerGodModeCheckbox)
                {
                    PlayerGodMode = _checked;
                }
                // Invisibility toggled.
                else if (item == invisibleCheckbox)
                {
                    PlayerInvisible = _checked;
                    SetEntityVisible(Game.PlayerPed.Handle, !PlayerInvisible, false);
                }
                // Unlimited Stamina toggled.
                else if (item == unlimitedStaminaCheckbox)
                {
                    PlayerStamina = _checked;
                    StatSetInt((uint)GetHashKey("MP0_STAMINA"), _checked ? 100 : 0, true);
                }
                // Fast run toggled.
                else if (item == fastRunCheckbox)
                {
                    PlayerFastRun = _checked;
                    SetRunSprintMultiplierForPlayer(Game.Player.Handle, _checked ? 1.49f : 1f);
                }
                // Fast swim toggled.
                else if (item == fastSwimCheckbox)
                {
                    PlayerFastSwim = _checked;
                    SetSwimMultiplierForPlayer(Game.Player.Handle, _checked ? 1.49f : 1f);
                }
                // Super jump toggled.
                else if (item == superJumpCheckbox)
                {
                    PlayerSuperJump = _checked;
                }
                // No ragdoll toggled.
                else if (item == noRagdollCheckbox)
                {
                    PlayerNoRagdoll = _checked;
                }
                // Never wanted toggled.
                else if (item == neverWantedCheckbox)
                {
                    PlayerNeverWanted = _checked;
                    if (!_checked)
                    {
                        SetMaxWantedLevel(5);
                    }
                    else
                    {
                        SetMaxWantedLevel(0);
                    }
                }
                // Everyone ignores player toggled.
                else if (item == everyoneIgnoresPlayerCheckbox)
                {
                    PlayerIsIgnored = _checked;

                    // Manage player is ignored by everyone.
                    SetEveryoneIgnorePlayer(Game.Player.Handle, PlayerIsIgnored);
                    SetPoliceIgnorePlayer(Game.Player.Handle, PlayerIsIgnored);
                    SetPlayerCanBeHassledByGangs(Game.Player.Handle, !PlayerIsIgnored);
                }
                else if (item == playerStayInVehicleCheckbox)
                {
                    PlayerStayInVehicle = _checked;
                }
                // Freeze player toggled.
                else if (item == playerFrozenCheckbox)
                {
                    PlayerFrozen = _checked;

                    if (!MainMenu.NoClipEnabled)
                    {
                        FreezeEntityPosition(Game.PlayerPed.Handle, PlayerFrozen);
                    }
                    else if (!MainMenu.NoClipEnabled)
                    {
                        FreezeEntityPosition(Game.PlayerPed.Handle, PlayerFrozen);
                    }
                }
            };

            // List selections
            menu.OnListSelect += (sender, listItem, itemIndex) =>
            {
                // Set wanted Level
                if (listItem == setWantedLevel)
                {
                    SetPlayerWantedLevel(Game.Player.Handle, listItem.Index, false);
                    SetPlayerWantedLevelNow(Game.Player.Handle, false);
                }
                // Player Scenarios 
                else if (listItem == playerScenarios)
                {
                    PlayScenario(PedScenarios.ScenarioNames[PedScenarios.Scenarios[listItem.Index]]);
                }
                else if (listItem == setArmorItem)
                {
                    Game.PlayerPed.Armor = listItem.Index * 20;
                }
            };

            // button presses
            menu.OnItemSelect += (sender, item, index) =>
            {
                // Force Stop Scenario button
                if (item == stopScenario)
                {
                    // Play a new scenario named "forcestop" (this scenario doesn't exist, but the "Play" function checks
                    // for the string "forcestop", if that's provided as th scenario name then it will forcefully clear the player task.
                    PlayScenario("forcestop");
                }
                else if (item == healPlayerBtn)
                {
                    Game.PlayerPed.Health = Game.PlayerPed.MaxHealth;
                    Notify.Success("Player healed.");
                }
                else if (item == cleanPlayerBtn)
                {
                    Game.PlayerPed.ClearBloodDamage();
                    Notify.Success("Player clothes have been cleaned.");
                }
                else if (item == dryPlayerBtn)
                {
                    Game.PlayerPed.WetnessHeight = 0f;
                    Notify.Success("Player is now dry.");
                }
                else if (item == wetPlayerBtn)
                {
                    Game.PlayerPed.WetnessHeight = 2f;
                    Notify.Success("Player is now wet.");
                }
                else if (item == suicidePlayerBtn)
                {
                    CommitSuicide();
                }
            };
            #endregion

        }

        private int GetCustomDrivingStyle()
        {
            List<UIMenuItem> items = CustomDrivingStyleMenu.MenuItems;
            int[] flags = new int[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                UIMenuItem item = items[i];
                if (item is UIMenuCheckboxItem checkbox)
                {
                    flags[i] = checkbox.Checked ? 1 : 0;
                }
            }
            string binaryString = "";
            IEnumerable<int> reverseFlags = flags.Reverse();
            foreach (int i in reverseFlags)
            {
                binaryString += i;
            }
            uint binaryNumber = Convert.ToUInt32(binaryString, 2);
            return (int)binaryNumber;
        }

        private int GetStyleFromIndex(int index)
        {
            int style = index switch
            {
                0 => 443,// normal
                1 => 575,// rushed
                2 => 536871355,// Avoid highways
                3 => 1467,// Go in reverse
                4 => GetCustomDrivingStyle(),// custom driving style;
                _ => 0,// no style (impossible, but oh well)
            };
            return style;
        }

        /// <summary>
        /// Checks if the menu exists, if not then it creates it first.
        /// Then returns the menu.
        /// </summary>
        /// <returns>The Player Options Menu</returns>
        public UIMenu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }

    }
}
