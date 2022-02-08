using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Sebastien.Guidez.FeatureMatching.Tests
{

    public class FeatureMatchingUnitTest
    {
        [Fact]
        public async Task ObjectShouldBeDetectedCorrectly()
        {
            var executingPath = GetExecutingPath();
            var imageScenesData = new List<byte[]>();
            foreach (var imagePath in
                Directory.EnumerateFiles(Path.Combine(executingPath, "Scenes")))
            {
                var imageBytes = await File.ReadAllBytesAsync(imagePath);
                imageScenesData.Add(imageBytes);
            }

            var objectImageData = await
                File.ReadAllBytesAsync(Path.Combine(executingPath, "Guidez-Sebastien-object.jpg"));
            var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenes(objectImageData, imageScenesData);
            Assert.Equal("[{\"X\":0,\"Y\":0},{\"X\":0,\"Y\":4030},{\"X\":3022,\"Y\":4030},{\"X\":3023,\"Y\":0}]",
                JsonSerializer.Serialize(detectObjectInScenesResults[0].Points));

            Assert.Equal("[{\"X\":0,\"Y\":0},{\"X\":0,\"Y\":4030},{\"X\":3022,\"Y\":4030},{\"X\":3023,\"Y\":0}]",
                JsonSerializer.Serialize(detectObjectInScenesResults[1].Points));
        }

        private static string GetExecutingPath()
        {
            var executingAssemblyPath =
                Assembly.GetExecutingAssembly().Location;
            var executingPath = Path.GetDirectoryName(executingAssemblyPath);
            return executingPath;
        }

    }
}