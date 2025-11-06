using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Application.DTOs.Accounts;
using ShoeStore.Application.Interfaces;
using ShoeStore.Application.Interfaces.Services;
using ShoeStore.Application.Interfaces.Services.Users;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;


    public UserService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetSingleAsync(
            x => x.UserId == id,
            include: x => x
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .Include(u => u.RefreshToken)
                .Include(u => u.OrderCustomers),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"User with id {id} not found");

        _unitOfWork.Users.Remove(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetSingleAsync(
            x => x.UserId == id,
            include: x => x
                .Include(u => u.Address)
                .Include(u => u.Roles),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"User with id {id} not found");

        return _mapper.Map<UserDto>(user);
    }

    public async Task ResetPassword(Guid id, ResetPasswordDto resetPassword, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetSingleAsync(
            x => x.UserId == id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"User with id {id} not found");

        if (!_passwordHasher.Verify(resetPassword.CurrentPassword, user.PasswordHash))
        {
            throw new ArgumentException("Invalid current password");
        }

        user.PasswordHash = _passwordHasher.Hash(resetPassword.NewPassword);

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AddressDto> UpdateAddressAsync(Guid id, AddressDto addressDto, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(addressDto, cancellationToken);

        var address = _mapper.Map<Address>(addressDto);
        address.UserId = id;

        await _unitOfWork.Users.UpdateAddressAsync(address, cancellationToken);

        return addressDto;
    }
}