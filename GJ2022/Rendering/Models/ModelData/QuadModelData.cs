using GJ2022.Rendering.Shaders;

namespace GJ2022.Rendering.Models
{
    public class QuadModelData : ModelData
    {

        //Singleton for the block model data
        //Interestingly C# only creates the singleton when we reference it for
        //the first time.
        public static QuadModelData Singleton { get; } = new QuadModelData();

        private QuadModelData() : base(
            new ShaderSet("instanceShader"),
            new Model(
                new float[] {
                    0.5f, 0.5f, 0,        //(1, 1)
                    0.5f, -0.5f, 0,       //Bottom Right
                    -0.5f, -0.5f, 0,      //(-1, -1)
                    -0.5f, -0.5f, 0,      //Bottom Left
                    -0.5f, 0.5f, 0,       //(-1, 1)
                    0.5f, 0.5f, 0,         //Top Right
                },
                new float[] {
                    0.0f, 0.0f,
                    1.0f, 0.0f,
                    1.0f, 1.0f,
                    1.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 0.0f
                })
            )
        { }

    }
}
