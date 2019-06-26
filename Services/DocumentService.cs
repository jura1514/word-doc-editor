using Microsoft.Office.Interop.Word;
using System;
using WordDocEditor.Interfaces;
using WordDocEditor.Models;

namespace WordDocEditor.Services
{
    public class DocumentService : IDocumentService
    {
        public void DeleteDocument(string fileName)
        {
            System.IO.File.Delete($"{Constants.FileStoragePath}{fileName}");
        }

        public void WriteDocument(WordDocument file)
        {
            var base64WithoutType = file.Base64.Split(",")[1];
            Byte[] bytes = Convert.FromBase64String(base64WithoutType);
            System.IO.File.WriteAllBytes($"{Constants.FileStoragePath}{file.FileName}", bytes);
        }

        public string GetBase64FromFile(string fileName)
        {
            Byte[] bytes = System.IO.File.ReadAllBytes($"{Constants.FileStoragePath}{fileName}");
            String base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public void SetWordDocFont(Range docRange, int fontSize, string font)
        {
            docRange.Font.Size = fontSize;
            docRange.Font.Name = font;
        }

        public void SetWordDocPageMargins(Range docRange, float left, float right, float top, float bottom)
        {
            docRange.PageSetup.LeftMargin = left;
            docRange.PageSetup.RightMargin = right;
            docRange.PageSetup.TopMargin = top;
            docRange.PageSetup.BottomMargin = bottom;
        }

        public Range GetPageRange(Application app, Document document, int startPage, int endPage)
        {
            object oMission = System.Type.Missing;
            object what = WdGoToItem.wdGoToPage;
            object which = WdGoToDirection.wdGoToAbsolute;
            object start = startPage;
            object end = endPage;

            object rangeStart = app.Selection.GoTo(ref what, ref which, ref start, ref oMission).Start;
            object rangeEnd = app.Selection.GoTo(ref what, ref which, ref end, ref oMission).End;

            Range pageRange = document.Range(ref rangeStart, ref rangeEnd);
            return pageRange;
        }

        public void FindBoldAndReplaceWithItalic(Range docRange)
        {
            for (int i = 1; i <= docRange.Words.Count; i++)
            {
                if (docRange.Words[i].Bold == -1 ||
                   docRange.Words[i].Font.Bold == -1
                 )

                {
                    docRange.Words[i].Italic = -1;
                    docRange.Words[i].Font.Italic = -1;
                    docRange.Words[i].Bold = 0;
                    docRange.Words[i].Font.Bold = 0;
                }
            }
        }

        public void EditPrimaryHeader(Document document, int SectionNumber)
        {
            var primaryHeader = document.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary];

            if (primaryHeader != null)
            {
                primaryHeader.LinkToPrevious = false;
                primaryHeader.PageNumbers.StartingNumber = 5;
                primaryHeader.Range.Font.Size = 12;
                primaryHeader.Range.Font.Name = "Times New Roman";

                PageNumber pnPrimary = primaryHeader.PageNumbers.Add(WdPageNumberAlignment.wdAlignPageNumberCenter, true);
                if (pnPrimary != null)
                {
                    pnPrimary.Alignment = WdPageNumberAlignment.wdAlignPageNumberCenter;
                }
            }
        }

        public void ApplyChangesForDocTables(Range docRange, Document document)
        {
            for (int i = 1; i <= docRange.Tables.Count; i++)
            {
                Paragraph tableNumberParagraph = document.Paragraphs.Add(docRange.Tables[i].Range.Previous(WdUnits.wdParagraph));
                tableNumberParagraph.Range.Text = $"Table {i}";
                tableNumberParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                docRange.Tables[i].Title = $"Table {i}";
            }
        }

        public void ApplyChangesForDocInlineShapes(Range docRange, Document document)
        {
            for (int i = 1; i <= docRange.InlineShapes.Count; i++)
            {
                var newParAfter = document.Paragraphs.Add(docRange.InlineShapes[i].Range);
                newParAfter.Range.InsertParagraphAfter();

                Paragraph tableNumberParagraph = document.Paragraphs.Add(docRange.InlineShapes[i].Range.Next(WdUnits.wdParagraph));

                tableNumberParagraph.Range.Text = $"Figure {i}";
                tableNumberParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                docRange.InlineShapes[i].Title = $"Figure {i}";
            }
        }
    }
}
