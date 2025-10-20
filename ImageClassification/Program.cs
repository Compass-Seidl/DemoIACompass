
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Drawing;

class Program
{
    static void Main(string[] args)
    {
        var session = new InferenceSession("resnet50.onnx");
        var image = new Bitmap("image.jpg");

        var input = new DenseTensor<float>(new[] { 1, 3, 224, 224 });
        for (int y = 0; y < 224; y++)
        {
            for (int x = 0; x < 224; x++)
            {
                var pixel = image.GetPixel(x, y);
                input[0, 0, y, x] = pixel.R / 255f;
                input[0, 1, y, x] = pixel.G / 255f;
                input[0, 2, y, x] = pixel.B / 255f;
            }
        }

        var inputs = new List<NamedOnnxValue> {
            NamedOnnxValue.CreateFromTensor("data", input)
        };

        using var results = session.Run(inputs);
        var output = results.First().AsEnumerable<float>().ToArray();
        var maxIndex = Array.IndexOf(output, output.Max());

        Console.WriteLine($"Classe prevista: {maxIndex}");
    }
}
