using ScaleformUI.Menu;

namespace vMenuClient.menus
{
    public class Recording
    {
        // Variables
        private UIMenu menu;

        private void CreateMenu()
        {
            AddTextEntryByHash(0x86F10CE6, "Upload To Cfx.re Forum"); // Replace the "Upload To Social Club" button in gallery
            AddTextEntry("ERROR_UPLOAD", "Are you sure you want to upload this photo to Cfx.re forum?"); // Replace the warning message text for uploading

            // Create the menu.
            menu = new UIMenu("Recording", "Recording Options");

            UIMenuItem takePic = new UIMenuItem("Take Photo", "Takes a photo and saves it to the Pause Menu gallery.");
            UIMenuItem openPmGallery = new UIMenuItem("Open Gallery", "Opens the Pause Menu gallery.");
            UIMenuItem startRec = new UIMenuItem("Start Recording", "Start a new game recording using GTA V's built in recording.");
            UIMenuItem stopRec = new UIMenuItem("Stop Recording", "Stop and save your current recording.");
            UIMenuItem openEditor = new UIMenuItem("Rockstar Editor", "Open the rockstar editor, note you might want to quit the session first before doing this to prevent some issues.");

            menu.AddItem(takePic);
            menu.AddItem(openPmGallery);
            menu.AddItem(startRec);
            menu.AddItem(stopRec);
            menu.AddItem(openEditor);

            menu.OnItemSelect += async (sender, item, index) =>
            {
                if (item == startRec)
                {
                    if (IsRecording())
                    {
                        Notify.Alert("You are already recording a clip, you need to stop recording first before you can start recording again!");
                    }
                    else
                    {
                        StartRecording(1);
                    }
                }
                else if (item == openPmGallery)
                {
                    ActivateFrontendMenu((uint)GetHashKey("FE_MENU_VERSION_MP_PAUSE"), true, 3);
                }
                else if (item == takePic)
                {
                    BeginTakeHighQualityPhoto();
                    SaveHighQualityPhoto(-1);
                    FreeMemoryForHighQualityPhoto();
                }
                else if (item == stopRec)
                {
                    if (!IsRecording())
                    {
                        Notify.Alert("You are currently NOT recording a clip, you need to start recording first before you can stop and save a clip.");
                    }
                    else
                    {
                        StopRecordingAndSaveClip();
                    }
                }
                else if (item == openEditor)
                {
                    if (GetSettingsBool(Setting.vmenu_quit_session_in_rockstar_editor))
                    {
                        QuitSession();
                    }
                    ActivateRockstarEditor();
                    // wait for the editor to be closed again.
                    while (IsPauseMenuActive())
                    {
                        await BaseScript.Delay(0);
                    }
                    // then fade in the screen.
                    DoScreenFadeIn(1);
                    Notify.Alert("You left your previous session before entering the Rockstar Editor. Restart the game to be able to rejoin the server's main session.", true, true);
                }
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
