using System;

namespace GJ2022.Utility.MathConstructs
{
    public struct Colour
    {

        private static Random random = new Random(0);

        public static Colour White { get; } = new Colour(1, 1, 1);
        public static Colour Red { get; } = new Colour(1, 0, 0);
        public static Colour Blue { get; } = new Colour(0, 1, 0);
        public static Colour Green { get; } = new Colour(0, 0, 1);
        public static Colour Yellow { get; } = new Colour(1, 1, 0);
        public static Colour Cyan { get; } = new Colour(0, 1, 1);
        public static Colour Purple { get; } = new Colour(1, 0, 1);
        public static Colour Random { get => new Colour((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()); }

        public Colour(float red = 0, float green = 0, float blue = 0, float alpha = 1.0f)
        {
            bool isRegularFormat = red + green + blue > 3;
            this.red = red / (isRegularFormat ? 256.0f : 1);
            this.green = green / (isRegularFormat ? 256.0f : 1);
            this.blue = blue / (isRegularFormat ? 256.0f : 1);
            this.alpha = alpha;
        }

        public Colour GetNormalized()
        {
            float maximum = Math.Max(1, Math.Max(red, Math.Max(green, blue)));
            Colour output = new Colour();
            output.red = red / maximum;
            output.green = green / maximum;
            output.blue = blue / maximum;
            output.alpha = Math.Min(alpha, 1);
            return output;
        }

        public static Colour operator -(Colour colour, Colour other)
        {
            Colour output = new Colour();
            output.red = colour.red - other.red;
            output.green = colour.green - other.green;
            output.blue = colour.blue - other.blue;
            output.alpha = colour.alpha - other.alpha;
            return output;
        }

        public static Colour operator +(Colour colour, Colour other)
        {
            Colour output = new Colour();
            output.red = colour.red + other.red;
            output.green = colour.green + other.green;
            output.blue = colour.blue + other.blue;
            output.alpha = colour.alpha + other.alpha;
            return output;
        }

        public static Colour operator *(float multiplier, Colour colour) => colour * multiplier;
        public static Colour operator *(Colour colour, float multiplier)
        {
            Colour output = new Colour();
            output.red = colour.red * multiplier;
            output.green = colour.green * multiplier;
            output.blue = colour.blue * multiplier;
            output.alpha = colour.alpha * multiplier;
            return output;
        }

        public override string ToString()
        {
            return $"Colour:{{{red}, {green}, {blue}, {alpha}}}";
        }

        public float red { get; set; }
        public float green { get; set; }
        public float blue { get; set; }
        public float alpha { get; set; }

    }
}
