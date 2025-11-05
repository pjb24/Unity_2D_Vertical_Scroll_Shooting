using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _type;
    public string Type => _type;
    [SerializeField] private float _speed = 3f;

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rigid.linearVelocity = Vector2.down * _speed;
    }
}
