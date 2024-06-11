using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test
{
    public class OfertaViagemConstrutor
    {
        [Theory]
        [InlineData("", null, "2024-01-01", "2024-01-02", 0, false)]
        [InlineData("OrigemTeste", "DestinoTeste", "2024-01-01", "2024-01-02", 100, true)]
        [InlineData(null, "DestinoTeste", "2024-01-01", "2024-01-02", -1, false)]
        [InlineData("Vitória", "São Paulo", "2024-01-01", "2024-01-01", 0, false)]
        [InlineData("Rio de Janeiro", "São Paulo", "2024-01-01", "2024-01-02", -500, false)]
        public void RetornaEhValidoDeAcordoComDadoDeEntrada(string origem, string destino, string dataIda, string dataVolta, double preco, bool validacao)
        {
            Rota rota = new(origem, destino);
            Periodo periodo = new(DateTime.Parse(dataIda), DateTime.Parse(dataVolta));

            OfertaViagem oferta = new(rota, periodo, preco);

            Assert.Equal(validacao, oferta.EhValido);
        }

        [Fact]
        public void RetornaMensagemDeErroDeRotaOuPeriodoInvalidoQuandoRotaNula()
        {
            Rota rota = null;
            Periodo periodo = new(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = 100.00;

            OfertaViagem oferta = new(rota, periodo, preco);

            Assert.Contains("A oferta de viagem não possui rota ou período válidos.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Fact]
        public void RetornaMensagemDeErroDeDataInvalidaQuandoDataInicialMaiorQueFinal()
        {
            Rota rota = new("OrigemTeste", "DestinoTeste");
            Periodo periodo = new(new DateTime(2025, 2, 1), new DateTime(2024, 2, 5));
            double preco = 100.00;

            OfertaViagem oferta = new(rota, periodo, preco);

            Assert.Contains("Erro: Data de ida não pode ser maior que a data de volta.", oferta.Periodo.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-250)]
        public void RetornaMensagemDeErroDePrecoInvalidoQuandoPrecoMenorOuIgualAZero(double preco)
        {
            Rota rota = new("OrigemTeste", "DestinoTeste");
            Periodo periodo = new(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));

            OfertaViagem oferta = new(rota, periodo, preco);

            Assert.Contains("O preço da oferta de viagem deve ser maior que zero.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Fact]
        public void RetornaTresErrosDeValidacaoQuandoRotaPeriodoPrecoSaoInvalidos()
        {
            int quantidadeEsperada = 3;
            Rota rota = null;
            Periodo periodo = new(new DateTime(2025, 2, 1), new DateTime(2024, 2, 5));
            double preco = -100;

            OfertaViagem oferta = new(rota, periodo, preco);

            Assert.Equal(quantidadeEsperada, oferta.Erros.Count());
        }
    }
}