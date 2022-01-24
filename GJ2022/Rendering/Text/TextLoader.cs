using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Rendering.Text
{
    public static class TextLoader
    {

        public const int textHeight = 32;

        public static Dictionary<int, TextCharacter> textCharacters = new Dictionary<int, TextCharacter>();

        public static void LoadText()
        {
            //Load the data file and process
            IEnumerable<string> fileContents = File.ReadLines($"Data/FontData.csv");
            //Go through each line in the file
            foreach (string line in fileContents)
            {
                //If the line isn't a character, ignore it
                if (!line.StartsWith("Char"))
                    continue;
                //Get the character ID (Unicode / Ascii character ID)
                int character_id;
                if (!int.TryParse(line.Split(' ')[1], out character_id))
                    continue;
                //Get the value of the variable from the line
                int value;
                if (!int.TryParse(line.Split(',')[1], out value))
                    continue;
                //Get the name of the variable from the line.
                string variable = line.Substring(line.IndexOf(" ", 5) + 1, line.IndexOf(",") - line.IndexOf(" ", 5) - 1);
                TextCharacter textCharacter;
                //Find or create text character
                if (textCharacters.ContainsKey(character_id))
                {
                    //Text character already exists, update existing variables
                    textCharacter = textCharacters[character_id];
                }
                else
                {
                    //Text character is not in the static dictionary of characters, create a new one and add it
                    textCharacter = new TextCharacter();
                    textCharacter.spriteX = character_id % 16;
                    textCharacter.spriteY = character_id / 16;
                    textCharacters.Add(character_id, textCharacter);
                }
                //Load the variable
                switch (variable)
                {
                    //Sets the width offset
                    case "Width Offset":
                        textCharacter.width_offset = value;
                        break;
                    //Sets the base width of the character
                    case "Base Width":
                        textCharacter.base_width = value;
                        break;
                    //Sets the X offset of the character from (0,0)
                    case "X Offset":
                        textCharacter.x_offset = value;
                        break;
                    //Sets the Y offset of the character from (0,0)
                    case "Y Offset":
                        textCharacter.y_offset = value;
                        break;
                }
            }
            //All characters loaded.
            Log.WriteLine($"Successfully loaded {textCharacters.Count} characters.");
        }

    }
}
