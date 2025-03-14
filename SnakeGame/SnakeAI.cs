using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using SnakeGame;


namespace SnakeGame
{
     class SnakeAI
    {
        private Sequential network;
        private float epsilon = 0.1f; // Exploration rate (1.0 = explore, 0.0 = exploit)
        private float epsilonDecay = 0.995f; // How fast exploration decreases
        private float epsilonMin = 0.01f; // Minimum exploration rate
        private float gamma = 0.95f; // Discount factor for future rewards

        // Experience Replay (stores game states for training)
        public List<(NDArray, int, float, NDArray, bool)> replayBuffer = new List<(NDArray, int, float, NDArray, bool)>();
        private int batchSize = 32;
        private int maxMemorySize = 5000;
        public SnakeAI() {

            BuildNeural();
        }

        public int ChooseAction(NDArray state)
        {
            Random rand = new Random();
            if (rand.NextDouble() < epsilon)
            {
                return rand.Next(0, 3); // Random action (exploration)
            }

            var prediction = network.Apply(state);
            return np.argmax(prediction.numpy());
        }
        
        private void BuildNeural()
        {
            network = new Sequential(new SequentialArgs
            {
                Layers = new List<ILayer>
        {
            new Dense(new DenseArgs { Units = 64, Activation = tf.keras.activations.Relu6, InputShape = new Shape(11) }),
            new BatchNormalization(new BatchNormalizationArgs()), // Add batch normalization
            new Dropout(new DropoutArgs { Rate = 0.2f }), // Add dropout for regularization
            new Dense(new DenseArgs { Units = 128, Activation = tf.keras.activations.Relu6 }),
            new BatchNormalization(new BatchNormalizationArgs()), // Add batch normalization
            new Dropout(new DropoutArgs { Rate = 0.2f }), // Add dropout for regularization
            new Dense(new DenseArgs { Units = 64, Activation = tf.keras.activations.Relu6 }),
            new Dense(new DenseArgs { Units = 3, Activation = tf.keras.activations.Linear }) // Q-values
        }
            });
       

            network.compile(optimizer: new Adam(0.01f), loss: tf.keras.losses.MeanSquaredError());
        }
        public void TrainModel()
        {
            try
            {
                if (replayBuffer.Count < batchSize)
                {
                    Console.WriteLine("⚠️ Not enough experiences for training");
                    return;
                }

                Random rand = new Random();
                var batch = replayBuffer.OrderDescending().Take(batchSize).ToList();
                int trainedSamples = 0;

                foreach (var (state, action, reward, nextState, done) in replayBuffer.ToList())
                {
                    try
                    {
                        // Get current Q-values
                        using var currentQTensor = network.Apply(state);
                        using var nextQTensor = network.Apply(nextState);
                        
                        var currentQ = currentQTensor.numpy();
                        var nextQ = nextQTensor.numpy();

                        // Calculate future reward (use max Q-value, not argmax)
                        float maxFutureQ = done ? 0 : nextQ.Cast<float>().Max();
                        
                        // Update target Q-value using Bellman equation
                        currentQ[0, action] = reward + gamma * maxFutureQ;

                        // Train on this sample
                        network.fit(state, currentQ, epochs: 1, verbose: 0);
                        trainedSamples++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Training iteration error: {ex.Message}");
                        continue; // Skip this sample and continue with next
                    }
                }

                // Update exploration rate
                if (epsilon > epsilonMin)
                {
                    epsilon *= epsilonDecay;
                }

                // Log progress
                Console.WriteLine($"✅ Training Complete! Samples trained: {trainedSamples}, Epsilon: {epsilon:F3}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Training failed: {ex.Message}");
            }
            finally
            {
                GC.Collect(); // Help clean up any leaked tensors
            }
        }
        public void StoreExperience(NDArray state, int action, float reward, NDArray nextState, bool done)
        {
            replayBuffer.Add((state, action, reward, nextState, done));
            if (replayBuffer.Count > maxMemorySize)
            {
                replayBuffer.RemoveAt(0); // Keep memory size small
            }
        }

    }
}
