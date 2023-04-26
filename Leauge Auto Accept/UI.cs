﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace Leauge_Auto_Accept
{
    internal class UI
    {
        public static string currentWindow = "";
        public static string previousWindow = "";

        public static int currentChampPicker = 0;
        public static int currentSpellSlot = 0;

        public static int totalChamps = 0;
        public static int totalSpells = 0;

                                        // normal/grid/pages/nocursor/messageEdit
        public static string windowType = "";
        public static int messageIndex = 0; //index for the message currently being edit

        public static int totalRows = SizeHandler.WindowHeight - 2;
        public static int columnSize = 20;
        public static int topPad = 0;
        public static int leftPad = 0;
        public static int maxPos = 0;
        public static int currentPage = 0;
        public static int totalPages = 0;

        public static string selectedLane = "Top";

        public static void initializingWindow()
        {
            Print.canMovePos = false;
            currentWindow = "initializing";
            windowType = "nocursor";

            Print.printCentered("Initializing...", SizeHandler.HeightCenter);
        }

        public static void leagueClientIsClosedMessage()
        {
            Print.canMovePos = false;
            currentWindow = "leagueClientIsClosedMessage";
            windowType = "nocursor";

            Console.Clear();

            Print.printCentered("League client cannot be found.", SizeHandler.HeightCenter);
        }

        public static void consoleTooSmallMessage(string direction)
        {
            // Remember what was previously open
            if (currentWindow != "consoleTooSmallMessage")
            {
                previousWindow = currentWindow;
            }

            currentWindow = "consoleTooSmallMessage";
            windowType = "nocursor";

            Console.Clear();

            if (direction == "width")
            {
                Print.printCentered("Console width is too small. Please resize it.", SizeHandler.HeightCenter);
                Print.printCentered("Minimum width:" + SizeHandler.minWidth + " | Current width:" + SizeHandler.WindowWidth);
            }
            else
            {
                Print.printCentered("Console height is too small. Please resize it.", SizeHandler.HeightCenter);
                Print.printCentered("Minimum height:" + SizeHandler.minHeight + " | Current height:" + SizeHandler.WindowHeight);
            }
        }

        public static void reloadWindow(string windowToReload = "current")
        {
            if (windowToReload == "current") {
                windowToReload = currentWindow;
            }
            else
            {
                windowToReload = previousWindow;
            }
            switch (windowToReload)
            {
                case "mainScreen":
                    mainScreen();
                    break;
                case "settingsMenu":
                    settingsMenu();
                    break;
                case "leagueClientIsClosedMessage":
                    leagueClientIsClosedMessage();
                    break;
                case "infoMenu":
                    infoMenu();
                    break;
                case "exitMenu":
                    exitMenu();
                    break;
                case "champSelector":
                    champSelector();
                    break;
                case "spellSelector":
                    spellSelector();
                    break;
                case "chatMessagesWindow":
                    chatMessagesWindow();
                    break;
            }
        }

        public static void mainScreen()
        {
            //chatMessagesWindow();
            //return;
            Print.canMovePos = false;
            Navigation.currentPos = Navigation.lastPosMainNav;
            Navigation.consolePosLast = Navigation.lastPosMainNav;

            currentWindow = "mainScreen";
            windowType = "normal";
            topPad = SizeHandler.HeightCenter - 1;
            leftPad = SizeHandler.WidthCenter - 25;
            maxPos = 8;

            Console.Clear();

            // Define logo
            string[] logo =
            {
                @"  _                                                 _                                     _   ",
                @" | |                                     /\        | |            /\                     | |  ",
                @" | |     ___  __ _  __ _ _   _  ___     /  \  _   _| |_ ___      /  \   ___ ___ ___ _ __ | |_ ",
                @" | |    / _ \/ _` |/ _` | | | |/ _ \   / /\ \| | | | __/ _ \    / /\ \ / __/ __/ _ \ '_ \| __|",
                @" | |___|  __/ (_| | (_| | |_| |  __/  / ____ \ |_| | || (_) |  / ____ \ (_| (_|  __/ |_) | |_ ",
                @" |______\___|\__,_|\__, |\__,_|\___| /_/    \_\__,_|\__\___/  /_/    \_\___\___\___| .__/ \__|",
                @"                    __/ |                                                          | |        ",
                @"                   |___/                                                           |_|        "
            };

            // Print logo
            for (int i = 0; i < logo.Length; i++)
            {
                Print.printCentered(logo[i], SizeHandler.HeightCenter - 9 + i);
            }

            // Define options
            string[] optionName = {
                "Select a lane",
                "Select a champion",
                "Select a ban",
                "Select summoner spell 1",
                "Select summoner spell 2",
                "Instant chat messages",
                "Enable auto accept"
            };

            LaneSettings currentLaneSettings = Settings.getLaneSettings(UI.selectedLane);

            string[] optionValue = {
                selectedLane,
                currentLaneSettings.ChampName,
                currentLaneSettings.BanName,
                currentLaneSettings.Spell1Name,
                currentLaneSettings.Spell2Name,
                Settings.chatMessagesEnabled ? "Enabled, " + Settings.chatMessages.Count : "Disabled",
                MainLogic.isAutoAcceptOn ? "Enabled" : "Disabled"
            };

            

            // Print options
            for (int i = 0; i < optionName.Length; i++)
            {
                Print.printCentered(addDotsInBetween(optionName[i], optionValue[i]), topPad + i);
            }

            // Print the two bottom buttons that are not actaul settings
            Print.printWhenPossible("Info", SizeHandler.HeightCenter + 6, leftPad + 43);
            Print.printWhenPossible("Settings", SizeHandler.HeightCenter + 6, leftPad + 3);

            
            Print.printWhenPossible("v" + Updater.appVersion, SizeHandler.WindowHeight - 1, 0, false);

            Navigation.handlePointerMovementPrint();

            Print.canMovePos = true;
        }

        public static void toggleAutoAcceptSettingUI()
        {
            Print.printWhenPossible(MainLogic.isAutoAcceptOn ? ". Enabled" : " Disabled", topPad + 6, leftPad + 38);
        }

        public static void settingsMenu()
        {
            Print.canMovePos = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "settingsMenu";
            windowType = "normal";
            topPad = SizeHandler.HeightCenter - 3;
            leftPad = SizeHandler.WidthCenter - 25;
            maxPos = 6;

            Console.Clear();

            // Define options
            string[] optionName = {
                "Save settings/config",
                "Preload data",
                "Instalock bans/picks",
                "Lock/ban delay",
                "Disable update check",
                "Automatically trade pick order"
            };
            string[] optionValue = {
                Settings.saveSettings ? "Yes" : "No",
                Settings.preloadData ? "Yes" : "No",
                Settings.instaLock ? "Yes" : "No",
                Settings.lockDelay.ToString(),
                Settings.disableUpdateCheck ? "Yes" : "No",
                Settings.autoPickOrderTrade ? "Yes" : "No"
            };

            // Print options
            for (int i = 0; i < optionName.Length; i++)
            {
                Print.printCentered(addDotsInBetween(optionName[i], optionValue[i]), topPad + i);
            }

            Navigation.handlePointerMovementPrint();

            Print.canMovePos = true;

            settingsMenuDesc(0);
        }

        public static void settingsMenuDesc(int item)
        {
            // settings descrptions
            switch (item)
            {
                case 0:
                    Print.printCentered("Save settings for the next time you open the app.", topPad + 7);
                    Print.printCentered("This will create a settings file in the %AppData% folder.", topPad + 8);
                    break;
                case 1:
                    Print.printCentered("Preload all data the app will need on launch.", topPad + 7);
                    Print.printCentered("This includes champions list, summoner spells list and more.", topPad + 8);
                    break;
                case 2:
                    Print.printCentered("Instanly lock in picks/bans when it's your turn.", topPad + 7);
                    Print.printCentered("", topPad + 8);
                    break;
                case 3:
                    Print.printCentered("Lock in/ban delay before your turn to do so is over.", topPad + 7);
                    Print.printCentered("Value is in milliseconds. There's a 500 minimum.", topPad + 8);
                    break;
                case 4:
                    Print.printCentered("Disable update check on startup.", topPad + 7);
                    Print.printCentered("", topPad + 8);
                    break;
                case 5:
                    Print.printCentered("Automatically trade pick order when someone requests to.", topPad + 7);
                    Print.printCentered("", topPad + 8);
                    break;
            }
        }

        public static void settingsMenuUpdateUI(int item)
        {
            // Select item to toggle from settings

            string outputText = item switch
            {
                0 => Settings.saveSettings ? " Yes" : ". No",
                1 => Settings.preloadData ? " Yes" : ". No",
                2 => Settings.instaLock ? " Yes" : ". No",
                3 => (" " + Settings.lockDelayString).PadLeft(9, '.'),
                4 => Settings.disableUpdateCheck ? " Yes" : ". No",
                5 => Settings.autoPickOrderTrade ? " Yes" : ". No",
                _ => ""
            };
            Print.printWhenPossible(outputText, item + topPad, SizeHandler.WidthCenter + 22 - outputText.Length);
        }

        public static void infoMenu()
        {
            Print.canMovePos = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "infoMenu";
            windowType = "nocursor";

            Console.Clear();

            Print.printCentered(addDotsInBetween("Made by", "Artem"), SizeHandler.HeightCenter - 2);
            Print.printCentered(addDotsInBetween("Version", Updater.appVersion), SizeHandler.HeightCenter - 1);

            Print.printCentered("Source code:", SizeHandler.HeightCenter + 1);
            Print.printCentered(" github.com/sweetriverfish/LeagueAutoAccept", SizeHandler.HeightCenter + 2);
        }

        public static void exitMenu()
        {
            Print.canMovePos = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "exitMenu";
            windowType = "sideways";
            topPad = SizeHandler.HeightCenter + 1;
            leftPad = SizeHandler.WidthCenter - 19;
            maxPos = 2;

            Console.Clear();

            Print.printCentered("Are you sure you want to close this app?", topPad - 2);
            Print.printWhenPossible((" No").PadLeft(32, ' '), topPad, leftPad + 3, false);
            Print.printWhenPossible("Yes ", topPad, leftPad + 3, false);

            Navigation.handlePointerMovementPrint();

            Print.canMovePos = true;
        }

        public static void champSelector()
        {
            Print.canMovePos = false;

            totalRows = SizeHandler.WindowHeight - 2;

            currentWindow = "champSelector";
            windowType = "grid";

            Navigation.currentInput = "";

            Console.Clear();

            Data.loadChampionsList();

            updateCurrentFilter();
            displayChamps();
        }

        public static void laneSelector()
        {
            Print.canMovePos = false;

            totalRows = SizeHandler.WindowHeight - 2;

            currentWindow = "laneSelector";
            windowType = "grid";

            Navigation.currentInput = "";

            Console.Clear();

            Console.WriteLine("Please select a lane:");
            selectedLane = Console.ReadLine();

            updateCurrentFilter();

        }

        private static void displayChamps()
        {
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            topPad = 0;
            leftPad = 0;
            maxPos = totalChamps;

            Console.SetCursorPosition(0, 0);

            List<itemList> champsFiltered = new List<itemList>();
            if ("none".Contains(Navigation.currentInput.ToLower()))
            {
                champsFiltered.Add(new itemList() { name = "None", id = "0" });
            }
            foreach (var champ in Data.champsSorterd)
            {
                if (champ.name.ToLower().Contains(Navigation.currentInput.ToLower()))
                {
                    // Make sure the champ is free or if it's for a ban before adding it to the list
                    if (champ.free || currentChampPicker == 1)
                    {
                        champsFiltered.Add(new itemList() { name = champ.name, id = champ.id });
                    }
                }
            }

            totalChamps = champsFiltered.Count;

            int currentRow = 0;
            string[] champsOutput = new string[totalRows];

            foreach (var champ in champsFiltered)
            {
                string line = "   " + champ.name;
                line = line.PadRight(columnSize, ' ');

                champsOutput[currentRow] += line;

                currentRow++;
                if (currentRow >= totalRows)
                {
                    currentRow = 0;
                }
            }

            foreach (var line in champsOutput)
            {
                string lineNew;
                if (line != null)
                {
                    lineNew = line.Remove(line.Length - 1);
                    lineNew = lineNew.PadRight(119, ' ');
                }
                else
                {
                    lineNew = "".PadRight(119, ' ');
                }
                Print.printWhenPossible(lineNew);
            }
            Navigation.handlePointerMovementPrint();
            Print.canMovePos = true;
        }

        public static void spellSelector()
        {
            Print.canMovePos = false;

            totalRows = SizeHandler.WindowHeight - 2;

            currentWindow = "spellSelector";
            windowType = "grid";

            Navigation.currentInput = "";

            Console.Clear();

            Data.loadSpellsList();

            updateCurrentFilter();
            displaySpells();
        }

        private static void displaySpells()
        {
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            topPad = 0;
            leftPad = 0;
            maxPos = totalSpells;

            Console.SetCursorPosition(0, 0);

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

            totalSpells = spellsFiltered.Count;

            int currentRow = 0;
            string[] spelloutput = new string[totalRows];

            foreach (var spell in spellsFiltered)
            {
                string line = "   " + spell.name;
                line = line.PadRight(columnSize, ' ');

                spelloutput[currentRow] += line;

                currentRow++;
                if (currentRow >= totalRows)
                {
                    currentRow = 0;
                }
            }

            foreach (var line in spelloutput)
            {
                string lineNew;
                if (line != null)
                {
                    lineNew = line.Remove(line.Length - 1);
                    lineNew = lineNew.PadRight(119, ' ');
                }
                else
                {
                    lineNew = "".PadRight(119, ' ');
                }
                Print.printWhenPossible(lineNew);
            }
            Navigation.handlePointerMovementPrint();
            Print.canMovePos = true;
        }

        public static void updateCurrentFilter()
        {
            if (currentWindow == "champSelector")
            {
                displayChamps();
            }
            else if (currentWindow == "spellSelector")
            {
                displaySpells();
            }
            else if (currentWindow == "laneSelector")
            {
                mainScreen();
            }
            Navigation.currentPos = 0;
            string consoleLine = "Search: " + Navigation.currentInput;
            Print.printCentered(consoleLine, Console.WindowHeight - 1, false);
            Console.SetCursorPosition(0, 0);
        }

        public static void printHeart()
        {
            int[] position = { 54, 10 };

            string[] lines = {
                "  oooo   oooo",
                " o    o o    o ",
                "o      o      o",
                " o           o",
                "  o         o",
                "   o       o",
                "    o     o",
                "     o   o",
                "      o o",
                "       o"
            };

            foreach (string line in lines)
            {
                Console.SetCursorPosition(position[0], position[1]++);
                Console.WriteLine(line);
            }
        }

        private static string addDotsInBetween(string firstString, string secondString, int totalLength = 44)
        {
            int firstStringLength = firstString.Length + 1;
            int secondStringLength = secondString.Length + 1;
            int dotsCount = totalLength - firstStringLength - secondStringLength;

            return firstString + " " + new string('.', dotsCount) + " " + secondString;
        }

        public static void chatMessagesWindow(int pageToLoad = 0)
        {
            Print.canMovePos = false;
            Console.Clear();
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "chatMessagesWindow";
            windowType = "pages";
            topPad = 1;
            leftPad = 2;
            maxPos = Settings.chatMessages.Count + 1; // +1 for "new message" row
            int messageWidth = SizeHandler.WindowWidth - (leftPad * 2) - 6; // calclate the amount of characters to display of each messages before cropping it
            totalRows = SizeHandler.WindowHeight - 4; // calculate rows per page
            if (maxPos > totalRows)
            {
                maxPos = totalRows;
            }

            {
                double totalPagesTmp = ((double)Settings.chatMessages.Count + 1) / (double)totalRows;
                int totalPagesTmp2 = (int)Math.Ceiling(totalPagesTmp);
                totalPages = totalPagesTmp2;
            }

            int currentConsoleRow = topPad;
            int currentMessagePrint = 0;
            int startingIndex = pageToLoad * totalRows;

            // Print all messages
            foreach (var message in Settings.chatMessages)
            {
                if (startingIndex > 0)
                {
                    startingIndex--;
                    continue;
                }

                if (currentMessagePrint + 1 > totalRows)
                {
                    break;
                }

                // Limit messages to console width, crop and add an ellipsis at the end if the message is too long
                string messageOutput = message.Length > messageWidth ? message.Substring(0, messageWidth - 3) + "..." : message;
                Print.printWhenPossible(messageOutput, currentConsoleRow++, leftPad + 3, false);
                currentMessagePrint++;
            }

            // Add a button to create a new message
            if (!(currentMessagePrint + 1 > totalRows)) // +1 for "new message" row
            {
                Print.printWhenPossible("[new message]", currentConsoleRow++, leftPad + 3, false);
            }

            // Print pages count, if needed
            if (totalPages > 1)
            {
                string pagesPrint = Print.centerString("Current page: " + (pageToLoad + 1) + " / " + totalPages);
                pagesPrint = Print.replaceAt(pagesPrint, "<- previous page", leftPad + 3);
                pagesPrint = Print.replaceAt(pagesPrint, "next page ->", SizeHandler.WindowWidth - 17);
                Print.printWhenPossible(pagesPrint, SizeHandler.WindowHeight - 2, 0, false);
            }

            Print.canMovePos = true;
            Navigation.handlePointerMovementPrint();
        }

        public static void chatMessagesEdit()
        {
            Print.canMovePos = false;
            Console.Clear();
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "chatMessagesEdit";
            windowType = "messageEdit";
            topPad = SizeHandler.HeightCenter - 2;
            leftPad = SizeHandler.WidthCenter;
            maxPos = 3;

            if (Settings.chatMessages.Count > messageIndex)
            {
                Navigation.currentInput = Settings.chatMessages[messageIndex];
            }
            else
            {
                Navigation.currentInput = "";
            }

            updateMessageEdit();

            Print.printCentered("Save          Delete         Cancel", topPad + 3);

            Print.canMovePos = true;
            Navigation.handlePointerMovementPrint();
        }

        public static void updateMessageEdit()
        {
            string message = Navigation.currentInput;
            int chunkLength = 100; // Length of each chunk

            List<string> chunks = new List<string>();

            // Extract chunks from the input message
            for (int i = 0; i < message.Length; i += chunkLength)
            {
                int length = Math.Min(chunkLength, message.Length - i);
                chunks.Add(message.Substring(i, length));
            }

            string chunk1 = chunks.Count > 0 ? chunks[0] : "";
            string chunk2 = chunks.Count > 1 ? chunks[1] : "";

            // Pad the chunks with spaces, centered
            int totalPadding = (chunkLength - chunk1.Length) / 2;
            chunk1 = chunk1.PadLeft(chunk1.Length + totalPadding, ' ').PadRight(chunkLength, ' ');

            totalPadding = (chunkLength - chunk2.Length) / 2;
            chunk2 = chunk2.PadLeft(chunk2.Length + totalPadding, ' ').PadRight(chunkLength, ' ');

            Print.printCentered(chunk1, topPad);
            Print.printCentered(chunk2);
        }
    }
}
