using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordDocEditor.Models;

namespace WordDocEditor.Interfaces
{
    public interface IDocumentService
    {
        void DeleteDocument(string fileName);
        void WriteDocument(WordDocument file);
        string GetBase64FromFile(string fileName);
        void SetWordDocFont(Range docRange, int fontSize, string font);
        void SetWordDocPageMargins(Range docRange, float left, float right, float top, float bottom);
        Range GetPageRange(Application app, Document document, int startPage, int endPage);
        void FindBoldAndReplaceWithItalic(Range docRange);
        void EditPrimaryHeader(Document document, int SectionNumber);
        void ApplyChangesForDocTables(Range docRange, Document document);
        void ApplyChangesForDocInlineShapes(Range docRange, Document document);
    }
}
