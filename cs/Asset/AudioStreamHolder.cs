using Godot;

namespace Asset;

/// <summary>
/// Class <c> AudioStreamHolder </c> that holds AudioStream Resource.
/// A godot editor plugin has been implemented to auto generate holders 
/// for res://assets/audio/
/// </summary>
public partial class AudioStreamHolder
{
    protected string AudioStreamPath;

    public AudioStream GetAudioStream()
    {
        if (GD.Load(AudioStreamPath) is AudioStream audio) { return audio; }
        return null;
    }
}