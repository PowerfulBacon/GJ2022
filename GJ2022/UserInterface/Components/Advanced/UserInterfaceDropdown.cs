using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.UserInterface.Components.Advanced
{

    public class DropdownSettings
    {
        public bool OpenToSide { get; set; } = false;
        public float TextScale { get; set; } = CoordinateHelper.PixelsToScreen(100);
        public Colour Colour { get; set; } = new Colour(15, 38, 74);
        public Vector<float> Scale { get; set; } = CoordinateHelper.PixelsToScreen(new Vector<float>(300, 80));

        public DropdownSettings SetOpenToSide(bool value)
        {
            OpenToSide = value;
            return this;
        }

        public DropdownSettings SetTextScale(float value)
        {
            TextScale = value;
            return this;
        }

        public DropdownSettings SetColour(Colour value)
        {
            Colour = value;
            return this;
        }

        public DropdownSettings SetScale(Vector<float> value)
        {
            Scale = value;
            return this;
        }

    }

    public class UserInterfaceDropdown : UserInterfaceButton
    {

        //List of all dropdowns with no parents
        private static List<UserInterfaceDropdown> TopLevelDropdowns = new List<UserInterfaceDropdown>();

        //The settings of the dropdown menu
        private DropdownSettings settings;

        private bool toggled = false;

        private List<UserInterfaceComponent> DropdownComponents { get; } = new List<UserInterfaceComponent>();

        private float cachedHeight = 0;

        public override UserInterfaceComponent Parent
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                if (value == null && TopLevelDropdowns.Contains(this))
                    TopLevelDropdowns.Remove(this);
                else if (value != null && !TopLevelDropdowns.Contains(this))
                    TopLevelDropdowns.Add(this);
            }
        }

        public UserInterfaceDropdown(Vector<float> position, string text, DropdownSettings settings) : base(position, settings.Scale, text, settings.TextScale, settings.Colour)
        {
            this.settings = settings;
            onButtonPressed = Toggle;
            TopLevelDropdowns.Add(this);
        }

        public void AddDropdownComponent(UserInterfaceComponent component)
        {
            if (settings.OpenToSide)
            {
                //We open to the side
                component.Position = new Vector<float>(Position[0] + (settings.Scale[0] + component.Scale[0]) * 0.5f, Position[1] + cachedHeight);
                cachedHeight += component.Scale[1];
            }
            else
            {
                //We open above.
                cachedHeight += component.Scale[1];
                component.Position = new Vector<float>(Position[0], Position[1] + cachedHeight);
            }
            //Hide components if the dropdown is toggled off.
            if (!toggled)
            {
                component.Hide();
            }
            //Add to the list of tracked dropdown components
            component.Parent = this;
            DropdownComponents.Add(component);
        }

        public override Vector<float> Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                UpdateChildrenPosition();
            }
        }

        private void UpdateChildrenPosition()
        {
            cachedHeight = 0;
            foreach (UserInterfaceComponent component in DropdownComponents)
            {
                if (settings.OpenToSide)
                {
                    //We open to the side
                    component.Position = new Vector<float>(Position[0] + (settings.Scale[0] + component.Scale[0]) * 0.5f, Position[1] + cachedHeight);
                    cachedHeight += component.Scale[1];
                }
                else
                {
                    //We open above.
                    cachedHeight += component.Scale[1];
                    component.Position = new Vector<float>(Position[0], Position[1] + cachedHeight);
                }
            }
        }

        /// <summary>
        /// Closes the dropdown menus of all children
        /// </summary>
        public void CloseAllChildrenDropdownsExcluding(UserInterfaceComponent exclusion)
        {
            foreach (UserInterfaceComponent component in DropdownComponents)
            {
                UserInterfaceDropdown dropdown = component as UserInterfaceDropdown;
                if (dropdown == null || component == exclusion)
                    continue;
                dropdown.Close();
            }
        }

        private void Toggle()
        {
            Log.WriteLine("toggled.");
            if (toggled)
                Close();
            else
                Open();
        }

        private void CloseAllTopLevelDropdownsExcluding(UserInterfaceDropdown exclusion)
        {
            foreach (UserInterfaceDropdown dropdown in TopLevelDropdowns)
            {
                if(dropdown != exclusion)
                    dropdown.Close();
            }
        }

        /// <summary>
        /// Open the dropdown menu and create the subbuttons.
        /// </summary>
        private void Open()
        {
            if (toggled)
                return;
            toggled = true;
            //Close children dropdowns
            if (Parent == null)
                CloseAllTopLevelDropdownsExcluding(this);
            else
                (Parent as UserInterfaceDropdown)?.CloseAllChildrenDropdownsExcluding(this);
            //Open our dropdowns
            foreach (UserInterfaceComponent component in DropdownComponents)
            {
                component.Show();
            }
        }

        /// <summary>
        /// Close the dropdown menu, delete all subbuttons
        /// </summary>
        private void Close()
        {
            if (!toggled)
                return;
            toggled = false;
            foreach (UserInterfaceComponent component in DropdownComponents)
            {
                component.Hide();
            }
        }

        public override void Hide()
        {
            base.Hide();
            Close();
        }

        public override void Show()
        {
            base.Show();
        }

    }
}
