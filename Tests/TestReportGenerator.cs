using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HafifPlatofrmArayuz.Tests
{
	public static class TestReportGenerator
	{
		private static readonly string reportFilePath = "../../../Tests/TestReport.pdf";

		public static void GenerateReport(string[] logs)
		{
			try
			{
				iTextSharp.text.Document document = new iTextSharp.text.Document();
				PdfWriter.GetInstance(document, new FileStream(reportFilePath, FileMode.Create));
				document.Open();

				document.Add(new iTextSharp.text.Paragraph("Test Raporu"));
				document.Add(new iTextSharp.text.Paragraph($"Tarih: {DateTime.Now:dd-MM-yyyy HH:mm:ss}"));
				document.Add(new iTextSharp.text.Paragraph("\n"));

				foreach (var log in logs)
				{
					document.Add(new iTextSharp.text.Paragraph(log));
				}

				document.Close();
				Console.WriteLine($"Test raporu oluşturuldu: {reportFilePath}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Test raporu hatası: {e.Message}");	
			}
		}
	}
}
