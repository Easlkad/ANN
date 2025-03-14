using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;
namespace SnakeGame.TorchSharp
{
   public class QNetwork : nn.Module
    {
        private Sequential model;
        public QNetwork(int inputSize , int hiddenSize, int outputSize) : base("q_net")
        {
            model = nn.Sequential(
                nn.Linear(inputSize, 128), // input 13 hidden 256
            //    nn.BatchNorm1d(hiddenSize),
                nn.ReLU(),
                nn.Linear(128, 128), // hidden 256 hidden 256
                nn.ReLU(),
                nn.Linear(128, outputSize)); // hidden 128 output 3
            RegisterComponents();
        }
      
        public  Tensor forward(Tensor x) =>   model.forward(x);    
        
    }
}
