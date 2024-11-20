using Godot;

public partial class CameraController : Node3D
{
    [Export]
    public float MovementSpeed { get; set; } = 5.0f;
    
    [Export]
    public float MouseSensitivity { get; set; } = 0.002f;
    
    private Camera3D _camera;
    private Vector3 _velocity = Vector3.Zero;
    private float _rotationX = 0;
    private float _rotationY = 0;
    
    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && 
            Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            // First rotate around Y axis (left/right)
            RotateObjectLocal(Vector3.Up, -mouseMotion.Relative.X * MouseSensitivity);
            
            // Then rotate around local X axis (up/down)
            float xRotation = -mouseMotion.Relative.Y * MouseSensitivity;
            xRotation = Mathf.Clamp(
                _rotationX + xRotation, 
                -Mathf.Pi/2, 
                Mathf.Pi/2
            ) - _rotationX;
            _rotationX += xRotation;
            RotateObjectLocal(Vector3.Right, xRotation);
        }
        
        if (@event.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured 
                ? Input.MouseModeEnum.Visible 
                : Input.MouseModeEnum.Captured;
        }
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector3 direction = Vector3.Zero;
        Transform3D transform = Transform;
        
        if (Input.IsActionPressed("ui_up"))
            direction -= transform.Basis.Z;
        if (Input.IsActionPressed("ui_down"))
            direction += transform.Basis.Z;
        if (Input.IsActionPressed("ui_left"))
            direction -= transform.Basis.X;
        if (Input.IsActionPressed("ui_right"))
            direction += transform.Basis.X;
        
        if (Input.IsKeyPressed(Key.E))
            direction += Vector3.Up;
        if (Input.IsKeyPressed(Key.Q))
            direction += Vector3.Down;
        
        direction = direction.Normalized();
        Position += direction * MovementSpeed * (float)delta;
    }
}
