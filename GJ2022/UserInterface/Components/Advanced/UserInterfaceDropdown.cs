using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.UserInterface.Components.Advanced
{
    public class UserInterfaceDropdown : UserInterfaceButton
    {

        private bool toggled = false;
        private List<UserInterfaceComponent> DropdownComponents { get; } = new List<UserInterfaceComponent>();
        private float cachedHeight = 0;

        public UserInterfaceDropdown(Vector<float> position, Vector<float> scale, string text, float textScale, Colour colour) : base(position, scale, text, textScale, colour)
        {
            onButtonPressed = Toggle;
        }

        public void AddDropdownComponent(UserInterfaceComponent component)
        {
            cachedHeight += CoordinateHelper.PixelsToScreen(80);
            component.Position = new Vector<float>(Position[0], Position[1] + cachedHeight);
            if (!toggled)
            {
                //Hide if we are toggled off.
                component.Hide();
            }
            DropdownComponents.Add(component);
        }

        private void Toggle()
        {
            Log.WriteLine("toggled.");
            if (toggled)
                Close();
            else
                Open();
        }

        /// <summary>
        /// Open the dropdown menu and create the subbuttons.
        /// </summary>
        private void Open()
        {
            if (toggled)
                return;
            toggled = true;
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
        }

        public override void Show()
        {
            base.Show();
        }

    }
}
