using Microsoft.ML;
using Microsoft.ML.Data;

namespace SentimentAnalysis
{
    public class SentimentData
    {
        [LoadColumn(0)]
        public string? Text { get; set; }

        [LoadColumn(1)]
        public bool Label { get; set; }
    }

    public class SentimentPrediction : SentimentData
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }

    class Program
    {
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "data", "sentiment.tsv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "MLModel.zip");

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentData>(
                path: _dataPath,
                hasHeader: true,
                separatorChar: '\t');

            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            var model = pipeline.Fit(dataView);

            mlContext.Model.Save(model, dataView.Schema, _modelPath);
            Console.WriteLine("Modelo treinado e salvo em: " + _modelPath);

            var predictor = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

            while (true)
            {
                Console.Write("Digite uma frase para análise de sentimento (ou 'sair'): ");
                var input = Console.ReadLine();
                if (input?.ToLower() == "sair") break;

                var prediction = predictor.Predict(new SentimentData { Text = input });
                Console.WriteLine($"Sentimento: {(prediction.Prediction ? "Positivo" : "Negativo")} (Confiança: {prediction.Probability:P2})");
            }
        }
    }
}
