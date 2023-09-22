using ScaleformUI.Menu;
using System.Collections.Generic;

namespace vMenuClient.menus
{
    public class TimeOptions
    {
        // Variables
        private UIMenu menu;
        public UIMenuItem freezeTimeToggle;

        /// <summary>
        /// Creates the menu.
        /// </summary>
        private void CreateMenu()
        {
            // Create the menu.
            menu = new UIMenu(Game.Player.Name, "Time Options");

            // Create all menu items.
            freezeTimeToggle = new UIMenuItem("Freeze/Unfreeze Time", "Enable or disable time freezing.");
            UIMenuItem earlymorning = new UIMenuItem("Early Morning", "Set the time to 06:00.");
            earlymorning.SetRightLabel("06:00");
            UIMenuItem morning = new UIMenuItem("Morning", "Set the time to 09:00.");
            morning.SetRightLabel("09:00");
            UIMenuItem noon = new UIMenuItem("Noon", "Set the time to 12:00.");
            noon.SetRightLabel("12:00");
            UIMenuItem earlyafternoon = new UIMenuItem("Early Afternoon", "Set the time to 15:00.");
            earlyafternoon.SetRightLabel("15:00");
            UIMenuItem afternoon = new UIMenuItem("Afternoon", "Set the time to 18:00.");
            afternoon.SetRightLabel("18:00");
            UIMenuItem evening = new UIMenuItem("Evening", "Set the time to 21:00.");
            evening.SetRightLabel("21:00");
            UIMenuItem midnight = new UIMenuItem("Midnight", "Set the time to 00:00.");
            midnight.SetRightLabel("00:00");
            UIMenuItem night = new UIMenuItem("Night", "Set the time to 03:00.");
            night.SetRightLabel("03:00");

            List<dynamic> hours = new List<dynamic>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09" };
            List<dynamic> minutes = new List<dynamic>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09" };
            for (int i = 10; i < 60; i++)
            {
                if (i < 24)
                {
                    hours.Add(i.ToString());
                }
                minutes.Add(i.ToString());
            }
            UIMenuListItem manualHour = new UIMenuListItem("Set Custom Hour", hours, 0);
            UIMenuListItem manualMinute = new UIMenuListItem("Set Custom Minute", minutes, 0);

            // Add all menu items to the menu.
            if (IsAllowed(Permission.TOFreezeTime))
            {
                menu.AddItem(freezeTimeToggle);
            }
            if (IsAllowed(Permission.TOSetTime))
            {
                menu.AddItem(earlymorning);
                menu.AddItem(morning);
                menu.AddItem(noon);
                menu.AddItem(earlyafternoon);
                menu.AddItem(afternoon);
                menu.AddItem(evening);
                menu.AddItem(midnight);
                menu.AddItem(night);
                menu.AddItem(manualHour);
                menu.AddItem(manualMinute);
            }

            // Handle button presses.
            menu.OnItemSelect += (sender, item, index) =>
            {
                // If it's the freeze time button.
                if (item == freezeTimeToggle)
                {
                    Subtitle.Info($"Time will now {(EventManager.IsServerTimeFrozen ? "~y~continue" : "~o~freeze")}~s~.", prefix: "Info:");
                    UpdateServerTime(EventManager.GetServerHours, EventManager.GetServerMinutes, !EventManager.IsServerTimeFrozen);
                }
                else
                {
                    // Set the time using the index and some math :)
                    // eg: index = 3 (12:00) ---> 3 * 3 (=9) + 3 [= 12] ---> 12:00
                    // eg: index = 8 (03:00) ---> 8 * 3 (=24) + 3 (=27, >23 so 27-24) [=3] ---> 03:00
                    int newHour = 0;
                    if (IsAllowed(Permission.TOFreezeTime))
                    {
                        newHour = (index * 3) + 3 < 23 ? (index * 3) + 3 : (index * 3) + 3 - 24;
                    }
                    else
                    {
                        newHour = ((index + 1) * 3) + 3 < 23 ? ((index + 1) * 3) + 3 : ((index + 1) * 3) + 3 - 24;
                    }

                    int newMinute = 0;
                    Subtitle.Info($"Time set to ~y~{(newHour < 10 ? $"0{newHour}" : newHour.ToString())}~s~:~y~" +
                        $"{(newMinute < 10 ? $"0{newMinute}" : newMinute.ToString())}~s~.", prefix: "Info:");
                    UpdateServerTime(newHour, newMinute, EventManager.IsServerTimeFrozen);
                }

            };

            menu.OnListSelect += (sender, item, itemIndex) =>
            {
                int newHour = EventManager.GetServerHours;
                int newMinute = EventManager.GetServerMinutes;
                if (item == manualHour)
                {
                    newHour = item.Index;
                }
                else if (item == manualMinute)
                {
                    newMinute = item.Index;
                }

                Subtitle.Info($"Time set to ~y~{(newHour < 10 ? $"0{newHour}" : newHour.ToString())}~s~:~y~" +
                        $"{(newMinute < 10 ? $"0{newMinute}" : newMinute.ToString())}~s~.", prefix: "Info:");
                UpdateServerTime(newHour, newMinute, EventManager.IsServerTimeFrozen);
            };
        }

        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
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
