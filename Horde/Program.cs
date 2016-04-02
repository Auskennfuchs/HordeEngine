using System;
using SharpDX.Windows;
using System.Windows.Forms;

namespace Horde {
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                var form = new MainWindow("Horde");
                RenderLoop.Run(form, form.MainLoop);
                form.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message+"\n"+exc.StackTrace, "Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
