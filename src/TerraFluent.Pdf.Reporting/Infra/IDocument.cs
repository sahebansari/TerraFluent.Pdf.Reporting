namespace TerraFluent.Pdf.Reporting.Infra;

/// <summary>Represents a reusable, composable PDF document template.</summary>
/// <example>
/// <code>
/// class MyReport : IDocument
/// {
///     public void Compose(IDocumentContainer container) =>
///         container.Page(page =>
///         {
///             page.Size(PageSize.A4);
///             page.Content().Text("Hello, World!");
///         });
/// }
///
/// PdfDocument.Create(new MyReport()).PublishPdf("report.pdf");
/// </code>
/// </example>
public interface IDocument
{
    void Compose(IDocumentContainer container);
}
