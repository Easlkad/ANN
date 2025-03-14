using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensorflow;
using Tensorflow.Keras;
using Tensorflow.Keras.ArgsDefinition;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Layers;
using Tensorflow.Keras.Optimizers;
using Tensorflow.NumPy;
using static Tensorflow.Binding;
namespace SnakeGame.Tensorflow
{
    class QNetwork
    {
        public Sequential network;
        public Sequential targetNetwork;
        private IVariableV1[] networkWeights;
        private IVariableV1[] targetNetworkWeights;

        public QNetwork(int inputSize , int hiddenSize, int outputSize)
        {
            network = BuildNetwork();
            targetNetwork = BuildNetwork();
            networkWeights = network.TrainableVariables.ToArray();
            targetNetworkWeights = targetNetwork.TrainableVariables.ToArray();
            targetNetwork.set_weights(network.get_weights());
        }

        private Sequential BuildNetwork()
        {
          Sequential  a = new Sequential(new SequentialArgs
            {
                Layers = new List<ILayer>
                {
                   new Dense(new DenseArgs { Units = 256, Activation = tf.keras.activations.Relu, InputShape = new Shape(11) }),
                   new BatchNormalization(new BatchNormalizationArgs { Axis = -1, Momentum = 0.99f, Epsilon = 0.001f, Center = true, Scale = true }),
                    new Dense(new DenseArgs { Units = 256, Activation = tf.keras.activations.Relu }),
                    new Dense(new DenseArgs { Units = 128, Activation = tf.keras.activations.Relu }),

                    new Dense(new DenseArgs { Units = 3, Activation = tf.keras.activations.Linear }) // Left, Right, Forward
                }
            });
            a.compile(optimizer: new Adam(0.001f), loss: tf.keras.losses.MeanSquaredError());

            return a;
          
        }
       
        public NDArray Predict(NDArray x ) => network.predict(x).numpy();
        public NDArray TargetPredict(NDArray x) => targetNetwork.predict(x).numpy();


       
    }

}
