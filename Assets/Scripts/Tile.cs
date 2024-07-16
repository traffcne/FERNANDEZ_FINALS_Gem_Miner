using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [Header("Tile Sprites")]
    [SerializeField] private Sprite unclickedPlot;
    [SerializeField] private Sprite markedPlot;
    [SerializeField] private List<Sprite> clickedPlots;
    [SerializeField] private Sprite caveIn;
    [SerializeField] private Sprite Gems;
    [SerializeField] private Sprite Goblin;

    [Header("GM set via code")]
    public GameManager gameManager;

    private SpriteRenderer spriteRenderer;
    public bool marked = false;
    public bool active = true;
    public bool isCaveIn = false;
    public bool isGems = false;
    public int caveInCount = 0;
    public int gemsCount = 0;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    
    void Update()
    {
       
    }

    private void OnMouseOver()
    {
        if (active){
            if (Input.GetMouseButtonDown(0))
            {
                ClickedTile();
            }
            
            else if (Input.GetMouseButtonDown(1))
            {
                marked = !marked;
                if (marked)
                {
                    spriteRenderer.sprite = markedPlot;
                }
                else
                {
                    spriteRenderer.sprite = unclickedPlot;
                }
            }

            else
            {
                if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
                {
                    gameManager.ExpandIfMarked(this);
                }
            }
        }
    }

    public void ClickedTile()
    {
        if (active & !marked)
        {
            active = false;

            if (isCaveIn)
            {
                spriteRenderer.sprite = caveIn;
                gameManager.GameOver();
            }
            else if (isGems)
            {
                spriteRenderer.sprite = Gems;
                gameManager.Score++;
                gameManager.AltCheckGameOver(gameManager.Score);
            }
            else
            {
                spriteRenderer.sprite = clickedPlots[caveInCount];
                if (caveInCount == 0 && gemsCount != 1)
                {
                    gameManager.ClickNeighbours(this);
                }
                gameManager.CheckGameOver();
            }
        }

    }

    public void ShowGameOverState()
    {
        if (active)
        {
            active = false;
            if (isCaveIn & !marked)
            {
                spriteRenderer.sprite = caveIn;
            }
            if (marked & !isCaveIn)
            {
                spriteRenderer.sprite = Goblin;
            }
        }
    }

    public void SetMarkedCaveIn()
    {
        if (isCaveIn)
        {
            marked = true;
            spriteRenderer.sprite = markedPlot;
        }
    }

}
