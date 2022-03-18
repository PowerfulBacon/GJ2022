using GJ2022.Entities;
using GJ2022.Entities.Blueprints;
using GJ2022.Entities.Pawns;
using GJ2022.EntityLoading;
using GJ2022.Game.Construction;
using GJ2022.Game.Construction.Blueprints;
using GJ2022.Game.Construction.BlueprintSets;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering;
using GJ2022.Rendering.Text;
using GJ2022.Subsystems;
using GJ2022.UserInterface.Components;
using GJ2022.UserInterface.Components.Advanced;
using GJ2022.UserInterface.Factory;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GJ2022.UserInterface.Components.UserInterfaceButton;

namespace GJ2022.UserInterface
{
    public static class UserInterfaceCreator
    {

        private static UserInterfaceDropdown dropdown;

        public static TextObject SelectorTextObject;

        public static TextObject DeletionInformation;

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

            SelectorTextObject = new TextObject("Selected Pawn: N/A", Colour.White, CoordinateHelper.PixelsToScreen(0, 1000), TextObject.PositionModes.SCREEN_POSITION, CoordinateHelper.PixelsToScreen(120));

            DeletionInformation = new TextObject("", Colour.White, CoordinateHelper.PixelsToScreen(-800, 1000), TextObject.PositionModes.SCREEN_POSITION, CoordinateHelper.PixelsToScreen(120));

            Task.Run(() => {
                while (Subsystem.Firing)
                {
                    DeletionInformation.Text = $"C: {World.EntitiesCreated}, D: {World.EntitiesGarbageCollected}/{World.EntitiesDestroyed}";
                    Thread.Sleep(1000);
                }
            });

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

            CreateEntitySpawnDD();

            DropdownFactory.CreateDropdown(
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 450, -1080 + 40)),
                "Debug",
                new string[] {
                    "Draw path lines",
                    "Reload EntityDefs",
                    "Print Trackers",
                },
                new OnButtonPressed[] {
                    () => { Pawn.DrawLines = !Pawn.DrawLines; },
                    () => {
                        EntityLoader.LoadEntities();
                        CreateEntitySpawnDD();
                    },
                    () => {
                        foreach(string key in World.Current.TrackedComponentHandlers.Keys)
                        {
                            Log.WriteLine($"{key} => {World.Current.TrackedComponentHandlers[key]}", LogType.DEBUG);
                        }
                    },
                });
        }

        private static void CreateEntitySpawnDD()
        {
            if (dropdown != null)
            {
                dropdown.Hide();
                dropdown = null;
            }

            List<string> buttonNames = new List<string>();
            List<OnButtonPressed> buttonPressed = new List<OnButtonPressed>();

            foreach (string entityDef in EntityConfig.LoadedEntityDefs.Keys)
            {
                if (EntityConfig.LoadedEntityDefs[entityDef].Tags.ContainsKey("Abstract"))
                    continue;
                buttonNames.Add(entityDef);
                buttonPressed.Add(() => {
                    EntityCreator.CreateEntity<Entity>(entityDef, new Vector<float>(
                        (int)(RenderMaster.mainCamera.ViewMatrix[4, 1] / RenderMaster.mainCamera.ViewMatrix[1, 1]),
                        (int)(RenderMaster.mainCamera.ViewMatrix[4, 2] / RenderMaster.mainCamera.ViewMatrix[2, 2])
                        ));
                });
            }

            dropdown = DropdownFactory.CreateDropdown(
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 750, -1080 + 40)),
                "Spawn Entity",
                buttonNames.ToArray(),
                buttonPressed.ToArray());
        }

    }
}
