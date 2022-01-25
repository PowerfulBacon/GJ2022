using GJ2022.Rendering.Text;
using GJ2022.UserInterface.Components;
using GJ2022.UserInterface.Components.Advanced;
using GJ2022.UserInterface.Factory;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using static GJ2022.UserInterface.Components.UserInterfaceButton;

namespace GJ2022.UserInterface
{
    public static class UserInterfaceCreator
    {

        public static void CreateUserInterface()
        {

            new UserInterfaceTextIcon(
                "Iron - 50",
                "iron",
                Colour.White,
                CoordinateHelper.PixelsToScreen(-1920 + 60, 1080 - 70),
                CoordinateHelper.PixelsToScreen(100, 100),
                TextObject.PositionModes.SCREEN_POSITION,
                CoordinateHelper.PixelsToScreen(100),
                10);

            DropdownSettings defaultDropdownSettings = new DropdownSettings();
            DropdownSettings sideDropdownSettings = new DropdownSettings();
            sideDropdownSettings.OpenToSide = true;

            //Create the building dropdown
            UserInterfaceDropdown buildingDropdown = new UserInterfaceDropdown(
                CoordinateHelper.PixelsToScreen(-1920 + 150, -1080 + 40),
                "Building",
                defaultDropdownSettings
                );

            //============================
            //Create the options for Zones
            //============================
            UserInterfaceDropdown zoneDropdown = new UserInterfaceDropdown(
                new Vector<float>(0, 0),
                "Zones",
                new DropdownSettings()
                    .SetOpenToSide(true)
                );
            //Add all the possible structure options
            zoneDropdown.AddDropdownComponent(new UserInterfaceButton(
                new Vector<float>(0, 0),
                CoordinateHelper.PixelsToScreen(500, 80),
                "Stockpile Zone",
                CoordinateHelper.PixelsToScreen(100),
                Colour.UserInterfaceColour
                ));

            //============================
            //Create the options for structures
            //============================
            UserInterfaceDropdown structureDropdown = new UserInterfaceDropdown(
                new Vector<float>(0, 0),
                "Structure",
                new DropdownSettings()
                    .SetOpenToSide(true)
                );
            //Add all the possible structure options
            structureDropdown.AddDropdownComponent(new UserInterfaceButton(
                new Vector<float>(0, 0),
                CoordinateHelper.PixelsToScreen(500, 80),
                "Steel Foundations",
                CoordinateHelper.PixelsToScreen(100),
                Colour.UserInterfaceColour
                ));
            structureDropdown.AddDropdownComponent(new UserInterfaceButton(
                new Vector<float>(0, 0),
                CoordinateHelper.PixelsToScreen(500, 80),
                "Debugium Foundations",
                CoordinateHelper.PixelsToScreen(100),
                Colour.UserInterfaceColour
                ));

            //============================
            //Add all the dropdown options to the parent building dropdown
            //============================
            buildingDropdown.AddDropdownComponent(zoneDropdown);
            buildingDropdown.AddDropdownComponent(structureDropdown);


            DropdownFactory.CreateDropdown(
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 450, -1080 + 40)),
                "Debug",
                new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "1", "2", "3", "4", "5", "6", "7", "8" },
                new OnButtonPressed[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });
        }

    }
}
