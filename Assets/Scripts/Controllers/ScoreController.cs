using System;
using UnityEngine;
using Scripts.Level;

namespace Scripts.Controllers
{
    public class ScoreController : MonoBehaviour
    {

        [Header("Score Lines Positions")]
        [Tooltip("Z Positions")]
        [SerializeField] private float firstLinePos;
        [SerializeField] private int linesAmount;

        [Header("Score Table")]
        [Tooltip("0 - Step, 1 - Home, 2 - FiveHome, 3 - Time")]
        [SerializeField] private int[] scoreValues = new int[4];

        private float[] linesPositions;
        public static bool[] linesReacheds;

        public static int[] scoreTable;
        public static int score { get; private set; }

        private void Start()
        {
            GetLinesPositions();
            linesReacheds = new bool[linesPositions.Length];

            InitScoreTable();
        }

        private void Update()
        {
            print(score);
        }

        private void GetLinesPositions()
        {
            linesPositions = new float[linesAmount];
            for (int i = 0; i < linesAmount; i++)
                linesPositions[i] = firstLinePos + i;
        }

        private void InitScoreTable()
        {
            int lenght = 0;
            for (int i = 0; i < Enum.GetValues(typeof(ScoreType)).Length; i++)
                lenght++;
            scoreTable = new int[lenght];
            scoreTable = scoreValues;
        }

        public static void IncreaseScore(ScoreType scoreType)
        {
            if(scoreType == ScoreType.Time)
                score += scoreTable[(int)scoreType] * (int)FindObjectOfType<GameController>().timeLeft;
            else
                score += scoreTable[(int)scoreType];
        }

        public static void ResetScoreLines()
        {
            for (int i = 0; i < linesReacheds.Length; i++)
                linesReacheds[i] = false;
        }

        public void CheckScoreLine(Player player)
        {
            for (int i = 0; i < linesReacheds.Length; i++)
                if (!linesReacheds[i] && player.transform.position.z == linesPositions[i])
                {
                    IncreaseScore(ScoreType.Step);
                    linesReacheds[i] = true;
                }
        }

        public enum ScoreType { Step, Home, FiveHome, Time }

    }
}