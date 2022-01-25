using GJ2022.UserInterface.Components;
using GJ2022.UserInterface.Components.Advanced;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GJ2022.UserInterface.Components.UserInterfaceButton;

namespace GJ2022.UserInterface.Factory
{
    public static class DropdownFactory
    {

        public static UserInterfaceDropdown CreateDropdown(
            Vector<float> position,
            string dropdownTitle,
            string[] options,
            OnButtonPressed[] actions,
            DropdownSettings settings = null)
        {
            UserInterfaceDropdown dropdown = new UserInterfaceDropdown(
                position,
                dropdownTitle,
                settings ?? new DropdownSettings());
            for (int i = 0; i < options.Length; i++)
            {
                UserInterfaceButton button = new UserInterfaceButton(
                    position,
                    CoordinateHelper.PixelsToScreen(new Vector<float>(300, 80)),
                    options[i],
                    CoordinateHelper.PixelsToScreen(100),
                    new Colour(15, 38, 74));
                button.onButtonPressed = actions[i];
                dropdown.AddDropdownComponent(button);
            }
            return dropdown;
        }

    }
}
