# Kogane Rect Transform Visualizer

RectTransform の範囲を可視化する機能

## 使用例

```cs
using Kogane;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    private readonly RectTransformVisualizer m_visualizer = new RectTransformVisualizer
    (
        outlineSize: 2,
        outlineColor: Color.red,
        gameObject => gameObject.GetComponent<Graphic>() != null
    );

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            m_visualizer.Show();
        }
        if ( Input.GetKeyDown( KeyCode.X ) )
        {
            m_visualizer.Hide();
        }
    }
}
```

![Image (24)](https://user-images.githubusercontent.com/6134875/84562084-895fb900-ad8c-11ea-8a6b-1fb0904ef87b.gif)
