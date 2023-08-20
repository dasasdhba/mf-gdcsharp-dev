#if TOOLS

using Godot;
using Godot.Collections;
using System.Linq;
using GlobalClass;

namespace Editor.Addon;

/// <summary>
/// Create SpriteFrames resource through aseprite json files.
/// </summary>
[Tool]
public partial class AsepriteFrames
{
    public struct FramesInfo
    {
        public string TexPath { get; set; }
        public Dictionary Json { get; set; }
        public string AnimName { get; set; }
        public bool Loop { get; set; }
        public bool TagOnly { get; set; }

        public FramesInfo(string t, Dictionary j, string a, bool l, bool tag) =>
            (TexPath, Json, AnimName, Loop, TagOnly) = (t, j, a, l, tag);

    }

    private FramesInfo[] Infos;

    public AsepriteFrames(System.Collections.Generic.IEnumerable<FramesInfo> infos) 
        => Infos = infos.ToArray();

    public SpriteFrames Create(bool usingOffset = false)
    {
        SpriteFrames spr = usingOffset? new SpriteFramesOffset() : new SpriteFrames();
        spr.RemoveAnimation("default");

        // use tag only if no relevant file exists
        if (Infos.Length == 1)
        {
            AddAnimation(spr, Infos[0], true);
            return spr;
        }

        foreach (FramesInfo info in Infos)
        {
            AddAnimation(spr, info, info.TagOnly);
        }
        
        return spr;
    }

    private static void AddAnimation(SpriteFrames spr, FramesInfo info, bool tagOnly = false)
    {
        Dictionary json = info.Json;

        Array<Dictionary> frames = (Array<Dictionary>)json["frames"];
        Array<Dictionary> tags = (Array<Dictionary>)((Dictionary)json["meta"])["frameTags"];

        if (tags.Count > 0)
        {
            foreach (Dictionary tag in tags)
            {
                string name = (string)tag["name"];
                if (!tagOnly)
                    name = info.AnimName + "." + name;

                int from = (int)tag["from"];
                int to = (int)tag["to"];

                string dir = (string)tag["direction"];

                Dictionary[] tagFrames = frames.ToArray()[from..(to + 1)];

                AddFramesToAnimation(spr, info, name, tagFrames, dir);
            }

            return;
        }

        AddFramesToAnimation(spr, info, info.AnimName, frames.ToArray());

    }

    private static void AddFramesToAnimation(SpriteFrames spr, FramesInfo info,
        string animName, Dictionary[] frames, string direction = "forward")
    {
        // oneshot support
        bool oneshot = animName.EndsWith("_oneshot");
        if (oneshot)
            animName = animName[..^8];

        if (spr.HasAnimation(animName)) { return; }

        spr.AddAnimation(animName);

        float minDuration = GetMinDuration(frames);
        float fps = GetFps(minDuration);

        bool loop = !oneshot && info.Loop;
        spr.SetAnimationLoop(animName, loop);
        spr.SetAnimationSpeed(animName, fps);

        bool reversed = direction == "reverse" || direction == "pingpong_reverse";
        System.Collections.Generic.IEnumerable<Dictionary> iFrames =
            reversed ? frames.Reverse() : frames;

        if (direction.StartsWith("pingpong"))
        {
            if (!reversed)
                iFrames = iFrames.Concat(frames[1..^1].Reverse());
            else
                iFrames = iFrames.Concat(frames[1..^1]);
        }

        Texture2D texture = (Texture2D)GD.Load(info.TexPath);
        texture.TakeOverPath(info.TexPath);

        System.Collections.Generic.Dictionary<Rect2, AtlasTexture> cachedTexture = new();

        foreach (Dictionary frame in iFrames)
        {
            Dictionary frameRect = (Dictionary)frame["frame"];
            Rect2 rect = new((float)frameRect["x"], (float)frameRect["y"],
                (float)frameRect["w"], (float)frameRect["h"]);

            AtlasTexture atlasTex;

            if (cachedTexture.ContainsKey(rect)) 
            { 
                atlasTex = cachedTexture[rect];
            }
            else
            {
                atlasTex = new()
                {
                    Atlas = texture,
                    Region = rect
                };
            }

            float duration = (float)frame["duration"];

            spr.AddFrame(animName, atlasTex, duration/minDuration);
        }

    }

    private static float GetMinDuration(Dictionary[] frames)
    {
        float result = Mathf.Inf;
        foreach (Dictionary frame in frames)
        {
            float duration = (float)frame["duration"];
            result = duration < result ? duration : result;
        }

        return result;
    }

    private static float GetFps(float minDuration) => Mathf.Ceil(1000.0f / minDuration);

}

#endif