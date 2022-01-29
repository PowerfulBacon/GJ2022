using GJ2022.Game.GameWorld;
using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GJ2022.Tests.RendererTests.RenderBatchTests
{

    public class TestTexture : Texture
    {

        public override void ReadTexture(string fileName)
        {
            Log.WriteLine("!!! USING SAMPLE TEST TEXTURE. READING TEST TEXTURE DOES NOTHING AND SHOULD ONLY BE USED FOR TESTS. !!!");
            return;
        }

    }

    public class TestRenderable : IStandardRenderable
    {
        private int sample;

        public RenderSystem<IStandardRenderable, InstanceRenderSystem> RenderSystem => InstanceRenderSystem.Singleton;

        public Directions Direction => Directions.NONE;

        public Vector<float> GetInstancePosition() { return new Vector<float>(0, 0, 0); }

        public Colour GetLightingColour()
        {
            return new Colour();
        }

        public void SetRenderableBatchIndex(object associatedSet, int index)
        {
            sample = index;
        }

        public int GetRenderableBatchIndex(object associatedSet)
        {
            return sample;
        }

        public Vector<float> GetInstanceScale()
        {
            return new Vector<float>(1, 1);
        }

        public RendererTextureData GetRendererTextureData()
        {
            throw new NotImplementedException();
        }

        public Model GetModel()
        {
            throw new NotImplementedException();
        }

        public uint GetTextureUint()
        {
            throw new NotImplementedException();
        }

        public Vector<float> GetPosition()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class TestRenderBatches
    {

        private RendererTextureData sampleTextureData = new RendererTextureData(new TestTexture(), new TextureJson("", 0, 0, 0, 0));

        [TestMethod]
        public void TestBasicAdding()
        {
            RenderBatchSet<IStandardRenderable, InstanceRenderSystem> set = new RenderBatchSet<IStandardRenderable, InstanceRenderSystem>(sampleTextureData, 0, new int[] { });
            Assert.AreEqual(0, set.renderElements, "Set should contain no render elements");
            IStandardRenderable testRenderable = new TestRenderable();
            set.AddToBatch(testRenderable, sampleTextureData);
            Assert.AreEqual(1, set.renderElements, "Set should contain 1 render elements");
        }

        [TestMethod]
        public void TestBasicRemoving()
        {
            RenderBatchSet<IStandardRenderable, InstanceRenderSystem> set = new RenderBatchSet<IStandardRenderable, InstanceRenderSystem>(sampleTextureData, 0, new int[] { });
            Assert.AreEqual(0, set.renderElements, "Set should contain no render elements");
            IStandardRenderable testRenderable = new TestRenderable();
            set.AddToBatch(testRenderable, sampleTextureData);
            Assert.AreEqual(1, set.renderElements, "Set should contain 1 render elements");
            IStandardRenderable testRenderable2 = new TestRenderable();
            set.AddToBatch(testRenderable2, sampleTextureData);
            Assert.AreEqual(2, set.renderElements, "Set should contain 2 render elements");
            Assert.AreEqual(testRenderable, set.renderBatches[0].instanceRenderables[0], "Instance 1 should be stored at 0");
            Assert.AreEqual(testRenderable2, set.renderBatches[0].instanceRenderables[1], "Instance 2 should be stored at 1");
            set.RemoveFromBatch(testRenderable);
            Assert.AreEqual(1, set.renderElements, "Set should contain 1 render elements");
            Assert.AreEqual(testRenderable2, set.renderBatches[0].instanceRenderables[0], "Instance 2 should be stored at 0 after removal");
        }

        [TestMethod]
        public void TestClearing()
        {
            RenderBatchSet<IStandardRenderable, InstanceRenderSystem> set = new RenderBatchSet<IStandardRenderable, InstanceRenderSystem>(sampleTextureData, 0, new int[] { });
            Assert.AreEqual(0, set.renderElements, "Set should contain no render elements");
            IStandardRenderable testRenderable = new TestRenderable();
            set.AddToBatch(testRenderable, sampleTextureData);
            Assert.AreEqual(1, set.renderElements, "Set should contain 1 render elements");
            IStandardRenderable testRenderable2 = new TestRenderable();
            set.AddToBatch(testRenderable2, sampleTextureData);
            Assert.AreEqual(2, set.renderElements, "Set should contain 2 render elements");
            set.RemoveFromBatch(testRenderable);
            Assert.AreEqual(1, set.renderElements, "Set should contain 1 render elements");
            set.RemoveFromBatch(testRenderable2);
            Assert.AreEqual(0, set.renderElements, "Set should contain 0 render elements");
        }

        [TestMethod]
        public void TestBatchOverflowing()
        {
            RenderBatchSet<IStandardRenderable, InstanceRenderSystem> set = new RenderBatchSet<IStandardRenderable, InstanceRenderSystem>(sampleTextureData, 0, new int[] { });

            //Fill with 25000 elements

            //THIS IS REUSED, USING THIS WILL CAUSE PROBLEMS AND IS NOT THE POINT OF THIS TEST
            IStandardRenderable reusableTestRenderable = new TestRenderable();

            for (int i = 0; i < RenderBatch<IStandardRenderable, InstanceRenderSystem>.MAX_BATCH_SIZE; i++)
            {
                set.AddToBatch(reusableTestRenderable, sampleTextureData);
            }

            Assert.AreEqual(RenderBatch<IStandardRenderable, InstanceRenderSystem>.MAX_BATCH_SIZE, set.renderElements, $"There should be {RenderBatch<IStandardRenderable, InstanceRenderSystem>.MAX_BATCH_SIZE} items inside the render batch.");
            Assert.AreEqual(1, set.renderBatches.Count, "There should only be 1 render batch");

            //Cause an overflow
            IStandardRenderable overflowingElement = new TestRenderable();
            set.AddToBatch(overflowingElement, sampleTextureData);

            Assert.AreEqual(RenderBatch<IStandardRenderable, InstanceRenderSystem>.MAX_BATCH_SIZE + 1, set.renderElements, $"There should be {RenderBatch<IStandardRenderable, InstanceRenderSystem>.MAX_BATCH_SIZE + 1} items inside the render batch.");
            Assert.AreEqual(2, set.renderBatches.Count, "Due to an overflow, there should be 2 render batches");

            //Remove it
            set.RemoveFromBatch(overflowingElement);

            Assert.AreEqual(RenderBatch<IStandardRenderable, InstanceRenderSystem>.MAX_BATCH_SIZE, set.renderElements, "Testing correct batch size");
            Assert.AreEqual(1, set.renderBatches.Count, "Testing correct batch count: Extra batch should have been deleted");

        }

    }
}
