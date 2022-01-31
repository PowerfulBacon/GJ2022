﻿using GJ2022.Entities.Items;
using GJ2022.Rendering.Text;
using GJ2022.UserInterface.Components.Advanced;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;

namespace GJ2022.Managers.Stockpile
{
    public static class StockpileUserInterfaceManager
    {

        //List of UI text objects currently being displayed
        public static Dictionary<Type, UserInterfaceTextIcon> displayedUiComponents = new Dictionary<Type, UserInterfaceTextIcon>();

        public static void UpdateUserInterface(Item item, Type type, int amount)
        {
            if (amount == 0)
            {
                if (displayedUiComponents.ContainsKey(type))
                {
                    displayedUiComponents[type].Hide();
                    displayedUiComponents.Remove(type);
                    int i = 0;
                    foreach (Type uiType in displayedUiComponents.Keys)
                    {
                        displayedUiComponents[uiType].Position = CoordinateHelper.PixelsToScreen(-1920 + 60, 1080 - 70 - 100 * i);
                        i++;
                    }
                }
                return;
            }
            if (displayedUiComponents.ContainsKey(item.GetType()))
            {
                displayedUiComponents[item.GetType()].Text.Text = $"{item.Name} x{amount}";
            }
            else
            {
                displayedUiComponents.Add(item.GetType(), new UserInterfaceTextIcon(
                    $"{item.Name} x{amount}",
                    item.UiTexture,
                    Colour.White,
                    CoordinateHelper.PixelsToScreen(-1920 + 60, 1080 - 70 - 100 * displayedUiComponents.Count),
                    CoordinateHelper.PixelsToScreen(100, 100),
                    TextObject.PositionModes.SCREEN_POSITION,
                    CoordinateHelper.PixelsToScreen(100),
                    10));
            }
        }

    }
}
