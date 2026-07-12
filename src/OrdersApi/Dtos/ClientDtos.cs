using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Dtos;

public class ClientCreateDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateOnly BirthDate { get; set; }
}

public class ClientUpdateDto : ClientCreateDto
{
}
