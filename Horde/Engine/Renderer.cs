using System;
using SharpDX.Direct3D11;
using SharpDX.Direct3D;

using Horde.Engine.Task;

namespace Horde.Engine {

    class SceneRenderTaskPayLoad : TaskPayload {
        public RenderPipeline pipeline;
        public CommandList commandList;
        public int threadId;
    }

    public class Renderer : TaskManager, IDisposable{

        #region Members
        private static Renderer instance;

        public static Renderer Instance {
            get {
                return instance;
            }
        }

        private Device device;
        public Device Device {
            get {
                return device;
            }
        }
        private DeviceContext immContext;

        public DeviceContext DeviceContext {
            get {
                return immContext;
            }
        }

        private RenderPipeline[] pipelines;
        private CommandList[] commandLists;
        private SceneRenderTaskPayLoad[] payloads;

        private int numCores;

        #endregion Members

        public Renderer() {
            instance = this;

            numCores = Environment.ProcessorCount-1;

            device = new Device(DriverType.Hardware, DeviceCreationFlags.Debug);
            immContext = device.ImmediateContext;

            pipelines = new RenderPipeline[numCores];
            payloads = new SceneRenderTaskPayLoad[numCores];
            for (int i = 0; i < numCores; i++) {
                pipelines[i] = new RenderPipeline();
                payloads[i] = new SceneRenderTaskPayLoad();
            }
            commandLists = new CommandList[numCores];
        }

        public void Dispose() {
            for (int i = 0; i < pipelines.GetLength(0); i++) {
                pipelines[i].Dispose();
            }
            if (device != null) {
                device.Dispose();
            }
        }

        protected override void OnProcessTasks() {
            for (int i = QueuedTasks.Count-1; i >= 0 ; i -= numCores) {
                int count = 0;
                for (int j = 0; j < numCores; j++) {
                    if (i - j >= 0) { //noch ein Task in der Queue?
                        count++;
                        payloads[j].task = (SceneRenderTask)QueuedTasks[i - j];
                        payloads[j].pipeline = pipelines[j];
                        payloads[j].commandList = commandLists[j];
                        payloads[j].threadId = j;
                        StartTask((payload) => {
                            ((SceneRenderTaskPayLoad)payload).pipeline.DeviceContext.ClearState();
                            ((SceneRenderTask)payload.task).Execute(((SceneRenderTaskPayLoad)payload).pipeline);
                            commandLists[((SceneRenderTaskPayLoad)payload).threadId] = ((SceneRenderTaskPayLoad)payload).pipeline.GenerateCommandList();
                        }, payloads[j]);
                    } 
                }

                WaitAllTasks();

                //CommandLists auf Immediate-Context abbilden
                for (int k = 0; count > 0; count--, k++) {
                    immContext.ExecuteCommandList(commandLists[k], false);
                    commandLists[k].Dispose();
                }
            }
        }
    }
}
