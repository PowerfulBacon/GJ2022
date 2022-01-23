using GJ2022.Utility.MathConstructs;

namespace GJ2022.Rendering.RenderSystems.LineRenderer
{
    public class Line
    {

        private Vector _start;
        public Vector Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                //Calculate delta
                Vector delta = End - Start;
                //Calculate object matrix
                ObjectMatrix = Matrix.GetTranslationMatrix(-Start[0], -Start[1], -Start[2]) * Matrix.GetScaleMatrix(delta[0], delta[1], delta[2]);
            }
        }

        private Vector _end;
        public Vector End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                //Calculate delta
                Vector delta = End - Start;
                //Calculate object matrix
                ObjectMatrix = Matrix.GetTranslationMatrix(-Start[0], -Start[1], -Start[2]) * Matrix.GetScaleMatrix(delta[0], delta[1], delta[2]);
            }
        }

        public Colour Colour { get; set; }

        public Matrix ObjectMatrix { get; private set; }

        public static Line StartDrawingLine(Vector start, Vector end, Colour? colour = null)
        {
            Colour lineColour;
            if (colour == null)
                lineColour = Colour.White;
            else
                lineColour = (Colour)colour;
            //Create the line object to render
            Line line = new Line(start, end, lineColour);
            //Add the line to the line renderer
            LineRenderer.Singleton.StartRendering(line);
            //Return the created line
            return line;
        }

        public Line(Vector start, Vector end, Colour colour)
        {
            _start = start;
            _end = end;
            Colour = colour;
            //Calculate delta
            Vector delta = End - Start;
            //Calculate object matrix
            ObjectMatrix = Matrix.GetTranslationMatrix(-Start[0], -Start[1], -Start[2]) * Matrix.GetScaleMatrix(delta[0], delta[1], delta[2]);
        }

        public void StopDrawing()
        {
            //Stop rendering this line
            LineRenderer.Singleton.StopRendering(this);
        }

    }
}
