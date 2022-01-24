using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.Text
{

    public class TextCharacter
    {

        //Offset values of the sprite in the sprite sheet
        public int spriteX;
        public int spriteY;

        //The width of the character sprite.
        public int base_width;
        //The offset of the width. (Distance between 2 characters)
        public int width_offset;
        //X offset of the character sprite
        public int x_offset;
        //Y offset of the character sprite
        public int y_offset;

    }

}
