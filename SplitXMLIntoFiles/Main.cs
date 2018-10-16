using System;
using System.Windows.Forms;
using SplitXMLIntoFiles.Properties;

namespace SplitXMLIntoFiles
{
  public partial class Main : Form
  {
    private const int Mb = 1000000;
    public Main()
    {
      InitializeComponent();
    }

    private string ImportXmlDoc()
    {
      openFileDlg = new OpenFileDialog { Filter = "XML files|*.xml|All files|*.*" };
      openFileDlg.ShowDialog();
      return openFileDlg.FileName;
    }

    private void BtnSplitClick(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(txtFileSize.Text))
      {
        MessageBox.Show(Resources.Main_btnSplit_Click_Enter_split_value_in_MB);
        return;
      }

      var fileName = ImportXmlDoc();
      var size = double.Parse(txtFileSize.Text.Trim()) * Mb;
      var fileProcessor = new FileProcessor();
      var numOfNewFiles = fileProcessor.SplitFile(size, fileName);

      MessageBox.Show(Resources.Main_btnSplit_Click_Done__ + numOfNewFiles + Resources.Main_btnSplit_Click_ + Application.StartupPath);
    }
  }
}