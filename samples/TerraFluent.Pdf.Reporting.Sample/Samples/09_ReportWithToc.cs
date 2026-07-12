namespace TerraFluent.Pdf.Reporting.Sample.Samples;

using TerraFluent.Pdf.Reporting.Core;

internal static class ReportWithTocSample
{
    internal static void Generate(string path)
    {
        PdfDocument.Create(new ReportWithToc()).PublishPdf(path);
        Console.WriteLine($"  [9] TOC demo               -> {path}");
    }
}
