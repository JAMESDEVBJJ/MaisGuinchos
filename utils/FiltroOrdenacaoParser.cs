public enum FiltroOrdenacao
{
    Distancia,
    Avaliacao,
    Preco
}

public static class FiltroOrdenacaoParser
{
    public static List<FiltroOrdenacao> Parse(string? filtros)
    {
        if (string.IsNullOrWhiteSpace(filtros))
        {
            return [];
        }

        return filtros
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(f => Enum.TryParse<FiltroOrdenacao>(f, ignoreCase: true, out var parsed)
                ? parsed
                : (FiltroOrdenacao?) null)
            .Where(f => f.HasValue)
            .Select(f => f!.Value)
            .Distinct()
            .ToList();
    }
}