using MediatR;

namespace PdfConverter.Commands
{
    public record ConvertToPdfCommand(byte[] bytes) : IRequest<byte[]>;
}
