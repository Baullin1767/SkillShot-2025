using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class UIAnimations
{
    public static async UniTask ScaleUp(VisualElement el, float duration = 2f)
    {
        el.style.scale = new Scale(new Vector2(0.5f, 0.5f));
        el.style.opacity = 0;

        var tcs = new UniTaskCompletionSource();

        DOTween.To(() => el.resolvedStyle.scale.value.x,
                   x => el.style.scale = new Scale(new Vector2(x, x)),
                   1f, duration)
               .SetEase(Ease.OutBack)
               .SetUpdate(true);

        DOTween.To(() => el.style.opacity.value,
                   x => el.style.opacity = x,
                   1f, duration)
               .SetUpdate(true)
               .OnComplete(() => tcs.TrySetResult());

        await tcs.Task;
    }
    public static void HideElement(VisualElement el)
    {
        el.style.opacity = 0;
    }

    public static void Pulse(VisualElement el, float scale = 1.2f, float duration = 0.3f)
    {
        var tcs = new UniTaskCompletionSource();

        DOTween.To(() => el.resolvedStyle.scale.value.x,
                   x => el.style.scale = new Scale(new Vector2(x, x)),
                   scale, duration / 2)
               .SetEase(Ease.OutBack)
               .SetUpdate(true)
               .OnComplete(() =>
               {
                   DOTween.To(() => el.resolvedStyle.scale.value.x,
                              x => el.style.scale = new Scale(new Vector2(x, x)),
                              1f, duration / 2)
                          .SetEase(Ease.InOutSine)
                          .SetUpdate(true)
                          .OnComplete(() => tcs.TrySetResult());
               });
    }

}
