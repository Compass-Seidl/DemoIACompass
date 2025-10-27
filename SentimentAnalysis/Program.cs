using Microsoft.ML;
using Microsoft.ML.Data;
using System.Reflection;

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
        public bool Prediction { get; set; }//resultado previsto pelo modelo
        public float Probability { get; set; }//confiança na previsão
        public float Score { get; set; } // valro bruto da previsão (usado internamenbte)
    }

    class Program
    {
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "data", "sentiment.tsv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "MLModel.zip");

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();
            //popular os campos
            IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentData>(
                path: _dataPath,
                hasHeader: true,
                separatorChar: '\t');
            //pipeline de apendeizagfem 
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
            // FeaturizeText: transforma o texto em vetores numéricos (features).
            // SdcaLogisticRegression: algoritmo de classificação binária usado para treinar o modelo.


            var model = pipeline.Fit(dataView);

            mlContext.Model.Save(model, dataView.Schema, _modelPath);
            Console.WriteLine("Modelo treinado e salvo em: " + _modelPath);
            // Fit: treina o modelo com os dados.
	        //• Save: salva o modelo treinado em disco para uso posterior.


            var predictor = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

            while (true)
            {
                Console.Write("Digite uma frase para análise de sentimento (ou 'sair'): ");
                var input = Console.ReadLine();
                if (input?.ToLower() == "sair") break;

                var prediction = predictor.Predict(new SentimentData { Text = input });
                Console.WriteLine($"Sentimento: {(prediction.Prediction ? "Positivo" : "Negativo")} (Confiança: {prediction.Probability:P2})");
            }
            //Loop interativo para o usuário testar frases.
	        //Mostra o sentimento previsto e a confiança da predição.

        }
    }
}
