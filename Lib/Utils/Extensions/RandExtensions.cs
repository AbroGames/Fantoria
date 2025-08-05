using System;
using System.Collections.Generic;
using System.Linq;
using Fantoria.Lib.Services;

namespace Fantoria.Lib.Utils.Extensions;

public static class RandExtensions
{
    /// <summary>
    /// Возвращает случайный элемент списка
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetRandom<T>(this IEnumerable<T> sequence)
    {
        if (sequence == null)
        {
            throw new ArgumentNullException();
        }

        if (!sequence.Any())
        {
            throw new ArgumentException("The sequence is empty.");
        }

        //optimization for ICollection<T>
        if (sequence is ICollection<T>)
        {
            ICollection<T> col = (ICollection<T>)sequence;
            return col.ElementAt(LibService.Rand.Range(col.Count));
        }

        int count = 1;
        T selected = default(T);

        foreach (T element in sequence)
        {
            if (LibService.Rand.Range(count++) == 0)
            {
                //Select the current element with 1/count probability
                selected = element;
            }
        }

        return selected;
    }
}