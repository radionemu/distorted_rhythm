using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static string username;
    public static string userhash;
    public static int score;

    public static bool LoggedIn{ get{return username !=null;}}

    public static void LogOut(){
        username = null;
    }
}
