using Application.Dtos;
using MediatR;

namespace Application.Queries.GetUserByIdQuery;

public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;