using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string modelPath = "resnet50.onnx";
        string imagePath = @"imag1.png";
        string labelsPath = "imagenet_classes.txt"; // Relative path

        try
        {
            // Load the ONNX model
            var session = new InferenceSession(modelPath);

            // Load the image
            using (var originalImage = new Bitmap(imagePath))
            {
                // Convert to RGB format (if necessary)
                Bitmap image;
                if (originalImage.PixelFormat != PixelFormat.Format24bppRgb)
                {
                    image = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format24bppRgb);
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
                    }
                }
                else
                {
                    image = new Bitmap(originalImage); //Crie uma cópia para evitar modificar o original
                }

                // Resize the image
                int inputWidth = 224;
                int inputHeight = 224;
                Bitmap resizedImage = new Bitmap(inputWidth, inputHeight);
                using (Graphics g = Graphics.FromImage(resizedImage))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; // Optional: Improve resizing quality
                    g.DrawImage(image, 0, 0, inputWidth, inputHeight);
                }

                // Preprocess the image and create the input tensor
                var input = new DenseTensor<float>(new[] { 1, 3, inputHeight, inputWidth });
                for (int y = 0; y < inputHeight; y++)
                {
                    for (int x = 0; x < inputWidth; x++)
                    {
                        Color pixel = resizedImage.GetPixel(x, y);
                        input[0, 0, y, x] = pixel.R / 255f; // Normalize
                        input[0, 1, y, x] = pixel.G / 255f; // Normalize
                        input[0, 2, y, x] = pixel.B / 255f; // Normalize
                    }
                }

                // Create the ONNX input
                var inputs = new List<NamedOnnxValue> {
                    NamedOnnxValue.CreateFromTensor("data", input)
                };

                // Run the inference
                using (var results = session.Run(inputs))
                {
                    // Process the output
                    var output = results.First().AsEnumerable<float>().ToArray();
                    var maxIndex = Array.IndexOf(output, output.Max());

                    var labels = File.ReadAllLines(labelsPath);
                    var className = labels[maxIndex];
                    Console.WriteLine($"Classe prevista: {className} (índice: {maxIndex})");
                }

                // Dispose of the images
                image.Dispose();
                resizedImage.Dispose();
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: Model file not found: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: Invalid argument: {ex.Message}");
        }
        catch (OnnxRuntimeException ex)
        {
            Console.WriteLine($"Error: ONNX runtime error: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error: Could not read image file: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}