using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainPoint : MonoBehaviour
{
    private static float _visibleDuration = 1.0f;
    private static float _disappearDuration = 1.0f;
    public float SpeedUp;
    public List<Color> Colors;
    public TextMesh Text;
    private bool _move = false;

    void Start()
    {
    }

    public void Gain(int point)
    {
        Text.color = Colors[Random.Range(0, Colors.Count)];
        Text.text = $"+{point}";
        _move = true;
        StartCoroutine(_handleGainPoint());
    }

    IEnumerator _handleGainPoint()
    {
        yield return new WaitForSeconds(_visibleDuration);

        float step = 1.0f / 5.0f;
        float time = _disappearDuration;
        while (time > 0.0f)
        {
            float coef = (time / _disappearDuration);
            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, coef);
            time -= step;
            yield return new WaitForSeconds(step);
        }
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 0.0f);
        Destroy(this.gameObject);
    }
    public void Update()
    {
        if (_move)
            this.transform.position += Vector3.up * SpeedUp * Time.deltaTime;
    }

}
