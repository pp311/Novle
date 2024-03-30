using AutoMapper;
using Novle.Application.ViewModels.Author.Requests;
using Novle.Application.ViewModels.Author.Responses;
using Novle.Domain.Entities;
using Novle.Domain.Exceptions;
using Novle.Domain.Repositories;
using Novle.Domain.Repositories.Base;

namespace Novle.Application.Services;

public class AuthorService(IUnitOfWork unitOfWork, IMapper mapper, IAuthorRepository authorRepository)
	: BaseService(unitOfWork, mapper)
{
	public async Task<GetAuthorResponse> GetAuthorByIdAsync(int id, CancellationToken cancellationToken)
	{
		var author = await authorRepository.GetByIdAsync(id, cancellationToken);
		EntityNotFoundException.ThrowIfNull(author, id);

		return Mapper.Map<GetAuthorResponse>(author);
	}
	
	public async Task<int> CreateAuthorAsync(UpsertAuthorRequest request)
	{
		var isAuthorExisted = await authorRepository.IsAuthorExistedAsync(request.Name);
		EntityAlreadyExistsException.ThrowIfTrue<Author>(isAuthorExisted);
		
		var author = new Author(request.Name, request.Description, request.AvatarUrl, request.BirthDay);
		authorRepository.Add(author);
		await UnitOfWork.SaveChangesAsync();

		return author.Id;
	}
	
	public async Task UpdateAuthorAsync(int id, UpsertAuthorRequest request)
	{
		var author = await authorRepository.GetByIdAsync(id);
		EntityNotFoundException.ThrowIfNull(author, id);

		author!.Update(request.Name, request.Description, request.AvatarUrl, request.BirthDay);
		await UnitOfWork.SaveChangesAsync();
	}
	
	public async Task DeleteAuthorAsync(int id)
	{
		var author = await authorRepository.GetByIdAsync(id);
		EntityNotFoundException.ThrowIfNull(author, id);

		author!.Delete();
		await UnitOfWork.SaveChangesAsync();
	}
}