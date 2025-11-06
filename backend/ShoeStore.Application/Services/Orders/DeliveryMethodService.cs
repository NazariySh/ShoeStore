using AutoMapper;
using ShoeStore.Application.DTOs.Orders.DeliveryMethods;
using ShoeStore.Application.Interfaces.Services.Orders;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Services.Orders;

public class DeliveryMethodService : IDeliveryMethodService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeliveryMethodService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DeliveryMethodDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deliveryMethod = await _unitOfWork.DeliveryMethods.GetSingleAsync(
            x => x.DeliveryMethodId == id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Delivery method with id {id} not found");

        return _mapper.Map<DeliveryMethodDto>(deliveryMethod);
    }

    public async Task<IReadOnlyList<DeliveryMethodDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var deliveryMethods = await _unitOfWork.DeliveryMethods.GetAllAsync(
            cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods);
    }
}