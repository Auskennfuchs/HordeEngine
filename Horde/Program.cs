using System;
using SlimDX.Windows;
using System.Windows.Forms;

using Horde.Engine;

using System.IO;

namespace Horde
{
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
                MessagePump.Run(form, form.MainLoop);
                form.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
