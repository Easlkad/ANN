using System;
using System.Linq;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow;
using Tensorflow.NumPy;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Layers;
using Tensorflow.Keras.Models;
using Tensorflow.Keras.ArgsDefinition;
using Tensorflow.Keras.Optimizers;
using Tensorflow.Keras;

class Program
{
    static string modelPath = "C:\\Users\\HP\\source\\repos\\ANN\\ANN\\Models\\model1.h5";
    static void Main(string[] args)
    {
        Sequential model; // ✅ Declare model once at the start

        // ✅ Check if the model is already saved
        if (Directory.Exists(modelPath))
        {

            Console.WriteLine("📁 Found existing model. Loading...");
            model = tf.keras.models.load_model(modelPath) as Sequential; // ✅ No redeclaration
            Console.WriteLine("✅ Model loaded successfully!\n");
            
        }
        else
        {
            Console.WriteLine("⚡ No saved model found. Training a new model...");

            // 1️⃣ Define Training Data for y = 3x² + 2x + 5
            var X_train = np.array(new float[,] { { 1 }, { 2 }, { 3 }, { 4 }, { 5 }, { 6 }, { 7 }, { 8 }, { 9 }, { 10 },{ 11},{ 12} });
            var Y_train = np.array(new float[,]
            {
                { 3 * 1 * 1 + 2 * 1 + 5 },
                { 3 * 2 * 2 + 2 * 2 + 5 },
                { 3 * 3 * 3 + 2 * 3 + 5 },
                { 3 * 4 * 4 + 2 * 4 + 5 },
                { 3 * 5 * 5 + 2 * 5 + 5 },
                { 3 * 6 * 6 + 2 * 6 + 5 },
                { 3 * 7 * 7 + 2 * 7 + 5 },
                { 3 * 8 * 8 + 2 * 8 + 5 },
                { 3 * 9 * 9 + 2 * 9 + 5 },
                { 3 * 10 * 10 + 2 * 10 + 5 },
                { 3 * 11 * 11 + 2 * 11 + 5 },
                { 3 * 12 * 12 + 2 * 12 + 5 }
            });

            // 2️⃣ Build Model
            model = new Sequential(new SequentialArgs
            {
                Layers = new List<ILayer>
                {
                    new Dense(new DenseArgs { Units = 128, Activation = tf.keras.activations.Relu, InputShape = new Shape(1) }),
                    new Dense(new DenseArgs { Units = 64, Activation = tf.keras.activations.Relu }),
                    new Dense(new DenseArgs { Units = 32, Activation = tf.keras.activations.Relu }),
                    new Dense(new DenseArgs { Units = 16, Activation = tf.keras.activations.Relu }),
                    new Dense(new DenseArgs { Units = 1 })
                }
            });

            // 3️⃣ Compile Model
            model.compile(optimizer: new Adam(0.01f), loss: tf.keras.losses.MeanSquaredError());

            // 4️⃣ Train Model
            Console.WriteLine("Training the model... 🧠");
            model.fit(X_train, Y_train, epochs: 3000, verbose: 0);
            Console.WriteLine("✅ Model training complete!");

            // 5️⃣ Save Model
            model.save(modelPath);
            Console.WriteLine($"📁 Model saved to: {modelPath}\n");
            Console.WriteLine($"🔍 Model is saved at: {Path.GetFullPath("ANN/Models/model1.h5")}");

        }
        while (true)
        {
            Console.Write("Enter a number to predict (or type 'exit' to quit): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit") break; // Exit the game

            if (float.TryParse(input, out float userNumber))
            {
                var test_input = np.array(new float[,] { { userNumber } }); // Ensure 2D shape
                var prediction = model.Apply(test_input);

                Console.WriteLine($"🤖 Model's Prediction for y = 3x² + 2x + 5: {prediction.numpy()[0, 0]:F2}\n");
            }
            else
            {
                Console.WriteLine("❌ Invalid input. Please enter a valid number.");
            }
        }

        Console.WriteLine("Goodbye! 👋");
    }
}

