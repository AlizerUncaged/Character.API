using Newtonsoft.Json;

namespace Character.API.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Character
{
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("visibility")]
    public string Visibility { get; set; }

    [JsonProperty("copyable")]
    public bool? Copyable { get; set; }

    [JsonProperty("greeting")]
    public string Greeting { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("identifier")]
    public string Identifier { get; set; }

    [JsonProperty("avatar_file_name")]
    public string AvatarFileName { get; set; }

    [JsonProperty("songs")]
    public List<object> Songs { get; set; }

    [JsonProperty("img_gen_enabled")]
    public bool? ImgGenEnabled { get; set; }

    [JsonProperty("base_img_prompt")]
    public string BaseImgPrompt { get; set; }

    [JsonProperty("img_prompt_regex")]
    public string ImgPromptRegex { get; set; }

    [JsonProperty("strip_img_prompt_from_msg")]
    public bool? StripImgPromptFromMsg { get; set; }

    [JsonProperty("user__username")]
    public string UserUsername { get; set; }

    [JsonProperty("participant__name")]
    public string ParticipantName { get; set; }

    [JsonProperty("participant__num_interactions")]
    public int? ParticipantNumInteractions { get; set; }

    [JsonProperty("participant__user__username")]
    public string ParticipantUserUsername { get; set; }

    [JsonProperty("voice_id")]
    public int? VoiceId { get; set; }

    [JsonProperty("usage")]
    public string Usage { get; set; }
    
    public override string ToString()
    {
        return $"{Name}, (ID: {ExternalId}) \nTitled: {Title} with a greeting of '{Greeting}'. \nVisibility: {Visibility} and has a description of '{Description}'.\nAvatar Url: {AvatarFileName}";
    }
}

public class CharacterInfo
{
    [JsonProperty("character")] public Character Character { get; set; }

    public string Name => Character.Name;
    
    public string Title => Character.Title;

    [JsonProperty("status")] public string Status { get; set; }
}