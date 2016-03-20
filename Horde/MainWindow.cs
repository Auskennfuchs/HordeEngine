using Horde.Engine;
using System.Windows.Forms;
using SwapChain = Horde.Engine.SwapChain;

namespace Horde {
    public class MainWindow : Form
    {

        private HordeEngine engine;

        private SceneRenderTest sceneRender;

        private SwapChain swapChain;

        public MainWindow(string windowName) : 
            base()
        {
            InitializeComponent();
            this.Text = windowName;
            this.Width = 1024;
            this.Height = 768;

            engine = new HordeEngine();
            engine.Init(this);

            swapChain = new SwapChain(this);

            sceneRender = new SceneRenderTest(swapChain.RenderTarget);
            sceneRender.Init();
        }

        public new void Close()
        {
            sceneRender.Dispose();
            if (swapChain != null) {
                swapChain.Dispose();
            }
            engine.Dispose();
            base.Close();
        }

        public void MainLoop()
        {
            Horde.Engine.Renderer.Instance.QueueTask(sceneRender);
            Horde.Engine.Renderer.Instance.ProcessTasks();
            swapChain.Present();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.SuspendLayout();
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ResumeLayout(false);

        }
    }
}
