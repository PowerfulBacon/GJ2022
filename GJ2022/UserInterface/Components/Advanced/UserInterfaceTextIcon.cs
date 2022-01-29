using GJ2022.Rendering.Text;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.UserInterface.Components.Advanced
{
    public class UserInterfaceTextIcon : UserInterfaceImage
    {

        public TextObject Text { get; }

        private Vector<float> _scale;

        public override Vector<float> Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                Text.Position = value + new Vector<float>(_scale[0], 0);
            }
        }

        public UserInterfaceTextIcon(string text, string icon, Colour colour, Vector<float> position, Vector<float> scale, TextObject.PositionModes positionMode = TextObject.PositionModes.SCREEN_POSITION, float fontSize = 1, float width = 10)
            : base(position, scale, icon)
        {
            _scale = scale;
            Text = new TextObject(text, colour, position + new Vector<float>(scale[0], 0), positionMode, fontSize, width);
        }

        public override void Hide()
        {
            base.Hide();
            Text.StopRendering();
        }

        public override void Show()
        {
            base.Show();
            Text.StartRendering();
        }

    }
}
