using System;
using System.Collections.Generic;
using DG.Tweening;

public class TweenChain
{
    public Queue<Sequence> SequenceQueue = new Queue<Sequence>();

    public TweenChain()
    {
        // empty
    }

    public void AddAndPlay(Tween tween)
    {
        // Create a paused DOTween sequence to "wrap" our tween
        var sequence = DG.Tweening.DOTween.Sequence();
        sequence.Pause();
        // "Wrap" the tween
        sequence.Append(tween);
        // Add tween to queue
        SequenceQueue.Enqueue(sequence);
        // If this is the only tween in queue, play it immediately
        if (SequenceQueue.Count == 1)
        {
            SequenceQueue.Peek().Play();
        }
        // When the tween finishes, we'll evaluate the queue
        sequence.OnComplete(OnComplete);
    }

    private void OnComplete()
    {
        // Tween completed. Remove it.
        SequenceQueue.Dequeue();

        // Other tweens awaiting?
        if (SequenceQueue.Count > 0)
        {
            // Play next tween in queue
            SequenceQueue.Peek().Play();
        }
    }

    public bool IsRunning()
    {
        // Are tweens being processed?
        return SequenceQueue.Count > 0;
    }

    public void Destroy()
    {
        // Goodbye. Thanks for your hard work.
        foreach (var sequence in SequenceQueue)
        {
            sequence.Kill();
        }
        SequenceQueue.Clear();
    }
}
