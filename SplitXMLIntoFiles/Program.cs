using System;
using System.Windows.Forms;

namespace SplitXMLIntoFiles
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      // start the application
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Main());
    }
  }
}