using Horde.Engine;
using System.Windows.Forms;
using SwapChain = Horde.Engine.SwapChain;
using Horde.Engine.Events;
using SharpDX;

namespace Horde {
    public class MainWindow : Form
    {

        private HordeEngine engine;

        private SceneRenderTest sceneRender;

        private SwapChain swapChain;

        private EventManager eventManager;

        private FirstPersonCamera cam;

        public MainWindow(string windowName) : 
            base()
        {
            InitializeComponent();
            this.Text = windowName;
            this.Width = 1024;
            this.Height = 768;

            this.ClientSize = new System.Drawing.Size(this.Width,this.Height);
            
            eventManager = new EventManager();

            this.KeyDown += (o, e) => {
                eventManager.ProcessEvent(new EventKeyDown(new SKeyEvent() {
                    keyCode = e.KeyCode,
                    alt = e.Alt,
                    control = e.Control,
                    shift = e.Shift
                }));
            };
            this.KeyUp += (o, e) => {
                eventManager.ProcessEvent(new EventKeyUp(new SKeyEvent() {
                    keyCode = e.KeyCode,
                    alt = e.Alt,
                    control = e.Control,
                    shift = e.Shift
                }));
            };

            this.MouseDown += (o, e) => {
                eventManager.ProcessEvent(new EventMouseDown(new SMouseEvent() {
                    position = e.Location,
                    button = e.Button
                }));
            };
            this.MouseUp += (o, e) => {
                eventManager.ProcessEvent(new EventMouseUp(new SMouseEvent() {
                    position = e.Location,
                    button = e.Button
                }));
            };
            this.MouseMove += (o, e) => {
                eventManager.ProcessEvent(new EventMouseMove(new SMouseEvent() {
                    position = e.Location
                }));
            };

            engine = new HordeEngine();
            engine.Init(this);

            swapChain = new SwapChain(this);

            sceneRender = new SceneRenderTest(swapChain.RenderTarget);
            sceneRender.Init();

            cam = new FirstPersonCamera();
            cam.EventManager = eventManager;
            cam.SetProjection(0.1f, 1000.0f, (float)ClientSize.Width / (float)ClientSize.Height, 3.14159265f / 4.0f);

            cam.Position = new Vector3(0.0f, 0.0f, 0.0f);
            cam.SceneRenderTask = sceneRender;

            sceneRender.cam = cam;

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
            eventManager.ProcessEvent(new EventFrameStart());
            cam.RenderFrame(Renderer.Instance);
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
