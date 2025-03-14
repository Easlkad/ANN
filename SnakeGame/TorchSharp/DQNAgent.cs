using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
using static TorchSharp.torch;

namespace SnakeGame.TorchSharp
{
    class DQNAgent
    {

        private QNetwork qNetwork;
        private QNetwork targetNetwork;
        private optim.Optimizer optimizer;
        private ReplayBuffer buffer;
        private double epsilon = 1.0;
        private int stepCounter = 0;
        private Device device = torch.cuda.is_available() ? torch.CUDA : torch.CPU;

        public DQNAgent()
        {
            Console.WriteLine($"[INFO] Running on: {device}");
            qNetwork = new QNetwork(11, 256, 3);
            targetNetwork = new QNetwork(11, 256, 3);
            targetNetwork.load_state_dict(qNetwork.state_dict());
            optimizer = optim.Adam(qNetwork.parameters(), 0.001);
            buffer = new ReplayBuffer(10000);
        }

        public long SelectAction(Tensor state)
        {
            if (new Random().NextDouble() < epsilon)
                return new Random().Next(0, 3);

            using (torch.no_grad())
            {
                return qNetwork.forward(state)
                    .argmax()
                    .item<long>();
            }
        }
        public void OptimizeModel()
        {
            if (buffer.Count < 64) return;

            var (states, actions, rewards, nextStates, dones) = buffer.Sample(64);
            
             
            states = states.squeeze(1); // Removes redundant dimension if shape is [64, 1, 11]
            nextStates = nextStates.squeeze(1);

            actions = actions.unsqueeze(1).to(torch.int64);
       
            
            // Compute Q(s_t, a)
            var qValues = qNetwork.forward(states).gather(1,actions);
            // Compute V(s_{t+1})
            var nextQ = targetNetwork.forward(nextStates).max(1).values.detach();
            var targetQ = rewards + 0.99 * nextQ * (1 - dones);

            // Huber loss
            var loss = nn.functional.smooth_l1_loss(qValues, targetQ.unsqueeze(1));

            // Backprop
            optimizer.zero_grad();
            loss.backward();
            torch.nn.utils.clip_grad_norm_(qNetwork.parameters(), 100);
            optimizer.step();

            // Update target network
            if (stepCounter % 10 == 0)
            {
                var targetParams = targetNetwork.named_parameters().ToList();
                var qParams = qNetwork.named_parameters().ToList();

                for (int i = 0; i < targetParams.Count; i++)
                {
                    targetParams[i].parameter.mul(0.99)
                        .add(qParams[i].parameter * 0.01);
                }
            }

            // Epsilon decay
            epsilon = Math.Max(0.01, epsilon * 0.995);
        }
        public void StoreExperience(Tensor state, long action, float reward, Tensor nextState, bool gameOver)
        {
            buffer.Add(state, action, reward, nextState, gameOver);
        }
    }
}
