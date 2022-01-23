using GJ2022.Rendering.Shaders;

namespace GJ2022.Rendering.Models
{
    public class BlockModelData : ModelData
    {

        //Singleton for the block model data
        //Interestingly C# only creates the singleton when we reference it for
        //the first time.
        public static BlockModelData Singleton { get; } = new BlockModelData();

        private Model topFace;
        private Model bottomFace;
        private Model frontFace;
        private Model backFace;
        private Model rightFace;
        private Model leftFace;

        /// <summary>
        /// Since we are a block, we can load a generic mesh.
        /// </summary>
        private BlockModelData() : base(new ShaderSet("instanceShader"), null)
        {

            //Depreciated in this project, we are using 2D
            Log.WriteLine("Warning: A block model was created despite using a 2D rendering system", LogType.WARNING);

            //====================
            //Setup the above face
            //====================
            topFace = new Model(
                new float[] {
                    0.5f, 0.5f, 0.5f,        //(1, 1)
                    0.5f, 0.5f, -0.5f,       //Bottom Right
                    -0.5f, 0.5f, -0.5f,      //(-1, -1)
                    -0.5f, 0.5f, -0.5f,      //Bottom Left
                    -0.5f, 0.5f, 0.5f,       //(-1, 1)
                    0.5f, 0.5f, 0.5f,         //Top Right
                },
                new float[] {
                    0.0f, 0.0f,
                    1.0f, 0.0f,
                    1.0f, 1.0f,
                    1.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 0.0f
                });
            //====================
            //Setup the below face
            //====================
            bottomFace = new Model(
                new float[] {
                    -0.5f, -0.5f, -0.5f,      //Bottom Left
                    0.5f, -0.5f, -0.5f,       //Bottom Right
                    0.5f, -0.5f, 0.5f,        //Top Right
                    0.5f, -0.5f, 0.5f,         //Top Right
                    -0.5f, -0.5f, 0.5f,       //Top Left
                    -0.5f, -0.5f, -0.5f,      //Bottom Left
                },
                new float[] {
                    0.0f, 0.0f,
                    0.0f, 1.0f,
                    1.0f, 1.0f,
                    1.0f, 1.0f,
                    1.0f, 0.0f,
                    0.0f, 0.0f
                });
            //====================
            //Setup the front face
            //====================
            frontFace = new Model(
                new float[] {
                    0.5f, -0.5f, -0.5f,      //(-1, -1)
                    0.5f, 0.5f, -0.5f,       //(1, -1)
                    0.5f, 0.5f, 0.5f,        //(1, 1)
                    0.5f, 0.5f, 0.5f,         //(1, 1)
                    0.5f, -0.5f, 0.5f,       //(-1, 1)
                    0.5f, -0.5f, -0.5f,      //(-1, -1)
                },
                new float[] {
                    1.0f, 0.0f,
                    0.0f, 0.0f,
                    0.0f, 1.0f,
                    0.0f, 1.0f,
                    1.0f, 1.0f,
                    1.0f, 0.0f
                });
            //====================
            //Setup the back face
            //====================
            backFace = new Model(
                new float[] {
                    -0.5f, 0.5f, 0.5f,        //(1, 1)
                    -0.5f, 0.5f, -0.5f,       //(1, -1)
                    -0.5f, -0.5f, -0.5f,      //(-1, -1)
                    -0.5f, -0.5f, -0.5f,      //(-1, -1)
                    -0.5f, -0.5f, 0.5f,       //(-1, 1)
                    -0.5f, 0.5f, 0.5f,         //(1, 1)
                },
                new float[] {
                    1.0f, 1.0f,
                    1.0f, 0.0f,
                    0.0f, 0.0f,
                    0.0f, 0.0f,
                    0.0f, 1.0f,
                    1.0f, 1.0f
                });
            //====================
            //Setup the right face
            //====================
            rightFace = new Model(
                new float[] {
                    -0.5f, -0.5f, 0.5f,      //(-1, -1)
                    0.5f, -0.5f, 0.5f,       //(1, -1)
                    0.5f, 0.5f, 0.5f,        //(1, 1)
                    0.5f, 0.5f, 0.5f,         //(1, 1)
                    -0.5f, 0.5f, 0.5f,       //(-1, 1)
                    -0.5f, -0.5f, 0.5f,      //(-1, -1)
                },
                new float[] {
                    0.0f, 0.0f,
                    0.0f, 1.0f,
                    1.0f, 1.0f,
                    1.0f, 1.0f,
                    1.0f, 0.0f,
                    0.0f, 0.0f
                });
            //====================
            //Setup the left face
            //====================
            leftFace = new Model(
                new float[] {
                    0.5f, 0.5f, -0.5f,        //(1, 1)
                    0.5f, -0.5f, -0.5f,       //(1, -1)
                    -0.5f, -0.5f, -0.5f,      //(-1, -1)
                    -0.5f, -0.5f, -0.5f,      //(-1, -1)
                    -0.5f, 0.5f, -0.5f,       //(-1, 1)
                    0.5f, 0.5f, -0.5f,         //(1, 1)
                },
                new float[] {
                    1.0f, 0.0f,
                    1.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 0.0f,
                    1.0f, 0.0f
                });
        }

    }
}
