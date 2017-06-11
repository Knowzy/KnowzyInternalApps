using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Cognitive.CustomVision;
using Microsoft.Cognitive.CustomVision.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CustomVisionTrainer
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Custom Vision Image Trainer V1.0");
            if (args.Length < 4 || args.Contains("-?"))
            {
                Console.WriteLine("Usage: customvisiontrainer.exe {custom vision account key} {custom vision project} {source image uri} {image tag(s)}");
                return 1;
            }
            // Create the Api, passing in a credentials object that contains the training key
            TrainingApiCredentials trainingCredentials = new TrainingApiCredentials(args[0]);
            TrainingApi trainingApi = new TrainingApi(trainingCredentials);
            
            // Get a reference to the project and create a tag
            var project = trainingApi.GetProjects().First(proj => String.Equals(proj.Name, args[1], StringComparison.OrdinalIgnoreCase));

            // Create the specified tag(s), if it doesn't already exist
            var tags = args[3].Split(';')
                .Select(tag =>
                {
                    var tagObject = trainingApi.GetTags(project.Id).Tags.FirstOrDefault(projTag => String.Equals(projTag.Name, tag, StringComparison.OrdinalIgnoreCase));
                    if (tagObject == null)
                    {
                        tagObject = trainingApi.CreateTag(project.Id, tag);
                    }
                    return tagObject;
                })
                .ToList();

            // Enumerate the list of images from the specified root uri
            var storageUri = new UriBuilder(args[2]);
            var path = storageUri.Path;
            storageUri.Path = "";
            var blobClient = new CloudBlobClient(storageUri.Uri);
            var pathSegments = path.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var container = blobClient.GetContainerReference(pathSegments[0]);
            var blobUris = container.ListBlobs(String.Join("/", pathSegments.Skip(1)))
                .Select(blobItem => blobItem.Uri)
                .ToList();

            // Upload the images directly to the custom vision project
            var images = new ImageUrlCreateBatch(tags.Select(tag => tag.Id).ToList(),
                blobUris.Select(blobUri => blobUri.ToString()).ToList());
            trainingApi.CreateImagesFromUrlsAsync(project.Id, images, new CancellationTokenSource(60000).Token).Wait();
            return 0;
        }
    }
}
