using Application.Dtos;
using MediatR;

namespace Application.Queries.GetAllUsersQuery;

public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>;