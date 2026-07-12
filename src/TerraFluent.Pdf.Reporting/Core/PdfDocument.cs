using TerraFluent.Pdf.Reporting.Infra;

namespace TerraFluent.Pdf.Reporting.Core;

/// <summary>
/// Entry point for the TerraFluent.Pdf.Reporting library.
/// </summary>
/// <example>
/// <code>
/// PdfDocument.Create(container =>
/// {
///     container.Page(page =>
///     {
///         page.Size(PageSize.A4);
///         page.Margin(2, Unit.Centimetre);
///         page.PageColor(Colors.White);
///
///         page.Header()
///             .Text("Hello PDF!")
///             .SemiBold().FontSize(36).FontColor(Colors.Blue.Darken2);
///
///         page.Content()
///             .Column(x =>
///             {
///                 x.Spacing(20);
///                 x.Item().Text("Paragraph 1");
///                 x.Item().Text("Paragraph 2");
///             });
///
///         page.Footer()
///             .AlignCenter()
///             .Text(x =>
///             {
///                 x.Span("Page ");
///                 x.CurrentPageNumber();
///             });
///     });
/// })
/// .PublishPdf("output.pdf");
/// </code>
/// </example>
public static class PdfDocument
{
    /// <summary>Creates a new document using an inline composition callback.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="compose"/> is <c>null</c>.</exception>
    public static DocumentComposer Create(Action<IDocumentContainer> compose)
    {
        ArgumentNullException.ThrowIfNull(compose);
        var composer = new DocumentComposer();
        compose(composer);
        return composer;
    }

    /// <summary>Creates a new document from an <see cref="IDocument"/> implementation.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="document"/> is <c>null</c>.</exception>
    public static DocumentComposer Create(IDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        var composer = new DocumentComposer();
        document.Compose(composer);
        return composer;
    }
}
