public class RawgGamesResponseDto
{
    public int count { get; set; }
    public string next { get; set; } = null!;
    public string previous { get; set; } = null!;
    public List<RawgGameDto> results { get; set; } = null!;
}
