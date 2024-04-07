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
                boardSlot.Position = new Vector2(i, j);
                boardSlot.FillRandom();
                _grid[i, j] = boardSlot;

                slotGameObject.name = "GridSlot(" + i + ", " + j + ")";
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void Swap()
    {

    }
}
