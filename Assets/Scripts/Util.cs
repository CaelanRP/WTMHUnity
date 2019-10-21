using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    private static System.Random r;
    public static System.Random random{
		get{
			if (r == null){
				r = new System.Random();
			}
			return r;
		}
	}
    public static T RandomSelection<T>(this IEnumerable<T> enumerable, Func<T, int> weightFunc)
    {
        int totalWeight = 0; // this stores sum of weights of all elements before current
        T selected = default(T); // currently selected element
        foreach (var data in enumerable)
        {
            int weight = weightFunc(data); // weight of current element
            int value = random.Next(totalWeight + weight); // random value
            if (value >= totalWeight) // probability of this is weight/(totalWeight+weight)
                selected = data; // it is the probability of discarding last selected element and selecting current one instead
            totalWeight += weight; // increase weight sum
        }

		if (selected == null){

		}

        return selected; // when iterations end, selected is some element of sequence. 
    }

    public static T RandomSelection<T>(this IEnumerable<T> enumerable){
        return RandomSelection(enumerable, e => 1);
    }
}
