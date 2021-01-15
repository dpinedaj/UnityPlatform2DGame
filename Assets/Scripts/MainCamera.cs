using System.Runtime.InteropServices;
using UnityEngine;

// TODO handle the camera movements to up or down more smooth
public class MainCamera : MonoBehaviour
{
    public Transform objective; // Transform to reference the Hero

    //constants and Variables
    private const float Smooth = 5f; // Smooth to make it more natural 
    private const float CameraMovement = 3f;
    private Vector3 _gap; // Gap to  check the difference between the Transform (hero) and the camera

    // Validations
    private bool _followHero = true; // TODO for a future stopper in the game
    
    // Start is called before the first frame update
    private void Start()
    {
        _gap = transform.position - objective.position;
    }

    private void FollowHero([Optional] Vector3 diffPosition)
    {
        // Function to Follow Hero
        if (!_followHero) return;

        var objectivePosition = objective.position + _gap + diffPosition;
        transform.position = Vector3.Lerp(transform.position, objectivePosition, Smooth * Time.deltaTime);
    }

    private void HandleVerticalMovements()
    {
        Vector3 Position(float val)
        {
            var newPosition = new Vector3(0, objective.position.y + val, 0);
            return newPosition;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            var diffPosition = Position(CameraMovement);
            FollowHero(diffPosition);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            var diffPosition = Position(-CameraMovement);
            FollowHero(diffPosition);
        }
    }

    // FixedUpdate is recommended if use forces or physics in general
    private void FixedUpdate()
    {
        FollowHero();
        HandleVerticalMovements();
    }

  
}