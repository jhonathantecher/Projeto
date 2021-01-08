using Microsoft.AspNetCore.Mvc;
using Projeto.Entity;
using Projeto.Service;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {
        VeiculoService servico = new VeiculoService();

        // GET: api/<VeiculoController>
        [HttpGet]
        public List<Veiculo> Get()
        {
            return servico.ListagemVeiculos();
        }

        // GET api/<VeiculoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VeiculoController>
        [HttpPost]
        public ActionResult Post([FromBody] Veiculo veiculo)
        {
            try
            {
                this.servico.CadastrarVeiculo(veiculo);

                return Ok("Cadastrado com Sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }

        // PUT api/<VeiculoController>/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] Veiculo veiculo)
        {
            try
            {
                this.servico.AtualizarVeiculo(veiculo);

                return Ok("Atualizado com Sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }

        // DELETE api/<VeiculoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
