using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public enum Direction
    {
        LEFT, UP, RIGHT, DOWN
    }

    public Direction moveDirection;
    public float delayStep; //Tempo entre passos
    public float step; //Quantidade a mover por passo
    public Transform head;
    public List<Transform> snakeBody;

    public GameObject tailPrefab;
    public Transform foodTransform;

    public int cols = 29;
    public int rows = 15;

    public Text txtScore;
    public Text txtHiScore;
    private int score;
    private int hiScore;

    private Vector3 lastPosition;

    public GameObject gameOver;
    public GameObject gameTitle;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MoveSnake");
        SetFood();
        hiScore = PlayerPrefs.GetInt("HiScore");
        txtHiScore.text = $" Hi-Score: {hiScore} ";
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (moveDirection == Direction.DOWN)
                return;
            moveDirection = Direction.UP;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (moveDirection == Direction.LEFT)
                return;
            moveDirection = Direction.RIGHT;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (moveDirection == Direction.UP)
                return;
            moveDirection = Direction.DOWN;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (moveDirection == Direction.RIGHT)
                return;
            moveDirection = Direction.LEFT;
        }

       

    }

    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(delayStep);
        Vector3 nextPosition = Vector3.zero;
        switch (moveDirection)
        {

            case Direction.DOWN:
                nextPosition = Vector3.down;
                head.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.RIGHT:
                nextPosition = Vector3.right;
                head.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.UP:
                nextPosition = Vector3.up;
                head.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case Direction.LEFT:
                nextPosition = Vector3.left;
                head.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
        nextPosition *= step;
        lastPosition = head.position;
        head.position += nextPosition;
        foreach(Transform block in snakeBody)
        {
            Vector3 temp = block.position;
            block.position = lastPosition;
            lastPosition = temp;
            block.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        StartCoroutine("MoveSnake");
    }

    public void Eat()
    {
        Vector3 tailPos = head.position;
        if(snakeBody.Count > 0)
        {
            tailPos = snakeBody[snakeBody.Count - 1].position;
        }

        GameObject temp = Instantiate(tailPrefab, tailPos, transform.localRotation);
        snakeBody.Add(temp.transform);
        score += 10;
        txtScore.text = $"Score: {score}";
        SetFood();
    }

    void SetFood()
    {
        int x = Random.Range((cols - 1) / 2 *  -1, (cols - 1) / 2);
        int y = Random.Range((rows -1)/2 *-1, (rows - 1) / 2);

        foodTransform.position = new Vector2(x * step, y * step);
    }

    public void GameOver()
    {
        SoundManager.PlaySound("death");
        Time.timeScale = 0;
        gameOver.SetActive(true);
        if (score > hiScore)
        {
            PlayerPrefs.SetInt("HiScore", score);
            txtHiScore.text = $"New Hi-Score: {score}";
        }
    }

    public void PlayAgain()
    {

        head.position = Vector3.zero;
        SetFood();
        score = 0;

        moveDirection = Direction.LEFT;
        head.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        foreach(Transform t in snakeBody)
        {
            Destroy(t.gameObject);
        }
        snakeBody.Clear();

        txtScore.text = $" Score: {score} ";
        hiScore = PlayerPrefs.GetInt("HiScore");
        txtHiScore.text = $" Hi-Score: {hiScore}";

        gameOver.SetActive(false);
        gameTitle.SetActive(false);

        Time.timeScale = 1;
    }
}
