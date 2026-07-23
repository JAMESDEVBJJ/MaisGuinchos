using MaisGuinchos.Dtos.Guincho;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class UserProfileResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string NumeroTelefone { get; set; }

    public string Cpf { get; set; }

    public string Tipo { get; set; }

    public double Estrelas { get; set; }

    public TowGuinchoDTO? Guincho { get; set; }
}

public enum UserType
{
    Cliente = 0,
    Motorista = 1,
    Empresa = 2
}