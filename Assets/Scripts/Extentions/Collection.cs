﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collection{
	public static void Shuffle<T>(this IList<T> ts)
    {
		UnityEngine.Random.seed = (int)PhotonNetwork.room.CustomProperties ["seed"];
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
    
}
