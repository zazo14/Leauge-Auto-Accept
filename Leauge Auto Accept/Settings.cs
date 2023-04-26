using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Leauge_Auto_Accept
{
    internal class Settings
    {
        public List<LaneSettings> laneSettingsList = new List<LaneSettings>();

        public static LaneSettings topLaneSettings = new LaneSettings("Top");
        public static LaneSettings botLaneSettings = new LaneSettings("Bot");
        public static LaneSettings supportLaneSettings = new LaneSettings("Support");
        public static LaneSettings midLaneSettings = new LaneSettings("Mid");
        public static LaneSettings jungleLaneSettings = new LaneSettings("Jungle");

        public Settings()
        {
            laneSettingsList.Add(topLaneSettings);
            laneSettingsList.Add(botLaneSettings);
            laneSettingsList.Add(supportLaneSettings);
            laneSettingsList.Add(midLaneSettings);
            laneSettingsList.Add(jungleLaneSettings);
        }


        public static string[] currentChamp = { "None", "0" };
        public static string[] currentBan = { "None", "0" };
        public static string[] currentSpell1 = { "None", "0" };
        public static string[] currentSpell2 = { "None", "0" };
        public static bool chatMessagesEnabled = false;
        public static List<string> chatMessages = new List<string>();

        public static bool saveSettings = false;
        public static bool preloadData = false;
        public static bool instaLock = false;
        public static int lockDelay = 1500;
        public static string lockDelayString = "1500";
        public static bool disableUpdateCheck = false;
        public static bool autoPickOrderTrade = false;
        public static bool shouldAutoAcceptbeOn = false;

        public static void settingsModify(int item)
        {
            switch (item)
            {
                case 0:
                    if (saveSettings && preloadData)
                    {
                        preloadData = !preloadData;
                        UI.settingsMenuUpdateUI(1);
                    }
                    if (saveSettings && disableUpdateCheck)
                    {
                        disableUpdateCheck = !disableUpdateCheck;
                        UI.settingsMenuUpdateUI(4);
                    }
                    saveSettings = !saveSettings;
                    break;
                case 1:
                    if (!preloadData && !saveSettings)
                    {
                        saveSettings = !saveSettings;
                        UI.settingsMenuUpdateUI(0);
                    }
                    preloadData = !preloadData;
                    break;
                case 2:
                    instaLock = !instaLock;
                    break;
                case 3:
                    if (lockDelayString.Length == 0)
                    {
                        lockDelayString = "0";
                    }
                    else
                    {
                        lockDelayString = lockDelayString.TrimStart('0');
                    }
                    lockDelay = Int32.Parse(lockDelayString);
                    if (lockDelay < 500)
                    {
                        lockDelay = 500;
                    }
                    break;
                case 4:
                    if (!disableUpdateCheck && !saveSettings)
                    {
                        saveSettings = !saveSettings;
                        UI.settingsMenuUpdateUI(0);
                    }
                    disableUpdateCheck = !disableUpdateCheck;
                    break;
                case 5:
                    autoPickOrderTrade = !autoPickOrderTrade;
                    break;
            }

            if (saveSettings)
            {
                settingsSave();
            }
            else if (item == 0)
            {
                deleteSettings();
            }
        }

        public static void saveSelectedChamp()
        {


            List<itemList> champsFiltered = new List<itemList>();
            if ("none".Contains(Navigation.currentInput.ToLower()))
            {
                champsFiltered.Add(new itemList() { name = "None", id = "0" });
            }
            foreach (var champ in Data.champsSorterd)
            {
                if (champ.name.ToLower().Contains(Navigation.currentInput.ToLower()))
                {
                    if (UI.currentChampPicker == 0)
                    {
                        if (!champ.free)
                        {
                            continue;
                        }
                    }
                    champsFiltered.Add(new itemList() { name = champ.name, id = champ.id });
                }
            }

            if (champsFiltered.Count > 0)
            {
                string name;
                string id;
                if (Navigation.currentPos < 0)
                {
                    name = "None";
                    id = "0";
                }
                else
                {
                    name = champsFiltered[Navigation.currentPos].name;
                    id = champsFiltered[Navigation.currentPos].id;
                }
                LaneSettings currentLaneSettings = getLaneSettings(UI.selectedLane);
                
                if (UI.currentChampPicker == 0)
                {

                    currentLaneSettings.ChampName = name;
                    currentLaneSettings.ChampId = id;
                }
                else
                {
                    currentLaneSettings.BanName = name;
                    currentLaneSettings.BanId = id;
                }

                if (saveSettings)
                {
                    settingsSave();
                }
            }
        }

        public static LaneSettings getLaneSettings(string lane)
        {
            LaneSettings laneSettings = null;
            switch (lane.ToLower())
            {
                case "top":
                    laneSettings = topLaneSettings;
                    break;
                case "jungle":
                    laneSettings = jungleLaneSettings;
                    break;
                case "mid":
                case "middle":
                    laneSettings = midLaneSettings;
                    break;
                case "adc":
                case "bot":
                case "bottom":
                    laneSettings = botLaneSettings;
                    break;
                case "support":
                case "supp":
                    laneSettings = supportLaneSettings;
                    break;
                default:
                    Console.WriteLine("stoopid, wrong lane");
                    break;
            }
            return laneSettings;
        }

        public static void saveSelectedSpell()
        {

            LaneSettings currentLaneSettings = getLaneSettings(UI.selectedLane);
            List<itemList> spellsFiltered = new List<itemList>();
            if ("none".Contains(Navigation.currentInput.ToLower()))
            {
                spellsFiltered.Add(new itemList() { name = "None", id = "0" });
            }
            foreach (var spell in Data.spellsSorted)
            {
                if (spell.name.ToLower().Contains(Navigation.currentInput.ToLower()))
                {
                    spellsFiltered.Add(new itemList() { name = spell.name, id = spell.id });
                }
            }

            if (spellsFiltered.Count > 0)
            {
                string name;
                string id;
                if (Navigation.currentPos < 0)
                {
                    name = "None";
                    id = "0";
                }
                else
                {
                    name = spellsFiltered[Navigation.currentPos].name;
                    id = spellsFiltered[Navigation.currentPos].id;
                }
                if (UI.currentSpellSlot == 0)
                {
                    currentLaneSettings.Spell1Name = name;
                    currentLaneSettings.Spell1Id = id;
                }
                else
                {
                    currentLaneSettings.Spell2Name = name;
                    currentLaneSettings.Spell2Id = id;
                }
                if (saveSettings)
                {
                    settingsSave();
                }
            }
        }

        public static void updateChatMessage()
        {
            if (chatMessages.Count > UI.messageIndex)
            {
                chatMessages[UI.messageIndex] = Navigation.currentInput;
            }
            else
            {
                chatMessages.Add(Navigation.currentInput);
            }
            updateChatMessagesToggle();
            if (saveSettings)
            {
                settingsSave();
            }
        }

        public static void deleteChatMessage()
        {
            if (chatMessages.Count > UI.messageIndex)
            {
                chatMessages.RemoveAt(UI.messageIndex);
            }
            updateChatMessagesToggle();

            if (saveSettings)
            {
                settingsSave();
            }
        }

        private static void updateChatMessagesToggle()
        {
            if (chatMessages.Count > 0)
            {
                chatMessagesEnabled = true;
            }
            else
            {
                chatMessagesEnabled = false;
            }
        }

        private static string encodeMessagesIntoBase64()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(string.Join('|', chatMessages));
            string base64String = Convert.ToBase64String(byteArray);

            return base64String;
        }

        private static void decodeMessagesFromBase64(string messages)
        {
            if (messages == "") { return; }
            byte[] byteArray = Convert.FromBase64String(messages);
            string joinedString = Encoding.UTF8.GetString(byteArray);
            chatMessages = new List<string>(joinedString.Split('|'));
        }

        public static void settingsSave()
        {
            Settings settings = new Settings();
            StringBuilder sb = new StringBuilder();
            foreach (LaneSettings laneSettings in settings.laneSettingsList)
            {
                sb.Append(laneSettings.Lane + ":");
                sb.Append("champName=" + laneSettings.ChampName + ",");
                sb.Append("champId=" + laneSettings.ChampId + ",");
                sb.Append("banName=" + laneSettings.BanName + ",");
                sb.Append("banId=" + laneSettings.BanId + ",");
                sb.Append("spell1Name=" + laneSettings.Spell1Name + ",");
                sb.Append("spell1Id=" + laneSettings.Spell1Id + ",");
                sb.Append("spell2Name=" + laneSettings.Spell2Name + ",");
                sb.Append("spell2Id=" + laneSettings.Spell2Id + ",");
                sb.Append("autoAcceptOn=" + Settings.shouldAutoAcceptbeOn + ",");
                sb.Append("preloadData=" + Settings.preloadData + ",");
                sb.Append("instaLock=" + Settings.instaLock + ",");
                sb.Append("lockDelay=" + Settings.lockDelay + ",");
                sb.Append("autoPickOrderTrade=" + Settings.autoPickOrderTrade + ",");
                sb.Append("disableUpdateCheck=" + Settings.disableUpdateCheck + ",");
                sb.Append("chatMessages=" + encodeMessagesIntoBase64() + "\n");
            }

            string dirParameter = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Leauge Auto Accept Config.txt";
            using (StreamWriter m_WriterParameter = new StreamWriter(dirParameter, false))
            {
                m_WriterParameter.Write(sb.ToString());
            }
        }


        public static void toggleAutoAcceptSetting()
        {
            if (MainLogic.isAutoAcceptOn)
            {
                MainLogic.isAutoAcceptOn = false;
                shouldAutoAcceptbeOn = false;
            }
            else
            {
                MainLogic.isAutoAcceptOn = true;
                shouldAutoAcceptbeOn = true;
            }
            if (saveSettings)
            {
                settingsSave();
            }
        }

        public static void deleteSettings()
        {
            string dirParameter = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Leauge Auto Accept Config.txt";
            File.Delete(dirParameter);
        }

        public static void loadSettings(LaneSettings laneSettings)
        {
            string dirParameter = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Leauge Auto Accept Config.txt";
            if (File.Exists(dirParameter))
            {
                string text = File.ReadAllText(dirParameter);

                string[] lanes = text.Split('\n');
                foreach (var lane in lanes)
                {
                    string[] laneData = lane.Split(':');

                    if (laneData[0] != laneSettings.Lane)
                    {
                        continue;
                    }
                    string[] settings = laneData[1].Split(',');
                    foreach (var setting in settings)
                    {

                        string[] columns = setting.Split('=');
                        switch (columns[0])
                        {
                            case "champName":
                                laneSettings.ChampName = columns[1];
                                break;
                            case "champId":
                                laneSettings.ChampId = columns[1];
                                break;
                            case "banName":
                                laneSettings.BanName = columns[1];
                                break;
                            case "banId":
                                laneSettings.BanId = columns[1];
                                break;
                            case "spell1Name":
                                laneSettings.Spell1Name = columns[1];
                                break;
                            case "spell1Id":
                                laneSettings.Spell1Id = columns[1];
                                break;
                            case "spell2Name":
                                laneSettings.Spell2Name = columns[1];
                                break;
                            case "spell2Id":
                                laneSettings.Spell2Id = columns[1];
                                break;
                            case "lockDelay":
                                Settings.lockDelay = Int32.Parse(columns[1]);
                                Settings.lockDelayString = columns[1];
                                break;
                            case "autoAcceptOn":
                                Settings.shouldAutoAcceptbeOn = Boolean.Parse(columns[1]);
                                break;
                            case "preloadData":
                                Settings.preloadData = Boolean.Parse(columns[1]);
                                break;
                            case "instaLock":
                                Settings.instaLock = Boolean.Parse(columns[1]);
                                break;
                            case "disableUpdateCheck":
                                Settings.disableUpdateCheck = Boolean.Parse(columns[1]);
                                break;
                            case "autoPickOrderTrade":
                                Settings.autoPickOrderTrade = Boolean.Parse(columns[1]);
                                break;
                            case "chatMessages":
                                decodeMessagesFromBase64(columns[1]);
                                updateChatMessagesToggle();
                                break;
                        }
                    }
                    saveSettings = true;
                }
            }
        }

    }
}
