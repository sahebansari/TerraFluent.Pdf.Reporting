using TerraFluent.Pdf.Reporting.Core;
using TerraFluent.Pdf.Reporting.Elements;
using TerraFluent.Pdf.Reporting.Helpers;
using TerraFluent.Pdf.Reporting.Infra;
using Xunit;

namespace TerraFluent.Pdf.Reporting.Tests;

/// <summary>
/// Tests for TextBlock self-splitting: paragraphs taller than the remaining
/// page flow across pages, split between wrapped lines by the fragment engine.
/// </summary>
public sealed class TextSplittingTests
{
    private static byte[] Build(Action<IDocumentContainer> compose) =>
        PdfDocument.Create(compose).PublishPdf();

    private static int CountPages(byte[] pdf)
    {
        string text = System.Text.Encoding.Latin1.GetString(pdf);
        int count = 0, idx = 0;
        while ((idx = text.IndexOf("/Type /Page /", idx, StringComparison.Ordinal)) >= 0) { count++; idx++; }
        return count;
    }

    /// <summary>A paragraph long enough to overflow at least two A4 pages.</summary>
    private static string LongParagraph(out string first, out string last)
    {
        first = "STARTSENTINEL";
        last  = "ENDSENTINEL";
        var words = new System.Text.StringBuilder(first);
        for (int i = 0; i < 1200; i++)
            words.Append(" filler").Append(i);
        words.Append(' ').Append(last);
        return words.ToString();
    }

    [Fact]
    public void LongParagraphFlowsAcrossPages()
    {
        string para = LongParagraph(out string first, out string last);

        byte[] pdf = Build(c => c.Page(p =>
        {
            p.Size(PageSize.A4);
            p.Margin(2, Unit.Centimetre);
            p.Content().Column(col =>
            {
                col.Item().Text(para);
            });
        }));

        Assert.True(CountPages(pdf) >= 2,
            $"A ~1200-word paragraph must span multiple pages, got {CountPages(pdf)}.");

        // Every word survives the split.
        string content = PdfTestUtils.InflatedText(pdf);
        Assert.Contains(first, content);
        Assert.Contains(last, content);
        Assert.Contains("filler600", content);   // middle of the paragraph
    }

    [Fact]
    public void SplitContentIsDistributedNotDuplicated()
    {
        string para = LongParagraph(out string first, out string last);

        byte[] pdf = Build(c => c.Page(p =>
        {
            p.Size(PageSize.A4);
            p.Margin(2, Unit.Centimetre);
            p.Content().Column(col => col.Item().Text(para));
        }));

        string content = PdfTestUtils.InflatedText(pdf);

        // First and last sentinels must live in different content streams
        // (an "endstream" boundary sits between them), and each appears once.
        int idxFirst = content.IndexOf(first, StringComparison.Ordinal);
        int idxLast  = content.IndexOf(last, StringComparison.Ordinal);
        Assert.True(idxFirst >= 0 && idxLast > idxFirst);
        Assert.True(content.IndexOf("endstream", idxFirst, StringComparison.Ordinal) < idxLast,
            "Start and end of the paragraph must be on different pages.");

        Assert.Equal(idxFirst, content.LastIndexOf(first, StringComparison.Ordinal));
        Assert.Equal(idxLast,  content.LastIndexOf(last,  StringComparison.Ordinal));
    }

    [Fact]
    public void PaddedLongParagraphStillSplits()
    {
        string para = LongParagraph(out string first, out string last);

        byte[] pdf = Build(c => c.Page(p =>
        {
            p.Size(PageSize.A4);
            p.Margin(2, Unit.Centimetre);
            p.Content().Column(col =>
            {
                col.Item().Padding(20).Text(para);
            });
        }));

        Assert.True(CountPages(pdf) >= 2);
        string content = PdfTestUtils.InflatedText(pdf);
        Assert.Contains(first, content);
        Assert.Contains(last, content);
    }

