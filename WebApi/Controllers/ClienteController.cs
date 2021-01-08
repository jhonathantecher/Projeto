﻿using Microsoft.AspNetCore.Mvc;
using Projeto.Entity;
using Projeto.Service;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        ClienteService servico = new ClienteService();

        // GET api/cliente
        [HttpGet]
        public ActionResult<List<Cliente>> Get()
        {
            return servico.ListagemClientes();
        }

        // GET api/cliente/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/cliente
        [HttpPost]
        public ActionResult Post([FromBody] Cliente cliente)
        {
            try
            {
                this.servico.CadastrarCliente(cliente);

                return Ok("Cadastrado com Sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }

        // PUT api/cliente/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] Cliente cliente)
        {
            try
            {
                this.servico.AtuaizarCliente(cliente);

                return Ok("Atualizado com Sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }

        // DELETE api/cliente/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
