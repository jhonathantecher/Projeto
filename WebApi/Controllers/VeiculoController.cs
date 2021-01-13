using Microsoft.AspNetCore.Mvc;
using Projeto.Entity;
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
        public async Task<List<Veiculo>> Get()
        {
            return await servico.ListagemVeiculos();
        }

        [HttpGet("{placa}")]
        public async Task<Veiculo> Get(string placa)
        {
            return await this.servico.BuscarVeiculo(placa);
        }

        [HttpPost]
        public async Task<bool> Post(Veiculo veiculo)
        {
            return await this.servico.CadastrarVeiculo(veiculo);
        }

        [HttpPut]
        public async Task<bool> Put(Veiculo veiculo)
        {
            return await this.servico.AtualizarVeiculo(veiculo);
        }

        [HttpDelete("{placa}")]
        public async Task<bool> Delete(string placa)
        {
            return await this.servico.ExcluirVeiculo(placa);
        }
    }
}
