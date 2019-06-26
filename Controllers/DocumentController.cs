using System;
using Microsoft.Office.Interop.Word;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WordDocEditor.Interfaces;
using WordDocEditor.Models;

namespace WordDocEditor.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            this._documentService = documentService;
        }

        [HttpPost("[action]")]
        public ActionResult<WordDocument> EditDocument([FromBody] WordDocument file)
        {
            try
            {
                this._documentService.WriteDocument(file);

                Application app = new Application {
                    Visible = false
                };

                Document document = app.Documents.Open($"{Constants.FileStoragePath}{file.FileName}");

                try
                {
                    // get the range of the whole document
                    Range docRange = document.Range(document.Content.Start, document.Content.End);

                    this._documentService.SetWordDocFont(docRange, 12, "Times New Roman");
                    this._documentService.SetWordDocPageMargins(docRange, 85.04f, 28.35f, 56.69f, 56.69f);

                    docRange.ParagraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
                    // se word document alignment
                    docRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    // set word document indent
                    docRange.ParagraphFormat.LeftIndent = 28.35f;

                    // get first page range only
                    Range firstPageRange = this._documentService.GetPageRange(app, document, 1, 2);

                    this._documentService.FindBoldAndReplaceWithItalic(firstPageRange);

                    this._documentService.AddPageNumberingToHeader(app, document, 5);

                    this._documentService.ApplyChangesForDocTables(docRange, document);

                    this._documentService.ApplyChangesForDocInlineShapes(docRange, document);

                    // save changes
                    document.Save();
                    // close document and word application
                    document.Close();
                    app.Quit();

                    file.Base64 = this._documentService.GetBase64FromFile(file.FileName);
                    // delete file locally since we will not need it anymore
                    this._documentService.DeleteDocument(file.FileName);
                    return file;
                }
                catch (Exception ex)
                {
                    document.Close();
                    app.Quit();
                    this._documentService.DeleteDocument(file.FileName);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                this._documentService.DeleteDocument(file.FileName);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
