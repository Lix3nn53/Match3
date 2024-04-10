using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int ItemCount = 0;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _halfDistance = .5f;

    private BoardSlot[,] _grid;

    private GameObject _slotPrefab;
    private GameObject _slotFactoryPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        _slotPrefab = AssetManager.Instance.SlotPrefab;
        _slotFactoryPrefab = AssetManager.Instance.SlotFactoryPrefab;

        _grid = new BoardSlot[_width, _height + 1];

        SetUp();
    }

    private void SetUp()
    {
        int fillDebug = 0;

        float startX = (-_width * _halfDistance) + transform.position.x;
        float startY = (-_height * _halfDistance) + transform.position.y;

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height + 1; j++)
            {
                Vector2 pos = new Vector2(startX + (i * _halfDistance * 2) + _halfDistance, startY + (j * _halfDistance * 2) + _halfDistance);

                GameObject prefab = j == _height ? _slotFactoryPrefab : _slotPrefab;

                GameObject slotGameObject = Instantiate(prefab, pos, Quaternion.identity, transform);
                slotGameObject.name = "GridSlot(" + i + ", " + j + ")";

                BoardSlot boardSlot = slotGameObject.GetComponent<BoardSlot>();
                _grid[i, j] = boardSlot;

                boardSlot.Board = this;
                boardSlot.Position = new Vector2Int(i, j);

                List<BoardItemType> defaultFactory = boardSlot.DefaultFactoryValues();

                boardSlot.FillRandom(defaultFactory);
                fillDebug++;

                if (j != _height)
                {
                    SolvedData solvedData = GetSolution(boardSlot.Position);

                    while (solvedData.IsSolved())
                    {
                        defaultFactory.Remove(boardSlot.CurrentItem.ItemType);
                        if (defaultFactory.Count == 0)
                        {
                            Debug.LogError("Cant place item without solution at " + boardSlot.Position);
                            break;
                        }
                        boardSlot.FillRandom(defaultFactory);
                        fillDebug++;
                        solvedData = GetSolution(boardSlot.Position);
                    }
                }
            }
        }

        Debug.Log("fillDebug: " + fillDebug);
    }

    public BoardSlot GetBoardSlot(Vector2Int center, Vector2Int offset = new Vector2Int(), bool includeFactory = true)
    {
        int x = center.x + offset.x;
        int y = center.y + offset.y;

        // Check if the coordinates are within the bounds of the grid
        int height = _height;
        if (includeFactory)
        {
            height++;
        }

        if (x >= 0 && x < _width && y >= 0 && y < height)
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
        if (item == null)
        {
            return;
        }
        if (item.CurrentSlot == null)
        {
            return;
        }

        Vector2Int currentPosition = item.CurrentSlot.Position;

        bool destroy = item.DestroySelf();

        if (!destroy)
        {
            return;
        }

        // StartFallingInto(currentPosition);
    }

    public bool StartFalling(BoardSlot slot)
    {
        if (slot != null)
        {
            BoardItem currentItem = slot.CurrentItem;
            if (currentItem == null)
            {
                // if empty do nothing and return
                return true;
            }
            bool startFall = currentItem.StartFalling();
            if (startFall)
            {
                StartFallingInto(slot.Position);
                return true;
            }
        }

        return false;
    }

    public void StartFallingInto(Vector2Int emptyPosition)
    {
        // Middle Up
        BoardSlot slot = GetBoardSlot(emptyPosition, Vector2Int.up);
        bool startFall = StartFalling(slot);
        if (startFall)
        {
            return;
        }

        // Middle Up is obstacle
        // Check Middle Up Left
        slot = GetBoardSlot(emptyPosition, Vector2Int.up + Vector2Int.left);
        startFall = StartFalling(slot);
        if (startFall)
        {
            return;
        }

        // Middle Up Left is obstacle
        // Check Middle Up Right
        slot = GetBoardSlot(emptyPosition, Vector2Int.up + Vector2Int.right);
        startFall = StartFalling(slot);
        if (startFall)
        {
            return;
        }
    }

    public void OnSlotEmpty(BoardSlot slot)
    {
        if (slot.Position.y == _height)
        {
            slot.FillRandom();
        }
    }

    public void Swap(Vector2Int pos1, Vector2Int pos2)
    {
        BoardSlot slot1 = GetBoardSlot(pos1);
        BoardSlot slot2 = GetBoardSlot(pos2);

        if (slot1 == null || slot2 == null || slot1.CurrentItem == null || slot2.CurrentItem == null)
        {
            return;
        }

        BoardItem item1 = slot1.CurrentItem;
        BoardItem item2 = slot2.CurrentItem;

        if (item1 is BoardItemObstacle || item2 is BoardItemObstacle)
        {
            return;
        }

        item1.ClearCurrentSlot();
        item2.ClearCurrentSlot();

        item1.MoveTo(slot2, null);
        item2.MoveTo(slot1, () => OnSwapComplete(pos1, pos2, item1, item2, slot1, slot2, true));
    }

    private void OnSwapComplete(Vector2Int pos1, Vector2Int pos2, BoardItem item1, BoardItem item2, BoardSlot slot1, BoardSlot slot2, bool revert)
    {
        // Check match
        SolvedData solvedData = GetSolution(pos1, pos2);

        if (solvedData.IsSolved())
        {
            foreach (BoardSlot slot in solvedData.GetSolvedGridSlots())
            {
                // slot.CurrentItem.Debug();
                DestroyOne(slot.CurrentItem);
            }
            foreach (BoardSlot slot in solvedData.GetSolvedGridSlots())
            {
                StartFallingInto(slot.Position);
            }
        }
        else if (revert)
        {
            // Revert swap
            item1.ClearCurrentSlot();
            item2.ClearCurrentSlot();

            item1.MoveTo(slot1, null);
            item2.MoveTo(slot2, () => OnSwapComplete(pos1, pos2, item1, item2, slot1, slot2, false));
        }
    }

    protected SolvedData GetSolution(params Vector2Int[] positions)
    {
        return GameManager.Instance.BoardSolver.Solve(this, positions);
    }

    public void OnFallComplete(Vector2Int pos)
    {
        // Check match
        SolvedData solvedData = GetSolution(pos);

        if (solvedData.IsSolved())
        {
            foreach (BoardSlot slot in solvedData.GetSolvedGridSlots())
            {
                // slot.CurrentItem.Debug();
                DestroyOne(slot.CurrentItem);
            }
            foreach (BoardSlot slot in solvedData.GetSolvedGridSlots())
            {
                StartFallingInto(slot.Position);
            }
        }
    }
}
