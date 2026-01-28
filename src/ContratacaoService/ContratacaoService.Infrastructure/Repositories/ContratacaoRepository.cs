using Microsoft.EntityFrameworkCore;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Repositories;
using ContratacaoService.Infrastructure.Data;

namespace ContratacaoService.Infrastructure.Repositories;

public class ContratacaoRepository : IContratacaoRepository
{
    private readonly ContratacaoDbContext _context;

    public ContratacaoRepository(ContratacaoDbContext context)
    {
        _context = context;
    }

    public async Task<Contratacao?> ObterPorIdAsync(Guid id)
    {
        return await _context.Contratacoes.FindAsync(id);
    }

    public async Task<IEnumerable<Contratacao>> ObterTodasAsync()
    {
        return await _context.Contratacoes.ToListAsync();
    }

    public async Task<Contratacao> CriarAsync(Contratacao contratacao)
    {
        _context.Contratacoes.Add(contratacao);
        await _context.SaveChangesAsync();
        return contratacao;
    }

    public async Task<bool> ExistePorPropostaIdAsync(Guid propostaId)
    {
        return await _context.Contratacoes.AnyAsync(c => c.PropostaId == propostaId);
    }
}