    [Fact]
    public void HyperlinkWrappedLongTextDoesNotSplit()
    {
        // Link is opaque to the walker (splitting would bypass Link.Draw and
        // drop the annotation), so the legacy single-page overflow behaviour
        // is preserved and the URI annotation survives.
        string para = LongParagraph(out _, out _);

        byte[] pdf = Build(c => c.Page(p =>
        {
            p.Size(PageSize.A4);
            p.Margin(2, Unit.Centimetre);
            p.Content().Column(col =>
            {
                col.Item().Hyperlink("https://example.com/long").Text(para);
            });
        }));

        Assert.Equal(1, CountPages(pdf));
        Assert.Contains("/URI", System.Text.Encoding.Latin1.GetString(pdf));
    }

    [Fact]
    public void ShortParagraphsAreUnaffected()
    {
        byte[] pdf = Build(c => c.Page(p =>
        {
            p.Size(PageSize.A4);
            p.Margin(2, Unit.Centimetre);
            p.Content().Column(col =>
            {
                col.Item().Text("A perfectly ordinary paragraph.");
                col.Item().Text("And another one.");
            });
        }));

        Assert.Equal(1, CountPages(pdf));
    }

    [Fact]
    public void SplitParagraphFollowedByMoreItemsKeepsOrder()
    {
        string para = LongParagraph(out _, out string last);

        byte[] pdf = Build(c => c.Page(p =>
        {
            p.Size(PageSize.A4);
            p.Margin(2, Unit.Centimetre);
            p.Content().Column(col =>
            {
                col.Item().Text(para);
                col.Item().Text("AFTERSENTINEL");
            });
        }));

        string content = PdfTestUtils.InflatedText(pdf);
        int idxLast  = content.IndexOf(last, StringComparison.Ordinal);
        int idxAfter = content.IndexOf("AFTERSENTINEL", StringComparison.Ordinal);
        Assert.True(idxLast >= 0 && idxAfter > idxLast,
            "The item after a split paragraph must render after its final slice.");
    }

    // ── Force-breaking of words wider than the available width ──────────────

    [Fact]
    public void OverlongWordIsForceBrokenWithinAvailableWidth()
    {
        const string word = "TerraFluent.Pdf.Reporting.UnbreakableSponsorName";
        var block = new TextBlock(word);
        const double width = 60;

        var size = block.Measure(width, 800);
        var (lines, _, _) = block.LayoutLines(width, null, 1);

        Assert.True(lines.Count > 1,
            "A word wider than the line must wrap onto multiple lines.");
        Assert.True(size.Width <= width,
            $"No wrapped line may exceed the available width ({size.Width:F2} > {width}).");

        // No characters are lost or duplicated by the forced break.
        string reassembled = string.Concat(lines.SelectMany(l => l.Tokens).Select(t => t.Text));
        Assert.Equal(word, reassembled);
    }

    [Fact]
    public void OverlongWordMidParagraphKeepsSurroundingWordsAndWidth()
    {
        const string text = "PLATINUM TerraFluent.Pdf.Reporting.Co sponsors";
        var block = new TextBlock(text);
        const double width = 70;

        var size = block.Measure(width, 800);
        Assert.True(size.Width <= width,
            $"No wrapped line may exceed the available width ({size.Width:F2} > {width}).");

        // Wrapping trims inter-line whitespace; every word character must survive.
        var (lines, _, _) = block.LayoutLines(width, null, 1);
        string reassembled = string.Concat(lines.SelectMany(l => l.Tokens).Select(t => t.Text));
        Assert.Equal(text.Replace(" ", ""), reassembled.Replace(" ", ""));
    }

    [Fact]
    public void PathologicallyNarrowWidthStillTerminates()
    {
        // Narrower than any single glyph: one character per line, no infinite loop.
        var block = new TextBlock("WIDE");
        var (lines, _, _) = block.LayoutLines(0.1, null, 1);

        Assert.Equal(4, lines.Count);
        string reassembled = string.Concat(lines.SelectMany(l => l.Tokens).Select(t => t.Text));
        Assert.Equal("WIDE", reassembled);
    }
}
