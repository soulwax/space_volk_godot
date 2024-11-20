using Godot;

public partial class CameraController : Node3D
{
    [Export] public float MovementSpeed { get; set; } = 5.0f;
    [Export] public float BoostMultiplier { get; set; } = 2.5f;
    [Export] public float MouseSensitivity { get; set; } = 0.002f;
    [Export] public float PanningSpeed { get; set; } = 0.1f;
    [Export] public float ZoomSpeed { get; set; } = 0.5f;
    
    private Camera3D _camera;
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _rotationDegrees = Vector3.Zero;
    private Vector2 _lastMousePosition;
    private bool _isPanning;
    private bool _isRotating;
    private float _targetZoom = 10.0f;
    
    public enum CameraMode
    {
        Fly,    // Free flight mode
        Orbit,  // Orbit around a point
        Pan     // Pan and zoom
    }
    
    [Export] public CameraMode CurrentMode { get; set; } = CameraMode.Fly;
    
    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            HandleMouseButton(mouseButton);
        }
        else if (@event is InputEventMouseMotion mouseMotion)
        {
            HandleMouseMotion(mouseMotion);
        }
        else if (@event.IsActionPressed("ui_cancel"))
        {
            ToggleMouseMode();
        }
        else if (@event is InputEventKey eventKey)
        {
            HandleKeyInput(eventKey);
        }
    }
    
    private void HandleMouseButton(InputEventMouseButton mouseButton)
    {
        switch (mouseButton.ButtonIndex)
        {
            case MouseButton.Right:
                _isRotating = mouseButton.Pressed;
                if (_isRotating)
                {
                    _lastMousePosition = mouseButton.Position;
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                }
                else if (!_isPanning)
                {
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                }
                break;
                
            case MouseButton.Middle:
                _isPanning = mouseButton.Pressed;
                if (_isPanning)
                {
                    _lastMousePosition = mouseButton.Position;
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                }
                else if (!_isRotating)
                {
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                }
                break;
                
            case MouseButton.WheelUp:
                _targetZoom = Mathf.Max(1.0f, _targetZoom - ZoomSpeed);
                break;
                
            case MouseButton.WheelDown:
                _targetZoom += ZoomSpeed;
                break;
        }
    }
    
    private void HandleMouseMotion(InputEventMouseMotion mouseMotion)
    {
        if (_isRotating)
        {
            switch (CurrentMode)
            {
                case CameraMode.Fly:
                    HandleFlyRotation(mouseMotion);
                    break;
                case CameraMode.Orbit:
                    HandleOrbitRotation(mouseMotion);
                    break;
            }
        }
        else if (_isPanning)
        {
            HandlePanning(mouseMotion);
        }
    }
    
    private void HandleFlyRotation(InputEventMouseMotion mouseMotion)
    {
        _rotationDegrees.Y -= mouseMotion.Relative.X * MouseSensitivity;
        _rotationDegrees.X -= mouseMotion.Relative.Y * MouseSensitivity;
        _rotationDegrees.X = Mathf.Clamp(_rotationDegrees.X, -89f, 89f);
        RotationDegrees = _rotationDegrees;
    }
    
    private void HandleOrbitRotation(InputEventMouseMotion mouseMotion)
    {
        var rotationChange = mouseMotion.Relative * MouseSensitivity;
        RotateObjectLocal(Vector3.Up, -rotationChange.X);
        RotateObjectLocal(Vector3.Right, -rotationChange.Y);
    }
    
    private void HandlePanning(InputEventMouseMotion mouseMotion)
    {
        Vector3 panDirection = new Vector3(
            -mouseMotion.Relative.X * PanningSpeed,
            mouseMotion.Relative.Y * PanningSpeed,
            0
        );
        
        // Transform the pan direction to be relative to the camera's orientation
        panDirection = Transform.Basis * panDirection;
        Position += panDirection;
    }
    
    private void HandleKeyInput(InputEventKey eventKey)
    {
        if (eventKey.Pressed && eventKey.Keycode == Key.Tab)
        {
            CurrentMode = (CameraMode)(((int)CurrentMode + 1) % 3);
            GD.Print($"Camera Mode: {CurrentMode}");
        }
    }
    
    private void ToggleMouseMode()
    {
        Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured 
            ? Input.MouseModeEnum.Visible 
            : Input.MouseModeEnum.Captured;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        HandleMovement(delta);
        HandleZoom(delta);
    }
    
    private void HandleMovement(double delta)
    {
        float speed = MovementSpeed * (Input.IsKeyPressed(Key.Shift) ? BoostMultiplier : 1.0f);
        Vector3 direction = Vector3.Zero;
        
        if (Input.IsActionPressed("ui_up"))
            direction -= Transform.Basis.Z;
        if (Input.IsActionPressed("ui_down"))
            direction += Transform.Basis.Z;
        if (Input.IsActionPressed("ui_left"))
            direction -= Transform.Basis.X;
        if (Input.IsActionPressed("ui_right"))
            direction += Transform.Basis.X;
        if (Input.IsKeyPressed(Key.E))
            direction += Vector3.Up;
        if (Input.IsKeyPressed(Key.Q))
            direction += Vector3.Down;
            
        direction = direction.Normalized();
        Position += direction * speed * (float)delta;
    }
    
    private void HandleZoom(double delta)
    {
        if (CurrentMode != CameraMode.Pan) return;
        
        float currentZoom = _camera.Position.Z;
        float newZoom = Mathf.Lerp(currentZoom, _targetZoom, (float)delta * 5.0f);
        _camera.Position = new Vector3(0, 0, newZoom);
    }
}
