using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensorflow;
using Tensorflow.Gradients;
using Tensorflow.Keras.Losses;
using Tensorflow.Keras.Optimizers;
using Tensorflow.NumPy;
using static Tensorflow.Binding;
namespace SnakeGame.Tensorflow
{
    class Snake
    {
        private QNetwork network;
        
        private List<List<(NDArray state, int action, float reward, NDArray nextState, bool gameOver)>> replayBuffer;
        private readonly int batchSize = 48;
        private float epsilon = 1.0f;  // Start with full exploration
        private float epsilonMin = 0.01f;
        private float decayRate = 0.005f;
        public int gamesPlayed = 0;
        private Random rand = new Random();
        public Snake()
        {
            network = new QNetwork(5, 128, 3);
            
            replayBuffer = new List<List<(NDArray, int, float, NDArray, bool)>>(2000);
        }

        public int ChooseAction(NDArray state)
        {
            
            if (rand.NextSingle() < epsilon)
            {
                return rand.Next(0, 3);
            }
            var prediction = network.network.predict(state);
            return np.argmax(prediction.numpy());
        }
        /*
        public void StoreExperience(NDArray state, int action, float reward, NDArray nextState, bool gameOver)
        {
            replayBuffer.Add((state, action, reward, nextState, gameOver));
            if(replayBuffer.Count > 1000) replayBuffer.RemoveAt(0);
            
        }*/
        public void StoreExperience(List<(NDArray state, int action, float reward, NDArray nextState, bool gameOver)> game)
        {
            // Store the full game sequence in the replay buffer
            replayBuffer.Add(game);

            // Keep buffer size manageable
            if (replayBuffer.Count > 10000)
            {
                replayBuffer.RemoveRange(0, replayBuffer.Count - 10000);
            }
        }
        public List<(NDArray, int, float, NDArray, bool)> SampleExperience(int batchSize)
        {
            

            // 🟢 Prioritize higher rewards and closer-to-death experiences
            /*return  replayBuffer.OrderByDescending(x => Math.Abs(x.reward) + (x.gameOver ? 1 : 0) * 0.5)
                    .Take(batchSize)
                    .ToList();*/
            var sampledGames = replayBuffer.OrderByDescending(game => game.Sum(move => Math.Abs(move.reward)))
                                   .Take(batchSize)
                                   .ToList();

            // Flatten the sampled games to get individual moves
            var sampledMoves = sampledGames.SelectMany(game => game).ToList();

            return sampledMoves;


        }

        public void TrainModel()
        {
            if (gamesPlayed % 2 == 0) // Every 5 games
            {
                network.targetNetwork.set_weights(network.network.get_weights());
            }
            gamesPlayed++;
            if (replayBuffer.Count == 0) return;

            var batch = replayBuffer.Last();
            int batchSize = batch.Count;
            // var batch = SampleExperience(batchSize);
            int num_features = 11;
            

            // 🟢 Initialize NDArrays directly
            NDArray stateBatch = np.zeros(new Shape(batchSize, num_features), dtype: np.float32);
            NDArray actionBatch = np.zeros(new Shape(batchSize, 1), dtype: np.int32);
            NDArray rewardBatch = np.zeros(new Shape(batchSize, 1), dtype: np.float32);
            NDArray nextStateBatch = np.zeros(new Shape(batchSize, num_features), dtype: np.float32);
            NDArray gameOverBatch = np.zeros(new Shape(batchSize, 1), dtype: np.float32);

            // 🎯 Fill NDArrays using a loop
            for (int i = 0; i < batchSize; i++)
            {
                var (state, action, reward, nextState, gameOver) = batch[i];

                stateBatch[i] = state.reshape(new Shape(num_features));  // Store state
                actionBatch[i] = action;  // Store action
                rewardBatch[i] = reward;  // Store reward
                nextStateBatch[i] = nextState.reshape(new Shape(num_features));  // Store next state
                gameOverBatch[i] = gameOver ? 1f : 0f;  // Convert gameOver to float
            }


            // 🎯 Get predicted Q-values for current and next states
           NDArray targets = network.network.predict(stateBatch).numpy();

           NDArray nextQValues = network.targetNetwork.predict(nextStateBatch).numpy();
           NDArray maxNextQValues = np.amax(nextQValues, axis: 1).reshape(new Shape(batchSize, 1)); // Max Q-value for each sample
            
           //  🎯 Update Q-values for taken actions
         //   targets[np.arange(batchSize), actionBatch] = rewardBatch + 0.9f * maxNextQValues * (1 - gameOverBatch);
            for (int i = 0; i < batchSize; i++)
            {
                int actionIndex = actionBatch[i].ToArray<int>()[0];
                targets[i, actionIndex] = rewardBatch[i] + 0.9f * maxNextQValues[i] * (1 - gameOverBatch[i]);
            }

            //using (var tape = tf.GradientTape())
            //{
            //    var qValues = network.network.predict(stateBatch);
            //    var actionMask = tf.one_hot(actionBatch, depth: 3);
            //    var predictedQ = tf.reduce_sum(qValues * actionMask, axis: 1);

            //    var targetQ = rewardBatch + 0.9f * maxNextQValues * (1f - gameOverBatch);

            //   var loss = tf.reduce_mean(tf.square(targetQ - predictedQ));

            //    // Inside TrainModel's gradient tape block


            //    var gradients = tape.gradient(loss, network.network.TrainableVariables);
            //    network.network.Optimizer.apply_gradients(gradients.Zip(network.network.TrainableVariables,(g,v) => (g,v)));
            //    Console.WriteLine($" Current Loss: {loss.numpy():F4}");
            //}

            // 🚀 Train the model with the whole batch
            network.network.fit(stateBatch, targets, epochs: 15, verbose: 1);

        }

        public void ChangeEpsilon()
        {
            epsilon = Math.Max(epsilonMin, epsilon * 0.995f);
            /*
            epsilon = 1f * (float)Math.Exp(-decayRate * gamesPlayed / 2);
            epsilon = Math.Max(epsilon, epsilonMin);*/
            Console.WriteLine(epsilon);
        }
     }
}
