namespace MiniProjects.Scripts.Game
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class OnlyDistinctItemsFromList : MonoBehaviour
    {
        private readonly List<StudentInfo> studentInfos = new List<StudentInfo>()
        {
            new StudentInfo { ID = 1, Name = "Name1", AverageScore = 1},
            new StudentInfo { ID = 2, Name = "Name2", AverageScore = 2},
            new StudentInfo { ID = 3, Name = "Name3", AverageScore = 3},
            new StudentInfo { ID = 4, Name = "Name4", AverageScore = 1},
            new StudentInfo { ID = 5, Name = "Name5", AverageScore = 2},
            new StudentInfo { ID = 6, Name = "Name1", AverageScore = 3},
            new StudentInfo { ID = 7, Name = "Name2", AverageScore = 1},
            new StudentInfo { ID = 8, Name = "Name3", AverageScore = 2},
            new StudentInfo { ID = 9, Name = "Name4", AverageScore = 3},
            new StudentInfo { ID = 10, Name = "Name5", AverageScore = 1},
            new StudentInfo { ID = 11, Name = "Name1", AverageScore = 2},
            new StudentInfo { ID = 12, Name = "Name2", AverageScore = 3},
            new StudentInfo { ID = 13, Name = "Name3", AverageScore = 1},
            new StudentInfo { ID = 14, Name = "Name4", AverageScore = 2},
            new StudentInfo { ID = 15, Name = "Name5", AverageScore = 3},
        };
        
        private void Awake()
        {
            var distinctByAverageScore = studentInfos
                .ToLookup(p => p.AverageScore)
                .Select(coll => coll.First());
            
            var distinctByName = studentInfos
                .ToLookup(p => p.Name)
                .Select(coll => coll.First());
        }
        
        private class StudentInfo
        {
            public int ID;
            public string Name;
            public float AverageScore;
        }
    }
}
