using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
namespace Snake
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("🔍 Checking TensorFlow Backend...");
                    var tf_version = tf.VERSION;
                    Console.WriteLine($"✅ TensorFlow.NET Loaded: Version {tf_version}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ TensorFlow Initialization Failed!");
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }).Wait();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
