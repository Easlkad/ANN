using System.Runtime.InteropServices;
using Tensorflow;
using static Tensorflow.Binding;
namespace SnakeGame
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();  // Creates a console window
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            AllocConsole();  // 🟢 Open Console
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form2());
        }

        public static void CheckGPU()
        {
            Console.WriteLine(tf.config.list_physical_devices());
            var gpus = tf.config.list_physical_devices().ToList();
            if (gpus.Count > 0)
            {
                Console.WriteLine("✅ GPU is available!");
                foreach (var gpu in gpus)
                {
                    Console.WriteLine($"GPU: {gpu}");
                }
            }
            else
            {
                Console.WriteLine("❌ No GPU detected. Falling back to CPU.");
            }
        }

        public static void EnableGPU()
        {
            var gpus = tf.config.list_physical_devices("GPU").ToList();
            if (gpus.Count > 0)
            {
                foreach (var gpu in gpus)
                {
                    tf.config.experimental.set_memory_growth(gpu, true);
                }
                tf.device("/device:GPU:0");
                Console.WriteLine("🚀 Using GPU for TensorFlow operations.");
            }
            else
            {
                Console.WriteLine("⚠️ No GPU found. Falling back to CPU.");
                tf.device("/device:CPU:0");
            }
        }
    }
}