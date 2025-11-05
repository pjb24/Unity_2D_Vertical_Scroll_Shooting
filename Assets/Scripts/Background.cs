using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private int _startIndex;
    private int _endIndex;
    [SerializeField] private Transform[] _sprites;

    private float _viewHeight;

    private void Awake()
    {
        _viewHeight = Camera.main.orthographicSize * 2;

        _startIndex = _sprites.Length - 1;
        _endIndex = 0;
    }

    private void Update()
    {
        Move();
        Scrolling();
    }

    private void Move()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * _speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    private void Scrolling()
    {
        if (_sprites[_endIndex].position.y < _viewHeight * (-1))
        {
            // Sprite Reuse.
            Vector3 backSpritePos = _sprites[_startIndex].localPosition;
            Vector3 frontSpritePos = _sprites[_endIndex].localPosition;

            _sprites[_endIndex].localPosition = backSpritePos + Vector3.up * _viewHeight;

            // Cursor Index Change
            int startIndexSave = _startIndex;
            _startIndex = _endIndex;
            _endIndex = (startIndexSave - 1 == -1) ? _sprites.Length - 1 : startIndexSave - 1;
        }
    }
}
