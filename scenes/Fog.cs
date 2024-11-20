using Godot;

public partial class Fog : Node3D
{
    private WorldEnvironment _worldEnvironment;
    private Node3D _orientationObjects;
    
    public override void _Ready()
    {
        SetupEnvironment();
        GenerateOrientationObjects();
    }
    
    private void SetupEnvironment()
    {
        _worldEnvironment = new WorldEnvironment();
        var environment = new Environment();
        
        // Setup fog parameters
        environment.VolumetricFogEnabled = true;
        environment.VolumetricFogDensity = 0.05f;
        environment.VolumetricFogAlbedo = new Color(0.8f, 0.8f, 0.8f);
        environment.VolumetricFogLength = 64.0f;
        environment.VolumetricFogAmbientInject = 0.3f;
        
        _worldEnvironment.Environment = environment;
        AddChild(_worldEnvironment);
    }
    
    private void GenerateOrientationObjects()
    {
        _orientationObjects = new Node3D();
        AddChild(_orientationObjects);
        
        // Generate some primitive shapes for orientation
        for (int i = 0; i < 10; i++)
        {
            var mesh = new CsgBox3D();
            mesh.Size = new Vector3(2, 2, 2);
            mesh.Position = new Vector3(
                GD.RandRange(-20, 20),
                GD.RandRange(0, 10),
                GD.RandRange(-20, 20)
            );
            
            // Random color for each box
            var material = new StandardMaterial3D();
            material.AlbedoColor = new Color(
                GD.Randf(),
                GD.Randf(),
                GD.Randf()
            );
            mesh.Material = material;
            
            _orientationObjects.AddChild(mesh);
        }
    }
}
