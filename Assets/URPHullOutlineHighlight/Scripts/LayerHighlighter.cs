using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LayerHighlighter : MonoBehaviour
{
    public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;
    public LayerMask LayerMask = 0;

    public Material material1;
    public Material material2;
    public Material material3;

    [System.Serializable]
    public struct HighlightEntry
    {
        public string name;
        public Transform targetTransform;
        public MeshFilter meshFilter;
    }

    private LayerHighlighterPass highlighterPass;

    void DrawMesh(ScriptableRenderContext context, Camera cam)
    {
        if (material1 == null || material2 == null || material3 == null) return;

        var renderer = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(0);
        renderer.EnqueuePass(highlighterPass);
    }

    [ContextMenu("Example: Show Highlights")]
    public void ShowHighlights()
    {
        RenderPipelineManager.beginCameraRendering -= DrawMesh;

        if (Event < RenderPassEvent.BeforeRenderingPrePasses)
            Event = RenderPassEvent.BeforeRenderingPrePasses;

        highlighterPass = new LayerHighlighterPass(Event, LayerMask);

        highlighterPass.pass1Material = material1;
        highlighterPass.pass2Material = material2;
        highlighterPass.pass3Material = material3;

        highlighterPass.renderPassEvent = Event;
        RenderPipelineManager.beginCameraRendering += DrawMesh;
    }

    [ContextMenu("Example: Hide Highlights")]
    public void HideHighlights()
    {
        RenderPipelineManager.beginCameraRendering -= DrawMesh;
    }
}