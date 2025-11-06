namespace ShoeStore.Application.DTOs;

public record ValidationErrorResponseDto(Dictionary<string, string> Errors)
    : ErrorResponseDto("Validation errors");