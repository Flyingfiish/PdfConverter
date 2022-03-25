using MediatR;
using Microsoft.Office.Interop.Word;
using PdfConverter.Commands;

namespace PdfConverter.CommandHandlers
{
    public class ConvertToPdfCommandHandler : IRequestHandler<ConvertToPdfCommand, byte[]>
    {
        private readonly Application _app;
        public ConvertToPdfCommandHandler(Application app)
        {
            _app = app;
        }
        public async Task<byte[]> Handle(ConvertToPdfCommand request, CancellationToken cancellationToken)
        {
            var tmpFile = Path.GetTempFileName();
            await File.WriteAllBytesAsync(tmpFile, request.bytes);

            Document file = _app.Documents.Open(tmpFile);
            var pdfFile = Path.GetTempFileName();
            file.ExportAsFixedFormat(pdfFile, WdExportFormat.wdExportFormatPDF);
            file.Close(false);
            //_app.Quit();
            return await File.ReadAllBytesAsync(pdfFile);
        }
    }
}
