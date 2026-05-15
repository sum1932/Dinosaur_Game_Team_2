using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MeteorEffect : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private GameObject shardModel;
    [SerializeField] private Material shardMaterial;

    [Header("Settings")]
    [SerializeField] private float emissionRate = 80f;
    [SerializeField] private float startSizeMin = 0.5f;
    [SerializeField] private float startSizeMax = 1.2f;
    [SerializeField] private float lifetime = 3.5f;

    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        SetupParticleSystem();
    }

    private void OnValidate()
    {
        // Only auto-setup in editor when not in play mode to avoid spamming errors
        if (!Application.isPlaying && ps != null) 
        {
            SetupParticleSystem();
        }
    }

    [ContextMenu("Setup Particle System")]
    public void SetupParticleSystem()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        if (ps == null) return;

        // Some properties like 'duration' cannot be set while the system is playing.
        bool wasPlaying = ps.isPlaying;
        if (wasPlaying)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // Main Module
        var main = ps.main;
        main.duration = 1.0f;
        main.loop = true;
        main.startLifetime = lifetime;
        main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
        main.startSize = new ParticleSystem.MinMaxCurve(startSizeMin, startSizeMax);
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.gravityModifier = 0.1f;

        // ... (rest of the modules)
        SetupModules();

        if (wasPlaying)
        {
            ps.Play();
        }
    }

    private void SetupModules()
    {
        // Move other module settings here to keep SetupParticleSystem clean
        // Emission Module
        var emission = ps.emission;
        emission.enabled = true;
        emission.rateOverTime = emissionRate;

        // Shape Module
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 15f;
        shape.radius = 0.3f;

        // Color over Lifetime
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(new Color(1.0f, 0.9f, 0.2f), 0.0f),
                new GradientColorKey(new Color(1.0f, 0.4f, 0.0f), 0.3f),
                new GradientColorKey(new Color(0.2f, 0.1f, 0.1f), 0.7f),
                new GradientColorKey(Color.black, 1.0f)
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f), 
                new GradientAlphaKey(1.0f, 0.6f), 
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        colorOverLifetime.color = grad;

        // Size over Lifetime
        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0.0f, 0.5f);
        sizeCurve.AddKey(0.2f, 1.0f);
        sizeCurve.AddKey(1.0f, 0.0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        // Rotation over Lifetime
        var rotationOverLifetime = ps.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(Mathf.Deg2Rad * -360, Mathf.Deg2Rad * 360);

        // Renderer
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Mesh;
        
        if (shardModel != null)
        {
            MeshFilter mf = shardModel.GetComponentInChildren<MeshFilter>();
            if (mf != null) renderer.mesh = mf.sharedMesh;
        }

        if (shardMaterial != null) renderer.material = shardMaterial;
        
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        renderer.receiveShadows = true;
    }
}
