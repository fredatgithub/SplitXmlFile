using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SplitXMLIntoFiles
{
  internal class FileProcessor
  {
    private string _curXSDFile;
    private XDocument _xsdDoc;
    private string _schemaXMLDoc;
    private string _xmlFileName;

    private static XNamespace GetXs()
    {
      // return the standard namespace
      return XNamespace.Get("http://www.w3.org/2001/XMLSchema");
    }

    internal void CreateXsd(string xmlFilePath)
    {
      var proc = new Process();
      var fi = new FileInfo(xmlFilePath);
      var arguments = xmlFilePath + " /outputdir:" + fi.DirectoryName;
      var xsdExe = Application.StartupPath + "\\xsd.exe"; // bin\debug\xsd.exe";
      proc.StartInfo.Arguments = arguments;
      proc.StartInfo.FileName = xsdExe;
      proc.Start();

      proc.Close();
      _curXSDFile = fi.DirectoryName + "\\" + fi.Name.Substring(0, fi.Name.IndexOf('.')) + ".xsd";
    }

    protected string GetElementFullClosingPath(string element)
    {
      var path = string.Empty;
      var xs = GetXs();
      if (_schemaXMLDoc == null)
      {
        _schemaXMLDoc = GetSchemaXml();
        _xsdDoc = XDocument.Parse(_schemaXMLDoc);
      }

      foreach (var elt in _xsdDoc.Descendants(xs + "element"))
      {
        var xAttribute = elt.Attribute("name");
        if (xAttribute != null)
        {
          if (xAttribute.Value == element)
          {
            var e = elt.Parent;
            do
            {
              if (e == null) break;
              var attribute = e.Attribute("name");
              if (attribute != null) path += $"</{attribute.Value}>";
              e = e.Parent;
            } while (e != null);

            break;
          }
        }
      }

      return path;
    }

    private string GetSchemaXml()
    {
      try
      {
        using (var reader = new StreamReader(_curXSDFile, Encoding.UTF8))
        {
          return reader.ReadToEnd();
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Split file based on max size in MB
    /// </summary>
    public int SplitFile(double maxFileSplitSize, string fileName)
    {
      if (maxFileSplitSize <= 0 || string.IsNullOrWhiteSpace(fileName)) return 0;
      CreateXsd(fileName);
      _xmlFileName = fileName;
      var fi = new FileInfo(fileName);
      var numOfNewFiles = Math.Ceiling(fi.Length / maxFileSplitSize);
      var fileCnt = 0;
      long pos = 0;
      bool newPart = false;
      var closeingPath = string.Empty;

      //Read Large XML File
      using (var streamReader = new StreamReader(fileName, Encoding.UTF8))
      {
        var streamWriter = new StreamWriter(GetFilePartName(++fileCnt, fi), false);

        while (!streamReader.EndOfStream)
        {
          var line = streamReader.ReadLine();
          if (line != null)
          {
            var lineTrimed = line.Trim();

            if (newPart)
            {
              streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
              streamWriter.WriteLine($"<!-- File {fileCnt} of {numOfNewFiles}. -->");
              var startSequence = ReversePath(closeingPath);
              streamWriter.WriteLine(startSequence);
              newPart = false;
            }

            pos += Encoding.UTF8.GetByteCount(line) + 2; // 2 extra bits for end of line chars.
            streamWriter.WriteLine(line);
            streamWriter.Flush();

            var eltName = GetEltName(lineTrimed);

            Match m = Regex.Match(lineTrimed, @"^(<[\w]{1})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (m.Success && (pos >= maxFileSplitSize * fileCnt) && (CanCloseFile(eltName, lineTrimed)))
            {
              closeingPath = GetElementFullClosingPath(eltName);
              streamWriter.WriteLine(closeingPath);

              streamWriter.Flush();
              streamWriter.Close();
              streamWriter.Dispose();
              streamWriter = new StreamWriter(GetFilePartName(++fileCnt, fi), false);
              newPart = true;
            }
          }
        }

        streamWriter.Flush();
        streamWriter.Close();
        streamWriter.Dispose();
      }

      return fileCnt;
    }

    private static bool CanCloseFile(string eltName, string lineTrimed)
    {
      // trim
      return lineTrimed.EndsWith("/>") || (lineTrimed.Contains("<" + eltName) && lineTrimed.Contains("</" + eltName));
    }

    private static string GetEltName(string lineTrimed)
    {
      var eltName = lineTrimed.Split(' ')[0];
      if (eltName.Contains(">"))
      {
        eltName = eltName.Substring(1, eltName.IndexOf('>'));
      }

      eltName = eltName.Replace("<", "").Replace(">", "");
      return eltName;
    }

    private string GetLastNode(ref long pos)
    {
      string output = string.Empty;
      try
      {
        using (var fs = new FileStream(_xmlFileName, FileMode.Open, FileAccess.Read))
        {
          fs.Seek(pos, SeekOrigin.Begin);
          try
          {
            using (var reader = XmlReader.Create(fs))
            {
              var content = reader.MoveToContent();

              var d = new XmlDocument();
              d.Load(reader.ReadSubtree());
              output = d.InnerXml;
              reader.Close();
            }
          }
          finally
          {
            pos = fs.Position;
            fs.Close();
          }
        }
      }
      catch
      {
        return string.Empty;
      }

      return output;
    }

    private static string ReversePath(string closeingPath)
    {
      var path = string.Empty;
      var str = closeingPath.Replace("</", "").Replace(">", "|");
      var nodes = str.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
      for (var i = nodes.Length - 1; i >= 0; i--)
      {
        path += $"<{nodes[i]}>";
      }

      return path;
    }
    private string ReversePathFirstElement(string closeingPath)
    {
      var str = closeingPath.Replace("</", "").Replace(">", "|");
      var nodes = str.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
      return $"{nodes[0]}";
    }

    private static string GetFilePartName(int fileCnt, FileSystemInfo fileSystemInfo)
    {
      return Application.StartupPath + "/" + fileSystemInfo.Name + ".part" + fileCnt + fileSystemInfo.Extension;
    }
  }
}