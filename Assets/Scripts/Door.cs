using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    [SerializeField] private Side _enterSide = Side.Left;
    [SerializeField] private UnityEvent<bool> _playerPassedThrough = new UnityEvent<bool>();

    private BoxCollider2D _boxCollider2D;

    private void OnEnable()
    {
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();   
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _playerPassedThrough.Invoke(CameIn(collision));
        }
    }

    private bool CameIn(Collider2D collision)
    {
        bool isHorizontal = _enterSide == Side.Left || _enterSide == Side.Right;

        float collisionCentreCoordinate;
        float doorCentreCoordinate;

        if (isHorizontal)
        {
            collisionCentreCoordinate = collision.bounds.center.x;
            doorCentreCoordinate = _boxCollider2D.bounds.center.x;

            if (collisionCentreCoordinate > doorCentreCoordinate)
            {
                return _enterSide == Side.Left;
            }
            else
            {
                return _enterSide == Side.Right;
            }
        }
        else
        {
            collisionCentreCoordinate = collision.bounds.center.y;
            doorCentreCoordinate = _boxCollider2D.bounds.center.y;

            if (collisionCentreCoordinate < doorCentreCoordinate)
            {
                return _enterSide == Side.Up;
            }
            else
            {
                return _enterSide == Side.Down;
            }
        }
    }

    private enum Side
    {
        Up, Down, Left, Right
    }
}
