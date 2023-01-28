namespace Character.API.CharacterAI;

/// <summary>
/// A static class containing known endpoints used in the Character.AI API.
/// </summary>
public static class KnownEndpoints
{
    /// <summary>
    /// The endpoint for authenticating a user.
    /// </summary>
    public const string Auth = "https://beta.character.ai/chat/auth/lazy";

    /// <summary>
    /// The endpoint for retrieving information about a character.
    /// </summary>
    public const string CharacterInfo = "https://beta.character.ai/chat/character/info-cached";
    
    public const string CharacterChatRoom = "https://beta.character.ai/chat?char=";
}