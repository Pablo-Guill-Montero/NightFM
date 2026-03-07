using System; // <--- ¡Importante!
using System.Collections.Generic;

[Serializable]
public class FullScore
{
    public List<BeatStep> steps = new List<BeatStep>();
}