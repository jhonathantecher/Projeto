using Microsoft.AspNetCore.Mvc;
using Projeto.Model;
using Projeto.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/veiculo")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {
        VeiculoService servico = new VeiculoService();

        [HttpGet]
        public async Task<List<VeiculoModel>> ObterListagemVeiculos()
        {
            return await servico.ListagemVeiculos();
        }

        [HttpGet("{pesquisa}")]
        public async Task<List<VeiculoModel>> PesquisarListagemVeiculos(string pesquisa)
        {
            return await this.servico.PesquisarVeiculos(pesquisa);
        }

        [HttpPut]
        public async Task<bool> AtualizarVeiculo(VeiculoModel veiculo)
        {
            return await this.servico.AtualizarVeiculo(veiculo);
        }

        [HttpDelete("{placa}")]
        public async Task<bool> ExcluirVeiculo(string placa)
        {
            return await this.servico.ExcluirVeiculo(placa);
        }
    }
}
