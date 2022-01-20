using GJ2022.Rendering.RenderSystems.RenderData;
using System.Collections.Generic;

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
        private BlockModelData() : base(null)
        {
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

        /// <summary>
        /// Get the face we want
        /// </summary>
        public override RenderableData[] GetModelRenderableData(Renderable renderable, CubeFaceFlags faceFlag)
        {
            List<RenderableData> models = new List<RenderableData>();
            if ((faceFlag & CubeFaceFlags.FACE_ABOVE) == CubeFaceFlags.FACE_ABOVE)
                models.Add(new RenderableData(topFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_ABOVE)));
            if ((faceFlag & CubeFaceFlags.FACE_BELOW) == CubeFaceFlags.FACE_BELOW)
                models.Add(new RenderableData(bottomFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_BELOW)));
            if ((faceFlag & CubeFaceFlags.FACE_FRONT) == CubeFaceFlags.FACE_FRONT)
                models.Add(new RenderableData(frontFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_FRONT)));
            if ((faceFlag & CubeFaceFlags.FACE_BACK) == CubeFaceFlags.FACE_BACK)
                models.Add(new RenderableData(backFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_BACK)));
            if ((faceFlag & CubeFaceFlags.FACE_RIGHT) == CubeFaceFlags.FACE_RIGHT)
                models.Add(new RenderableData(rightFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_RIGHT)));
            if ((faceFlag & CubeFaceFlags.FACE_LEFT) == CubeFaceFlags.FACE_LEFT)
                models.Add(new RenderableData(leftFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_LEFT)));
            return models.ToArray();
        }

        public override RenderableData[] GetAllModelRenderableData(Renderable renderable)
        {
            return new RenderableData[]
            {
                new RenderableData(topFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_ABOVE)),
                new RenderableData(bottomFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_BELOW)),
                new RenderableData(frontFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_FRONT)),
                new RenderableData(backFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_BACK)),
                new RenderableData(rightFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_RIGHT)),
                new RenderableData(leftFace, renderable, renderable.GetRendererTexture(CubeFaceFlags.FACE_LEFT)),
            };
        }

    }
}
