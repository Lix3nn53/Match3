using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _halfDistance = .5f;

    private BoardSlot[,] _grid;

    private GameObject _slotPrefab;
    // Singleton
    public static Board Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _slotPrefab = AssetManager.Instance.SlotPrefab;

        _grid = new BoardSlot[_width, _height];

        SetUp();
    }
    private void SetUp()
    {
        float startX = (-_width * _halfDistance) + transform.position.x;
        float startY = (-_height * _halfDistance) + transform.position.y;

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Vector2 pos = new Vector2(startX + (i * _halfDistance * 2) + _halfDistance, startY + (j * _halfDistance * 2) + _halfDistance);

                GameObject slotGameObject = Instantiate(_slotPrefab, pos, Quaternion.identity, transform);
                BoardSlot boardSlot = slotGameObject.GetComponent<BoardSlot>();
                boardSlot.Position = new Vector2Int(i, j);
                boardSlot.FillRandom();
                _grid[i, j] = boardSlot;

                slotGameObject.name = "GridSlot(" + i + ", " + j + ")";
            }
        }
    }

    public BoardSlot GetBoardSlot(Vector2Int center, Vector2Int offset)
    {
        int x = center.x + offset.x;
        int y = center.y + offset.y;

        // Check if the coordinates are within the bounds of the grid
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            return _grid[x, y];
        }
        else
        {
            // Handle out-of-bounds case
            return null;
        }
    }

    public void DestroyOne(BoardItem item)
    {
        Vector2Int currentPosition = item.GetCurrentSlot().Position;

        item.DestroySelf();

        StartFalling(currentPosition);
    }

    public void StartFalling(Vector2Int emptyPosition)
    {
        // Middle Up
        BoardSlot slot = GetBoardSlot(emptyPosition, Vector2Int.up);
        if (slot != null)
        {
            BoardItem currentItem = slot.GetCurrentItem();
            if (currentItem == null)
            {
                // if Middle Up is empty do nothing and return
                return;
            }
            bool startFall = currentItem.StartFalling();
            if (startFall)
            {
                StartFalling(slot.Position);
                return;
            }
        }


        // Middle Up is obstacle
        // Check Middle Up Left
        slot = GetBoardSlot(emptyPosition, Vector2Int.up + Vector2Int.left);
        if (slot != null)
        {
            BoardItem currentItem = slot.GetCurrentItem();
            if (currentItem == null)
            {
                // if Middle Up is empty do nothing and return
                return;
            }
            bool startFall = currentItem.StartFalling();
            if (startFall)
            {
                StartFalling(slot.Position);
                return;
            }
        }

        // Middle Up Left is obstacle
        // Check Middle Up Right
        slot = GetBoardSlot(emptyPosition, Vector2Int.up + Vector2Int.right);
        if (slot != null)
        {
            BoardItem currentItem = slot.GetCurrentItem();
            if (currentItem == null)
            {
                // if Middle Up is empty do nothing and return
                return;
            }
            bool startFall = currentItem.StartFalling();
            if (startFall)
            {
                StartFalling(slot.Position);
                return;
            }
        }
    }
}
