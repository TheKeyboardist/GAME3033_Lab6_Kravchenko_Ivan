using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEvents
{
 public delegate void MouseCursorEnable(bool enable);

 public static event MouseCursorEnable MouseCurserEnable;

 public static void Invoke_MouseCursorEnable(bool enabled)
 {
   MouseCurserEnable?.Invoke(enabled);
 }
}
