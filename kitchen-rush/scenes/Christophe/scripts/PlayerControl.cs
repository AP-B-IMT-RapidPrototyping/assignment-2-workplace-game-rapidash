using Godot;
using System;

public partial class PlayerControl : CharacterBody3D
{
    [Export] private float _mouseSensitivity = 0.003f;

    [Export] private Camera3D _camera;

    [Export] private float _speed = 5.0f;
    [Export] private float _jumpVelocity = 4.5f;

    [Export] private RayCast3D _raycast;

    private Node3D heldObject = null;

    [Export] public Node3D HoldPoint;

	private float _cameraRotationX = 0f;
	private float _cameraPitch = 0f;

    private Node3D currentInteractable = null;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = _jumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * _speed;
            velocity.Z = direction.Z * _speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);
        }

        Velocity = velocity;
        MoveAndSlide();

        if (Input.IsActionJustPressed("interact") && _raycast.IsColliding())
        {
            var collider = _raycast.GetCollider();

            if (collider is Node node)
            {
                Node current = node;

                // Loop omhoog in de tree tot we Stove vinden
                while (current != null)
                {
                    GD.Print("Check: ", current, " | Type: ", current.GetType());

                    if (current is Stove stove)
                    {
                        GD.Print("🔥 STOVE GEVONDEN!");
                        stove.Interact(this);
                        break;
                    }

                    current = current.GetParent();
                }
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            // Horizontal rotation: roteer de HELE speler (Y-axis)
            RotateY(-mouseMotion.Relative.X * _mouseSensitivity);

            // Vertical rotation: roteer alleen de CAMERA (X-axis)
            _camera.RotateX(-mouseMotion.Relative.Y * _mouseSensitivity);

            // Voorkom dat de camera omdraait (clamp tussen -86° en +86°, of -1.5 en 1.5 radialen)
            Vector3 cameraRotation = _camera.Rotation;
            cameraRotation.X = Mathf.Clamp(cameraRotation.X, -1.8f, 1.8f);
            _camera.Rotation = cameraRotation;
        }
    }

    //Hold object from here on:

    public bool HasObject()
    {
        return heldObject != null;
    }

    public void HoldObject(Node3D obj)
    {
        heldObject = obj;

        // Verwijder uit huidige parent als die er is
        obj.GetParent()?.RemoveChild(obj);

        // Voeg toe aan HoldPoint
        HoldPoint.AddChild(obj);

        // Zet positie en schaal zodat je hem ziet
        obj.Position = new Vector3(0, 0, 0);  // midden van HoldPoint
        obj.Rotation = Vector3.Zero;
        obj.Scale = Vector3.One;              // of pas aan naar de juiste grootte

        GD.Print("🎉 Object in handen: ", obj.Name);
    }

    public void DropObject()
    {
        if (heldObject == null)
            return;

        GetParent().AddChild(heldObject);
        heldObject.GlobalTransform = HoldPoint.GlobalTransform;
        heldObject = null;
    }
}