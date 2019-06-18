using UnityEngine.UI;
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public static class DOTweenEx {



    /// <summary>Tweens a Text's color to the given value.
        /// Also stores the Text as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this TextMeshProUGUI target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        /// <summary>Tweens a Text's alpha color to the given value.
        /// Also stores the Text as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<Color, Color, ColorOptions> DOFade(this TextMeshProUGUI target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        /// <summary>Tweens a Text's text to the given value.
        /// Also stores the Text as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end string to tween to</param><param name="duration">The duration of the tween</param>
        /// <param name="richTextEnabled">If TRUE (default), rich text will be interpreted correctly while animated,
        /// otherwise all tags will be considered as normal text</param>
        /// <param name="scrambleMode">The type of scramble mode to use, if any</param>
        /// <param name="scrambleChars">A string containing the characters to use for scrambling.
        /// Use as many characters as possible (minimum 10) because DOTween uses a fast scramble mode which gives better results with more characters.
        /// Leave it to NULL (default) to use default ones</param>
        public static TweenerCore<string, string, StringOptions> DOText(this TextMeshProUGUI target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
        {
            TweenerCore<string, string, StringOptions> t = DOTween.To(() => target.text, x => target.text = x, endValue, duration);
            t.SetOptions(richTextEnabled, scrambleMode, scrambleChars)
                .SetTarget(target);
            return t;
        }

    public static Tweener DOTextInt(this Text text, int initialValue, int finalValue, float duration, Func<int, string> convertor) {
        return DOTween.To(
             () => initialValue,
             it => text.text = convertor(it),
             finalValue,
             duration
         );
    }

    public static Tweener DOTextInt(this Text text, int initialValue, int finalValue, float duration) {
        return DOTweenEx.DOTextInt(text, initialValue, finalValue, duration, it => it.ToString());
    }

    public static Tweener DOTextFloat(this Text text, float initialValue, float finalValue, float duration, Func<float, string> convertor) {
        return DOTween.To(
             () => initialValue,
             it => text.text = convertor(it),
             finalValue,
             duration
         );
    }

    public static Tweener DOTextFloat(this Text text, float initialValue, float finalValue, float duration) {
        return DOTweenEx.DOTextFloat(text, initialValue, finalValue, duration, it => it.ToString());
    }

    public static Tweener DOTextLong(this Text text, long initialValue, long finalValue, float duration, Func<long, string> convertor) {
        return DOTween.To(
             () => initialValue,
             it => text.text = convertor(it),
             finalValue,
             duration
         );
    }

    public static Tweener DOTextLong(this Text text, long initialValue, long finalValue, float duration) {
        return DOTweenEx.DOTextLong(text, initialValue, finalValue, duration, it => it.ToString());
    }

    public static Tweener DOTextDouble(this Text text, double initialValue, double finalValue, float duration, Func<double, string> convertor) {
        return DOTween.To(
             () => initialValue,
             it => text.text = convertor(it),
             finalValue,
             duration
         );
    }

    public static Tweener DOTextDouble(this Text text, double initialValue, double finalValue, float duration) {
        return DOTweenEx.DOTextDouble(text, initialValue, finalValue, duration, it => it.ToString());
    }
}