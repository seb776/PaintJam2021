using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public void SetText(string name, int score)
    {
        if (name == null)
            return;
        var croppedName = string.Concat(name.Take(10));

        Text.text = $"{croppedName}\t{score}";
    }
}
