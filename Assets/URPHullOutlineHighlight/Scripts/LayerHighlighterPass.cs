using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;
using static UnityEditor.ShaderData;

public class LayerHighlighterPass : ScriptableRenderPass
{
    FilteringSettings m_FilteringSettings;

    public Material pass1Material { get; set; }
    public Material pass2Material { get; set; }
    public Material pass3Material { get; set; }

    List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

    RenderStateBlock m_RenderStateBlock;

    public LayerHighlighterPass(RenderPassEvent renderPassEvent, int layerMask)
    {
        this.renderPassEvent = renderPassEvent;
        this.pass1Material = null;

        m_FilteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);

        m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
        m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
        m_ShaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));

        m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
        DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);

        CommandBuffer cmd = CommandBufferPool.Get();

        // Ensure we flush our command-buffer before we render...
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        drawingSettings.overrideMaterial = pass1Material;
        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

        drawingSettings.overrideMaterial = pass2Material;
        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

        drawingSettings.overrideMaterial = pass3Material;
        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);
    }
}

