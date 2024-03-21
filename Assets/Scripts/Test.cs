using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public List<int> solve(List<int> A, List<List<int>> B)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < A.Count; i++)
        {
            A[i] = A[i] % 7;
        }

        for (int i = 0; i < B.Count; i++)
        {
            int count = 0;
            for (int j = B[i][0]; j <= B[i][1]; j++)
            {
                if (A[j] % 7 == 0)
                    count++;
            }
        }

        return result;
    }

    public List<List<int>> solve(List<List<int>> A)
    {
        List<List<int>> result = new List<List<int>>();

        return result;
    }
}
