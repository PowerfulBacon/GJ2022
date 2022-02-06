using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Items.Clothing
{
    public enum ClothingFlags
    {
        NONE = 0,
        HIDE_HAIR = 1 << 0,
        HIDE_EYES = 1 << 1,
        HIDE_TAIL = 1 << 3
    }
}
