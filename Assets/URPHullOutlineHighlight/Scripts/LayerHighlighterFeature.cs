using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class LayerHighlighterFeature : ScriptableRendererFeature
{
    public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;
    public LayerMask LayerMask = 0;

    public Material pass1Material = null;
    public Material pass2Material = null;
    public Material pass3Material = null;

    LayerHighlighterPass highlighterPass;

    public override void Create()
    {
        if (Event < RenderPassEvent.BeforeRenderingPrePasses)
            Event = RenderPassEvent.BeforeRenderingPrePasses;

        highlighterPass = new LayerHighlighterPass(Event, LayerMask);

        highlighterPass.pass1Material = pass1Material;
        highlighterPass.pass2Material = pass2Material;
        highlighterPass.pass3Material = pass3Material;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(highlighterPass);
    }
}
