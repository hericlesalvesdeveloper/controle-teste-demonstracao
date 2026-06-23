using ControleTeste.DTOs;
using ControleTeste.Enums;
using ControleTeste.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ControleTeste.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlteracaoController : ControllerBase
{
    private readonly ControleTeste.Services.IAlteracaoService _service;

    public AlteracaoController(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(StatusAlteracao status)
    {
        var list = await _service.GetByStatusAsync(status);
        return Ok(list);
    }

    [HttpGet("com-observacao")]
    public async Task<IActionResult> GetWithObservacao()
    {
        var list = await _service.GetWithObservacaoAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RequisicaoAlteracaoDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.AlteracaoId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RequisicaoAlteracaoDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // Alterar status (ex.: para Retorno com observação obrigatória)
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromQuery] StatusAlteracao status, [FromQuery] string? observacao)
    {
        await _service.ChangeStatusAsync(id, status, observacao);
        return NoContent();
    }

    // Endpoint para alterar status via POST com body JSON
    [HttpPost("{id:int}/change")]
    public async Task<IActionResult> ChangeStatusPost(int id, [FromBody] DTOs.ChangeStatusRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _service.ChangeStatusAsync(id, request.Status, request.Observacao);
        return NoContent();
    }
}