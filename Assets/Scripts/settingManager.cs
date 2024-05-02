using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class settingManager
{
    public enum PortMode{
        Desktop,
        Mobile
    };

    public static PortMode BuildSetting;
}
