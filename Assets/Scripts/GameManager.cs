using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;

    public TextMeshProUGUI winState;
    public TextMeshProUGUI altWinState;
    public TextMeshProUGUI loseState;
    public TextMeshProUGUI scoreCount;

    public Button resetGame;
    public Button mainMenu;

    private List<Tile> tiles = new(); 

    private int width;
    private int height;
    private int numCaveIns;
    private int numGems;
    public int Score;

    private readonly float tileSize = 0.5f;

    void Start()
    {
        //CreateGameBoard(10, 12, 12, 10); //Lad (Easy)
        //CreateGameBoard(16, 12, 16, 7); //Friend (Medium)
        //CreateGameBoard(18, 12, 20, 5); //Brother (Hard)
        CreateGameBoard(20, 12, 36, 11); // A Real Dwarf (Ultra-Violence)
        ResetGameState();
    }

    
    void Update()
    {
        scoreCount.SetText(Score.ToString());
    }

    public void CreateGameBoard(int width, int height, int numCaveIns, int numGems)
    {
        this.width = width;
        this.height = height;   
        this.numCaveIns = numCaveIns;
        this.numGems = numGems;

        for(int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++) 
            { 
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder;
                
                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);

                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);
                Tile tile = tileTransform.GetComponent<Tile>();
                tiles.Add(tile);
                tile.gameManager = this;
            }
        }
    }

    private void ResetGameState()
    {
        int[] caveInPositions = Enumerable.Range(0, tiles.Count).OrderBy(x => Random.Range(0.0f, 1.0f)).ToArray();
        int[] gemsPositions = Enumerable.Range(0, tiles.Count).OrderBy(x => Random.Range(0.0f, 1.0f)).ToArray();

        for (int i = 0; i < numCaveIns; i++)
        {
            int cavePos = caveInPositions[i];
            tiles[cavePos].isCaveIn = true;
        }

        for (int i = 0; i < numGems; i++)
        {
            int gemPos = gemsPositions[i];
            tiles[gemPos].isGems = true;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].caveInCount = HowManyCaveIns(i);
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].gemsCount = HowManyGems(i);
        }
    }
    private int HowManyCaveIns(int location)
    {
        int count = 0;
        foreach (int pos in GetNeighbours(location))
        {
            if (tiles[pos].isCaveIn)
            {
                count++;
            }
        }
        return count;
    }

    private int HowManyGems(int location)
    {
        int count = 0;
        foreach (int pos in GetNeighbours(location))
        {
            if (tiles[pos].isGems)
            {
                count++;
            }
        }
        return count;
    }

    private List<int> GetNeighbours(int pos)
    {
        List<int> neighbours = new();
        int row = pos / width; 
        int col = pos % width;

        if (row < (height - 1))
        {
            neighbours.Add(pos + width);
            if (col > 0)
            {
                neighbours.Add(pos + width - 1);
            }
            if (col < (width - 1))
            {
                neighbours.Add(pos + width + 1);
            }
        }
        if (col > 0)
        {
            neighbours.Add(pos - 1);
        }
        if (col < (width - 1))
        {
            neighbours.Add(pos + 1);
        }
        if (row > 0)
        {
            neighbours.Add(pos - width);
            if (col > 0)
            {
                neighbours.Add(pos - width - 1);
            }
            if (col <(width -1))
            {
                neighbours.Add(pos - width + 1);
            }
        }
        return neighbours;
    }

    public void ClickNeighbours(Tile tile)
    {
        int location = tiles.IndexOf(tile);
        foreach (int pos in GetNeighbours(location))
        {
            tiles[pos].ClickedTile();
        }
    }

    public void GameOver()
    {
        loseState.gameObject.SetActive(true);
        resetGame.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(true);
        foreach (Tile tile in tiles)
        {
            tile.ShowGameOverState();
        }
    }

    public void CheckGameOver()
    {
        int count = 0;
        foreach (Tile tile in tiles)
        {
            if (tile.active)
            {
                count++;
            }
        }

        if (count == numCaveIns)
        {
            winState.gameObject.SetActive(true);
            resetGame.gameObject.SetActive(true);
            mainMenu.gameObject.SetActive(true);
            foreach (Tile tile in tiles)
            {
                tile.active = false;
                tile.SetMarkedCaveIn();
            }
        }
    }

    public void AltCheckGameOver(int Score)
    {
        if (Score == numGems)
        {
            altWinState.gameObject.SetActive(true);
            resetGame.gameObject.SetActive(true);
            mainMenu.gameObject.SetActive(true);
            foreach (Tile tile in tiles)
            {
                tile.active = false;
                tile.SetMarkedCaveIn();
            }
        }
    }


    public void ExpandIfMarked(Tile tile)
    {
        int location = tiles.IndexOf(tile);
        int markCount = 0;
        foreach (int pos in GetNeighbours(location)) 
        { 
            if (tiles[pos].marked)
            {
                markCount++;
            }
        }
        if(markCount == tile.caveInCount)
        {
            ClickNeighbours(tile);
        }
    }
}
