using AutoMapper;
using Novle.Application.Common.Extensions;
using Novle.Application.ViewModels.Genre.Requests;
using Novle.Domain.Entities;
using Novle.Domain.Repositories.Base;

namespace Novle.Application.Services;

public class GenreService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IRepository<Genre> genreRepository)
    : BaseService(unitOfWork, mapper)
{
    public async Task<List<GetGenreResponse>> GetGenreListAsync(CancellationToken cancellationToken)
    {
        return await genreRepository
            .GetQuery()
            .ProjectToListAsync<GetGenreResponse>(_mapper.ConfigurationProvider, cancellationToken);
    }
}