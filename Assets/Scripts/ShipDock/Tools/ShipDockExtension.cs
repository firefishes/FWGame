using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

static public class ShipDockExtension
{

    private static StringBuilder mBuilder;

    public static string Append(this string target, params string[] args)
    {
        if (mBuilder == null)
        {
            mBuilder = new StringBuilder();
        }
        mBuilder.Length = 0;
        mBuilder.Append(target);

        int max = args.Length;
        for (int i = 0; i < max; i++)
        {
            mBuilder.Append(args[i]);
        }
        return mBuilder.ToString();
    }

    public static bool IsEmpty(this string target)
    {
        return string.IsNullOrEmpty(target);
    }
}