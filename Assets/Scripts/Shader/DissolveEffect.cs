using UnityEngine;

[DisallowMultipleComponent] // 不允许在同一对象上挂载多个该组件
public class DissolveEffect : MonoBehaviour
{
    private Renderer[] renderers; // 渲染器
    private Material dissolveMat; // 消融材质
    private float burnSpeed = 0.25f; // 燃烧速度
    private float burnAmount = 0; // 燃烧量, 值越大模型镂空的越多

    private void Awake()
    {
        dissolveMat = Resources.Load<Material>("DissolveMat");
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.sharedMaterials;
            Material[] dissolveMaterials = new Material[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                Material newMaterial = new Material(dissolveMat);
                SetTexture(materials[i], newMaterial);
                SetColor(materials[i], newMaterial);
                newMaterial.SetFloat("_BurnAmount", 0);
                dissolveMaterials[i] = newMaterial;
            }
            renderer.sharedMaterials = dissolveMaterials;
        }
    }

    private void Update()
    {
        burnAmount += Time.deltaTime * burnSpeed;
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.sharedMaterials;
            foreach (Material material in materials)
            {
                material.SetFloat("_BurnAmount", burnAmount);
            }
        }
        if (burnAmount >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void SetTexture(Material oldMaterial, Material newMaterial)
    { // 设置材质
        if (oldMaterial.HasTexture("_MainTex"))
        {
            Texture texture = oldMaterial.GetTexture("_MainTex");
            newMaterial.SetTexture("_MainTex", texture);
        }
    }

    private void SetColor(Material oldMaterial, Material newMaterial)
    { // 设置颜色
        Color color = Color.white;
        if (oldMaterial.HasColor("_Color"))
        {
            color = oldMaterial.GetColor("_Color");
        }
        newMaterial.SetColor("_Color", color);
    }
}
