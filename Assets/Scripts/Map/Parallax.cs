using UnityEngine;

public class Parallax : MonoBehaviour 
{
    [Header("Loop Settings")]
    [SerializeField] private bool _xAxis = true;
    [SerializeField] private bool _yAxis = true;
    [SerializeField] private bool _isInfinite = true;
    [SerializeField] private Vector2 _offset;

    [Header("References")]
    [SerializeField] private Transform _subject;

    private SpriteRenderer _spriteRenderer;
    private Vector2 _startPos;
    private float _zPosition;
    private Camera _cam;

    private float TwoAspect => _cam.aspect * 2;
    private float TileWidth => (TwoAspect > 3 ? TwoAspect : 3);
    private float ViewWidth => _spriteRenderer.sprite.rect.width / _spriteRenderer.sprite.pixelsPerUnit;
    private Vector2 Travel => (Vector2)_cam.transform.position - _startPos;

    private float SubjectDistance => transform.position.z - _subject.position.z;
    private float ClippingPlane => (_cam.transform.position.z + (SubjectDistance > 0 ? _cam.farClipPlane : _cam.nearClipPlane));
    private float ParallaxFactor => Mathf.Abs(SubjectDistance) / ClippingPlane;

    // this script will apply a paralax value based on the distance the gameobject is from the player 
    // uses the camera clipping plans to calculate this value //

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _cam = Camera.main;
        _zPosition = transform.position.z;
        _startPos = new(transform.position.x + _offset.x, transform.position.y + _offset.y);

        if (!_isInfinite) return;
        float spriteSizeX = _spriteRenderer.sprite.rect.width / _spriteRenderer.sprite.pixelsPerUnit;
        float spriteSizeY = _spriteRenderer.sprite.rect.height / _spriteRenderer.sprite.pixelsPerUnit;

        _spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        _spriteRenderer.size = new Vector2(spriteSizeX * TileWidth, spriteSizeY);
        transform.localScale = Vector3.one;

    }

    void FixedUpdate()
    {
        Vector2 newPos = _startPos + Travel * ParallaxFactor;
        transform.position = new(_xAxis ? newPos.x : _startPos.x, _yAxis ? newPos.y : _startPos.y, _zPosition);

        if (!_isInfinite) return;
        Vector2 totalTravel = _cam.transform.position - transform.position;
        float boundsOffset = (ViewWidth / 2) * (totalTravel.x > 0 ? 1 : -1);
        float screens = (int)((totalTravel.x + boundsOffset) / ViewWidth);
        transform.position += new Vector3(screens * ViewWidth, 0);
    }
}