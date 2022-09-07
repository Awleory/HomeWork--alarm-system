using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D), typeof(AudioSource))]
public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private Side _enterSide = Side.Left;
    [SerializeField] private float spendTimeToMaxVolume = 2f;

    private BoxCollider2D _boxCollider2D;
    private AudioSource _audioSource;
    private bool _collisionIsInside;

    private void OnEnable()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _collisionIsInside = CameIn(collision);
            StartCoroutine(SetAlarmVolume(_collisionIsInside));
        }
    }

    private bool CameIn(Collider2D collision)
    {
        bool isHorizontal = _enterSide == Side.Left || _enterSide == Side.Right;

        float collisionCentreCoordinate;
        float alarmSystemCentreCoordinate;

        if (isHorizontal)
        {
            collisionCentreCoordinate = collision.bounds.center.x;
            alarmSystemCentreCoordinate = _boxCollider2D.bounds.center.x;

            if (collisionCentreCoordinate > alarmSystemCentreCoordinate)
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
            alarmSystemCentreCoordinate = _boxCollider2D.bounds.center.y;

            if (collisionCentreCoordinate < alarmSystemCentreCoordinate)
            {
                return _enterSide == Side.Up;
            }
            else
            {
                return _enterSide == Side.Down;
            }
        }
    }

    private IEnumerator SetAlarmVolume(bool toUpper)
    {
        float deltaRate = 1 / spendTimeToMaxVolume;
        float targetValue = 0;

        if (toUpper)
        {
            targetValue = 1f;

            if (_audioSource.isPlaying == false)
            {
                _audioSource.Play();
            }
        }

        while (_audioSource.volume != targetValue && _collisionIsInside == toUpper)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetValue, deltaRate * Time.deltaTime);
            yield return null;
        }

        if (toUpper == false && _audioSource.volume == 0f)
        {
            _audioSource.Stop();
        }
    }

    private enum Side
    {
        Up, Down, Left, Right
    }
}
