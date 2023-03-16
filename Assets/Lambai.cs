using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lambai : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("bai 1");
        for (uint i = 0; i < 5; i++)
        {
            Debug.Log(Bai1(i));
        }
        //int[] arr = new int[]{3,2,1,5,6,4};
        int[] arr = new int[]{3,2,13,14,1,5,6,4,7,8,9,10,12};
        Debug.Log("bai 2");
        Debug.Log(Bai2(arr, 3));
        
        Debug.Log("bai 3");
        
        int[][] matrix = new int[][] {
            new int[]{1,2,2},
            new int[]{1,2,0},
            new int[]{2,1,0},
        };
        Debug.Log(Bai3(matrix));
    }

   uint Bai1(uint n){
        uint temp = n/3;
        return n - temp*3; 
   }
   int Bai2(int[] arr, int k){
        List<int>  tempArr = new List<int>();
       int temp = 0;
       for (int i = 0; i < k; i++)
       {
            temp = 0;
           for (int j = 0; j < arr.Length; j++)
           {
               if(tempArr.Contains(arr[j])) continue;
               if(arr[j] > temp) temp = arr[j];
           }
           tempArr.Add(temp);
       }
       return temp;
   }
   //n = 3, m = 3
   string Bai3(int[][] matrix){
        //check row
        int result = 0;
        for (int i = 0; i < 3; i++)
        {
            if(matrix[1][i] != 0){
                result = matrix[1][i];
                bool isFound = true;
                for (int j = 1; j < 3; j++)
                {
                    if(matrix[1][j] != result) {
                    isFound = false;
                    break;
                    }
                }
                if(isFound) return (result == 2 ?"X" : "0")+" Win";
            }
        }
        result = 0;
        //check col
        for (int i = 0; i < 3; i++)
        {
            if(matrix[i][1] != 0){
                result = matrix[i][1];
                bool isFound = true;
                for (int j = 1; j < 3; j++)
                {
                    if(matrix[j][1] != result) {
                    isFound = false;
                    break;
                    }
                }
                if(isFound) return (result == 2 ?"X" : "0")+" Win";
            }
        }
        //check diagonal
        if(matrix[1][1] != 0) {
            result = matrix[1][1];
            if(matrix[0][0] == matrix[2][2] && matrix[0][0] == result) return  (result == 2 ?"X" : "0")+" Win";
            if(matrix[2][0] == matrix[0][2] && matrix[2][0] == result) return  (result == 2 ?"X" : "0")+" Win";
        }
        return "No one win yet";
   }
}
