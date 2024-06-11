using Bogus;
using JornadaMilhasV1.Gerencidor;
using JornadaMilhasV1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Test
{
    public class GerenciadorDeOfertasRecuperaMaiorDesconto
    {
        [Fact]
        public void RetornaOfertaNulaQUandoListaVazia()
        {
            List<OfertaViagem> lista = new List<OfertaViagem>();
            GerenciadorDeOfertas gerenciador = new(lista);
            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");

            OfertaViagem? oferta = gerenciador.RecuperaMaiorDesconto(filtro);

            Assert.Null(oferta);
        }

        [Fact]
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
        {
            Faker<Periodo> fakerPeriodo = new Faker<Periodo>()
                .CustomInstantiator(f =>
                {
                    DateTime dataInicio = f.Date.Soon();
                    return new Periodo(dataInicio, dataInicio.AddDays(30));
                });

            Rota rota = new("Curitiba", "São Paulo");

            Faker<OfertaViagem> fakerOferta = new Faker<OfertaViagem>()
                .CustomInstantiator(f => new OfertaViagem(
                    rota,
                    fakerPeriodo.Generate(),
                    100 * f.Random.Int(1, 100))
                )
                .RuleFor(o => o.Desconto, f => 40)
                .RuleFor(o => o.Ativa, f => true);

            OfertaViagem ofertaEscolhina = new(rota, fakerPeriodo.Generate(), 80)
            {
                Desconto = 40,
                Ativa = true
            };

            OfertaViagem ofertaInativa = new(rota, fakerPeriodo.Generate(), 70)
            {
                Desconto = 40,
                Ativa = false
            };

            List<OfertaViagem> lista = fakerOferta.Generate(200);
            lista.Add(ofertaEscolhina);
            lista.Add(ofertaInativa);
            GerenciadorDeOfertas gerenciador = new(lista);
            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            double precoEsperado = 40.00;

            OfertaViagem? oferta = gerenciador.RecuperaMaiorDesconto(filtro);

            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco);
        }
    }
}
