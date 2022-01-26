using GJ2022.Entities.Pawns;
using GJ2022.Game.Construction;
using GJ2022.Game.Construction.BlueprintSets;
using GJ2022.Managers.Stockpile;
using GJ2022.Rendering.Text;
using GJ2022.Subsystems;
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

            DropdownSettings defaultDropdownSettings = new DropdownSettings();
            DropdownSettings sideDropdownSettings = new DropdownSettings();
            sideDropdownSettings.OpenToSide = true;

            //Create the building dropdown
            UserInterfaceDropdown buildingDropdown = new UserInterfaceDropdown(
                CoordinateHelper.PixelsToScreen(-1920 + 150, -1080 + 40),
                "Building",
                defaultDropdownSettings
                );

            foreach (BlueprintCategory category in BlueprintLoader.BlueprintCategories.Values)
            {
                UserInterfaceDropdown dropdownOption = new UserInterfaceDropdown(
                    new Vector<float>(0, 0),
                    category.Name,
                    new DropdownSettings()
                        .SetOpenToSide(true)
                    );
                //Add category buttons
                foreach (BlueprintSet blueprintSet in category.Contents)
                {
                    UserInterfaceButton button = new UserInterfaceButton(
                        new Vector<float>(0, 0),
                        CoordinateHelper.PixelsToScreen(500, 80),
                        blueprintSet.Name,
                        CoordinateHelper.PixelsToScreen(100),
                        Colour.UserInterfaceColour
                        );
                    button.onButtonPressed = () => BuildModeSubsystem.Singleton.ActivateBuildMode(blueprintSet);
                    dropdownOption.AddDropdownComponent(button);
                }
                //Add to the master dropdown
                buildingDropdown.AddDropdownComponent(dropdownOption);
            }

            DropdownFactory.CreateDropdown(
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 450, -1080 + 40)),
                "Debug",
                new string[] { "Draw path lines" },
                new OnButtonPressed[] { () => { Pawn.DrawLines = !Pawn.DrawLines; } });
        }

    }
}
