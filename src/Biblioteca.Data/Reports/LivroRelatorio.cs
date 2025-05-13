using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Biblioteca.Data.Reports
{
    public class LivroRelatorio : ILivroRelatorio
    {
        public byte[] GerarRelatorio(List<Livro> livros)
        {
            var dataAtualRodape = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(8));

                    ConfigurarCabecalho(page);

                    page.Content()
                        .Border(1)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten1).Border(1).Text("Id").AlignCenter().FontSize(12).Bold();
                                header.Cell().Background(Colors.Grey.Lighten1).Border(1).Text("Título").AlignCenter().FontSize(12).Bold();
                                header.Cell().Background(Colors.Grey.Lighten1).Border(1).Text("Isbn").AlignCenter().FontSize(12).Bold();
                                header.Cell().Background(Colors.Grey.Lighten1).Border(1).Text("Autor").AlignCenter().FontSize(12).Bold();
                                header.Cell().Background(Colors.Grey.Lighten1).Border(1).Text("Status").AlignCenter().FontSize(12).Bold();
                            });

                            foreach (var livro in livros)
                            {
                                table.Cell().Border(1).Padding(2).Text(livro.Id.ToString()).FontSize(8).AlignCenter();
                                table.Cell().Border(1).Padding(2).Text(livro.Titulo).FontSize(8).AlignCenter();
                                table.Cell().Border(1).Padding(2).Text(livro.Isbn).FontSize(8).AlignCenter();
                                table.Cell().Border(1).Padding(2).Text(livro.Autor.Nome).FontSize(8).AlignCenter();
                                table.Cell().Border(1).Padding(2).Text(livro.Status.ToString()).FontSize(8).AlignCenter();
                            }
                        });

                    ConfigurarRodape(page, dataAtualRodape);
                });
            })
            .GeneratePdf();
        }

        private void ConfigurarCabecalho(PageDescriptor page)
        {
            page.Header()
                .Padding(5)
                .Text("Status do Acervo")
                .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium).AlignCenter();
        }

        private void ConfigurarRodape(PageDescriptor page, string dataAtual)
        {
            page.Footer()
                .PaddingTop(3)
                .Row(row =>
                {
                    row.RelativeItem()
                        .AlignLeft()
                        .Text($"Documento gerado na data {dataAtual}");

                    row.RelativeItem()
                        .AlignRight()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
        }
    }
}
