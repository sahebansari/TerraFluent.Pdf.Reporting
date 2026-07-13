# TerraFluent.Pdf.Reporting — Free C# PDF Library for .NET

**A free, open-source (MIT) C# PDF library for creating PDF documents in .NET** — generate invoices, reports, receipts, statements, labels, and certificates from C# code with a fluent API. 100% managed C#, zero dependencies, and free for personal **and commercial** use: no watermarks, no page limits, no paid tiers.

![TerraFluent.Pdf.Reporting](https://raw.githubusercontent.com/sahebansari/TerraFluent.Pdf.Reporting/master/logo.png)

[![NuGet](https://img.shields.io/nuget/v/TerraFluent.Pdf.Reporting.svg)](https://www.nuget.org/packages/TerraFluent.Pdf.Reporting)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

📚 **To know more about it and documentation and sample codes. Please visit its website** 

🌐 [https://terrafluent.dev/](https://terrafluent.dev/)


**TerraFluent.Pdf.Reporting** is a lightweight, zero-dependency, pure C# library for generating professional PDF 1.7 documents programmatically.
It provides a fluent, composable API that covers the full document-authoring lifecycle — from page layout and
rich text to tables, images, hyperlinks, and multi-page pagination — with no native binaries, no third-party
runtime packages, and no licensing restrictions.

---

## Why TerraFluent.Pdf.Reporting?

- **Truly free** — MIT licensed, free for commercial use. No royalties, no watermarks, no page limits, no "community edition" restrictions, no revenue caps.
- **Pure C#, zero dependencies** — no native binaries (no `libgdiplus`, no Chromium, no wkhtmltopdf), no third-party NuGet packages. The entire PDF writer is managed code.
- **Cross-platform** — runs anywhere .NET runs: Windows, Linux, macOS, Docker containers, Azure Functions, AWS Lambda, ASP.NET Core web apps, console apps, and background services.
- **Modern .NET** — targets .NET 8 (LTS), .NET 9, and .NET 10 (LTS).
- **Code-first, not HTML-to-PDF** — documents are composed from typed C# layout primitives (`Column`, `Row`, `Table`), so output is deterministic and fast — no browser engine to install or babysit.
- **Batteries included** — text styling, tables, images, hyperlinks, bookmarks, table of contents, headers/footers, page numbers, vector graphics, Code128 barcodes, QR codes, and AES-256 encryption.

## Common use cases

- Generate **invoice PDFs** in C# / ASP.NET Core
- Export **reports** and **statements** to PDF from .NET applications
- Create **receipts**, **delivery notes**, and **order confirmations**
- Print **shipping labels** with Code128 barcodes and QR codes
- Produce **certificates**, **letters**, and **contracts** from templates
- Server-side PDF generation in **Docker**, **Azure**, and **AWS** — no native dependencies to install

## Features

- No native dependencies, no third-party packages
- Targets **.NET 8**, **.NET 9** and **.NET 10**
- Text styling — bold, italic, bold-italic, strikethrough, underline, font size, colour
- Three built-in font families — Helvetica, Times, Courier — via `FontFamily()`, no embedding needed
- Configurable line-height multiplier per text block
- Per-span formatting inside mixed-style text blocks
- Margin and padding decorators with full unit support
- Background fills and borders (full, rounded, and per-edge)
- Rounded-corner borders and filled rounded boxes
- Per-edge borders — `BorderTop`, `BorderBottom`, `BorderLeft`, `BorderRight`
- Horizontal and vertical alignment
- Column, Row, and Table layouts
- PNG and JPEG image embedding
- Horizontal and vertical rule lines
 - Explicit page breaks via `PageBreak()`
 - Clickable hyperlink (URI) annotations via `Hyperlink()`
 - Internal document links (GoTo) via `InternalLink()`
 - Automatic Table of Contents generation from H1–H6 headings
 - PDF bookmarks / outlines with hierarchical nesting
 - Document metadata (Title, Author, Subject, Keywords, Creator)
 - Conditional rendering via `ShowIf`
 - Reusable components via `IComponent`
 - Headers, footers, and page numbers
 - **AES-256 PDF encryption by default** — user password, owner password, and fine-grained permission flags (`PdfPermissions`) via `container.Encrypt()`; AES-128 remains available for compatibility
- **Images from bytes and streams** — `Image(byte[])` / `Image(Stream)` with transparency and deduplication
- **Anchor-based bookmarks** — bookmark content directly to rendered elements and keep destinations accurate
 - Full **WinAnsiEncoding** character coverage
 - **Vector graphics canvas** — lines, rectangles, rounded rectangles, circles, ellipses, arbitrary Bézier paths, polygons, and grid helpers via `container.Canvas()`
 - **Code128 barcodes** — `container.Barcode(...)`, with optional human-readable caption, custom colours, and quiet zone
 - **QR codes** — `container.QrCode(...)`, full ISO/IEC 18004 generator (versions 1-40, error correction levels L/M/Q/H), rendered as vector rectangles
 - Fluent, composable API

---

## Installation

```sh
dotnet add package TerraFluent.Pdf.Reporting
```

### Migrating from TerraPDF

TerraFluent.Pdf.Reporting 2.x is the direct continuation of TerraPDF 1.x. To upgrade:

1. Replace the `TerraPDF` package reference with `TerraFluent.Pdf.Reporting`.
2. Update `using TerraPDF.*` directives to `using TerraFluent.Pdf.Reporting.*`.
3. Replace `Document.Create(...)` with `PdfDocument.Create(...)`.

No other API or behaviour changes are required — see the [CHANGELOG](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/CHANGELOG.md) for details.

---

## Quick Start

```csharp
using TerraFluent.Pdf.Reporting.Core;
using TerraFluent.Pdf.Reporting.Helpers;

PdfDocument.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSize.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Color.White);
        page.DefaultTextStyle(s => s.FontSize(11));

        page.Header()
            .Text("My Report")
            .Bold()
            .FontSize(24)
            .FontColor(Color.Blue.Darken2);

        page.Content()
            .Column(col =>
            {
                col.Spacing(8);
                col.Item().Text("Hello, TerraFluent.Pdf.Reporting!");
                col.Item().Text("This paragraph is italic.").Italic();
                col.Item()
                   .Margin(6).Background(Color.Grey.Lighten4).Padding(10)
                   .Text("Indented callout box").Bold();
            });

        page.Footer()
            .AlignCenter()
            .Text(t =>
            {
                t.Span("Page ");
                t.CurrentPageNumber().FontSize(9);
                t.Span(" / ");
                t.TotalPages().FontSize(9);
            });
    });
})
.PublishPdf("output.pdf");
```

---

## Full Documentation

For complete API reference and detailed guides, visit the [docs](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/tree/master/docs/) directory:

- **[Getting Started](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/getting-started.md)** — Installation, Quick Start, and basic usage
- **[Text & Spans](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/text-and-spans.md)** — Single-span and multi-span text, styling, page numbers
- **[Layout](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/layout.md)** — Column, Row, and Table layouts with alignment and spacing
- **[Row and Column Layout](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/row-and-column-layout.md)** — Detailed Row and Column layout examples
- **[Decorators](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/decorators.md)** — Padding, margin, backgrounds, borders, and styling
- **[Images](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/images.md)** — PNG and JPEG embedding with sizing and alignment
- **[Page Sizes & Units](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/page-sizes-and-units.md)** — Built-in page sizes and unit conversions
- **[Colors](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/colors.md)** — Material Design color palette with shades
- **[Encryption & Security](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/encryption.md)** — AES-256 by default, with AES-128 compatibility mode and permission flags
- **[Vector Graphics](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/vector-graphics.md)** — Canvas API, shapes, paths, grids, and charts
- **[Table of Contents](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/table-of-contents.md)** — Automatic TOC generation from headings
- **[Bookmarks](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/bookmarks.md)** — PDF bookmarks and outlines
- **[Components & Templates](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/components-and-templates.md)** — Reusable components and document templates
- **[Metadata](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/metadata.md)** — Document metadata (Title, Author, Subject, Keywords, Creator)
- **[Unicode & Character Encoding](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/docs/unicode-and-encoding.md)** — WinAnsiEncoding and character coverage
- **[Samples](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/tree/master/samples)** — Complete working examples demonstrating all features

---

## FAQ

**Is TerraFluent.Pdf.Reporting really free for commercial use?**
Yes. TerraFluent.Pdf.Reporting is released under the [MIT license](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/LICENSE) — you can use it in closed-source and commercial products at no cost, with no revenue caps, royalties, watermarks, or feature-limited tiers.

**How does TerraFluent.Pdf.Reporting compare to iTextSharp, QuestPDF, or PDFsharp?**
iText (iTextSharp) is AGPL-licensed, which requires a commercial license for most closed-source use. QuestPDF's Community license is free only below a company-revenue threshold. PDFsharp is MIT like TerraFluent.Pdf.Reporting, but uses an imperative drawing model. TerraFluent.Pdf.Reporting offers a fluent, composable, code-first API under a plain MIT license with zero runtime dependencies.

**Does TerraFluent.Pdf.Reporting convert HTML to PDF?**
No. TerraFluent.Pdf.Reporting is a code-first PDF generator: you compose documents from C# layout primitives (`Column`, `Row`, `Table`, `Text`, `Image`). That means no headless browser, deterministic output, and much faster rendering — but if your source content is HTML, an HTML-to-PDF converter is a better fit.

**Does it run on Linux, macOS, and in Docker?**
Yes. TerraFluent.Pdf.Reporting is 100% managed C# with no native binaries, so it runs on any platform supported by .NET 8/9/10 — including Alpine-based Docker images, Azure Functions, and AWS Lambda — with nothing extra to install.

**Can it create password-protected PDFs?**
Yes. Documents can be encrypted with AES-256 (default) or AES-128, with user/owner passwords and fine-grained permission flags (printing, copying, editing, etc.).

---

## Building from Source

```sh
git clone https://github.com/sahebansari/TerraFluent.Pdf.Reporting.git
cd TerraFluent.Pdf.Reporting
dotnet build
dotnet test
```

Requires the **.NET 10 SDK** (builds all targets), plus the .NET 8 and .NET 9
runtimes to execute the full multi-framework test suite.

---

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/CONTRIBUTING.md) for coding standards, project structure, how to run tests, and the pull-request process.

---

## Changelog

All notable changes are documented in [CHANGELOG.md](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/CHANGELOG.md), following the [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) format.

---

## Security

To report a vulnerability, please follow the responsible-disclosure process described in [SECURITY.md](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/SECURITY.md). **Do not open a public issue for security problems.**

---

## License

MIT — see [LICENSE](https://github.com/sahebansari/TerraFluent.Pdf.Reporting/blob/master/LICENSE) for details.
