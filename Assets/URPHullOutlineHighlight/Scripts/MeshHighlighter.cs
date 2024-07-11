using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MeshHighlighter : MonoBehaviour
{
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

    [SerializeField] public HighlightEntry[] highlightEntries;

    private DrawMeshPass m_DrawMeshPass;

    private Mesh targetMesh;
    private Transform targetTransform;

    void DrawMesh(ScriptableRenderContext context, Camera cam)
    {
        if (targetMesh == null || material1 == null || material2 == null || material3 == null) return;

        var renderer = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(0);
        m_DrawMeshPass.mesh = targetMesh;
        m_DrawMeshPass.material1 = material1;
        m_DrawMeshPass.material2 = material2;
        m_DrawMeshPass.material3 = material3;
        m_DrawMeshPass.matrix = targetTransform.localToWorldMatrix;
        renderer.EnqueuePass(m_DrawMeshPass);
    }

    public void ShowHighlightMesh(int index)
    {
        if (index >= highlightEntries.Length)
        {
            return;
        }

        RenderPipelineManager.beginCameraRendering -= DrawMesh;

        var highlightEntry = highlightEntries[index];
        targetMesh = highlightEntry.meshFilter.sharedMesh;
        targetTransform = highlightEntry.targetTransform;

        m_DrawMeshPass = new DrawMeshPass();
        m_DrawMeshPass.renderPassEvent = RenderPassEvent.AfterRendering;
        RenderPipelineManager.beginCameraRendering += DrawMesh;
    }

    [ContextMenu("Example: Hide Highlight Mesh")]
    public void HideHighlightMesh()
    {
        RenderPipelineManager.beginCameraRendering -= DrawMesh;
    }

    [ContextMenu("Example: Show Highlight Mesh")]
    public void ExampleHighlightMesh1()
    {
        ShowHighlightMesh(0);
    }
}

class DrawMeshPass : ScriptableRenderPass
{
    public Matrix4x4 matrix { get; set; }
    public Mesh mesh { get; set; }
    public Material material1 { get; set; }
    public Material material2 { get; set; }
    public Material material3 { get; set; }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cb = CommandBufferPool.Get("Draw Mesh Custom Pass Test");
        cb.DrawMesh(mesh, matrix, material1, 0, 0, null);
        cb.DrawMesh(mesh, matrix, material2, 0, 0, null);
        cb.DrawMesh(mesh, matrix, material3, 0, 0, null);
        context.ExecuteCommandBuffer(cb);
        context.Submit();
        CommandBufferPool.Release(cb);
    }
}