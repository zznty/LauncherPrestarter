using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Security.Policy;

namespace Prestarter
{
    internal enum ProgressBarState
    {
        Marqee,
        Progress
    }

    internal interface IUIReporter
    {
        void SetProgressBarState(ProgressBarState state);
        void SetProgress(float value);
        void SetStatus([Localizable(true)] string status);
        void ShowForm();
    }

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrestarterForm());
        }
    }
}
