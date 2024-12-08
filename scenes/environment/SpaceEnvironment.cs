using Godot;

public partial class SpaceEnvironment : Node3D
{
    private DirectionalLight3D _sun;
    private WorldEnvironment _worldEnvironment;
    private Node3D _fogSystem;

    [Export] public DirectionalLight3D Sun => _sun;
    [Export] public WorldEnvironment WorldEnvironment => _worldEnvironment;
    [Export] public Node3D FogSystem => _fogSystem;

    public override void _Ready()
    {
        CreateEnvironmentNodes();
        SetupSun();
        SetupEnvironment();
    }

    private void CreateEnvironmentNodes()
    {
        // Create and add WorldEnvironment
        _worldEnvironment = new WorldEnvironment();
        AddChild(_worldEnvironment);

        // Create and add Sun
        _sun = new DirectionalLight3D();
        _sun.Name = "Sun";
        AddChild(_sun);

        // Get existing FogSystem or create if needed
        _fogSystem = GetNodeOrNull<Node3D>("FogSystem");
        if (_fogSystem == null)
        {
            _fogSystem = new Node3D();
            _fogSystem.Name = "FogSystem";
            AddChild(_fogSystem);
        }
    }

    private void SetupSun()
    {
        _sun.LightEnergy = 1.5f;
        _sun.LightColor = new Color(1.0f, 0.98f, 0.95f);
        _sun.ShadowEnabled = true;
        _sun.DirectionalShadowMode = DirectionalLight3D.ShadowMode.Parallel4Splits;
        _sun.DirectionalShadowBlendSplits = true;
        _sun.DirectionalShadowMaxDistance = 1000.0f;
        _sun.RotationDegrees = new Vector3(-45, -45, 0);
    }

    private void SetupEnvironment()
    {
        var environment = new Environment();
        
        // Space environment settings
        environment.BackgroundMode = Environment.BGMode.Color;
        environment.BackgroundColor = Colors.Black;
        environment.AmbientLightSource = Environment.AmbientSource.Disabled;
        environment.FogEnabled = false;
        
        // Add some stars to background (optional)
        environment.BackgroundMode = Environment.BGMode.Sky;
        var sky = new Sky();
        var skyMaterial = new ProceduralSkyMaterial();
        skyMaterial.SkyTopColor = Colors.Black;
        skyMaterial.SkyHorizonColor = Colors.Black;
        skyMaterial.GroundBottomColor = Colors.Black;
        skyMaterial.GroundHorizonColor = Colors.Black;
        skyMaterial.SunAngleMax = 0;
        sky.SkyMaterial = skyMaterial;
        environment.Sky = sky;
        
        _worldEnvironment.Environment = environment;
    }

    // Method to update sun position
    public void UpdateSunPosition(Vector3 direction)
    {
        _sun.Basis = Basis.LookingAt(-direction.Normalized(), Vector3.Up);
    }
}
