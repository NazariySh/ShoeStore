using AutoMapper;
using ShoeStore.Application.DTOs.Orders;
using ShoeStore.Application.Interfaces.Services;
using ShoeStore.Application.Interfaces.Services.Orders;
using ShoeStore.Application.Utilities;
using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Models;
using ShoeStore.Domain.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Domain.Enums;

namespace ShoeStore.Application.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public OrderService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public async Task<Guid> CreateAsync(OrderCreateDto orderCreateDto, Guid userId, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(orderCreateDto, cancellationToken);

        var orderId = await _unitOfWork.Orders.CreateFromCartAsync(
            orderCreateDto.ShoppingCartId,
            userId,
            orderCreateDto.DeliveryMethodId,
            cancellationToken: cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return orderId;
    }

    public Task UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken = default)
    {
        return _unitOfWork.Orders.UpdateStatusAsync(id, status, cancellationToken);
    }

    public async Task<OrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Orders.GetSingleAsync(
            x => x.OrderId == id,
            x => x
                .Include(o => o.DeliveryMethod)
                .Include(o => o.Customer)
                    .ThenInclude(oc => oc.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Shoe)
                        .ThenInclude(ois => ois.ShoeImages),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Order with id {id} not found");

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<PagedList<OrderDto>> GetAllAsync(OrderQuery query, CancellationToken cancellationToken = default)
    {
        var pagination = query.Pagination;
        var sort = query.Sort;

        var orders = await _unitOfWork.Orders.GetAllAsync(
            pagination.PageNumber,
            pagination.PageSize,
            GetSearchExpression(query),
            include: x => x
                .Include(o => o.Employee)
                .Include(o => o.Customer)
                    .ThenInclude(oc => oc.Address)
                .Include(o => o.DeliveryMethod),
            sortBy: GetSortExpression(sort.SortBy),
            isSortDescending: sort.IsDescendingOrder,
            cancellationToken: cancellationToken);

        return _mapper.Map<PagedList<OrderDto>>(orders);
    }

    public async Task<PagedList<OrderDto>> GetAllAsync(OrderQuery query, Guid userId, CancellationToken cancellationToken = default)
    {
        var pagination = query.Pagination;
        var sort = query.Sort;

        var orders = await _unitOfWork.Orders.GetAllAsync(
            pagination.PageNumber,
            pagination.PageSize,
            GetSearchExpression(query, userId),
            include: x => x
                .Include(o => o.DeliveryMethod),
            sortBy: GetSortExpression(sort.SortBy),
            isSortDescending: sort.IsDescendingOrder,
            cancellationToken: cancellationToken);

        return _mapper.Map<PagedList<OrderDto>>(orders);
    }

    private static Expression<Func<Order, bool>>? GetSearchExpression(OrderQuery query, Guid userId)
    {
        var searchTerm = query.Search.SearchTerm;

        if (string.IsNullOrEmpty(searchTerm))
        {
            return x => (x.CustomerId == userId);
        }

        return x => (x.CustomerId == userId) &&
            ((x.Status.ToLower().Contains(searchTerm) ||
            x.Customer.Email.ToLower().Contains(searchTerm) ||
            (x.EmployeeId.HasValue && x.Employee != null && x.Employee.Email.ToLower().Contains(searchTerm))));
    }

    private static Expression<Func<Order, bool>>? GetSearchExpression(OrderQuery query)
    {
        var searchTerm = query.Search.SearchTerm;

        if (string.IsNullOrEmpty(searchTerm))
        {
            return null;
        }

        return x =>
                    ((x.Status.ToLower().Contains(searchTerm) ||
                      x.Customer.Email.ToLower().Contains(searchTerm) ||
                      (x.EmployeeId.HasValue && x.Employee != null && x.Employee.Email.ToLower().Contains(searchTerm))));
    }

    private static Expression<Func<Order, object>> GetSortExpression(string? sortBy)
    {
        return SortExpression.BuildOrDefault<Order>(sortBy, x => x.CreatedAt);
    }
}