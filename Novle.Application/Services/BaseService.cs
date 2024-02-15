using AutoMapper;
using Novle.Domain.Repositories.Base;

namespace Novle.Application.Services;

public abstract class BaseService(IUnitOfWork unitOfWork, IMapper mapper)
{
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;
    protected readonly IMapper _mapper = mapper;
}