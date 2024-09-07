using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ChattyBackend.Handlers.Interfaces;
using ChattyBackend.Helpers.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace ChattyBackend.Controllers;

[ApiController]
[Route("channels")]
public sealed class ChannelsController(IChannelsHandler channelsHandler) : Controller
{
    [HttpGet]
    public async Task<IActionResult> ListByToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type is "id");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var channelsData = await channelsHandler.ListChannelsByUserId(userId);

        return Ok(channelsData);
    }

    [HttpPost]
    public async Task<IActionResult> PostNewChannel(
        [FromBody] NewChannelRequestDto channelRequestDto
    )
    {
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type is "id");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var channelsData = await channelsHandler.CreateNewChannel(userId, channelRequestDto);

        return Ok(channelsData);
    }
}

public interface IChannelsHandler
{
    Task<ImmutableArray<ChatChannelInfo>> ListChannelsByUserId(Guid userId);
    Task<bool> CreateNewChannel(Guid userId, NewChannelRequestDto channelRequestDto);
}

public sealed class ChannelsHandler(IChannelsRepository channelsRepository) : IChannelsHandler
{
    public Task<ImmutableArray<ChatChannelInfo>> ListChannelsByUserId(Guid userId) =>
        channelsRepository.ListChannelsByUserId(userId);

    public async Task<bool> CreateNewChannel(Guid userId, NewChannelRequestDto channelRequestDto)
    {
        var newChannelId = Guid.NewGuid();

        var result = await channelsRepository.CreateNewChannel(
            new NewChannelRepoDto(
                newChannelId,
                userId,
                channelRequestDto.Name,
                channelRequestDto.ImageSmall
            )
        );

        return result;
    }
}

public interface IChannelsRepository
{
    Task<bool> CreateNewChannel(NewChannelRepoDto newChannelRepoDto);
    Task<ImmutableArray<ChatChannelInfo>> ListChannelsByUserId(Guid userId);
}

public sealed class ChannelsRepository(ISqlConnectionProvider sqlConnectionProvider)
    : IChannelsRepository
{
    public async Task<bool> CreateNewChannel(NewChannelRepoDto newChannelRepoDto)
    {
        const string query = """
            INSERT INTO channels (id, name, creation_user_id, image_small)
            SELECT @NewChannelId, @Name, @UserId, @ImageSmall
            """;

        using var conn = await sqlConnectionProvider.GetConnection();

        var result = await conn.ExecuteAsync(query, newChannelRepoDto);

        return result > 0;
    }

    public async Task<ImmutableArray<ChatChannelInfo>> ListChannelsByUserId(Guid userId)
    {
        // TODO: this is wrong, we need a map to tell which users are able to see a channel
        const string query = """
            SELECT c.id, name, image_small, message AS last_message
            FROM channels c
            LEFT JOIN messages ON c.id = messages.channel_id
            WHERE c.creation_user_id = @userId
            LIMIT 500
            """;

        using var conn = await sqlConnectionProvider.GetConnection();

        var result = await conn.QueryAsync<ChatChannelInfo>(query, new { userId });

        // ReSharper disable once UseCollectionExpression
        return result.AsList().ToImmutableArray();
    }
}

public sealed record ChatChannelInfo(Guid Id, string Name, string? ImageSmall, string? LastMessage);

public sealed record NewChannelRepoDto(
    Guid NewChannelId,
    Guid UserId,
    string Name,
    string? ImageSmall
);

public sealed record NewChannelRequestDto(
    [Required] [StringLength(160, MinimumLength = 1)] string Name,
    [StringLength(160)] string? ImageSmall
);
