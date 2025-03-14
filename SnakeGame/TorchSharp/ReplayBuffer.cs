using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
using static TorchSharp.torch;

namespace SnakeGame.TorchSharp
{
    class ReplayBuffer
    {
        private struct Experience {
            public Tensor state;
            public long action;
            public float reward;
            public Tensor nextState;
            public bool gameOver;
        }



        private readonly List<Experience> buffer = new();
        private readonly Random rand = new();
        private readonly int capacity;

        public ReplayBuffer(int capacity ) => this.capacity = capacity;

        public int Count => buffer.Count;
        public void Add(Tensor state, long action, float reward, Tensor nextState , bool gameOver)
        {
            if(buffer.Count >= capacity ) buffer.RemoveAt(0);
            buffer.Add(new Experience
            {
                state = state.clone(),
                action = action,
                reward = reward,
                nextState = nextState.clone(),
                gameOver = gameOver
            });
        }

        public (Tensor,Tensor,Tensor,Tensor,Tensor) Sample(int batchSize)
        {
           // var sample = Enumerable.Range(0, batchSize).Select(_ => buffer.OrderByDescending(exp=> Math.Abs(exp.reward)).Take(buffer.Count/2).ElementAt(rand.Next(buffer.Count/2))).ToList();
            var sample = Enumerable.Range(0, batchSize).Select(x => buffer[rand.Next(buffer.Count)]).ToList();
            return (
             torch.stack(sample.Select(x => x.state)),       // states
             torch.tensor(sample.Select(x => x.action).ToList(),dtype:int64),     // actions
             torch.tensor(sample.Select(x => x.reward).ToArray(), dtype: float32),     // rewards
             torch.stack(sample.Select(x => x.nextState)),   // next_states
             torch.tensor(sample.Select(x => x.gameOver ? 1f : 0f).ToArray(),dtype:float32)  // dones
         );
        }


    }

   
}
